namespace Exmo.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Threading.Tasks;

    using Exmo.Core.Api.Exmo.Models;
    using Exmo.Core.Models.Enums;
    using Exmo.Core.Utilities;
    using Exmo.ViewModels.Base;

    using Windows.Foundation;
    using Windows.Storage;
    using Windows.UI.Core;

    using Exmo.Core.Api.Exmo;

    public class MainViewModel : ObservableObject
    {
        #region Private Fields

        /// <summary>
        /// The sync root.
        /// </summary>
        private static readonly object SyncRoot = new object();

        /// <summary>
        /// The instance.
        /// </summary>
        private static volatile MainViewModel instance;

        private TickerViewModel selectedTicker;

        #endregion

        #region Public Propertys

        public string SelectedCurrencyPair
        {
            get
            {
                return this.GetLocal(string.Empty);
            }

            set
            {
                this.SetLocal(value);
            }
        }

        public ChartTimeFrame ChartTimeFrame
        {
            get
            {
                return (ChartTimeFrame)this.GetLocal((int)ChartTimeFrame.OneDay);
            }

            set
            {
                this.SetLocal((int)value);
            }
        }

        public TickerViewModel SelectedTicker
        {
            get
            {
                return this.selectedTicker;
            }

            set
            {
                if (this.Set(ref this.selectedTicker, value))
                {
                    this.SelectedCurrencyPair = value.CurrencyPair;
                }
            }
        }

        public Dictionary<string, TickerViewModel> Ticker { get; set; } = new Dictionary<string, TickerViewModel>();

        public Dictionary<string, UserTradesViewModel> UserTrades { get; set; } = new Dictionary<string, UserTradesViewModel>();

        /// <summary>
        /// Gets the instance.
        /// </summary>
        public static MainViewModel Instance
        {
            get
            {
                if (instance != null)
                {
                    return instance;
                }

                lock (SyncRoot)
                {
                    if (instance == null)
                    {
                        instance = new MainViewModel();
                    }
                }

                return instance;
            }
        }

        #endregion

        #region Public Methods

        public IAsyncAction UpdateTicker(Dictionary<string, TickerItem> tickers)
        {
            return RunOn.ApplicationViewAsync(
                () =>
                    {
                        var hasAdded = false;
                        foreach (var item in tickers)
                        {
                            if (this.Ticker.ContainsKey(item.Key))
                            {
                                if (item.Value.Updated != this.Ticker[item.Key].Updated)
                                {
                                    this.Ticker[item.Key].Updated = item.Value.Updated;
                                    this.Ticker[item.Key].SellPrice = item.Value.SellPrice;
                                    this.Ticker[item.Key].LastTrade = item.Value.LastTrade;
                                    this.Ticker[item.Key].BuyPrice = item.Value.BuyPrice;
                                    this.Ticker[item.Key].Avg = item.Value.Avg;
                                    this.Ticker[item.Key].High = item.Value.High;
                                    this.Ticker[item.Key].Low = item.Value.Low;
                                    this.Ticker[item.Key].Vol = item.Value.Vol;
                                    this.Ticker[item.Key].VolCurr = item.Value.VolCurr;
                                    this.Ticker[item.Key].CloseBuyPrice = item.Value.CloseBuyPrice;
                                }
                            }
                            else
                            {
                                var tickerItem =
                                    new TickerViewModel
                                        {
                                            CurrencyPair = item.Key,
                                            Updated = item.Value.Updated,
                                            SellPrice = item.Value.SellPrice,
                                            LastTrade = item.Value.LastTrade,
                                            BuyPrice = item.Value.BuyPrice,
                                            Avg = item.Value.Avg,
                                            High = item.Value.High,
                                            Low = item.Value.Low,
                                            Vol = item.Value.Vol,
                                            VolCurr = item.Value.VolCurr,
                                            CloseBuyPrice = item.Value.CloseBuyPrice,
                                        };

                                this.Ticker.Add(item.Key, tickerItem);

                                if (tickerItem.CurrencyPair == this.SelectedCurrencyPair)
                                {
                                    this.SelectedTicker = tickerItem;
                                }

                                hasAdded = true;
                            }
                        }

                        if (hasAdded)
                        {
                            this.RaisePropertyChanged("Ticker");

                            if (this.Ticker.ContainsKey(this.SelectedCurrencyPair))
                            {

                                this.SelectedTicker = this.Ticker[this.SelectedCurrencyPair];
                            }
                        }
                    },

                CoreDispatcherPriority.Low);

        }

        public async Task RefreshTrades(CancellationToken token, params string[] pairs)
        {
            var api = new ExmoApi();

            var result = await api.UserTrades(100, 0, token, pairs).ConfigureAwait(false);

            await RunOn.ApplicationViewAsync(
                () =>
                    {
                        foreach (var resultTrade in result)
                        {
                            if (!this.UserTrades.ContainsKey(resultTrade.Key))
                            {
                                this.UserTrades.Add(
                                    resultTrade.Key,
                                    new UserTradesViewModel
                                        {
                                            Updated = DateTime.Now,
                                            CurrencyPair = resultTrade.Key,
                                            Trades = new ObservableCollection<UserTradeItem>(resultTrade.Value)
                                        });
                            }
                            else
                            {
                                for (var i = resultTrade.Value.Length - 1; i <= 0; i--)
                                {
                                    if (this.UserTrades[resultTrade.Key]
                                        .Trades.Any(p => p.TradeId != resultTrade.Value[i].TradeId))
                                    {
                                        continue;
                                    }

                                    this.UserTrades[resultTrade.Key].Trades.Insert(0, resultTrade.Value[i]);
                                }

                                this.UserTrades[resultTrade.Key].Updated = DateTime.Now;
                            }
                        }
                    },
                CoreDispatcherPriority.Normal);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Updates the property and raises the changed event, but only if the new value does not equal the old value. 
        /// </summary>
        /// <param name="newValue">
        /// The new value. 
        /// </param>
        /// <param name="propertyName">
        /// The property name as lambda. 
        /// </param>
        /// <returns>
        /// True if the property has changed. 
        /// </returns>
        private bool SetLocal<T>(T newValue, [CallerMemberName] string propertyName = null)
        {
            var oldValue = ApplicationData.Current.LocalSettings.Values[propertyName];

            if (Equals(oldValue, newValue))
            {
                return false;
            }

            ApplicationData.Current.LocalSettings.Values[propertyName] = newValue;
            this.RaisePropertyChanged(propertyName);

            return true;
        }

        /// <summary>
        /// Gets the property value.
        /// </summary>
        /// <param name="defaultValue">
        /// The default value for the case when settings key not exist.
        /// </param>
        /// <param name="propertyName">
        /// The property name.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        /// The <see cref="T"/>.
        /// </returns>
        private T GetLocal<T>(T defaultValue, [CallerMemberName] string propertyName = null)
        {
            if (!ApplicationData.Current.LocalSettings.Values.ContainsKey(propertyName))
            {
                ApplicationData.Current.LocalSettings.Values[propertyName] = defaultValue;
            }

            return (T)ApplicationData.Current.LocalSettings.Values[propertyName];
        }

        #endregion
    }
}
