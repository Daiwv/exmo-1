namespace Exmo.Controls
{
    using System;

    using Windows.ApplicationModel.Core;
    using Windows.UI.Core;
    using Windows.UI.ViewManagement;

    using Exmo.ViewModels;

    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Input;

    public sealed partial class TickerControl
    {
        #region Fields

        /// <summary>
        ///     Identifies the view model dependency property.
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
            "ViewModel",
            typeof(TickerViewModel),
            typeof(TickerControl),
            new PropertyMetadata(null, ViewModelPropertyChangedCallback));


        #endregion

        #region Constructors

        public TickerControl()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Gets or sets the control view model
        /// </summary>
        public TickerViewModel ViewModel
        {
            get => (TickerViewModel)this.GetValue(ViewModelProperty);

            set => this.SetValue(ViewModelProperty, value);
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
        private static void ViewModelPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var model = e.NewValue as TickerViewModel;

            if (model != null)
            {
                model.PropertyChanged += (sender, args) =>
                    {
                        VisualStateManager.GoToState(d as Control, "Normal", false);
                        if (args.PropertyName == "SellPrice")
                        {
                            if (model.SellPrice > model.PrevSellPrice)
                            {
                                VisualStateManager.GoToState(d as Control, "SalePriceUp", false);
                            }

                            if (model.SellPrice < model.PrevSellPrice)
                            {
                                VisualStateManager.GoToState(d as Control, "SalePriceDown", false);
                            }
                        }
                    };
            }
        }

        protected override void OnUnload()
        {
            this.Bindings.StopTracking();          
            base.OnUnload();
        }

        #endregion

        private void TickerControl_OnPointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if (e.Pointer.PointerDeviceType == Windows.Devices.Input.PointerDeviceType.Mouse)
            {
                var properties = e.GetCurrentPoint(this).Properties;

                if (properties.IsRightButtonPressed)
                {
                    var menu = new MenuFlyout();
                    var showInOverlay = new MenuFlyoutItem { Text = "Show in overlay" };
                    showInOverlay.Click += this.ShowInOverlay_Click;
                    menu.Items.Add(showInOverlay);
                    
                    //the code can show the flyout in your mouse click 
                    menu.ShowAt(sender as UIElement, e.GetCurrentPoint(sender as UIElement).Position);
                }
            }
        }

        private async void ShowInOverlay_Click(object sender, RoutedEventArgs e)
        {
            int compactViewId = -1;
            var currencyPair = this.ViewModel.CurrencyPair;
            await CoreApplication.CreateNewView().Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    var frame = new Frame();
                    compactViewId = ApplicationView.GetForCurrentView().Id;
                    frame.Navigate(typeof(TickerOverlayPage), currencyPair);
                    Window.Current.Content = frame;
                    Window.Current.Activate();
                    ApplicationView.GetForCurrentView().Title = currencyPair.Replace('_', '/');
                });

            var compactOptions = ViewModePreferences.CreateDefault(ApplicationViewMode.CompactOverlay);
            compactOptions.CustomSize = new Windows.Foundation.Size(280, 100);

            bool viewShown = await ApplicationViewSwitcher.TryShowAsViewModeAsync(compactViewId, ApplicationViewMode.CompactOverlay, compactOptions);
        }
    }
}