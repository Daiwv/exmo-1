namespace Exmo.ViewModels
{
    using System;
    using System.Collections.ObjectModel;

    using Exmo.Core.Api.Exmo.Models;
    using Exmo.ViewModels.Base;

    public class UserTradesViewModel : ObservableObject
    {
        #region Fields

        private DateTime updated;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets or sets the currency pair.
        /// </summary>
        public string CurrencyPair { get; set; }

        /// <summary>
        ///     updated - дата и время обновления данных
        /// </summary>
        public DateTime Updated
        {
            get => this.updated;
            set => this.Set(ref this.updated, value);
        }

        /// <summary>
        /// Gets or sets the trades.
        /// </summary>
        public ObservableCollection<UserTradeItem> Trades { get; set; } = new ObservableCollection<UserTradeItem>();

        #endregion
    }
}