namespace Tresor
{
    using System;
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
            ResolveAll();
            StartApplication();
        }

        #endregion

        #region Methoden

        /// <summary>Lädt das Hauptfenster.</summary>
        private static void LoadMainWindow()
        {
            var mainWindow = Container.Resolve<MainWindow>();
            mainWindow.ShowDialog();
        }

        /// <summary>Registriert alle benötigten Typen beim IoC Container.</summary>
        private static void RegisterAll()
        {
            Container.RegisterType<IPanelModel, PanelModel>();
            Container.RegisterType<IPanelViewModel, PanelViewModel>();
            Container.RegisterType<UserControl, PanelView>();
        }

        /// <summary>Löst alle benötigten Typen auf.</summary>
        private static void ResolveAll()
        {
            Container.Resolve<PanelView>();
        }

        /// <summary>Startet die Anwendung.</summary>
        private static void StartApplication()
        {
            LoadMainWindow();
        }

        #endregion

        /// <summary>Zeigt einen Dialog, welcher eine <see cref="System.Exception"/> darstellt.</summary>
        /// <param name="exception">Die <see cref="System.Exception"/> welche dargestellt werden soll.</param>
        public static void ShowException(Exception exception)
        {
        }
    }
}