namespace Tresor.UnitTesting.Tresor.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    using NUnit.Framework;

    using global::Tresor.Contracts.Utilities;

    using global::Tresor.Framework.MVVM;

    using global::Tresor.Utilities;

    using global::Tresor.Utilities.Mediator;

    using global::Tresor.ViewModel;

    /// <summary>Testet das PasswordListViewModel,</summary>
    [TestFixture(Description = "Testet das PasswordListViewModel.")]
    public class PasswordListViewModelTests : Test
    {
        #region Fields

        /// <summary>Eine Instanz des <see cref="PasswordListViewModel"/>.</summary>
        private readonly PasswordListViewModel viewModel;

        #endregion

        #region Constructors and Destructors

        /// <summary> Initialisiert eine neue Instanz der <see cref="PasswordListViewModelTests"/> Klasse. </summary>
        public PasswordListViewModelTests()
        {
            var mediator = new PasswordMediator();
            viewModel = new PasswordListViewModel(mediator);
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>Prüft ob das ViewModel keine Ausnahme wirft wenn der Paremeter gültig ist.</summary>
        [Test(Description = "Prüft ob das ViewModel keine Ausnahme wirft wenn der Paremeter gültig ist.")]
        [Repeat(Tests)]
        public void OpenTabCommandDoesNotThrowIfParameterIsValid()
        {
            Assert.DoesNotThrow(() => viewModel.OpenTabCommand.Execute(new SCommandArgs(null, null, new Password())));
        }

        /// <summary>Prüft ob das ViewModel eine Ausnahme wirft wenn null als Parameter übergeben wird.</summary>
        [Test(Description = "Prüft ob das ViewModel eine Ausnahme wirft wenn null als Parameter übergeben wird.")]
        [Repeat(Tests)]
        public void OpenTabCommandThrowsExceptionOnNullParameter()
        {
            Assert.Throws<NullReferenceException>(() => viewModel.OpenTabCommand.Execute(null));
        }

        /// <summary>Prüft ob das setzen von Passwörtern die Überwachung der Auflistung startet.</summary>
        [Test(Description = "Prüft ob das setzen von Passwörtern die Überwachung der Auflistung startet.")]
        [Repeat(Tests)]
        public void SetPasswordsObservePasswords()
        {
            var changes = new List<string>();

            var pw1 = new Password { RecordID = Guid.NewGuid(), Account = "Blubb1" };
            var pw2 = new Password { RecordID = Guid.NewGuid(), Account = "Blubb2" };
            var pw3 = new Password { RecordID = Guid.NewGuid(), Account = "Blubb3" };

            viewModel.Passwords = new ObservableCollection<IPassword> { pw1, pw2, pw3 };

            viewModel.PropertyChanged += (sender, arguments) => changes.Add(arguments.PropertyName);

            viewModel.Passwords.First().Key = "1234";
            Assert.That(changes.Contains("Passwords"));
        }

        /// <summary>Prüft ob das setzen der Eigenschaft Passwords PropertyChanged auslöst.</summary>
        [Test(Description = "Prüft ob das setzen der Eigenschaft Passwords PropertyChanged auslöst.")]
        [Repeat(Tests)]
        public void SetPasswordsRaisesPropertyChanged()
        {
            var changes = new List<string>();
            viewModel.PropertyChanged += (sender, arguments) => changes.Add(arguments.PropertyName);
            viewModel.Passwords = new ObservableCollection<IPassword>();
            Assert.That(changes.Contains("Passwords"));
        }

        /// <summary>Prüft ob die Methode Recieve keine Ausnahme wirft bei ungültigem Parameter.</summary>
        [Test(Description = "Prüft ob die Methode Recieve keine Ausnahme wirft bei ungültigem Parameter.")]
        [Repeat(Tests)]
        public void RecieveDoesNotThrowOnInvalidParameter()
        {
            Assert.DoesNotThrow(() => viewModel.Recieve(null));
        }

        #endregion
    }
}