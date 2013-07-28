namespace Tresor.ViewModel
{
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;

    using Cinch;

    using Microsoft.Practices.ObjectBuilder2;

    using Tresor.Contracts.Utilities;
    using Tresor.Contracts.ViewModel;
    using Tresor.Framework.MVVM;
    using Tresor.Utilities.Mediator;

    /// <summary>ViewModel für die PasswordListView.</summary>
    public class PasswordListViewModel : ViewModel, IPasswordListViewModel
    {
        #region Fields

        /// <summary>Instanz des <see cref="PasswordMediator"/>.</summary>
        private readonly PasswordMediator mediator;

        /// <summary>Mitglied der Eigenschaft <see cref="Passwords"/>.</summary>
        private ObservableCollection<IPassword> passwords;

        #endregion

        #region Constructors and Destructors

        /// <summary>Initialisiert eine neue Instanz der <see cref="PasswordListViewModel"/> Klasse.</summary>
        /// <param name="mediator">Erwartet eine Instanz des <see cref="PasswordMediator"/>.</param>
        public PasswordListViewModel(PasswordMediator mediator)
        {
            this.mediator = mediator;
        }

        #endregion

        #region Public Properties

        /// <summary>Holt die Kommandostruktur zum Öffnen eines Tabs.</summary>
        public SimpleCommand<object, SCommandArgs> OpenTabCommand { get; private set; }

        /// <summary>Holt oder setzt alle verwalteten Passwörter.</summary>
        public ObservableCollection<IPassword> Passwords
        {
            get
            {
                return passwords;
            }

            set
            {
                passwords = value;
                PasswordsChanged(null, null);
                Passwords.CollectionChanged += PasswordsChanged;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>Empfängt ein <see cref="IPassword"/>.</summary>
        /// <param name="password">Das <see cref="IPassword"/> welches empfangen wird.</param>
        public void Recieve(IPassword password)
        {
        }

        /// <summary>Sendet ein <see cref="IPassword"/>. </summary>
        /// <param name="password">Das <see cref="IPassword"/> welches verschickt wird.</param>
        public void Send(IPassword password)
        {
            mediator.Send(this, password);
        }

        #endregion

        #region Methods

        /// <summary>Initialisiert alle Kommandos.</summary>
        protected override void InitCommands()
        {
            base.InitCommands();
            OpenTabCommand = new SimpleCommand<object, SCommandArgs>(OpenNewTab);
        }

        /// <summary>Öffnet einen neuen Tab.</summary>
        /// <param name="arguments">Erwartet ein <see cref="IPassword"/> in der Eigenschaft CommandParameter.</param>
        private void OpenNewTab(SCommandArgs arguments)
        {
            Send((IPassword)arguments.CommandParameter);
        }

        /// <summary>Tritt ein wenn sich die Auflistung Passwords geändert hat.</summary>
        /// <param name="sender">Dieser Parameter wird nicht verwendet.</param>
        /// <param name="arguments">Dieser Parameter wird nicht verwendet.</param>
        private void PasswordsChanged(object sender, NotifyCollectionChangedEventArgs arguments)
        {
            if (Passwords != null)
            {
                Passwords.ForEach(pw => pw.PropertyChanged += (s, args) => OnPropertyChanged("Passwords"));
            }
        }

        #endregion
    }
}