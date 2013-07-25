namespace Tresor
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Controls;
    using System.Windows.Input;

    using Microsoft.Practices.Unity;

    using Tresor.Contracts.Model;
    using Tresor.Contracts.Utilities;
    using Tresor.Contracts.ViewModel;
    using Tresor.Model;
    using Tresor.Utilities.EventArgs;
    using Tresor.View;
    using Tresor.View.Application;
    using Tresor.View.UserControls;
    using Tresor.ViewModel;
    using Tresor.ViewModel.Application;

    /// <summary>Statischer Bootstrapper der die Anwendung verkabelt.</summary>
    internal class Bootstrapper
    {
        #region Konstanten und Felder

        /// <summary>Der IoC Container.</summary>
        private readonly IUnityContainer container = new UnityContainer();

        /// <summary>Holt alle geöffneten Tabs von Typ <see cref="IPassword"/>.</summary>
        private readonly List<IPassword> openTabs = new List<IPassword>();

        /// <summary>Das Applikationsfenster.</summary>
        private MainWindow mainWindow;

        #endregion

        #region Methoden

        /// <summary>Lädt die Anwendung.</summary>
        internal void LoadApplication()
        {
            RegisterAll();
            LoadSplashView();
        }

        /// <summary>Holt einen neuen Header für ein TabItem.</summary>
        /// <param name="password">Das Passwort welches unter dem Tab dargestellt wird.</param>
        /// <returns>Der neu erzeugte Header.</returns>
        private StackPanel GetNewHeader(IPassword password)
        {
            var header = new StackPanel { Orientation = Orientation.Horizontal };
            var txt1 = new TextBlock { Text = password.Account };
            var txt2 = new TextBlock();

            if (password.IsDirty)
            {
                txt2.Text = " *";
            }

            header.Children.Add(txt1);
            header.Children.Add(txt2);
            return header;
        }

        /// <summary>Holt ein neues TabItem, welches ein Passwort darstellt.</summary>
        /// <param name="password">Das darzustellende Passwort.</param>
        /// <returns>Das neu erzeuget <see cref="TabItem"/>.</returns>
        private TabItem GetNewPasswordTab(IPassword password)
        {
            var view = container.Resolve<PasswordView>();
            var viewModel = container.Resolve<PasswordViewModel>();
            viewModel.Password = password;
            view.DataContext = viewModel;

            var header = GetNewHeader(password);
            var tabItem = GetNewTabItem(header, "Passwort bearbeiten", view);

            password.PropertyChanged += (sender, arguments) => tabItem.Header = GetNewHeader(password);

            viewModel.Password = password;
            view.DataContext = viewModel;
            return tabItem;
        }

        /// <summary>Holt ein neues TabItem.</summary>
        /// <param name="header">Der Header des TabItems.</param>
        /// <param name="toolTip">Das Tooltip des TabItems.</param>
        /// <param name="content">Der Inhalt des TabItems.</param>
        /// <returns>Das neu erzeugte <see cref="TabItem"/>.</returns>
        private TabItem GetNewTabItem(object header, string toolTip, object content)
        {
            var tabItem = new TabItem { Header = header, ToolTip = toolTip, Content = content, MinWidth = 75, MaxWidth = 300 };
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

        /// <summary>Initialisiert die SplashView.</summary>
        /// <returns>Die instanzierte SplashView.</returns>
        private SplashView InitializeSplashView()
        {
            var view = container.Resolve<SplashView>();
            var viewModel = container.Resolve<SplashViewModel>();
            view.DataContext = viewModel;
            viewModel.PropertyChanged += (sender, arguments) =>
                {
                    if (arguments.PropertyName == "KeyIsCorrect" && viewModel.KeyIsCorrect)
                    {
                        view.Close();
                        StartApplication();
                    }
                };
            return view;
        }

        /// <summary>Lädt das Hauptfenster.</summary>
        private void LoadMainWindow()
        {
            mainWindow = container.Resolve<MainWindow>();
            SetupTabControl();
            mainWindow.ShowDialog();
        }

        /// <summary>Lädt die SplashView.</summary>
        /// <remarks>Bei erfolgreicher Eingabe des Schlüssels zum Laden der Passwörter wird das MainWindow geladen.</remarks>
        private void LoadSplashView()
        {
            var view = InitializeSplashView();
            view.ShowDialog();
        }

        /// <summary>Tritt ein wenn ein neuer Tab geöffnet werden soll.</summary>
        /// <param name="sender">Dieser Parameter wird nicht verwendet.</param>
        /// <param name="arguments">Erwartet gültige <see cref="OpenTabRequestedEventArgs"/>.</param>
        private void OpenTabRequested(object sender, OpenTabRequestedEventArgs arguments)
        {
            var password = arguments.Content;
            if (password == null || openTabs.Any(tab => tab == password))
            {
                return;
            }

            openTabs.Add(password);
            var tabItem = GetNewPasswordTab(password);
            mainWindow.TabControl.Items.Add(tabItem);
            tabItem.IsSelected = true;
        }

        /// <summary>Registriert alle benötigten Typen beim IoC Container.</summary>
        private void RegisterAll()
        {
            container.RegisterInstance<IPanelModel>(new SqlitePanelModel("tresor.db"));
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

            var password = ((PasswordViewModel)((PasswordView)tabItem.Content).DataContext).Password;
            openTabs.Remove(password);

            mainWindow.TabControl.Items.Remove(tabItem);
        }

        #endregion
    }
}