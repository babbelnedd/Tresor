namespace Tresor.UnitTesting.Tresor.ViewModel.Application
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.Practices.Unity;

    using NUnit.Framework;

    using global::Tresor.Framework.MVVM;

    using global::Tresor.Model;

    using global::Tresor.Utilities;

    using global::Tresor.ViewModel.Application;

    /// <summary>Tests für das MainViewModel welches das MainWindow steuert.</summary>
    [TestFixture(Description = "Tests für das MainViewModel welches das MainWindow steuert.")]
    public class MainViewModelTests : Test
    {
        #region Fields

        /// <summary>Eine Instanz des MainViewModels.</summary>
        private MainViewModel viewModel;

        #endregion

        #region Public Methods and Operators

        /// <summary>Prüft ob das Hinzufügen eines Tabs PropertyChanged auslöst.</summary>
        [Test(Description = "Prüft ob das Hinzufügen eines Tabs PropertyChanged auslöst.")]
        [Repeat(Tests)]
        public void AddingTabRaisesPropertyChanged()
        {
            var changes = new List<string>();
            this.viewModel.PropertyChanged += (sender, arguments) => changes.Add(arguments.PropertyName);
            this.viewModel.Tabs.Add(null);
            Assert.Contains("Tabs", changes);
        }

        /// <summary>Prüft, dass der CloseTabCommand nichts macht, wenn der zu schließende Tab nicht in der Auflistung aller Tabs vorhanden ist.</summary>
        [Test(Description = "Prüft, dass der CloseTabCommand nichts macht, wenn der zu schließende Tab nicht in der Auflistung aller Tabs vorhanden ist.")]
        [Repeat(Tests)]
        public void CloseTabCommandDoNothingIfTabNotExists()
        {
            Assert.DoesNotThrow(() => viewModel.CloseTabCommand.Execute(new SCommandArgs(null, null, null)));
        }

        /// <summary>Prüft ob der CloseTabCommand keinen neuen Tab selektiert, wenn der geschlossene Tab nicht der selektierte war.</summary>
        [Test(Description = "Prüft ob der CloseTabCommand keinen neuen Tab selektiert, wenn der geschlossene Tab nicht der selektierte war.")]
        [Repeat(Tests)]
        public void CloseTabCommandDontSelectNewIfClosedTabIsntSelectedTab()
        {
            var tab1 = new Tab(new Password { RecordID = Guid.NewGuid() });
            var tab2 = new Tab(new Password { RecordID = Guid.NewGuid() });
            viewModel.OpenTab(tab1);
            viewModel.OpenTab(tab2);

            viewModel.CloseTabCommand.Execute(new SCommandArgs(null, null, tab1));

            Assert.That(viewModel.SelectedTab, Is.EqualTo(tab2));
        }

        /// <summary>Prüft ob das CloseTabCommand den angegebenen Tab von der Auflistung von Tabs entfernt.</summary>
        [Test(Description = "Prüft ob das CloseTabCommand den angegebenen Tab von der Auflistung von Tabs entfernt.")]
        [Repeat(Tests)]
        public void CloseTabCommandRemoveTabFromTabs()
        {
            var tab = new Tab(new Password { RecordID = Guid.NewGuid() });
            viewModel.OpenTab(tab);
            var tab2 = new Tab(new Password { RecordID = Guid.NewGuid() });
            viewModel.OpenTab(tab2);

            viewModel.CloseTabCommand.Execute(new SCommandArgs(null, null, tab));

            Assert.That(viewModel.Tabs.Contains(tab), Is.False);
        }

        /// <summary>Prüft ob der CloseTabCommand den zuletzt selektierten Tab selektiert, falls der aktuell selektierte Tab geschlossen wird.</summary>
        [Test(Description = "Prüft ob der CloseTabCommand den zuletzt selektierten Tab selektiert, falls der aktuell selektierte Tab geschlossen wird.")]
        [Repeat(Tests)]
        public void CloseTabCommandSelectLastSelectedTab()
        {
            var tab = new Tab(new Password { RecordID = Guid.NewGuid() });
            viewModel.OpenTab(tab);
            var tab2 = new Tab(new Password { RecordID = Guid.NewGuid() });
            viewModel.OpenTab(tab2);

            viewModel.CloseTabCommand.Execute(new SCommandArgs(null, null, tab2));

            Assert.That(viewModel.SelectedTab, Is.EqualTo(tab));
        }

        /// <summary>Prüft, dass der Konstruktor keine Ausnahme wirft.</summary>
        [Test(Description = "Prüft, dass der Konstruktor keine Ausnahme wirft.")]
        [Repeat(Tests)]
        public void CtorDontThrowException()
        {
            var database = string.Format("{0}.db", Guid.NewGuid());
            var model = new SqlitePanelModel(database);
            Assert.DoesNotThrow(() => new MainViewModel(model));
        }

        /// <summary>Prüft ob die Methode OpenTab einen Eintrag zu der Auflistung Tabs hinzufügt.</summary>
        [Test(Description = "Prüft ob die Methode OpenTab einen Eintrag zu der Auflistung Tabs hinzufügt.")]
        [Repeat(Tests)]
        public void OpenTabAddsEntryToTabs()
        {
            this.viewModel.OpenTab(new Tab(new Password()));
            Assert.IsTrue(this.viewModel.Tabs.Count == 2);
        }

        /// <summary>Prüft ob ein Tab nicht zu der Auflistung Tabs hinzugefügt wird, wenn dieser bereits exisitiert.</summary>
        [Test(Description = "Prüft ob ein Tab nicht zu der Auflistung Tabs hinzugefügt wird, wenn dieser bereits exisitiert.")]
        [Repeat(Tests)]
        public void OpenTabAddsNoEntryToTabsIfTabAlreadyExists()
        {
            var pw = new Password { RecordID = Guid.NewGuid() };
            var tab = new Tab(pw);
            this.viewModel.OpenTab(tab);
            var countBefore = this.viewModel.Tabs.Count;
            this.viewModel.OpenTab(tab);
            var countAfter = this.viewModel.Tabs.Count;
            Assert.AreEqual(countBefore, countAfter);
        }

        /// <summary>Testet ob der OpenTabCommand einen neuen Tab erzeugt und der Auflistung Tabs hinzufügt.</summary>
        [Test(Description = "Testet ob der OpenTabCommand einen neuen Tab erzeugt und der Auflistung Tabs hinzufügt.")]
        [Repeat(Tests)]
        public void OpenTabCommandAddsTab()
        {
            var countBefore = viewModel.Tabs.Count;

            viewModel.OpenPassword(new Password { RecordID = Guid.NewGuid() });

            var countAfter = viewModel.Tabs.Count;
            Assert.Greater(countAfter, countBefore);
            Assert.IsTrue(countAfter == countBefore + 1);
        }

        /// <summary>Prüft ob das OpenTabCommand den neu erzeugten Tab als SelectedTab wählt.</summary>
        [Test(Description = "Prüft ob das OpenTabCommand den neu erzeugten Tab als SelectedTab wählt.")]
        [Repeat(Tests)]
        public void OpenTabCommandSelectTabAsSelected()
        {
            var newPw = new Password { RecordID = Guid.NewGuid() };
            viewModel.OpenPassword(newPw);
            Assert.AreEqual(viewModel.SelectedTab.Content, newPw);
        }

        /// <summary>Prüft, ob die Auflistung Passwords nicht null ist.</summary>
        [Test(Description = "Prüft, ob die Auflistung Passwords nicht null ist.")]
        [Repeat(Tests)]
        public void PasswordsIsNotNullAtStart()
        {
            Assert.IsNotNull(this.viewModel.Passwords);
        }

        /// <summary>Testet die Methode Recieve.</summary>
        [Test(Description = "Testet ob die Methode Recieve ein neuen Tab selektiert.")]
        [Repeat(Tests)]
        public void Recieve()
        {
            var specificPassword = new Password { RecordID = Guid.NewGuid() };
            viewModel.OpenPassword(specificPassword);
            AddSomeRandomTabs();

            Assert.That(viewModel.SelectedTab.Content, Is.Not.EqualTo(specificPassword));
            viewModel.Recieve(specificPassword);
            Assert.That(viewModel.SelectedTab.Content, Is.EqualTo(specificPassword));
        }

        /// <summary>Testet ob die Methode SelectLastTab den ersten Tab selektiert, falls LastSelectedTab nicht mehr geöffnet oder null ist.</summary>
        [Test(Description = "Testet ob die Methode SelectLastTab den ersten Tab selektiert, falls LastSelectedTab nicht mehr geöffnet oder null ist.")
        ]
        [Repeat(Tests)]
        public void SelectLastTabSelectFirstTabIfLastSelectedTabIsNotOpenAnymore()
        {
            var firstTab = viewModel.Tabs.First();
            var tab1 = new Tab(new Password { RecordID = Guid.NewGuid() });
            var tab2 = new Tab(new Password { RecordID = Guid.NewGuid() });
            viewModel.OpenTab(tab1);
            viewModel.OpenTab(tab2);

            viewModel.CloseTabCommand.Execute(new SCommandArgs(null, null, tab1));
            viewModel.CloseTabCommand.Execute(new SCommandArgs(null, null, tab2));

            Assert.That(viewModel.SelectedTab, Is.EqualTo(firstTab));
        }

        /// <summary>Testet, dass die Methode OpenTab keinen neuen Tab erzeugt, falls dieser schon existiert.</summary>
        [Test(Description = "Testet, dass die Methode OpenTab keinen neuen Tab erzeugt, falls dieser schon existiert.")]
        [Repeat(Tests)]
        public void SelectTabAddNoTabIfAlreadyExists()
        {
            AddSomeRandomTabs();
            var tab = new Tab(new Password { RecordID = Guid.NewGuid() });
            viewModel.OpenTab(tab);

            var countBefore = viewModel.Tabs.Count;
            viewModel.OpenTab(tab);
            Assert.AreEqual(viewModel.Tabs.Count, countBefore);
        }

        /// <summary>Prüft ob die Methode SelectTab einen neuen Tab hinzufügt, falls dieser nicht existiert.</summary>
        [Test(Description = "Prüft ob die Methode SelectTab einen neuen Tab hinzufügt, falls dieser nicht existiert.")]
        [Repeat(Tests)]
        public void SelectTabAddsNewTabIfNotExists()
        {
            var countBefore = viewModel.Tabs.Count;
            var tab = new Tab(new Password { RecordID = Guid.NewGuid() });
            viewModel.OpenTab(tab);
            Assert.Greater(viewModel.Tabs.Count, countBefore);
        }

        /// <summary>Prüft, ob der SelectTabCommand wirklich einen Tab selektiert.</summary>
        [Test(Description = "Prüft, ob der SelectTabCommand wirklich einen Tab selektiert.")]
        [Repeat(Tests)]
        public void SelectTabCommandSelectsTab()
        {
            var tab1 = new Tab(new Password { RecordID = Guid.NewGuid() });
            var tab2 = new Tab(new Password { RecordID = Guid.NewGuid() });
            viewModel.OpenTab(tab1);
            viewModel.OpenTab(tab2);

            Assert.That(viewModel.SelectedTab, Is.EqualTo(tab2));
            viewModel.SelectTabCommand.Execute(new SCommandArgs(null, null, tab1));
            Assert.That(viewModel.SelectedTab, Is.EqualTo(tab1));

            viewModel.SelectTabCommand.Execute(new SCommandArgs(null, null, tab1));
            Assert.That(viewModel.SelectedTab, Is.EqualTo(tab1));
        }

        /// <summary>Prüft, ob die Eigenschaft SelectedTab nicht null ist.</summary>
        [Test(Description = "Prüft, ob die Eigenschaft SelectedTab nicht null ist.")]
        [Repeat(Tests)]
        public void SelectedTabIsSet()
        {
            Assert.IsNotNull(this.viewModel.SelectedTab);
        }

        /// <summary>Testet, dass die Methode Send keine Ausnahme wirft wenn ihr null übergeben wird.</summary>
        [Test(Description = "Testet, dass die Methode Send keine Ausnahme wirft wenn ihr null übergeben wird.")]
        [Repeat(Tests)]
        public void SendDoesNotThrowOnNull()
        {
            Assert.DoesNotThrow(() => viewModel.Send(null));
        }

        /// <summary>Prüft ob das setzen des Containers PropertyChanged auslöst.</summary>
        [Test(Description = "Prüft ob das setzen des Containers PropertyChanged auslöst.")]
        [Repeat(Tests)]
        public void SetContainerRaisesPropertyChanged()
        {
            var container = new UnityContainer();
            var changes = new List<string>();
            viewModel.PropertyChanged += (sender, arguments) => changes.Add(arguments.PropertyName);
            viewModel.Container = container;

            Assert.That(viewModel.Container, Is.EqualTo(container));
        }

        /// <summary>Initialisiert vor jedem Test die Testumgebung.</summary>
        [SetUp]
        [Repeat(Tests)]
        public void Setup()
        {
            var database = string.Format("{0}.db", Guid.NewGuid());
            var model = new SqlitePanelModel(database);
            this.viewModel = new MainViewModel(model);
        }

        /// <summary>Prüft, ob die Eigenschaft Tabs, nach Initialisierung genau einen Eintrag besitzt.</summary>
        [Test(Description = "Prüft, ob die Eigenschaft Tabs, nach Initialisierung genau einen Eintrag besitzt.")]
        [Repeat(Tests)]
        public void TabsHasOneItemAtStart()
        {
            Assert.IsTrue(this.viewModel.Tabs.Count == 1);
        }

        #endregion

        #region Methods

        /// <summary>Fügt dem ViewModel ein paar uufällige Tabs hinzu.</summary>
        private void AddSomeRandomTabs()
        {
            for (var i = 0; i < 10; i++)
            {
                viewModel.OpenPassword(new Password { RecordID = Guid.NewGuid(), Account = i.ToString(), Description = i.ToString(), Key = i.ToString() });
            }
        }

        #endregion
    }
}