namespace Tresor.UnitTesting.Resources.Converter
{
    using System.Windows;

    using NUnit.Framework;

    using global::Tresor.Resources.Converter;

    /// <summary>Testet den BooleanToVisibilityConverter.</summary>
    [TestFixture(Description = "Testet den BooleanToVisibilityConverter.")]
    public class NegativeBooleanToVisibilityConverterTests : Test
    {
        #region Fields

        /// <summary>Eine Instanz des BooleanToVisibilityConverter.</summary>
        private NegativeBooleanToVisibilityConverter converter;

        #endregion

        #region Public Methods and Operators

        /// <summary>Prüft ob die Methode ConvertBack True bei Collapsed zurückgibt.</summary>
        [Test(Description = "Prüft ob die Methode ConvertBack True bei Collapsed zurückgibt.")]
        [Repeat(Tests)]
        public void ConvertBackCollapsed()
        {
            Assert.That(converter.ConvertBack(Visibility.Collapsed, null, null, null), Is.True);
        }

        /// <summary>Prüft ob die Methode ConvertBack False bei Visible zurückgibt.</summary>
        [Test(Description = "Prüft ob die Methode ConvertBack False bei Visible zurückgibt.")]
        [Repeat(Tests)]
        public void ConvertBackVisible()
        {
            Assert.That(converter.ConvertBack(Visibility.Visible, null, null, null), Is.False);
        }

        /// <summary>Prüft ob die Methode Convert Visible bei False zurückgibt.</summary>
        [Test(Description = "Prüft ob die Methode Convert Visible bei False zurückgibt.")]
        [Repeat(Tests)]
        public void ConvertFalse()
        {
            Assert.That(converter.Convert(false, null, null, null), Is.EqualTo(Visibility.Visible));
        }

        /// <summary>Prüft ob die Methode Convert Collapsed bei True zurückgibt.</summary>
        [Test(Description = "Prüft ob die Methode Convert Collapsed bei True zurückgibt.")]
        [Repeat(Tests)]
        public void ConvertTrue()
        {
            Assert.That(converter.Convert(true, null, null, null), Is.EqualTo(Visibility.Collapsed));
        }

        /// <summary>Initialisiert vor jedem Test die Testumgebung.</summary>
        [SetUp]
        public void Setup()
        {
            converter = new NegativeBooleanToVisibilityConverter();
        }

        #endregion
    }
}