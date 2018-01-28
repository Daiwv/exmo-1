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
    using System.Collections.ObjectModel;

    using Exmo.ViewModels;

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SettingsPage
    {
        public SettingsPage()
        {
            this.InitializeComponent();

            this.InitViewModel();

            this.ViewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Ticker")
            {
                this.InitViewModel();
            }
        }

        #region Properties

        public MainViewModel ViewModel { get; private set; } = MainViewModel.Instance;

        public ObservableCollection<TickerViewModel> ListViewModel { get; set; } =
            new ObservableCollection<TickerViewModel>();

        #endregion

        #region Private Metods

        private void InitViewModel()
        {
            this.ListViewModel.Clear();

            if (MainViewModel.Instance.Ticker.Values.Count > 0)
            {
                foreach (var item in MainViewModel.Instance.Ticker.Values.OrderBy(p => p.OrderPosition))
                {
                    this.ListViewModel.Add(item);
                }
            }
        }

        protected override void OnUnload()
        {
            this.Bindings.StopTracking();

            this.ViewModel.PropertyChanged -= ViewModel_PropertyChanged;

            this.ViewModel = null;
            this.ListViewModel = null;

            base.OnUnload();
        }

        #endregion
    }
}
