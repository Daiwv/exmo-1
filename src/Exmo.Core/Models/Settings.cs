// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Settings.cs" company="MaxN">
//   Copyright © 2017 MaxN. All rights reserved.
// </copyright>
// <summary>
//   The settings.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Exmo.Core.Models
{
    #region

    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    using Exmo.Core.Models.Base;

    using Windows.Storage;

    #endregion

    /// <summary>
    /// The settings.
    /// </summary>
    public class Settings : ObservableObject
    {
        #region Private Fields

        /// <summary>
        /// The sync root.
        /// </summary>
        private static readonly object SyncRoot = new object();

        /// <summary>
        /// The instance.
        /// </summary>
        private static volatile Settings instance;
        
        #endregion

        #region Constructors

        /// <summary>
        /// Prevents a default instance of the <see cref="Settings"/> class from being created.
        /// </summary>
        private Settings()
        {   
        }

        #endregion

        #region Public Propertys

        /// <summary>
        /// Gets the instance.
        /// </summary>
        public static Settings Instance
        {
            get
            {
                if (instance != null)
                {
                    return instance;
                }

                lock (SyncRoot)
                {
                    if (instance == null)
                    {
                        instance = new Settings();
                    }
                }

                return instance;
            }
        }


        public string ExmoApiKey
        {
            get
            {
                return this.Get(string.Empty);
            }

            set
            {
                this.Set(value);
            }
        }

        public string ExmoApiSecret
        {
            get
            {
                return this.Get(string.Empty);
            }

            set
            {
                this.Set(value);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Updates the property and raises the changed event, but only if the new value does not equal the old value. 
        /// </summary>
        /// <param name="newValue">
        /// The new value. 
        /// </param>
        /// <param name="propertyName">
        /// The property name as lambda. 
        /// </param>
        /// <returns>
        /// True if the property has changed. 
        /// </returns>
        public bool Set<T>(T newValue, [CallerMemberName] string propertyName = null)
        {
            var oldValue = ApplicationData.Current.LocalSettings.Values[propertyName];

            if (Equals(oldValue, newValue))
            {
                return false;
            }

            ApplicationData.Current.LocalSettings.Values[propertyName] = newValue;
            this.RaisePropertyChanged(new PropertyChangedEventArgs(propertyName));

            return true;
        }

        /// <summary>
        /// Gets the property value.
        /// </summary>
        /// <param name="defaultValue">
        /// The default value for the case when settings key not exist.
        /// </param>
        /// <param name="propertyName">
        /// The property name.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        /// The <see cref="T"/>.
        /// </returns>
        public T Get<T>(T defaultValue, [CallerMemberName] string propertyName = null)
        {
            if (!ApplicationData.Current.LocalSettings.Values.ContainsKey(propertyName))
            {
                ApplicationData.Current.LocalSettings.Values[propertyName] = defaultValue;
            }

            return (T)ApplicationData.Current.LocalSettings.Values[propertyName];
        }

        #endregion
    }
}