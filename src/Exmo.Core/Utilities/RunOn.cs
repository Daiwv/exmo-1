// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RunOn.cs" company="MaxN">
//   Copyright © 2017 MaxN. All rights reserved.
// </copyright>
// <summary>
//   The UI thread dispatch helper class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Exmo.Core.Utilities
{
    using System;
    using System.Runtime.InteropServices.WindowsRuntime;
    using System.Threading.Tasks;

    using Windows.ApplicationModel.Core;
    using Windows.Foundation;
    using Windows.UI.Core;

    /// <summary>
    /// The UI thread dispatch helper class.
    /// </summary>
    public static class RunOn
    {
        /// <summary>
        /// Runs action on UI thread.
        /// </summary>
        /// <param name="onUiThreadDelegate">
        /// The UI thread delegate.
        /// </param>
        public static async void ApplicationView(DispatchedHandler onUiThreadDelegate)
        {
            if (onUiThreadDelegate == null)
            {
                return;
            }

            var dispatcher = CoreApplication.MainView.CoreWindow.Dispatcher;

            if (dispatcher.HasThreadAccess)
            {
                onUiThreadDelegate();
            }
            else
            {
                await dispatcher.RunAsync(CoreDispatcherPriority.Normal, onUiThreadDelegate);
            }
        }

        /// <summary>
        /// Runs action on UI thread.
        /// </summary>
        /// <param name="onUiThreadDelegate">
        /// The UI thread delegate.
        /// </param>
        /// <returns>
        /// The <see cref="IAsyncAction"/>.
        /// </returns>
        public static IAsyncAction ApplicationViewAsync(DispatchedHandler onUiThreadDelegate)
        {
            return CoreApplication.MainView.CoreWindow
                .Dispatcher.RunAsync(
                    CoreDispatcherPriority.Normal,
                    () =>
                        {
                            onUiThreadDelegate?.Invoke();
                        });
        }

        /// <summary>
        /// Runs action on UI thread.
        /// </summary>
        /// <param name="onUiThreadDelegate">
        /// The UI thread delegate.
        /// </param>
        /// <param name="runOnUi">
        /// The run On Ui.
        /// </param>
        /// <returns>
        /// The <see cref="IAsyncAction"/>.
        /// </returns>
        public static IAsyncAction ApplicationViewAsyncIfRequired(DispatchedHandler onUiThreadDelegate, bool runOnUi)
        {
            return runOnUi ? 
                ApplicationViewAsync(onUiThreadDelegate) : 
                AsyncInfo.Run(token => Task.Run(() => onUiThreadDelegate?.Invoke(), token));
        }

        /// <summary>
        /// Runs action on UI thread.
        /// </summary>
        /// <param name="onUiThreadDelegate">
        /// The UI thread delegate.
        /// </param>
        /// <param name="priority">
        /// Specifies the priority for event dispatch.
        /// </param>
        /// <returns>
        /// The <see cref="IAsyncAction"/>.
        /// </returns>
        public static IAsyncAction ApplicationViewAsync(DispatchedHandler onUiThreadDelegate, CoreDispatcherPriority priority)
        {
            return CoreApplication.MainView.CoreWindow
                .Dispatcher.RunAsync(
                    priority,
                    () =>
                        {
                            onUiThreadDelegate?.Invoke();
                        });
        }

        /// <summary>
        /// Runs action on UI thread.
        /// </summary>
        /// <param name="onUiThreadDelegate">
        /// The UI thread delegate.
        /// </param>
        public static async void CurrentView(DispatchedHandler onUiThreadDelegate)
        {
            if (onUiThreadDelegate == null)
            {
                return;
            }

            var dispatcher = CoreApplication.GetCurrentView().Dispatcher;

            if (dispatcher.HasThreadAccess)
            {
                onUiThreadDelegate();
            }
            else
            {
                await dispatcher.RunAsync(CoreDispatcherPriority.Normal, onUiThreadDelegate);
            }
        }

        /// <summary>
        /// Runs action on UI thread.
        /// </summary>
        /// <param name="onUiThreadDelegate">
        /// The UI thread delegate.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public static IAsyncAction CurrentViewAsync(DispatchedHandler onUiThreadDelegate)
        {
            return CoreApplication.GetCurrentView()
                .Dispatcher.RunAsync(
                    CoreDispatcherPriority.Normal,
                    () =>
                        {
                        onUiThreadDelegate?.Invoke();
                    });
        }
    }
}