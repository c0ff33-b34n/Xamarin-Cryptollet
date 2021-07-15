﻿using Cryptollet.Common.Base;
using Cryptollet.Common.Controllers;
using Cryptollet.Common.Models;
using Microcharts;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Cryptollet.Modules.Wallet
{
    public class WalletViewModel : BaseViewModel
    {
        private IWalletController _walletController;
        public WalletViewModel(IWalletController walletController)
        {
            _walletController = walletController;
        }

        public override async Task InitializeAsync(object parameter)
        {
            var transactions = await _walletController.GetTransactions();
            LatestTransactions = new ObservableCollection<Transaction>(transactions.Take(5));
            var assets = await _walletController.GetCoins();
            Assets = new ObservableCollection<Coin>(assets.Take(3));
            BuildChart(assets);
        }

        private void BuildChart(List<Coin> assets)
        {
            var whiteColor = SKColor.Parse("#ffffff");
            List<ChartEntry> entries = new List<ChartEntry>();
            var colors = Coin.GetAvailableAssets();
            foreach (var item in assets)
            {
                entries.Add(new ChartEntry((float)item.DollarValue)
                {
                    TextColor = whiteColor,
                    ValueLabel = item.Name,
                    Color = SKColor.Parse(colors.First(x => x.Symbol == item.Symbol).HexColor)
                });
            }
            var chart = new DonutChart { Entries = entries };
            chart.BackgroundColor = whiteColor;
            PortfolioView = chart;
        }

        private Chart _portfolioView;

        public Chart PortfolioView
        {
            get => _portfolioView;
            set { SetProperty(ref _portfolioView, value); }
        }

        private int _coinsHeight;
        public int CoinsHeight
        {
            get => _coinsHeight;
            set { SetProperty(ref _coinsHeight, value); }
        }

        private ObservableCollection<Coin> _assets;
        public ObservableCollection<Coin> Assets
        {
            get => _assets;
            set
            {
                SetProperty(ref _assets, value);
                if (_assets == null)
                {
                    return;
                }
                CoinsHeight = _assets.Count * 85;
            }
        }

        private int _transactionsHeight;
        public int TransactionsHeight
        {
            get => _transactionsHeight;
            set { SetProperty(ref _transactionsHeight, value); }
        }

        private ObservableCollection<Transaction> _latestTransactions;
        public ObservableCollection<Transaction> LatestTransactions
        {
            get => _latestTransactions;
            set
            {
                SetProperty(ref _latestTransactions, value);
                if (_latestTransactions == null)
                {
                    return;
                }

                if (_latestTransactions.Count == 0)
                {
                    TransactionsHeight = 430;
                    return;
                }
                TransactionsHeight = _latestTransactions.Count * 85;
            }
        }

        public ICommand AddNewTransactionCommand { get => new Command(async () => await AddNewTransaction()); }

        private async Task AddNewTransaction()
        {
            // TODO
        }

    }
}