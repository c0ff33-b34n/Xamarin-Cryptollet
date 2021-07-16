using Cryptollet.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Cryptollet.Modules.AddTransaction
{
    [QueryProperty("Id", "id")]
    public class AddTransactionViewModel : BaseViewModel
    {
        private bool _isDeposit;

        public bool IsDeposit
        {
            get => _isDeposit;
            set { SetProperty(ref _isDeposit, value); }
         }

        private string _id;
        public string Id
        {
            set
            {
                _id = Uri.UnescapeDataString(value);
            }
        }
    }
}
