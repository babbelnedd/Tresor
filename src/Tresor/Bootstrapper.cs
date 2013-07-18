namespace Tresor
{
    using System.Windows.Controls;

    using Microsoft.Practices.Unity;

    using global::Tresor.Contracts.Model;
    using global::Tresor.Contracts.ViewModel;

    using global::Tresor.Model;
    using global::Tresor.Utilities.EventArgs;
    using global::Tresor.View;

    using global::Tresor.View.Application;
    using global::Tresor.View.UserControls;
    using global::Tresor.ViewModel;

    /// <summary>Statischer Bootstrapper der die Anwendung verkabelt.</summary>
    public class Bootstrapper
    {
        #region Konstanten und Felder

        /// <summary>Der IoC Container.</summary>
        private readonly IUnityContainer Container = new UnityContainer();

        #endregion

        #region Öffentliche Methoden und Operatoren

        /// <summary>Lädt die Anwendung.</summary>
        public void LoadApplication()
        {
            RegisterAll();
            StartApplication();
        }

        #endregion

        #region Methoden

        /// <summary>Erstellt die <see cref="Tresor.View.PanelView"/> und gibt diese zurück.</summary>
        /// <returns>Die <see cref="Tresor.View.PanelView"/>.</returns>
        private PanelView GetPanelView()
        {
            var panelView = Container.Resolve<PanelView>();
            var panelViewModel = Container.Resolve<PanelViewModel>();
            panelViewModel.OpenTabRequested += OpenTabRequested;
            panelView.DataContext = panelViewModel;
            return panelView;
        }

        /// <summary>Tritt ein wenn ein neuer Tab geöffnet werden soll.</summary>
        /// <param name="sender">Dieser Parameter wird nicht verwendet.</param>
        /// <param name="arguments">Erwartet gültige <see cref="OpenTabRequestedEventArgs"/>.</param>
        private void OpenTabRequested(object sender, OpenTabRequestedEventArgs arguments)
        {
            var password = arguments.Content;
            if (password == null)
            {
                return;
            }

            var tabItem = new TabItem
                              {
                                  Header = password.Account,
                                  ToolTip = "Passwort bearbeiten",
                                  Content = Container.Resolve<PasswordView>()
                              };

            mainWindow.TabControl.Items.Add(tabItem);
        }

        /// <summary>Lädt das Hauptfenster.</summary>
        private void LoadMainWindow()
        {
            mainWindow = Container.Resolve<MainWindow>();
            SetupTabControl();
            mainWindow.ShowDialog();
        }

        /// <summary>Registriert alle benötigten Typen beim IoC Container.</summary>
        private void RegisterAll()
        {
            Container.RegisterInstance<IPanelModel>(new PanelModel());
            Container.RegisterInstance<UserControl>(new PanelView());
            Container.RegisterType<IPanelViewModel, PanelViewModel>();
        }

        /// <summary>Das Applikationsfenster.</summary>
        private MainWindow mainWindow;

        /// <summary>Setzt das TabControl auf.</summary>
        private void SetupTabControl()
        {
            mainWindow.TabControl.Items.Add(new TabItem { Header = "Passwörter", ToolTip = "Alle verwalteten Passwörter", Content = GetPanelView() });
        }

        /// <summary>Startet die Anwendung.</summary>
        private void StartApplication()
        {
            LoadMainWindow();
        }

        #endregion
    }
}