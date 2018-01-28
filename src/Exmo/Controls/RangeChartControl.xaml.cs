namespace Exmo.Controls
{
    using System;
    using System.ComponentModel;
    using System.Threading.Tasks;

    using Windows.UI.Xaml;

    using Exmo.Core.Api.CryptoCompare;
    using Exmo.ViewModels;

    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Controls.Primitives;

    using Exmo.Core.Models.Enums;

    public sealed partial class RangeChartControl
    {
        #region Constructors

        public RangeChartControl()
        {
            this.InitializeComponent();
        }

        #endregion
        
        #region Properties

        public MainViewModel ViewModel { get; private set; } = MainViewModel.Instance;

        #endregion

        #region Private Methods
        

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            this.ViewModel.ChartTimeFrame = (ChartTimeFrame)Enum.Parse(typeof(ChartTimeFrame), (sender as ToggleButton).Tag.ToString());
        }

        #endregion

        private async void Refresh_OnClick(object sender, RoutedEventArgs e)
        {
            await this.ChartControl.Refresh().ConfigureAwait(false);
        }

        protected override void OnUnload()
        {
            this.Bindings.StopTracking();
            this.ViewModel = null;

            base.OnUnload();
        }
    }
}