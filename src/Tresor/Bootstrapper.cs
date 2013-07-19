namespace Tresor
{
    using System.Windows.Controls;
    using System.Windows.Input;

    using Microsoft.Practices.Unity;

    using global::Tresor.Contracts.Model;

    using global::Tresor.Contracts.Utilities;

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
        private readonly IUnityContainer container = new UnityContainer();

        /// <summary>Das Applikationsfenster.</summary>
        private MainWindow mainWindow;

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

        /// <summary>Holt ein neues TabItem, welches ein Passwort darstellt.</summary>
        /// <param name="password">Das darzustellende Passwort.</param>
        /// <returns>Das neu erzeuget <see cref="TabItem"/>.</returns>
        private TabItem GetNewPasswordTab(IPassword password)
        {
            var tabItem = GetNewTabItem(password.Account, "Passwort bearbeiten", container.Resolve<PasswordView>());
            return tabItem;
        }

        /// <summary>Holt ein neues TabItem.</summary>
        /// <param name="header">Der Header des TabItems.</param>
        /// <param name="toolTip">Das Tooltip des TabItems.</param>
        /// <param name="content">Der Inhalt des TabItems.</param>
        /// <returns>Das neu erzeugte <see cref="TabItem"/>.</returns>
        private TabItem GetNewTabItem(string header, string toolTip, object content)
        {
            var tabItem = new TabItem { Header = header, ToolTip = toolTip, Content = content };
            tabItem.MouseUp += TabItemClicked;

            return tabItem;
        }

        /// <summary>Erstellt die <see cref="Tresor.View.PanelView"/> und gibt diese zurück.</summary>
        /// <returns>Die <see cref="Tresor.View.PanelView"/>.</returns>
        private PanelView GetPanelView()
        {
            var panelView = container.Resolve<PanelView>();
            var panelViewModel = container.Resolve<PanelViewModel>();
            panelViewModel.OpenTabRequested += OpenTabRequested;
            panelView.DataContext = panelViewModel;
            return panelView;
        }

        /// <summary>Lädt das Hauptfenster.</summary>
        private void LoadMainWindow()
        {
            mainWindow = container.Resolve<MainWindow>();
            SetupTabControl();
            mainWindow.ShowDialog();
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

            var tabItem = GetNewPasswordTab(password);
            mainWindow.TabControl.Items.Add(tabItem);
        }

        /// <summary>Registriert alle benötigten Typen beim IoC Container.</summary>
        private void RegisterAll()
        {
            container.RegisterInstance<IPanelModel>(new PanelModel());
            container.RegisterInstance<UserControl>(new PanelView());
            container.RegisterType<IPanelViewModel, PanelViewModel>();
        }

        /// <summary>Setzt das TabControl auf.</summary>
        private void SetupTabControl()
        {
            var tabItem = GetNewTabItem("Passwörter", "Alle verwalteten Passwörter", GetPanelView());
            mainWindow.TabControl.Items.Add(tabItem);
        }

        /// <summary>Startet die Anwendung.</summary>
        private void StartApplication()
        {
            LoadMainWindow();
        }

        /// <summary>Tritt ein wenn ein <strong>MouseUp Ereignis</strong> bei einem TabItem eintritt.</summary>
        /// <param name="sender">Erwartet das TabItem.</param>
        /// <param name="arguments">Prüft anhand der Eigenschaft ChangedButton welche Taste gedrückt wurde.</param>
        private void TabItemClicked(object sender, MouseButtonEventArgs arguments)
        {
            var tabItem = sender as TabItem;

            if (arguments.ChangedButton != MouseButton.Middle || tabItem == null || tabItem.Header.ToString() == "Passwörter")
            {
                return;
            }

            mainWindow.TabControl.Items.Remove(tabItem);
        }

        #endregion
    }
}