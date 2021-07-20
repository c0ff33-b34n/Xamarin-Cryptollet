using Cryptollet.Common.Base;
using Cryptollet.Common.Database;
using Cryptollet.Common.Dialog;
using Cryptollet.Common.Models;
using Cryptollet.Common.Navigation;
using Cryptollet.Common.Validation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Cryptollet.Modules.AddTransaction
{
    [QueryProperty("Id", "id")]
    public class AddTransactionViewModel : BaseViewModel
    {
        private IRepository<Transaction> _repository;
        private IDialogMessage _dialogMessage;
        private INavigationService _navigationService;

        public AddTransactionViewModel(IRepository<Transaction> repository,
                                       IDialogMessage dialogMessage,
                                       INavigationService navigationService)
        {
            _repository = repository;
            _dialogMessage = dialogMessage;
            _navigationService = navigationService;
            AvailableAssets = new ObservableCollection<Coin>(Coin.GetAvailableAssets());
            AddValidation();
        }

        public override async Task InitializeAsync(object parameter)
        {
            if (string.IsNullOrWhiteSpace(Id) || int.TryParse(Id, out int transactionId))
            {
                TransactionDate = DateTime.Now;
                IsDeposit = true;
                return;
            }
            var transaction = await _repository.GetById(transactionId);
            IsDeposit = transaction.Status == Constants.TRANSACTION_DEPOSITED;
            Amount.Value = transaction.Amount;
            TransactionDate = transaction.TransactionDate;
            SelectedCoin = Coin.GetAvailableAssets().First(x => x.Symbol == transaction.Symbol);
        }

        private bool _isDeposit;
        public bool IsDeposit
        {
            get => _isDeposit;
            set { SetProperty(ref _isDeposit, value); }
        }

        private string _id;
        public string Id
        {
            get => _id;
            set
            {
                _id = Uri.UnescapeDataString(value);
            }
        }

        private ObservableCollection<Coin> _availableAssets;
        public ObservableCollection<Coin> AvailableAssets
        {
            get => _availableAssets;
            set { SetProperty(ref _availableAssets, value); }
        }

        private Coin _selectedCoin;
        public Coin SelectedCoin
        {
            get => _selectedCoin;
            set { SetProperty(ref _selectedCoin, value); }
        }

        private DateTime _transactionDate;
        public DateTime TransactionDate
        {
            get => _transactionDate;
            set { SetProperty(ref _transactionDate, value); }
        }

        private ValidatableObject<decimal> _amount;
        public ValidatableObject<decimal> Amount
        {
            get => _amount;
            set { SetProperty(ref _amount, value); }
        }

        public ICommand AddTransactionCommand { get => new Command(async () => await AddTransaction(), () => IsNotBusy); }
        public ICommand GoBackCommand { get => new Command(async () => await GoBack()); }

        private async Task GoBack()
        {
            var shouldGoBack = await _dialogMessage.DisplayAlert("Confirm",
                "Are you sure you want to navigate back? Any unsaved changes will be lost.", "Ok", "Cancel");
            if (shouldGoBack)
            {
                await _navigationService.GoBackAsync();
            }
        }

        private async Task AddTransaction()
        {
            _amount.Validate();
            if (!_amount.IsValid)
            {
                return;
            }
            if (SelectedCoin == null)
            {
                await _dialogMessage.DisplayAlert("Error", "Please select a coin.", "Ok");
                return;
            }
            IsBusy = true;
            await SaveNewTransaction();
            await _navigationService.PopAsync();
            IsBusy = false;
        }

        private async Task SaveNewTransaction()
        {
            var transaction = new Transaction
            {
                Amount = Amount.Value,
                TransactionDate = TransactionDate,
                Symbol = SelectedCoin.Symbol,
                Status = IsDeposit == true ? Constants.TRANSACTION_DEPOSITED : Constants.TRANSACTION_WITHDRAWN,
                Id = string.IsNullOrEmpty(Id) ? 0 : int.Parse(Id)
            };
            await _repository.SaveAsync(transaction);
        }

        private void AddValidation()
        {
            _amount = new ValidatableObject<decimal>();
            _amount.Validations.Add(new NonNegativeRule { ValidationMessage = "Please enter amount greater than zero." });
        }
    }
}
