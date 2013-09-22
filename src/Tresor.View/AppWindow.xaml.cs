namespace Tresor.View
{
    /// <summary>Interaktionslogik für AppWindow.xaml.</summary>
    public partial class AppWindow
    {
        #region Constructors and Destructors

        private string version; 

        public string Version
        {
            get
            {
                return version;
            }

            set
            {
                version = value;
            }
        }

        /// <summary>Initialisiert eine neue Instanz der <see cref="AppWindow"/> Klasse.</summary>
        public AppWindow()
        {
            Version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            InitializeComponent();
        }

        #endregion
    }
}