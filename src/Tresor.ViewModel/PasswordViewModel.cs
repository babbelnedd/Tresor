namespace Tresor.ViewModel
{
    using Cinch;

    using Tresor.Contracts.Model;
    using Tresor.Contracts.Utilities;
    using Tresor.Contracts.ViewModel;
    using Tresor.Framework.MVVM;

    /// <summary>ViewModel für die <see cref="Tresor.View.PasswordView"/>.</summary>
    public class PasswordViewModel : ViewModel, IPasswordViewModel
    {
        #region Konstanten und Felder

        /// <summary>Das Datenmodel.</summary>
        private IPanelModel model;

        /// <summary>Mitglied der Eigenschaft <see cref="Password"/>.</summary>
        private IPassword password;

        #endregion

        #region Öffentliche Eigenschaften

        /// <summary>Holt oder setzt das Passwort welches dargestellt werden soll.</summary>
        public IPassword Password
        {
            get
            {
                return password;
            }

            set
            {
                password = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Holt die Kommandostruktur zum Speichern eines Passworts.</summary>
        public SimpleCommand<SCommandArgs, object> SavePasswordCommand { get; private set; }

        #endregion

        #region Konstruktoren und Destruktoren

        /// <summary>Initialisiert eine neue Instanz der <see cref="PasswordViewModel"/> Klasse.</summary>
        /// <param name="model">Das Datenmodel.</param>
        public PasswordViewModel(IPanelModel model)
        {
            this.model = model;
        }

        #endregion

        #region Methoden

        /// <summary>Erweitert das Basisverhalten um die Initlaisierung weiterer Kommandos.</summary>
        protected override void InitCommands()
        {
            base.InitCommands();
            SavePasswordCommand = new SimpleCommand<SCommandArgs, object>(SavePassword);
        }

        /// <summary>Speichert ein Passwort.</summary>
        /// <param name="arguments">Dieser Parameter wird nicht verwendet.</param>
        private void SavePassword(object arguments)
        {
        }

        #endregion
    }
}