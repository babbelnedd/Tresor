namespace Tresor.View
{
    using Tresor.Contracts.ViewModel;

    /// <summary>Interaktionslogik für ListView.xaml.</summary>
    public partial class PanelView
    {
        #region Konstruktoren und Destruktoren

        private IPanelViewModel viewModel;

        /// <summary>Initialisiert eine neue Instanz der <see cref="PanelView"/> Klasse.</summary>
        public PanelView()
        {
            InitializeComponent();
        }

        #endregion
    }
}