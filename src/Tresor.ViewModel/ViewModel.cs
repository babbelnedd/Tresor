namespace Tresor.ViewModel
{
    using System;

    using Cinch;

    using Tresor.Contracts.Utilities;
    using Tresor.Framework.MVVM;
    using Tresor.Utilities;
    using Tresor.Utilities.EventArgs;

    /// <summary>Basisklasse für ViewModels.</summary>
    public abstract class ViewModel : NotifyPropertyChanged
    {
        #region Öffentliche Eigenschaften

        /// <summary>Holt die Kommandostruktru zum Öffnen eines Tabs.</summary>
        public SimpleCommand<object, SCommandArgs> OpenTabCommand { get; private set; }

        #endregion

        #region Konstruktoren und Destruktoren

        /// <summary> Initialisiert eine neue Instanz der <see cref="ViewModel"/> Klasse. </summary>
        public ViewModel()
        {
            InitCommands();
        }

        #endregion

        #region Öffentliche Ereignisse

        /// <summary>Ereignis das ausgelöst wird, wenn ein neuer Tab geöffnet werden soll.</summary>
        public virtual event EventHandler<OpenTabRequestedEventArgs> OpenTabRequested;

        #endregion

        #region Methoden

        /// <summary>Initialisiert alle Kommandos.</summary>
        protected virtual void InitCommands()
        {
            OpenTabCommand = new SimpleCommand<object, SCommandArgs>(OpenTab);
        }

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

        protected virtual void OpenTab(SCommandArgs arguments)
        {
            var obj = arguments.CommandParameter;

            if (obj is IPassword)
            {
                OpenTab((IPassword)obj);
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