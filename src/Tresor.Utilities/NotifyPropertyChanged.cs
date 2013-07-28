namespace Tresor.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq.Expressions;
    using System.Runtime.CompilerServices;

    /// <summary>Implementierung der INotifyPropertyChanged Schnittstelle.</summary>
    public class NotifyPropertyChanged : INotifyPropertyChanged
    {
        #region Public Events

        /// <summary>Ereignis, dass ausgelöst wird wenn sich eine Eigenschaft geändert hat.</summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Methods

        /// <summary>Löst das PropertyChanged Ereignis aus.</summary>
        /// <param name="propertyName">Der Name der Eigenschaft die sich geändert hat. Optional.</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}