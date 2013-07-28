namespace Tresor.UnitTesting.Utilities
{
    using System.Collections.Generic;

    using NUnit.Framework;

    using global::Tresor.Utilities;

    /// <summary>Testet die Klasse Tab.</summary>
    [TestFixture(Description = "Testet die Klasse Tab.")]
    public class TabTests
    {
        #region Public Methods and Operators

        [Test(Description = "Testet ob eine Änderung an IsSelected PropertyChanged auslöst.")]
        public void IsSelectedChangedRaisesPropertyChanged()
        {
            var changes = new List<string>();
            var tab = new Tab(new Password());
            tab.PropertyChanged += (sender, arguments) => changes.Add(arguments.PropertyName);
            tab.IsSelected = true;

            Assert.That(changes.Count, Is.EqualTo(1));
            Assert.Contains("IsSelected", changes);
        }

        #endregion
    }
}