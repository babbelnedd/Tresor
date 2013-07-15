namespace Tresor.ViewModel
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;

    using Cinch;

    using Tresor.Contracts.Model;
    using Tresor.Contracts.Utilities;
    using Tresor.Contracts.ViewModel;
    using Tresor.Framework.MVVM;
    using Tresor.Utilities;

    /// <summary>ViewModel für die PanelView.</summary>
    public class PanelViewModel : NotifyPropertyChanged, IPanelViewModel
    {
        #region Konstanten und Felder

        /// <summary>Das Datenmodel des PanelViewModels.</summary>
        private IPanelModel model;

        #endregion


        #region Öffentliche Eigenschaften

        /// <summary>Holt einen Wert, der angibt, ob es ungespeicherte Änderungen gibt.</summary>
        public bool IsDirty
        {
            get
            {
                return model.IsDirty;
            }
        }

        /// <summary>Holt alle verwalteten Passwörter.</summary>
        public ObservableCollection<IPassword> Passwords
        {
            get
            {
                return model.Passwords;
            }
        }

        #endregion

        #region Konstruktoren und Destruktoren

        /// <summary>Initialisiert eine neue Instanz der <see cref="PanelViewModel"/> Klasse. </summary>
        /// <param name="model">Das Datenmodel.</param>
        public PanelViewModel(IPanelModel model)
        {
            this.model = model;
            model.PropertyChanged += ModelChanged;

            AddPasswordCommand = new SimpleCommand<object, SCommandArgs>(AddPassword);
        }

        private void AddPassword(SCommandArgs obj)
        {

        }

        #endregion

        #region Methoden

        /// <summary>Tritt ein wenn sich eine Eigenschaft im Model geändert hat.</summary>
        /// <param name="sender">Dieser Parameter wird nicht verwendet.</param>
        /// <param name="arguments">Überpüft anhand der Eigenschaft PropertyName welche Eigenschaft sich geändert hat.</param>
        private void ModelChanged(object sender, PropertyChangedEventArgs arguments)
        {
            if (arguments.PropertyName == "IsDirty")
            {
                OnPropertyChanged("IsDirty");
            }
        }

        #endregion

        public SimpleCommand<object, SCommandArgs> AddPasswordCommand { get; private set; }
    }
}