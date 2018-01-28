using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Exmo
{
    using System.Diagnostics;

    using Windows.ApplicationModel;
    using Windows.ApplicationModel.Background;
    using Windows.ApplicationModel.Core;

    using Exmo.Common;

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Shell : Page
    {
        public Shell()
        {
            this.InitializeComponent();

            CoreApplicationViewTitleBar titleBar = CoreApplication.GetCurrentView().TitleBar;
            titleBar.LayoutMetricsChanged += TitleBar_LayoutMetricsChanged;

            this.Loaded += Shell_Loaded;
        }

        private async void Shell_Loaded(object sender, RoutedEventArgs e)
        {
            var startup = await StartupTask.GetAsync("{F4E0CA22-ABB1-4310-AC20-0F72BF3A869B}");
            var state = startup.State;
            if (state == StartupTaskState.Disabled)
            {
                state = await startup.RequestEnableAsync();
            }

            Debug.WriteLine($"Info: Startup task state : {state}");

            var task = await BackgroundTasksHelper.RegisterThrottling();
            var trigger = task?.Trigger as ApplicationTrigger;

            var result = await trigger.RequestAsync();
        }

        private void TitleBar_LayoutMetricsChanged(CoreApplicationViewTitleBar sender, object args)
        {
            AppTitle.Margin = new Thickness(CoreApplication.GetCurrentView().TitleBar.SystemOverlayLeftInset + 12, 8, 0, 0);
        }

        private void NavView_Loaded(object sender, RoutedEventArgs e)
        {
            // set the initial SelectedItem 
            foreach (NavigationViewItemBase item in NavView.MenuItems)
            {
                if (item is NavigationViewItem && item.Tag.ToString() == "home")
                {
                    this.NavView.SelectedItem = item;
                    break;
                }
            }
        }

        private void NavView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.IsSettingsSelected)
            {
                ContentFrame.Navigate(typeof(SettingsPage));
            }
            else
            {

                NavigationViewItem item = args.SelectedItem as NavigationViewItem;

                switch (item.Tag)
                {
                    case "home":
                        ContentFrame.Navigate(typeof(MainPage));
                        break;

                    case "wallets":
                        ContentFrame.Navigate(typeof(WalletsPage));
                        break;
                }
            }
        }
    }
}
