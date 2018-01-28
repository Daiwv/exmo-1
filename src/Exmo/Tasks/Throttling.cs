namespace Exmo.Tasks
{
    #region

    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using Windows.ApplicationModel.Background;
    using Windows.Storage;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;

    using Windows.UI.Notifications;

    using Exmo.Core.Api;
    using Exmo.Core.Api.Exmo;
    using Exmo.ViewModels;

    using Microsoft.Toolkit.Uwp.Notifications;

    #endregion
    
    public sealed class Throttling : IBackgroundTask
    {
        #region Fields

        /// <summary>
        /// The cancel token source.
        /// </summary>
        private readonly CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
        
        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The run.
        /// </summary>
        /// <param name="taskInstance">
        /// The task instance.
        /// </param>
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            var deferral = taskInstance.GetDeferral();
            taskInstance.Canceled += this.OnCanceled;

            try
            {
                do
                {
                    try
                    {
                        var api = new ExmoApi();
                        var ticker = await api.GetTicker().ConfigureAwait(false);

                        //ticker.SaveTickerResults();
                       
                        await MainViewModel.Instance.UpdateTicker(ticker);

                        Debug.WriteLine(ticker["BTC_USD"], "Info");
                    }
                    catch (OperationCanceledException)
                    {
                        throw;
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }
                                  
                    //var content = new ToastContent()
                    //{                                                  
                    //    Visual = new ToastVisual()
                    //    {
                    //        BindingGeneric = new ToastBindingGeneric()
                    //        {
                    //            Children =
                    //                {
                    //                    new AdaptiveText()
                    //                    {
                    //                        Text = "Adaptive Tiles Meeting",
                    //                        HintMaxLines = 1
                    //                    },
                    //                    new AdaptiveText() { Text = DateTime.Now.ToString(CultureInfo.InvariantCulture) }
                    //                }
                    //        }
                    //    }
                    //};

                    //var toast = new ToastNotification(content.GetXml());

                    //ToastNotificationManager.CreateToastNotifier().Show(toast);

                    await Task.Delay(5000, this.cancelTokenSource.Token);
                }
                while (true);
            }
            catch (OperationCanceledException ex)
            {
            }
            catch (Exception ex)
            {
            }
            finally
            {
                deferral.Complete();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// The on canceled.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="reason">
        /// The reason.
        /// </param>
        private void OnCanceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
        {
            var content = new ToastContent()
            {
                Visual = new ToastVisual()
                {
                    BindingGeneric = new ToastBindingGeneric()
                    {
                        Children =
                            {
                                new AdaptiveText()
                                {
                                    Text = $"Warning: Throttling background task was canceled! Reason: {reason}"
                                }
                            }
                    }
                }
            };

            var toast = new ToastNotification(content.GetXml());

            ToastNotificationManager.CreateToastNotifier().Show(toast);

            Debug.WriteLine($"Warning: Throttling background task was canceled! Reason: {reason}");
   
            this.cancelTokenSource.Cancel();
        }

        #endregion
    }
}