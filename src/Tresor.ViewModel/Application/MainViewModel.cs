namespace Tresor.ViewModel.Application
{
    using System.Collections.ObjectModel;
    using System.Linq;

    using Cinch;

    using Microsoft.Practices.ObjectBuilder2;
    using Microsoft.Practices.Unity;

    using Tresor.Contracts.Model;
    using Tresor.Contracts.Utilities;
    using Tresor.Contracts.ViewModel.Application;
    using Tresor.Framework.MVVM;
    using Tresor.Utilities;

    /// <summary>ViewModel für das MainWindow.</summary>
    public class MainViewModel : ViewModel, IMainViewModel
    {
        #region Fields

        /// <summary>Das Datenmodel.</summary>
        private readonly IPanelModel model;

        /// <summary>Mitglied der Eigenschaft <see cref="Container"/>.</summary>
        private IUnityContainer container;

        /// <summary>Mitglied der Eigenschaft <see cref="Tabs"/>.</summary>
        private ObservableCollection<Tab> tabs;

        #endregion

        #region Constructors and Destructors

        /// <summary>Initialisiert eine neue Instanz der <see cref="MainViewModel"/> Klasse.</summary>
        /// <param name="model">Das Datenmodel.</param>
        public MainViewModel(IPanelModel model)
        {
            this.model = model;
            Tabs = new ObservableCollection<Tab>();
            var tab = new Tab(Passwords);
            Tabs.Add(tab);
            SelectTab(tab);
            Tabs.CollectionChanged += (sender, arguments) => OnPropertyChanged("Tabs");
        }

        #endregion

        #region Public Properties

        /// <summary>Holt die Kommandostruktur zum Schließen eines Tabs.</summary>
        public SimpleCommand<object, SCommandArgs> CloseTabCommand { get; private set; }

        /// <summary>Holt oder setzt den UnityContainer.</summary>
        public IUnityContainer Container
        {
            get
            {
                return container;
            }

            set
            {
                container = value;
                OnPropertyChanged();
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

        /// <summary>Holt den selektierten Tab.</summary>
        public Tab SelectedTab
        {
            get
            {
                return Tabs.SingleOrDefault(tab => tab.IsSelected);
            }
        }

        /// <summary>Holt die geöffneten Tabs.</summary>
        public ObservableCollection<Tab> Tabs
        {
            get
            {
                return tabs;
            }

            private set
            {
                tabs = value;
            }
        }

        #endregion

        #region Properties

        /// <summary>Holt oder setzt den zuletzt selektierten Tab.</summary>
        private Tab LastSelectedTab { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>Öffnet ein neuen Tab, welcher ein Passwort enthält.</summary>
        /// <param name="password">Das ausgewählte Passwort.</param>
        public void OpenPassword(IPassword password)
        {
            var tab = new Tab(password);
            OpenTab(tab);
        }

        /// <summary>Öffnet einen neuen Tab, falls dieser nicht bereits geöffnet ist.</summary>
        /// <param name="tab">Der zu öffnende Tab.</param>
        public void OpenTab(Tab tab)
        {
            if (!Tabs.Contains(tab))
            {
                Tabs.Add(tab);
                SelectTab(tab);
            }
        }

        /// <summary>Empfängt ein <see cref="IPassword"/>.</summary>
        /// <param name="password">Das <see cref="IPassword"/> welches empfangen wird.</param>
        public void Recieve(IPassword password)
        {
            OpenPassword(password);
        }

        /// <summary>Sendet ein <see cref="IPassword"/>. </summary>
        /// <param name="password">Das <see cref="IPassword"/> welches verschickt wird.</param>
        public void Send(IPassword password)
        {
        }

        #endregion

        #region Methods

        /// <summary>Erweitert das Basisverhalten um die Initialisierung weiterer Kommandos.</summary>
        protected override void InitCommands()
        {
            base.InitCommands();
            CloseTabCommand = new SimpleCommand<object, SCommandArgs>(CloseTab);
        }

        /// <summary>Schließt einen Tab.</summary>
        /// <param name="arguments">Erwartet einen <see cref="Tab"/> im CommandParameter.</param>
        private void CloseTab(SCommandArgs arguments)
        {
            var tab = (Tab)arguments.CommandParameter;

            if (!tabs.Contains(tab))
            {
                return;
            }

            if (tab.IsSelected)
            {
                SelectLastTab();
            }

            Tabs.Remove(tab); // Note: Vor SelectLastTab ausführen?!
        }

        /// <summary>Selektiert den zuletzt selektierten Tab.</summary>
        private void SelectLastTab()
        {
            if (LastSelectedTab != null)
            {
                SelectTab(LastSelectedTab);
            }
        }

        /// <summary>Selektiert einen Tab.</summary>
        /// <param name="tab">Der zu selektierende Tab.</param>
        private void SelectTab(Tab tab)
        {
            if (SelectedTab != null)
            {
                LastSelectedTab = SelectedTab;
            }

            Tabs.ForEach(x => x.IsSelected = false);
            tab.IsSelected = true;
            OnPropertyChanged("SelectedTab");
        }

        #endregion
    }
}