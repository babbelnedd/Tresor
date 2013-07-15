namespace Tresor
{
    using System.Windows.Controls;

    using Microsoft.Practices.Unity;

    using global::Tresor.Contracts.Model;
    using global::Tresor.Contracts.ViewModel;
    using global::Tresor.Model;
    using global::Tresor.View;
    using global::Tresor.View.Application;
    using global::Tresor.ViewModel;

    /// <summary>Statischer Bootstrapper der die Anwendung verkabelt.</summary>
    public static class Bootstrapper
    {
        #region Konstanten und Felder

        /// <summary>Der IoC Container.</summary>
        private static readonly IUnityContainer Container = new UnityContainer();

        #endregion

        #region Öffentliche Methoden und Operatoren

        /// <summary>Lädt die Anwendung.</summary>
        public static void LoadApplication()
        {
            RegisterAll();
            StartApplication();
        }

        #endregion

        #region Methoden

        /// <summary>Lädt das Hauptfenster.</summary>
        private static void LoadMainWindow()
        {
            var mainWindow = Container.Resolve<MainWindow>();
            
            var panelView = Container.Resolve<PanelView>();
            var panelViewModel = Container.Resolve<PanelViewModel>();

            mainWindow.ViewControl.Content = panelView;
            ((PanelView)mainWindow.ViewControl.Content).DataContext = panelViewModel;

            mainWindow.ShowDialog();
        }

        /// <summary>Registriert alle benötigten Typen beim IoC Container.</summary>
        private static void RegisterAll()
        {
            var panelModel = new PanelModel();
            Container.RegisterInstance<IPanelModel>(panelModel);
            Container.RegisterType<IPanelViewModel, PanelViewModel>();
            Container.RegisterType<UserControl, PanelView>();
        }


        /// <summary>Startet die Anwendung.</summary>
        private static void StartApplication()
        {
            LoadMainWindow();
        }

        #endregion
    }
}