namespace Tresor.UnitTesting.Utilities
{
    using System;
    using System.Collections.Generic;

    using global::Tresor.Contracts.Utilities;

    using NUnit.Framework;

    using global::Tresor.Utilities;

    /// <summary>Testet die Klasse Tab.</summary>
    [TestFixture(Description = "Testet die Klasse Tab.")]
    public class TabTests : Test
    {
        #region Public Methods and Operators

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

        #endregion

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
    }
}