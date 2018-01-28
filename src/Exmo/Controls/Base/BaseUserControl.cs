// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BaseUserControl.cs" company="MaxN">
//   Copyright © 2017 MaxN. All rights reserved.
// </copyright>
// <summary>
//   Defines the BaseControl type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Exmo.Controls.Base
{
    #region Usings

    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;

    #endregion

    /// <summary>
    /// The base control.
    /// </summary>
    public class BaseUserControl : UserControl
    {
        #region Private Fields

        /// <summary>
        ///     Identifies the release on unload dependency property.
        /// </summary>
        public static readonly DependencyProperty ReleaseOnUnloadProperty = DependencyProperty.Register(
            "ReleaseOnUnload",
            typeof(bool),
            typeof(BaseUserControl),
            new PropertyMetadata(true, ReleaseOnUnloadPropertyChangedCallback));

        #endregion

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseUserControl"/> class.
        /// </summary>
        public BaseUserControl()
        {
            this.RegisterUnloadReleaseEvent();
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the value indicating whether is control resources should be released on unload
        /// </summary>
        public bool ReleaseOnUnload
        {
            get
            {
                return (bool)this.GetValue(ReleaseOnUnloadProperty);
            }

            set
            {
                this.SetValue(ReleaseOnUnloadProperty, value);
            }
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
        /// The view model property changed callback.
        /// </summary>
        /// <param name="d">
        /// The d.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private static void ReleaseOnUnloadPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as BaseUserControl)?.RegisterUnloadReleaseEvent();
        }

        /// <summary>
        /// The base page unloaded.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void BaseControlUnloaded(object sender, RoutedEventArgs e)
        {
            this.ReleaseResources();

            this.Unloaded -= this.BaseControlUnloaded;
        }
        
        /// <summary>
        /// Registers release handler on unload event.
        /// </summary>
        private void RegisterUnloadReleaseEvent()
        {
            this.Unloaded -= this.BaseControlUnloaded;

            if (this.ReleaseOnUnload)
            {
                this.Unloaded += this.BaseControlUnloaded;
            }
        }

        #endregion
    }
}
