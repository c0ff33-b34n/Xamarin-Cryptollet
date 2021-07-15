﻿using Cryptollet.Common.Base;
using Cryptollet.Common.Controllers;
using Cryptollet.Common.Models;
using Microcharts;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            var assets = await _walletController.GetCoins();
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
    }
}
