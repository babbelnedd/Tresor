namespace Tresor.UnitTesting.Resources.Converter
{
    using System;
    using System.Collections.ObjectModel;
    using System.Windows;

    using NUnit.Framework;

    using global::Tresor.Contracts.Utilities;

    using global::Tresor.Resources.Converter;

    using global::Tresor.Utilities;

    /// <summary>Testet den ContentToToolTipConverter.</summary>
    [TestFixture(Description = "Testet den ContentToToolTipConverter.")]
    public class ContentIsDirtyToVisibilityTests : Test
    {
        #region Fields

        /// <summary>Eine Instanz des <see cref="ContentIsDirtyToVisibility"/>.</summary>
        private ContentIsDirtyToVisibility converter;

        #endregion

        #region Constructors and Destructors

        /// <summary>Initialisiert eine neue Instanz der <see cref="ContentIsDirtyToVisibilityTests"/> Klasse.</summary>
        public ContentIsDirtyToVisibilityTests()
        {
            converter = new ContentIsDirtyToVisibility();
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>Überprüft ob die Methode ConvertBack immer null zurückgibt.</summary>
        [Test(Description = "Überprüft ob die Methode ConvertBack immer null zurückgibt.")]
        [Repeat(Tests)]
        public void ConvertBackReturnsNull()
        {
            Assert.That(converter.ConvertBack(null, null, null, null), Is.Null);
            Assert.That(converter.ConvertBack(Visibility.Visible, null, null, null), Is.Null);
            Assert.That(converter.ConvertBack(Visibility.Collapsed, null, null, null), Is.Null);
            Assert.That(converter.ConvertBack(Visibility.Hidden, null, null, null), Is.Null);
        }

        /// <summary>Prüft ob ein Passwort mit ausstehenden Änderungen zu Visible konvertiert wird.</summary>
        [Test(Description = "Prüft ob ein Passwort mit ausstehenden Änderungen zu Visible konvertiert wird.")]
        [Repeat(Tests)]
        public void ConvertDirtyPasswordToVisible()
        {
            var pw = new Password { IsDirty = true };
            var result = converter.Convert(pw, null, null, null);
            Assert.That(result, Is.EqualTo(Visibility.Visible));
        }

        /// <summary>Überprüft ob eine Auflistung von Passwörtern immer zu Collapsed konvertiert wird.</summary>
        [Test(Description = "Überprüft ob eine Auflistung von Passwörtern immer zu Collapsed konvertiert wird.")]
        [Repeat(Tests)]
        public void ConvertListOfPasswordsToCollapsed()
        {
            var pw = new Password { IsDirty = false };
            var result = converter.Convert(pw, null, null, null);
            Assert.That(result, Is.EqualTo(Visibility.Collapsed));
        }

        /// <summary>Prüft ob ein unverändertes Passwort zu Collapsed konvertiert wird.</summary>
        [Test(Description = "Prüft ob ein unverändertes Passwort zu Collapsed konvertiert wird.")]
        [Repeat(Tests)]
        public void ConvertNotDirtyPasswordToCollapsed()
        {
            var list = new ObservableCollection<IPassword>();
            var result = converter.Convert(list, null, null, null);
            Assert.That(result, Is.EqualTo(Visibility.Collapsed));
        }

        /// <summary>Prüft ob die Methode Convert eine Ausnahme wirft, falls der erste Parameter falsch ist.</summary>
        [Test(Description = "Prüft ob die Methode Convert eine Ausnahme wirft, falls der erste Parameter falsch ist.")]
        [Repeat(Tests)]
        public void ConvertWithInvalidParameterThrowsException()
        {
            Assert.Throws<ArgumentException>(() => converter.Convert(null, null, null, null));
            Assert.Throws<ArgumentException>(() => converter.Convert(new object(), null, null, null));
        }

        #endregion
    }
}