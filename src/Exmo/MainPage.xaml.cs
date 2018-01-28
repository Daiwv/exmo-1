namespace Exmo
{
    using System.ComponentModel;
    using System.Diagnostics;

    using Exmo.Core.Api.Exmo;
    using Exmo.ViewModels;

    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;

    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage
    {
        #region Constructors

        public MainPage()
        {
            this.InitializeComponent();

            this.Loaded += this.MainPage_Loaded;

            MainViewModel.Instance.PropertyChanged += this.Instance_PropertyChanged;
        }

        #endregion

        #region Properties

        public MainViewModel ViewModel { get; private set; } = MainViewModel.Instance;

        #endregion

        #region Private Methods

        private void Instance_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Debug.WriteLine(e.PropertyName);
        }

        private async void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            var api = new ExmoApi();

            // var cur = await api.GetCurrencies();
            // var paits = await api.GetPairSettings();
            // var ticker = await api.GetTicker();
            // var ordersBook = await api.OrderBook(null, "BTC_USD");
            // var trades = await api.Trades("BTC_USD");
            // await api.ChartData("BTC_USD");

            // LoadChartContents();
        }


        protected override void OnUnload()
        {
            this.Bindings.StopTracking();

            this.Loaded -= this.MainPage_Loaded;
            MainViewModel.Instance.PropertyChanged -= this.Instance_PropertyChanged;

            this.ViewModel = null;

            base.OnUnload();
        }

        #endregion
    }
}