namespace Tresor.UnitTesting.Resources.Converter
{
    using System;
    using System.Collections.Generic;
    using System.Windows;

    using NUnit.Framework;

    using global::Tresor.Contracts.Utilities;

    using global::Tresor.Resources.Converter;

    using global::Tresor.Utilities;

    /// <summary>Testet den TabContentToVisibilityConverter</summary>
    [TestFixture(Description = "Testet den TabContentToVisibilityConverter.")]
    public class TabContentToVisibilityConverterTests : Test
    {
        #region Fields

        /// <summary>Eine Instanz des TabContentToVisibilityConverter</summary>
        private TabContentToVisibilityConverter converter;

        #endregion

        #region Public Methods and Operators

        /// <summary>Prüft ob die Methode ConvertBack immer null zurückgibt.</summary>
        [Test(Description = "Prüft ob die Methode ConvertBack immer null zurückgibt.")]
        [Repeat(Tests)]
        public void ConvertBackReturnsNull()
        {
            Assert.IsNull(converter.ConvertBack(Visibility.Visible, null, null, null));
            Assert.IsNull(converter.ConvertBack(Visibility.Collapsed, null, null, null));
            Assert.IsNull(converter.ConvertBack(Visibility.Hidden, null, null, null));
        }

        /// <summary>Prüft ob die Methode Convert Collapsed zurückgibt, wenn der Content eine Auflistung IPassword ist.</summary>
        [Test(Description = "Prüft ob die Methode Convert Collapsed zurückgibt, wenn der Content eine Auflistung von IPassword ist.")]
        [Repeat(Tests)]
        public void ConvertCollectionOfIPasswordReturnsVisible()
        {
            var list = new List<IPassword> { new Password() };
            converter.Convert(list, null, null, null);
        }

        /// <summary>Prüft ob die Methode Convert Visible zurückgibt, wenn der Content IPassword ist.</summary>
        [Test(Description = "Prüft ob die Methode Convert Visible zurückgibt, wenn der Content IPassword ist.")]
        [Repeat(Tests)]
        public void ConvertIPasswordReturnsVisible()
        {
            var pw = new Password();
            converter.Convert(pw, null, null, null);
        }

        /// <summary>Prüft ob die Methode Convert eine ArgumentException wirft wenn der erste Parameter falsch ist.</summary>
        [Test(Description = "Prüft ob die Methode Convert eine ArgumentException wirft wenn der erste Parameter falsch ist.")]
        [Repeat(Tests)]
        public void ConvertTrowsExceptionIfArgumentIsWrong()
        {
            var exception = Assert.Throws<ArgumentException>(() => converter.Convert(null, null, null, null));
            Assert.That(exception.Message, Is.EqualTo("value"));
        }

        /// <summary>Initialisiert vor jedem Test die Testumgebung.</summary>
        [SetUp]
        public void Setup()
        {
            converter = new TabContentToVisibilityConverter();
        }

        #endregion
    }
}