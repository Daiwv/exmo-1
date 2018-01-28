namespace Exmo.Controls
{
    using System;
    using System.Collections.ObjectModel;
    using System.Threading;
    using System.Threading.Tasks;

    using Exmo.Core.Api.CryptoCompare;
    using Exmo.Core.Api.CryptoCompare.Models;
    using Exmo.Core.Models.Enums;

    using Windows.UI.Core;
    using Windows.UI.Xaml;

    using Syncfusion.UI.Xaml.Charts;

    public sealed partial class ChartControl
    {
        #region Fields

        public static readonly DependencyProperty LastRefreshTimeProperty = DependencyProperty.Register(
            "LastRefreshTime",
            typeof(DateTime?),
            typeof(ChartControl),
            new PropertyMetadata(null));

        public static readonly DependencyProperty ChartTimeFrameProperty = DependencyProperty.Register(
            "ChartTimeFrame",
            typeof(ChartTimeFrame),
            typeof(ChartControl),
            new PropertyMetadata(ChartTimeFrame.OneDay, PropertyChangedCallback));

        public static readonly DependencyProperty CurrencyPairProperty = DependencyProperty.Register(
            "CurrencyPair",
            typeof(string),
            typeof(ChartControl),
            new PropertyMetadata(null, PropertyChangedCallback));

        public static readonly DependencyProperty PriceChartHeightProperty = DependencyProperty.Register(
            "PriceChartHeight",
            typeof(int),
            typeof(ChartControl),
            new PropertyMetadata(300));

        public static readonly DependencyProperty VolumeChartHeightProperty = DependencyProperty.Register(
            "VolumeChartHeight",
            typeof(int),
            typeof(ChartControl),
            new PropertyMetadata(100));

        /// <summary>
        ///     The cancellation.
        /// </summary>
        private CancellationTokenSource cancellation;

        #endregion

        #region Constructors

        public ChartControl()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Properties

        public ChartTimeFrame ChartTimeFrame
        {
            get => (ChartTimeFrame)this.GetValue(ChartTimeFrameProperty);

            set => this.SetValue(ChartTimeFrameProperty, value);
        }

        public string CurrencyPair
        {
            get => (string)this.GetValue(CurrencyPairProperty);

            set => this.SetValue(CurrencyPairProperty, value);
        }

        public int PriceChartHeight
        {
            get => (int)this.GetValue(PriceChartHeightProperty);

            set => this.SetValue(PriceChartHeightProperty, value);
        }

        public int VolumeChartHeight
        {
            get => (int)this.GetValue(VolumeChartHeightProperty);

            set => this.SetValue(VolumeChartHeightProperty, value);
        }

        public DateTime? LastRefreshTime
        {
            get => (DateTime?)this.GetValue(LastRefreshTimeProperty);

            set => this.SetValue(LastRefreshTimeProperty, value);
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

            var curr = this.CurrencyPair.Split('_', '/');

            var cc = new CryptoCompareApi();

            try
            {
                CryptoCompareResponse<HistoryDataItem[]> result;

                switch (this.ChartTimeFrame)
                {
                    case ChartTimeFrame.OneHour:
                        result = await cc.HistoryMinute(curr[0], curr[1], 1, 110, this.cancellation.Token).ConfigureAwait(false);
                        break;
                    case ChartTimeFrame.OneDay:
                        result = await cc.HistoryMinute(curr[0], curr[1], 5, 338, this.cancellation.Token).ConfigureAwait(false);
                        break;
                    case ChartTimeFrame.TwoWeeks:
                        result = await cc.HistoryHour(curr[0], curr[1], 1, 386, this.cancellation.Token).ConfigureAwait(false);
                        break;
                    case ChartTimeFrame.OneMonth:
                        result = await cc.HistoryHour(curr[0], curr[1], 3, 290, this.cancellation.Token).ConfigureAwait(false);
                        break;
                    case ChartTimeFrame.ThreeMonths:
                        result = await cc.HistoryHour(curr[0], curr[1], 12, 230, this.cancellation.Token).ConfigureAwait(false);
                        break;
                    case ChartTimeFrame.SixMonths:
                        result = await cc.HistoryDay(curr[0], curr[1], 1, 230, this.cancellation.Token).ConfigureAwait(false);
                        break;
                    case ChartTimeFrame.TwelveMonths:
                        result = await cc.HistoryDay(curr[0], curr[1], 1, 415, this.cancellation.Token).ConfigureAwait(false);
                        break;
                    case ChartTimeFrame.ThreeYears:
                        result = await cc.HistoryDay(curr[0], curr[1], 7, 207, this.cancellation.Token).ConfigureAwait(false);
                        break;
                    case ChartTimeFrame.AllData:
                        result = await cc.HistoryDay(curr[0], curr[1], 1, null, this.cancellation.Token, true).ConfigureAwait(false);
                        break;
                    default:
                        result = await cc.HistoryMinute(curr[0], curr[1], 5, 338, this.cancellation.Token).ConfigureAwait(false);
                        break;
                }

                await this.Dispatcher.RunAsync(
                    CoreDispatcherPriority.Normal,
                    () =>
                        {
                            this.SalesDateTimeAxis.Visibility = Visibility.Collapsed;
                            this.SalesSeries.ItemsSource = result.Data;
                            ApplyTimeFrameSettings(this.SalesDateTimeAxis, this.ChartTimeFrame);
                            this.SalesDateTimeAxis.Visibility = Visibility.Visible;

                            this.VolumeTimeAxis.Visibility = Visibility.Collapsed;
                            this.VolumeSeries.ItemsSource = result.Data;
                            ApplyTimeFrameSettings(this.VolumeTimeAxis, this.ChartTimeFrame);
                            this.VolumeTimeAxis.Visibility = Visibility.Visible;

                            this.LastRefreshTime = DateTime.Now;
                        });
            }
            catch (OperationCanceledException e)
            {
                Console.WriteLine(e);
            }
        }

        #endregion

        #region Private Methods

        public static void ApplyTimeFrameSettings(DateTimeAxis axis, ChartTimeFrame timeFrame)
        {
            switch (timeFrame)
            {
                case ChartTimeFrame.OneHour:
                    axis.Interval = 10;
                    axis.IntervalType = DateTimeIntervalType.Minutes;
                    axis.LabelFormat = "HH:mm";
                    break;
                case ChartTimeFrame.OneDay:
                    axis.Interval = 1;
                    axis.IntervalType = DateTimeIntervalType.Hours;
                    axis.LabelFormat = "HH:mm";
                    break;
                case ChartTimeFrame.TwoWeeks:
                    axis.Interval = 2;
                    axis.IntervalType = DateTimeIntervalType.Days;
                    axis.LabelFormat = "MMM dd";
                    break;
                case ChartTimeFrame.OneMonth:
                    axis.Interval = 3;
                    axis.IntervalType = DateTimeIntervalType.Days;
                    axis.LabelFormat = "MMM dd";
                    break;
                case ChartTimeFrame.ThreeMonths:
                    axis.Interval = 7;
                    axis.IntervalType = DateTimeIntervalType.Days;
                    axis.LabelFormat = "MMM dd";
                    break;
                case ChartTimeFrame.SixMonths:
                    axis.Interval = 1;
                    axis.IntervalType = DateTimeIntervalType.Months;
                    axis.LabelFormat = "MMM yyyy";
                    break;
                case ChartTimeFrame.TwelveMonths:
                    axis.Interval = 3;
                    axis.IntervalType = DateTimeIntervalType.Months;
                    axis.LabelFormat = "MMM yyyy";
                    break;
                case ChartTimeFrame.ThreeYears:
                    axis.Interval = 6;
                    axis.IntervalType = DateTimeIntervalType.Months;
                    axis.LabelFormat = "MMM yyyy";
                    break;
                case ChartTimeFrame.AllData:
                    axis.Interval = 12;
                    axis.IntervalType = DateTimeIntervalType.Months;
                    axis.LabelFormat = "MMM yyyy";
                    break;
                default:
                    axis.Interval = 1;
                    axis.IntervalType = DateTimeIntervalType.Hours;
                    axis.LabelFormat = "HH:mm";
                    break;
            }
        }

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
            var control = d as ChartControl;
            
            if (control != null)
            {
                await control.Refresh().ConfigureAwait(false);
            }
        }

        protected override void OnUnload()
        {
            this.VolumeSeries.ItemsSource = null;
            this.SalesSeries.ItemsSource = null;

            this.Bindings.StopTracking();
            base.OnUnload();
        }
        
        #endregion
    }
}