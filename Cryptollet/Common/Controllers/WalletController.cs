﻿using Cryptollet.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Cryptollet.Common.Controllers
{
    public interface IWalletController
    {
        Task<List<Coin>> GetCoins(bool forceReload = false);
        Task<List<Transaction>> GetTransactions(bool forceReload = false);
    }
    public class WalletController : IWalletController
    {
        private List<Coin> _defaultAssets = new List<Coin>
        {
                new Coin
                {
                    Name = "Bitcoin",
                    Amount = 1,
                    Symbol = "BTC",
                    DollarValue = 9500
                },
                new Coin
                {
                    Name = "Ethereum",
                    Amount = 2,
                    Symbol = "ETH",
                    DollarValue = 300
                },
                new Coin
                {
                    Name = "Litecoin",
                    Amount = 3,
                    Symbol = "LTC",
                    DollarValue = 150
                },
        };

        public Task<List<Coin>> GetCoins(bool forceReload = false)
        {
            return Task.FromResult(_defaultAssets);
        }

        public Task<List<Transaction>> GetTransactions(bool forceReload = false)
        {
            return Task.FromResult(new List<Transaction>
            {
                new Transaction
                {
                    Amount = 1,
                    DollarValue = 9500,
                    Status = Constants.TRANSACTION_DEPOSITED,
                    StatusImageSource = Constants.TRANSACTION_DEPOSITED_IMAGE,
                    Symbol = "BTC",
                    TransactionDate = DateTime.Now
                },
                new Transaction
                {
                    Amount = 2,
                    DollarValue = 600,
                    Status = Constants.TRANSACTION_DEPOSITED,
                    StatusImageSource = Constants.TRANSACTION_DEPOSITED_IMAGE,
                    Symbol = "ETM",
                    TransactionDate = DateTime.Now
                },
                new Transaction
                {
                    Amount = 3,
                    DollarValue = 150,
                    Status = Constants.TRANSACTION_DEPOSITED,
                    StatusImageSource = Constants.TRANSACTION_DEPOSITED_IMAGE,
                    Symbol = "LTC",
                    TransactionDate = DateTime.Now
                },
                new Transaction
                {
                    Amount = 1,
                    DollarValue = 40,
                    Status = Constants.TRANSACTION_WITHDRAWN,
                    StatusImageSource = Constants.TRANSACTION_WITHDRAWN_IMAGE,
                    Symbol = "LTC",
                    TransactionDate = DateTime.Now
                }
            });
        }
    }
}
