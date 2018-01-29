namespace Exmo
{
    using Windows.UI.Xaml;

    using Exmo.Core.Models;
    using Exmo.ViewModels;

    public sealed partial class LoginPage
    {
        #region Constructors

        public LoginPage()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Properties

        public Settings ViewModel { get; private set; } = Settings.Instance;

        #endregion

        #region Private Methods

        protected override void OnUnload()
        {
            this.Bindings.StopTracking();

            this.ViewModel = null;

            base.OnUnload();
        }

        #endregion

        private void OnLoginClick(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.ViewModel.ExmoApiKey) && !string.IsNullOrEmpty(this.ViewModel.ExmoApiSecret))
            {
                (Application.Current as App)?.InitShell();
            }
        }
    }
}