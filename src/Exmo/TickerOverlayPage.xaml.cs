namespace Exmo
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;

    using Windows.ApplicationModel.Core;
    using Windows.Foundation;
    using Windows.UI.Core;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Navigation;

    using Exmo.Controls;
    using Exmo.ViewModels;

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TickerOverlayPage
    {
        #region Fields

        /// <summary>
        ///     Identifies the view model dependency property.
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
            "ViewModel",
            typeof(TickerViewModel),
            typeof(TickerControl),
            new PropertyMetadata(new TickerViewModel(), ViewModelPropertyChangedCallback));


        #endregion

        #region Constructors

        public TickerOverlayPage()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Gets or sets the control view model
        /// </summary>
        public TickerViewModel ViewModel { get; set; } = new TickerViewModel();

        #endregion

        /// <summary>
        ///     The view model property changed callback.
        /// </summary>
        /// <param name="d">
        ///     The d.
        /// </param>
        /// <param name="e">
        ///     The e.
        /// </param>
        private static void ViewModelPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var page = d as TickerOverlayPage;
            var model = e.NewValue as TickerViewModel;           
        }
        
        private async void Model_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            try
            {
                var currencyPair = string.Empty;
                await this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                    () =>
                        {
                            currencyPair = this.ViewModel.CurrencyPair;
                        });
                var ticker = MainViewModel.Instance.Ticker[currencyPair];
                await this.CloneTicker(ticker);
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception);
                throw;
            }
        }

        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            try
            {
                if (e.PropertyName == "SellPrice")
                {
                    VisualStateManager.GoToState(this, "Normal", false);
                    if (this.ViewModel.SellPrice > this.ViewModel.PrevSellPrice)
                    {
                        VisualStateManager.GoToState(this, "SalePriceUp", false);
                    }

                    if (this.ViewModel.SellPrice < this.ViewModel.PrevSellPrice)
                    {
                        VisualStateManager.GoToState(this, "SalePriceDown", false);
                    }
                }
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception);
                throw;
            }
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {
                var currencyPair = e.Parameter as string;
                this.ViewModel.CurrencyPair = currencyPair;
                this.ViewModel.PropertyChanged += ViewModel_PropertyChanged;

                var ticker = MainViewModel.Instance.Ticker[currencyPair];
                await this.CloneTicker(ticker);

                ticker.PropertyChanged += this.Model_PropertyChanged;

                base.OnNavigatedTo(e);
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception);
                throw;
            }
        }

        private IAsyncAction CloneTicker(TickerViewModel ticker)
        {
            try
            {
                var avg = ticker.Avg;
                var buyPrice = ticker.BuyPrice;
                var closeBuyPrice = ticker.CloseBuyPrice;
                var high = ticker.High;
                var isVisible = ticker.IsVisible;
                var lastTrade = ticker.LastTrade;
                var orderPosition = ticker.OrderPosition;
                var sellPrice = ticker.SellPrice;
                var updated = ticker.Updated;
                var vol = ticker.Vol;
                var volCurr = ticker.VolCurr;

                return this.Dispatcher.RunAsync(
                    CoreDispatcherPriority.Normal,
                    () =>
                        {
                            try
                            {
                                this.ViewModel.Avg = avg;
                                this.ViewModel.BuyPrice = buyPrice;
                                this.ViewModel.CloseBuyPrice = closeBuyPrice;
                                this.ViewModel.High = high;

                                this.ViewModel.IsVisible = isVisible;
                                this.ViewModel.LastTrade = lastTrade;
                                this.ViewModel.OrderPosition = orderPosition;
                                this.ViewModel.SellPrice = sellPrice;
                                this.ViewModel.Updated = updated;

                                this.ViewModel.Vol = vol;
                                this.ViewModel.VolCurr = volCurr;
                            }
                            catch (Exception e)
                            {
                                Debug.WriteLine(e);
                                throw;
                            }
                            
                        });
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception);
                throw;
            }
        }
    }
}
