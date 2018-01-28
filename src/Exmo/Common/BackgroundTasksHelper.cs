using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exmo.Common
{
    using System.Diagnostics;

    using Windows.ApplicationModel.Background;
    using Windows.Storage;

    public static class BackgroundTasksHelper
    {
        /// <summary>
        /// Registers news mark as read task.
        /// </summary>
        /// <param name="recreateIfExist">
        /// The recreate If Exist.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public static async Task<BackgroundTaskRegistration> RegisterThrottling(bool recreateIfExist = false)
        {
            return await Register(
                       null,
                       "Throttling_ApplicationTrigger",
                       new ApplicationTrigger(),
                       null,
                       recreateIfExist);
        }

        /// <summary>
        /// Register a background task with the specified taskEntryPoint, name, trigger,and condition (optional).
        /// </summary>
        /// <param name="taskEntryPoint">
        /// Task entry point for the background task.
        /// </param>
        /// <param name="taskName">
        /// A name for the background task.
        /// </param>
        /// <param name="trigger">
        /// The trigger for the background task.
        /// </param>
        /// <param name="condition">
        /// Optional parameter. A conditional event that must be true for the task to fire.
        /// </param>
        /// <param name="recreateIfExist">
        /// Recreates task if it already registered.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public static async Task<BackgroundTaskRegistration> Register(
            string taskEntryPoint,
            string taskName,
            IBackgroundTrigger trigger,
            IBackgroundCondition[] condition,
            bool recreateIfExist = false)
        {
            var registered = BackgroundTaskRegistration.AllTasks.FirstOrDefault(cur => cur.Value.Name == taskName).Value as BackgroundTaskRegistration;

            if (registered != null && !recreateIfExist)
            {
                return registered;
            }

            if (registered != null)
            {
                if (ApplicationData.Current.LocalSettings.Values.ContainsKey(registered.TaskId.ToString()))
                {
                    ApplicationData.Current.LocalSettings.Values.Remove(registered.TaskId.ToString());
                }

                registered.Unregister(true);
            }

            var access = await BackgroundExecutionManager.RequestAccessAsync();

            var builder = new BackgroundTaskBuilder { Name = taskName };

            if (!string.IsNullOrEmpty(taskEntryPoint))
            {
                builder.TaskEntryPoint = taskEntryPoint;
            }

            builder.SetTrigger(trigger);

            if (condition != null)
            {
                foreach (var backgroundCondition in condition)
                {
                    builder.AddCondition(backgroundCondition);
                }

                builder.CancelOnConditionLoss = true;
            }

            return builder.Register();
        }

        /// <summary>
        /// Un Register a background task with the specified taskEntryPoint, name, trigger,and condition (optional).
        /// </summary>
        /// <param name="taskName">
        /// A name for the background task.
        /// </param>
        /// <returns>
        /// The <see cref="BackgroundTaskRegistration"/>.
        /// </returns>
        public static BackgroundTaskRegistration UnRegister(string taskName)
        {
            var registered = BackgroundTaskRegistration.AllTasks.FirstOrDefault(cur => cur.Value.Name == taskName).Value as BackgroundTaskRegistration;

            registered?.Unregister(true);

            return registered;
        }

        /// <summary>
        /// UnRegisters all background tasks
        /// </summary>
        public static void UnRegisterAll()
        {
            foreach (var item in BackgroundTaskRegistration.AllTasks)
            {
                var registered = item.Value as BackgroundTaskRegistration;

                registered?.Unregister(true);
            }
        }
    }
}
