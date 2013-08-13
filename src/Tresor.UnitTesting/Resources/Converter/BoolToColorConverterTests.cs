namespace Tresor.UnitTesting.Resources.Converter
{
    using System.Windows.Media;

    using NUnit.Framework;

    using global::Tresor.Resources.Converter;

    /// <summary>Testet den TabToColorConverter.</summary>
    [TestFixture(Description = "Testet den TabToColorConverter.")]
    [Category("ConverterTest")]
    public class BoolToColorConverterTests
    {
        #region Fields

        /// <summary>Eine Instanz des <see cref="BoolToColorConverter"/>.</summary>
        private BoolToColorConverter converter;

        #endregion

        #region Public Methods and Operators

        /// <summary>Testet ob die Methode ConvertBack null zurückgibt.</summary>
        [Test(Description = "Testet ob die Methode ConvertBack null zurückgibt.")]
        public void ConvertBack()
        {
            Assert.IsNull(converter.ConvertBack(null, null, null, null));
        }

        /// <summary>Testet die Methode Convert mit dem Wert False.</summary>
        [Test(Description = "Testet die Methode Convert mit dem Wert False.")]
        public void ConvertFalse()
        {
            Assert.DoesNotThrow(() => converter.Convert(false, null, null, null));
            var color = converter.Convert(false, null, null, null);
            Assert.DoesNotThrow(() => ColorConverter.ConvertFromString(color.ToString()));
        }

        /// <summary>Testet die Methode Convert mit dem Wert True.</summary>
        [Test(Description = "Testet die Methode Convert mit dem Wert True.")]
        public void ConvertTrue()
        {
            Assert.DoesNotThrow(() => converter.Convert(true, null, null, null));
            var color = converter.Convert(true, null, null, null);
            Assert.DoesNotThrow(() => ColorConverter.ConvertFromString(color.ToString()));
        }

        /// <summary>Initialisiert vor jedem Test die Testumgebung.</summary>
        [SetUp]
        public void Setup()
        {
            converter = new BoolToColorConverter();
        }

        #endregion
    }
}