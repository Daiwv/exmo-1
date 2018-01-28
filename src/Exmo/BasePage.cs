// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BaseUserControl.cs" company="MaxN">
//   Copyright © 2017 MaxN. All rights reserved.
// </copyright>
// <summary>
//   Defines the BaseControl type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Exmo
{
    #region Usings

    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;

    using Exmo.Controls.Base;

    #endregion

    /// <summary>
    /// The base control.
    /// </summary>
    public class BasePage : Page
    {
        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BasePage"/> class.
        /// </summary>
        public BasePage()
        {
            this.Unloaded += this.BasePageUnloaded;
        }

        #endregion
        
        #region Public Methods

        /// <summary>
        /// Releases control ViewModel and unsubscribes from events
        /// </summary>
        public void ReleaseResources()
        {
            this.OnUnload();
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// The on control finalized.
        /// </summary>
        protected virtual void OnUnload()
        {
        }
        
        #endregion

        #region Private Methods
        
        /// <summary>
        /// The base page unloaded.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void BasePageUnloaded(object sender, RoutedEventArgs e)
        {
            this.ReleaseResources();

            this.Unloaded -= this.BasePageUnloaded;
        }
        
        #endregion
    }
}
