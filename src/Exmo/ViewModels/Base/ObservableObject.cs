// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObservableObject.cs" company="MaxN">
//   Copyright © 2017 MaxN. All rights reserved.
// </copyright>
// <summary>
//   Provides an implementation of the  interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Exmo.ViewModels.Base
{
    #region

    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Runtime.Serialization;

    #endregion

    /// <summary>Provides an implementation of the <see cref="INotifyPropertyChanged" /> interface. </summary>
    [DataContract]
    public class ObservableObject : INotifyPropertyChanged
    {
        #region Public Events

        /// <summary>Occurs when a property value changes. </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Public Methods and Operators

        /// <summary>Raises the property changed event for all properties (string.Empty). </summary>
        public void RaiseAllPropertiesChanged()
        {
            this.RaisePropertyChanged(new PropertyChangedEventArgs(string.Empty));
        }

        /// <summary>
        /// Raises the property changed event. 
        /// </summary>
        /// <param name="propertyName">
        /// The property name. 
        /// </param>
        public void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.RaisePropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Updates the property and raises the changed event, but only if the new value does not equal the old value. 
        /// </summary>
        /// <param name="oldValue">
        /// A reference to the backing field of the property. 
        /// </param>
        /// <param name="newValue">
        /// The new value. 
        /// </param>
        /// <param name="propertyName">
        /// The property name as lambda. 
        /// </param>
        /// <returns>
        /// True if the property has changed. 
        /// </returns>
        public bool Set<T>(ref T oldValue, T newValue, [CallerMemberName] string propertyName = null)
        {
            return this.Set(propertyName, ref oldValue, newValue);
        }

        /// <summary>
        /// Updates the property and raises the changed event, but only if the new value does not equal the old value. 
        /// </summary>
        /// <param name="propertyName">
        /// The property name as lambda. 
        /// </param>
        /// <param name="oldValue">
        /// A reference to the backing field of the property. 
        /// </param>
        /// <param name="newValue">
        /// The new value. 
        /// </param>
        /// <returns>
        /// True if the property has changed. 
        /// </returns>
        public virtual bool Set<T>(string propertyName, ref T oldValue, T newValue)
        {
            if (Equals(oldValue, newValue))
            {
                return false;
            }

            oldValue = newValue;
            this.RaisePropertyChanged(new PropertyChangedEventArgs(propertyName));
            return true;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Raises the property changed event. 
        /// </summary>
        /// <param name="args">
        /// The arguments. 
        /// </param>
        protected virtual void RaisePropertyChanged(PropertyChangedEventArgs args)
        {
            var copy = this.PropertyChanged;
            copy?.Invoke(this, args);
        }

        #endregion
    }
}