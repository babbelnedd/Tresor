namespace Tresor
{
    using System.ComponentModel;
    using System.Windows.Controls;

    using Microsoft.Practices.Unity;

    using Tresor.Contracts.Model;
    using Tresor.Contracts.ViewModel;
    using Tresor.Model;
    using Tresor.Utilities.Mediator;
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
                        LoadMainWindow();
                    }
                };
            return view;
        }

        /// <summary>Lädt das Hauptfenster.</summary>
        private void LoadMainWindow()
        {
            mainWindow = container.Resolve<MainWindow>();
            var model = container.Resolve<IPanelModel>();
            container.RegisterInstance(new MainViewModel(model));
            var mainViewModel = container.Resolve<MainViewModel>();


            container.RegisterInstance(new PasswordMediator());
            var mediator = container.Resolve<PasswordMediator>();
            container.RegisterInstance<UserControl>("PasswordView", new PasswordView());
            container.RegisterInstance<UserControl>("PasswordListView", new PasswordListView());
            container.RegisterInstance<IPasswordViewModel>("PasswordViewModel", new PasswordViewModel(model, mediator));
            container.RegisterInstance<IPasswordListViewModel>("PasswordListViewModel", new PasswordListViewModel(mediator));
            
            var passwordViewModel = container.Resolve<IPasswordViewModel>("PasswordViewModel");
            var passwordListViewModel = container.Resolve<IPasswordListViewModel>("PasswordListViewModel");
            
            mediator.Add(passwordViewModel);
            mediator.Add(passwordListViewModel);
            mediator.Add(mainViewModel);


            mainViewModel.Container = container;
            mainWindow.DataContext = mainViewModel;
            mainWindow.ShowDialog();
        }

        /// <summary>Lädt die SplashView.</summary>
        /// <remarks>Bei erfolgreicher Eingabe des Schlüssels zum Laden der Passwörter wird das MainWindow geladen.</remarks>
        private void LoadSplashView()
        {
            var view = InitializeSplashView();
            view.ShowDialog();
        }

        /// <summary>Registriert alle benötigten Typen beim IoC Container.</summary>
        private void RegisterAll()
        {
            container.RegisterInstance<IPanelModel>(new SqlitePanelModel("tresor.db"));
        }

        #endregion
    }
}