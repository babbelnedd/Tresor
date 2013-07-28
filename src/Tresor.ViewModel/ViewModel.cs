namespace Tresor.ViewModel
{
    using Tresor.Utilities;

    /// <summary>Basisklasse für ViewModels.</summary>
    public abstract class ViewModel : NotifyPropertyChanged
    {
        #region Constructors and Destructors

        /// <summary> Initialisiert eine neue Instanz der <see cref="ViewModel"/> Klasse. </summary>
        public ViewModel()
        {
            InitCommands();
        }

        #endregion

        #region Methods

        /// <summary>Initialisiert alle Kommandos.</summary>
        protected virtual void InitCommands()
        {
        }

        #endregion
    }
}