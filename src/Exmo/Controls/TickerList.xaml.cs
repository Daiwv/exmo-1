

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236
namespace Exmo.Controls
{
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.Linq;

    using Windows.UI.Xaml;

    using Exmo.ViewModels;

    using Windows.UI.Xaml.Controls;

    public sealed partial class TickerList 
    {
        #region Constructors

        public TickerList()
        {
            this.InitializeComponent();

            this.InitViewModel();

            this.ViewModel.PropertyChanged += this.ViewModel_PropertyChanged;
        }

        private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Ticker")
            {
                this.InitViewModel();
                this.TickerListGridView.SelectedItem = this.ViewModel.SelectedTicker;
            }
        }

        private void ListViewModel_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                var ticker = (TickerViewModel)e.NewItems[0];
                ticker.OrderPosition = e.NewStartingIndex;
            }
        }


        #endregion

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
                foreach (var item in MainViewModel.Instance.Ticker.Values.OrderBy(p => p.OrderPosition).Where(p => p.IsVisible))
                {
                    this.ListViewModel.Add(item);
                }
            }
            

        }

        #endregion

        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.ViewModel.SelectedTicker = this.TickerListGridView.SelectedItem as TickerViewModel;
        }

        protected override void OnUnload()
        {
            this.Bindings.StopTracking();

            this.ViewModel.PropertyChanged -= ViewModel_PropertyChanged;
            this.ListViewModel.CollectionChanged -= ListViewModel_CollectionChanged;

            this.ViewModel = null;
            this.ListViewModel = null;

            base.OnUnload();
        }
    }
}