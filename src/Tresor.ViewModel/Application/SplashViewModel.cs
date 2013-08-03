namespace Tresor.ViewModel.Application
{
    using Cinch;

    using Tresor.Contracts.Model;
    using Tresor.Contracts.ViewModel.Application;
    using Tresor.Framework.MVVM;

    /// <summary>ViewModel für die SplashView.</summary>
    public class SplashViewModel : ViewModel, ISplashViewModel
    {
        #region Fields

        /// <summary>Mitglied der Eigenschaft <see cref="Input"/>.</summary>
        private string input;

        /// <summary>Mitglied der Eigenschaft <see cref="KeyIsCorrect"/>.</summary>
        /// <remarks>Anfangs auf True damit die Fehlermeldung nicht angezeigt wird.</remarks>
        private bool keyIsCorrect = true;

        /// <summary>Das Datenmodel.</summary>
        private IPanelModel model;

        #endregion

        #region Constructors and Destructors

        /// <summary>Initialisiert eine neue Instanz der <see cref="SplashViewModel"/> Klasse.</summary>
        /// <param name="model">Das Datenmodel.</param>
        public SplashViewModel(IPanelModel model)
        {
            this.model = model;
        }

        #endregion

        #region Public Properties

        /// <summary>Holt die Kommandostruktur zum Überprüfen der Eingabe.</summary>
        public SimpleCommand<SCommandArgs, object> CheckInputCommand { get; private set; }

        /// <summary>Holt oder setzt die Eingabe der SplashView.</summary>
        public string Input
        {
            get
            {
                return input;
            }

            set
            {
                input = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Holt einen Wert, der angibt, ob der eingegebene Schlüssel richtig ist.</summary>
        public bool KeyIsCorrect
        {
            get
            {
                return keyIsCorrect;
            }

            private set
            {
                keyIsCorrect = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Methods

        /// <summary>Erweitert das Basisverhalten um die Initalisierung weiterer Kommandos.</summary>
        protected override void InitCommands()
        {
            base.InitCommands();
            CheckInputCommand = new SimpleCommand<SCommandArgs, object>(CheckInput);
        }

        /// <summary>Überprüft ob der eingegebene Schlüssel korrekt ist.</summary>
        /// <param name="arguments">Dieser Parameter wird nicht verwendet.</param>
        private void CheckInput(object arguments)
        {
            KeyIsCorrect = model.IsKeyCorrect(Input);
        }

        #endregion
    }
}