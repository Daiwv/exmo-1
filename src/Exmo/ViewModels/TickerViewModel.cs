namespace Exmo.ViewModels
{
    using System;
    using System.Runtime.CompilerServices;

    using Windows.Storage;

    using Exmo.Core.Models.Base;

    public class TickerViewModel : ObservableObject
    {
        #region Fields

        private decimal avg;

        private decimal buyPrice;

        private decimal high;

        private decimal lastTrade;

        private decimal low;

        private decimal sellPrice;

        private DateTime updated;

        private decimal vol;

        private decimal volCurr;

        private decimal closeBuyPrice;

        #endregion

        #region Properties

        public int OrderPosition
        {
            get
            {
                return this.GetLocal(int.MaxValue);
            }

            set
            {
                this.SetLocal(value);
            }
        }

        public bool IsVisible
        {
            get
            {
                return this.GetLocal(true);
            }

            set
            {
                this.SetLocal(value);
            }
        }

        /// <summary>
        ///     avg - средняя цена сделки за 24 часа
        /// </summary>
        public decimal Avg
        {
            get => this.avg;
            set => this.Set(ref this.avg, value);
        }

        /// <summary>
        ///     buy_price - текущая максимальная цена покупки
        /// </summary>
        public decimal BuyPrice
        {
            get => this.buyPrice;
            set
            {
                if (this.Set(ref this.buyPrice, value))
                {
                    this.RaisePropertyChanged("PriceChange");
                }
            }
        }

        /// <summary>
        ///     Gets or sets the currency pair.
        /// </summary>
        public string CurrencyPair { get; set; }

        public decimal PriceChange
        {
            get
            {
                decimal priceChange = 0;

                if (this.closeBuyPrice != 0 && this.closeBuyPrice != this.buyPrice)
                {
                    priceChange = (this.buyPrice - this.closeBuyPrice) / (this.closeBuyPrice / 100);
                }

                return priceChange;
            }
        }

        public decimal CloseBuyPrice
        {
            get
            {
                return this.closeBuyPrice;
            }
            set
            {
                if (this.Set(ref this.closeBuyPrice, value))
                {
                    this.RaisePropertyChanged("PriceChange");
                }
            }
        }

        /// <summary>
        ///     high - максимальная цена сделки за 24 часа
        /// </summary>
        public decimal High
        {
            get => this.high;
            set
            {
                this.Set(ref this.high, value);
            }
        }

        /// <summary>
        ///     last_trade - цена последней сделки
        /// </summary>
        public decimal LastTrade
        {
            get => this.lastTrade;
            set => this.Set(ref this.lastTrade, value);
        }

        /// <summary>
        ///     low - минимальная цена сделки за 24 часа
        /// </summary>
        public decimal Low
        {
            get => this.low;
            set => this.Set(ref this.low, value);
        }

        public decimal PrevSellPrice { get; set; }

        /// <summary>
        ///     sell_price - текущая минимальная цена продажи
        /// </summary>
        public decimal SellPrice
        {
            get => this.sellPrice;
            set
            {
                this.PrevSellPrice = this.sellPrice;
                this.Set(ref this.sellPrice, value);
            }
        }

        /// <summary>
        ///     updated - дата и время обновления данных
        /// </summary>
        public DateTime Updated
        {
            get => this.updated;
            set => this.Set(ref this.updated, value);
        }

        /// <summary>
        ///     vol - объем всех сделок за 24 часа
        /// </summary>
        public decimal Vol
        {
            get => this.vol;
            set => this.Set(ref this.vol, value);
        }

        /// <summary>
        ///     vol_curr - сумма всех сделок за 24 часа
        /// </summary>
        public decimal VolCurr
        {
            get => this.volCurr;
            set => this.Set(ref this.volCurr, value);
        }

        #endregion

        #region Private Methods

        //// <summary>
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
            var composite = (ApplicationDataCompositeValue)ApplicationData.Current.LocalSettings.Values[this.CurrencyPair];

            if (composite == null)
            {
                composite = new ApplicationDataCompositeValue();
            }

            if (composite.ContainsKey(propertyName))
            {
                var oldValue = composite[propertyName];

                if (Equals(oldValue, newValue))
                {
                    return false;
                }
            }

            composite[propertyName] = newValue;
            ApplicationData.Current.LocalSettings.Values[this.CurrencyPair] = composite;

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

            var composite = (ApplicationDataCompositeValue)ApplicationData.Current.LocalSettings.Values[this.CurrencyPair];

            if (composite == null || !composite.ContainsKey(propertyName))
            {
                return defaultValue;
            }

            return (T)composite[propertyName];
        }

        #endregion
    }
}