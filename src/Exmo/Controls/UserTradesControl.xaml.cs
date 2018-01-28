namespace Exmo.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Threading;
    using System.Threading.Tasks;

    using Windows.ApplicationModel.Background;
    using Windows.System;

    using Exmo.Core.Api.CryptoCompare;
    using Exmo.Core.Api.CryptoCompare.Models;
    using Exmo.Core.Models.Enums;

    using Windows.UI.Core;
    using Windows.UI.Xaml;

    using Exmo.Core.Api.Exmo;
    using Exmo.Core.Api.Exmo.Base;
    using Exmo.Core.Api.Exmo.Models;
    using Exmo.ViewModels;

    public sealed partial class UserTradesControl
    {
        #region Fields

        public static readonly DependencyProperty CurrencyPairProperty = DependencyProperty.Register(
            "CurrencyPair",
            typeof(string),
            typeof(UserTradesControl),
            new PropertyMetadata(null, PropertyChangedCallback));

        public static readonly DependencyProperty UserCurrencyTradesProperty = DependencyProperty.Register(
            "UserCurrencyTrades",
            typeof(UserTradesViewModel),
            typeof(UserTradesControl),
            new PropertyMetadata(null));

        /// <summary>
        ///     The cancellation.
        /// </summary>
        private CancellationTokenSource cancellation;

        #endregion

        #region Constructors

        public UserTradesControl()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Properties

        public MainViewModel ViewModel { get; private set; } = MainViewModel.Instance;

        public UserTradesViewModel UserCurrencyTrades
        {
            get => (UserTradesViewModel)this.GetValue(UserCurrencyTradesProperty);

            set => this.SetValue(UserCurrencyTradesProperty, value);
        }

        public string CurrencyPair
        {
            get => (string)this.GetValue(CurrencyPairProperty);

            set => this.SetValue(CurrencyPairProperty, value);
        }

        #endregion

        #region Public Methods

        public async Task Refresh()
        {
            if (string.IsNullOrEmpty(this.CurrencyPair))
            {
                return;
            }

            this.cancellation?.Cancel();
            this.cancellation = new CancellationTokenSource();

            try
            {
                var pair = this.CurrencyPair;
                await this.ViewModel.RefreshTrades(this.cancellation.Token, pair).ConfigureAwait(false);
                var trade = this.ViewModel.UserTrades[pair];


                await this.Dispatcher.RunAsync(
                    CoreDispatcherPriority.Normal,
                    () =>
                        {
                            if (trade.CurrencyPair != this.UserCurrencyTrades?.CurrencyPair)
                            {
                                this.UserCurrencyTrades = trade;
                            }
                        });

            }
            catch (OperationCanceledException e)
            {
                Console.WriteLine(e);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        ///     The view model property changed callback.
        /// </summary>
        /// <param name="d">
        ///     The d.
        /// </param>
        /// <param name="e">
        ///     The e.
        /// </param>
        private static async void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as UserTradesControl;

            if (control != null)
            {
                await control.Refresh().ConfigureAwait(false);
            }
        }

        protected override void OnUnload()
        {
            //this.Bindings.StopTracking();

            this.UserCurrencyTrades = null;
            this.ViewModel = null;

            base.OnUnload();
        }

        #endregion
    }
}