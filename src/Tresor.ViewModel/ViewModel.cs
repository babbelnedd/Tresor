namespace Tresor.ViewModel
{
    using System;

    using Tresor.Contracts.Utilities;
    using Tresor.Utilities;
    using Tresor.Utilities.EventArgs;

    /// <summary>Basisklasse für ViewModels.</summary>
    public abstract class ViewModel : NotifyPropertyChanged
    {
        #region Öffentliche Ereignisse

        /// <summary>Ereignis das ausgelöst wird, wenn ein neuer Tab geöffnet werden soll.</summary>
        public virtual event EventHandler<OpenTabRequestedEventArgs> OpenTabRequested;

        #endregion

        #region Methoden

        /// <summary>Informiert Abonnenten von <see cref="OpenTabRequested"/> darüber, dass das Ereignis ausgelöst wurde.</summary>
        /// <param name="arguments">Die Argumente, welche zu dem Ereignis gehören.</param>
        protected virtual void OnOpenTabEvent(OpenTabRequestedEventArgs arguments)
        {
            var handler = OpenTabRequested;

            if (handler != null)
            {
                handler(this, arguments);
            }
        }

        /// <summary>Öffnet einen neuen Tab.</summary>
        /// <param name="password">Das Passwort, welches dargestellt werden soll.</param>
        protected virtual void OpenTab(IPassword password)
        {
            OnOpenTabEvent(new OpenTabRequestedEventArgs(password));
        }

        #endregion
    }
}