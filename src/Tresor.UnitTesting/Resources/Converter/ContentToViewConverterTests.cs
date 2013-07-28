namespace Tresor.UnitTesting.Resources.Converter
{
    using System;
    using System.Collections.ObjectModel;
    using System.Windows.Controls;

    using Microsoft.Practices.Unity;

    using NUnit.Framework;

    using global::Tresor.Contracts.Utilities;

    using global::Tresor.Model;

    using global::Tresor.Resources.Converter;

    using global::Tresor.Utilities;

    using global::Tresor.View;

    using global::Tresor.View.UserControls;

    using global::Tresor.ViewModel;

    /// <summary>Tests für den ContentToViewConverter.</summary>
    [TestFixture(Description = "Tests für den ContentToViewConverter.")]
    public class ContentToViewConverterTests : Test
    {
        #region Fields

        /// <summary>Eine Instanz des NameToViewConverters.</summary>
        private ContentToViewConverter converter;

        #endregion

        #region Public Methods and Operators

        /// <summary>Prüft ob die Methode ConvertBack null zurück gibt.</summary>
        [Test(Description = "Prüft ob die Methode ConvertBack null zurück gibt.")]
        [Repeat(Tests)]
        public void ConvertBack()
        {
            Assert.IsNull(converter.ConvertBack(null, null, null, null));
        }

        /// <summary>Prüft ob die Methode Convert ein IPassword zu einer View konvertiert.</summary>
        [Test(Description = "Prüft ob die Methode Convert ein IPassword zu einer View konvertiert.")]
        [Repeat(Tests)]
        [STAThread]
        public void ConvertIPassword()
        {
            var container = new UnityContainer();
            container.RegisterInstance<UserControl>("PasswordView", new PasswordView());
            container.RegisterInstance("PasswordViewModel", new PasswordViewModel(new SqlitePanelModel("x.db"), null));

            var values = new object[2];

            values[0] = container;
            values[1] = new Password { RecordID = Guid.NewGuid(), Account = "TestAccount", Description = "TestDescription", Key = "TestPasswort" };

            Assert.DoesNotThrow(() => converter.Convert(values, null, null, null));
            Assert.IsTrue(converter.Convert(values, null, null, null) is PasswordView);
        }

        /// <summary>Prüft ob die Methode Convert eine ObservableCollection vom Typ IPassword zu einer View konvertiert.</summary>
        [Test(Description = "Prüft ob die Methode Convert eine ObservableCollection<IPassword> zu einer View konvertiert.")]
        [Repeat(Tests)]
        [STAThread]
        public void ConvertObservableCollectionOfIPassword()
        {
            var container = new UnityContainer();
            container.RegisterInstance<UserControl>("PasswordListView", new PasswordListView());
            container.RegisterInstance("PasswordListViewModel", new PasswordListViewModel(null));

            var values = new object[2];

            values[0] = container;
            var pw = new Password { RecordID = Guid.NewGuid(), Account = "TestAccount", Description = "TestDescription", Key = "TestPasswort" };
            var list = new ObservableCollection<IPassword> { pw };
            values[1] = list;

            Assert.DoesNotThrow(() => converter.Convert(values, null, null, null));
            Assert.IsTrue(converter.Convert(values, null, null, null) is PasswordListView);
        }

        /// <summary>Prüft ob die Methode Convert null zurückgibt, wenn values null ist</summary>
        [Test(Description = "Prüft ob die Methode Convert null zurückgibt, wenn values null ist.")]
        [Repeat(Tests)]
        public void ConvertReturnsNullIfValuesCountIsNot2()
        {
            var arguments = new object[2];
            arguments[0] = new UnityContainer();
            arguments[1] = new Password();
            arguments[0] = string.Empty;

            Assert.That(converter.Convert(arguments, null, null, null), Is.Null);
        }

        /// <summary>Prüft ob die Methode Convert null zurückgibt, wenn values null ist</summary>
        [Test(Description = "Prüft ob die Methode Convert null zurückgibt, wenn values null ist.")]
        [Repeat(Tests)]
        public void ConvertReturnsNullIfValuesIsNull()
        {
            Assert.That(converter.Convert(null, null, null, null), Is.Null);
        }

        /// <summary>Prüft ob die Methode Convert null zurückgibt, wenn der erste Wert im Array nicht vom Typ IUnityContainer ist.</summary>
        [Test(Description = "Prüft ob die Methode Convert null zurückgibt, wenn der erste Wert im Array nicht vom Typ IUnityContainer ist.")]
        [Repeat(Tests)]
        public void ConvertReturnsNullValueIsWrong()
        {
            var arguments = new object[2];
            arguments[0] = new object();
            arguments[1] = new Password();

            Assert.That(converter.Convert(arguments, null, null, null), Is.Null);
        }

        /// <summary>Initialisiert vor jedem Test die Testumgebung.</summary>
        [SetUp]
        public void Setup()
        {
            converter = new ContentToViewConverter();
        }

        #endregion
    }
}