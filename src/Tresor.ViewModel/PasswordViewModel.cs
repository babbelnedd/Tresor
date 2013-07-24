namespace Tresor.ViewModel
{
    using System.ComponentModel;

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
                password.PropertyChanged += PasswordChanged;
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
            model.PropertyChanged += ModelChanged;
        }

        #endregion

        #region Methoden

        /// <summary>Erweitert das Basisverhalten um die Initlaisierung weiterer Kommandos.</summary>
        protected override void InitCommands()
        {
            base.InitCommands();
            SavePasswordCommand = new SimpleCommand<SCommandArgs, object>(SavePassword);
        }

        /// <summary>Tritt ein wenn sich eine Eigenschaft im Model geändert hat.</summary>
        /// <param name="sender">Dieser Parameter wird nicht verwendet.</param>
        /// <param name="arguments">Dieser Parameter wird nicht verwendet.</param>
        private void ModelChanged(object sender, PropertyChangedEventArgs arguments)
        {
            OnPropertyChanged("Password");
        }

        /// <summary>Tritt ein wenn sich am Passwort etwas geändert hat.</summary>
        /// <param name="sender">Dieser Parameter wird nicht verwendet.</param>
        /// <param name="arguments">Dieser Parameter wird nicht verwendet.</param>
        private void PasswordChanged(object sender, PropertyChangedEventArgs arguments)
        {
            OnPropertyChanged("Password");
        }

        /// <summary>Speichert ein Passwort.</summary>
        /// <param name="arguments">Dieser Parameter wird nicht verwendet.</param>
        private void SavePassword(object arguments)
        {
            model.Save(password);
        }

        #endregion
    }
}