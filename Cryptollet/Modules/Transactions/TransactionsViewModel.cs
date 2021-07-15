﻿using Cryptollet.Common.Base;
using Cryptollet.Common.Controllers;
using Cryptollet.Common.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Cryptollet.Modules.Transactions
{
    public class TransactionsViewModel : BaseViewModel
    {
        private IWalletController _walletController;
        private string _filter = string.Empty;

        public TransactionsViewModel(IWalletController walletController)
        {
            _walletController = walletController;
            Transactions = new ObservableCollection<Transaction>();
        }

        public override async Task InitializeAsync(object parameter)
        {
            _filter = parameter.ToString();
            await GetTransactions();
        }

        private async Task GetTransactions()
        {
            IsRefreshing = true;
            var transactions = await _walletController.GetTransactions();
            if (!string.IsNullOrEmpty(_filter))
            {
                transactions = transactions.Where(x => x.Status == _filter).ToList();
            }
            Transactions = new ObservableCollection<Transaction>(transactions);
            IsRefreshing = false;
        }

        private ObservableCollection<Transaction> _transactions;
        public ObservableCollection<Transaction> Transactions
        {
            get => _transactions;
            set { SetProperty(ref _transactions, value); }
        }

        private Transaction _selectedTransaction;
        public Transaction SelectedTransaction
        {
            get => _selectedTransaction;
            set { SetProperty(ref _selectedTransaction, value); }
        }

        private bool _isRefreshing;
        public bool IsRefreshing
        {
            get => _isRefreshing;
            set { SetProperty(ref _isRefreshing, value); }
        }

        public ICommand RefreshTransactionsCommand { get => new Command(async () => await RefreshTransactions()); }

        public ICommand TransactionSelectedCommand { get => new Command(async () => await TransactionSelected()); }

        private async Task TransactionSelected()
        {
            
        }

        private async Task RefreshTransactions()
        {
            await GetTransactions();
        }
    }
}
