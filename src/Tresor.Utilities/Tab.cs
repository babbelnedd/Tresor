namespace Tresor.Utilities
{
    using System.Collections.ObjectModel;

    using Tresor.Contracts.Utilities;

    /// <summary>Beschreibt einen Tab der innerhalb der Anwendung angezeigt werden kann.</summary>
    public class Tab : NotifyPropertyChanged
    {
        #region Fields

        /// <summary>Mitglied der Eigenschaft <see cref="IsSelected"/>.</summary>
        private bool isSelected;

        #endregion

        #region Constructors and Destructors

        /// <summary>Initialisiert eine neue Instanz der <see cref="Tab"/> Klasse. </summary>
        /// <param name="content">Der Inhalt des Tabs.</param>
        public Tab(IPassword content)
        {
            Content = content;
        }

        /// <summary>Initialisiert eine neue Instanz der <see cref="Tab"/> Klasse.</summary>
        /// <param name="content">Der Inhalt des Tabs.</param>
        public Tab(ObservableCollection<IPassword> content)
        {
            Content = content;
        }

        #endregion

        #region Public Properties

        /// <summary>Holt oder setzt den Inhalt des Tabs.</summary>
        public object Content { get; set; }

        /// <summary>Holt oder setzt einen Wert, der angibt, ob der Tab selektiert ist.</summary>
        public bool IsSelected
        {
            get
            {
                return isSelected;
            }

            set
            {
                isSelected = value;
                OnPropertyChanged();
            }
        }

        #endregion
    }
}