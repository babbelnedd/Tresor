namespace Tresor.UnitTesting.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    using NUnit.Framework;

    using global::Tresor.Contracts.Utilities;

    using global::Tresor.Utilities;

    /// <summary>Testet die Klasse Tab.</summary>
    [TestFixture(Description = "Testet die Klasse Tab.")]
    [Category("UtilitiesTest")]
    public class TabTests : Test
    {
        #region Public Methods and Operators

        /// <summary>Testet ob eine Änderung an Content PropertyChanged auslöst.</summary>
        [Test(Description = "Testet ob eine Änderung an Content PropertyChanged auslöst.")]
        [Repeat(Tests)]
        public void IfContentIsIPasswordPropertyChangedRaisesOnChanges()
        {
            var changes = new List<string>();
            var tab = new Tab(new Password());
            tab.PropertyChanged += (sender, arguments) => changes.Add(arguments.PropertyName);
            ((IPassword)tab.Content).Key = Guid.NewGuid().ToString();
            Assert.That(changes.Contains("Content"));
        }

        /// <summary>Prüft ob die Eigenschaft IsCloseable nach der Erzeugung eines Tabs True ist.</summary>
        [Test(Description = "Prüft ob die Eigenschaft IsCloseable nach der Erzeugung eines Tabs True ist.")]
        [Repeat(Tests)]
        public void IsCloseableIsTrueOnCreation()
        {
            var tab = new Tab(new Password());
            Assert.That(tab.IsCloseable);

            tab = new Tab(new ObservableCollection<IPassword> { new Password() });
            Assert.That(tab.IsCloseable);
        }

        /// <summary>Prüft ob das verändern von IsCloseable PropertyChanged auslöst.</summary>
        [Test(Description = "Prüft ob das verändern von IsCloseable PropertyChanged auslöst.")]
        [Repeat(Tests)]
        public void IsCloseableRaisesPropertyChanged()
        {
            var tab = new Tab(new Password());
            var changes = new List<string>();
            tab.PropertyChanged += (sender, arguments) => changes.Add(arguments.PropertyName);

            tab.IsCloseable = false;

            Assert.That(changes.Count, Is.EqualTo(1));
            Assert.That(changes.Contains("IsCloseable"));
        }

        /// <summary>Testet ob eine Änderung an IsSelected PropertyChanged auslöst.</summary>
        [Test(Description = "Testet ob eine Änderung an IsSelected PropertyChanged auslöst.")]
        [Repeat(Tests)]
        public void IsSelectedChangedRaisesPropertyChanged()
        {
            var changes = new List<string>();
            var tab = new Tab(new Password());
            tab.PropertyChanged += (sender, arguments) => changes.Add(arguments.PropertyName);
            tab.IsSelected = true;

            Assert.That(changes.Contains("IsSelected"));
        }

        /// <summary>Testet ob IsSelected False ist nach der Erstellung eines Tabs.</summary>
        [Test(Description = "Testet ob IsSelected False ist nach der Erstellung eines Tabs.")]
        [Repeat(Tests)]
        public void IsSelectedIsFalseOnCreation()
        {
            var tab = new Tab(new Password());
            Assert.That(!tab.IsSelected);
        }

        /// <summary>Testet ob das Setzen von Content PropertyChanged auslöst.</summary>
        [Test(Description = "Testet ob das Setzen von Content PropertyChanged auslöst.")]
        [Repeat(Tests)]
        public void SetContentRaisesPropertyChanged()
        {
            var changes = new List<string>();
            var tab = new Tab(new Password());
            tab.PropertyChanged += (sender, arguments) => changes.Add(arguments.PropertyName);
            tab.Content = null;
            Assert.That(changes.Contains("Content"));
        }

        #endregion
    }
}