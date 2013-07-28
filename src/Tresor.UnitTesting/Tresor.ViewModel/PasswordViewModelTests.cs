namespace Tresor.UnitTesting.Tresor.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.IO.Packaging;

    using global::Tresor.Framework.MVVM;

    using NUnit.Framework;

    using global::Tresor.Model;

    using global::Tresor.Utilities;

    using global::Tresor.Utilities.Mediator;

    using global::Tresor.ViewModel;

    /// <summary>Testet das PasswordViewModel.</summary>
    [TestFixture(Description = "Testet das PasswordViewModel.")]
    public class PasswordViewModelTests : Test
    {
        #region Fields

        /// <summary>Eine Instanz des <see cref="PasswordViewModel"/>.</summary>
        private PasswordViewModel viewModel;

        #endregion

        #region Constructors and Destructors

        /// <summary>Initialisiert eine neue Instanz der <see cref="PasswordViewModelTests"/> Klasse.</summary>
        public PasswordViewModelTests()
        {
            var model = new SqlitePanelModel("x.db");
            model.IsKeyCorrect("1234");
            var mediator = new PasswordMediator();
            viewModel = new PasswordViewModel(model, mediator);
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>Prüft ob das Setzen eines Passworts PropertyChanged auslöst.</summary>
        [Test(Description = "Prüft ob das Setzen eines Passworts PropertyChanged auslöst.")]
        [Repeat(Tests)]
        public void SetPasswordRaisesPropertyChanged()
        {
            var changes = new List<string>();
            viewModel.PropertyChanged += (sender, arguments) => changes.Add(arguments.PropertyName);
            viewModel.Password = new Password();
            Assert.That(changes.Contains("Password"));
        }

        /// <summary>Testet ob die Methode Recieve die Eigenschaft Password füllt.</summary>
        [Test(Description = "Testet ob die Methode Recieve die Eigenschaft Password füllt.")]
        [Repeat(Tests)]
        public void RecieveSetPassword()
        {
            var pw = new Password { RecordID = Guid.NewGuid() };
            viewModel.Recieve(pw);
            Assert.That(viewModel.Password, Is.EqualTo(pw));
        }

        /// <summary>Testet ob die Methode Recieve eine Ausnahme wirft, falls der übergebene Parameter ungültig ist.</summary>
        [Test(Description = "Testet ob die Methode Recieve eine Ausnahme wirft, falls der übergebene Parameter ungültig ist.")]
        [Repeat(Tests)]
        public void RecieveDoesThrowOnInvalidArgument()
        {
            Assert.Throws<NullReferenceException>(() => viewModel.Recieve(null));
        }

        /// <summary>Prüft ob die Methode Send keine Ausnahme wirft, wenn der übergebene Parameter null ist.</summary>
        [Test(Description = "Prüft ob die Methode Send keine Ausnahme wirft, wenn der übergebene Parameter null ist.")]
        [Repeat(Tests)]
        public void SendDoesNotThrowExceptionIfParameterIsNull()
        {
            Assert.DoesNotThrow(() => viewModel.Send(null));
        }

        /// <summary>Prüft ob eine Änderung am Model PropertyChanged für die Auflistung Passwords auslöst.</summary>
        [Test(Description = "Prüft ob eine Änderung am Model PropertyChanged für die Auflistung Passwords auslöst.")]
        [Repeat(Tests)]
        public void ModelChangedRaisesPropertyChanged()
        {
            var model = new SqlitePanelModel("1234.db");
            model.IsKeyCorrect("1234");
            var viewModel = new PasswordViewModel(model, new PasswordMediator());

            var changes = new List<string>();
            viewModel.PropertyChanged += (sender, arguments) => changes.Add(arguments.PropertyName);
            model.AddPassword(new Password { RecordID = Guid.NewGuid() });
            Assert.That(changes.Contains("Password"));
        }

        [Test(Description = "Überprüft ob eine Änderung am Passwort PropertyChanged auslöst")]
        [Repeat(Tests)]
        public void PasswordChangedRaisesProeprtyChanged()
        {
            viewModel.Password = new Password
                                     {
                                         RecordID = Guid.NewGuid(),
                                         Account = "00"
                                     };
            var changes = new List<string>();
            viewModel.PropertyChanged += (sender, arguments) => changes.Add(arguments.PropertyName);
            viewModel.Password.Key = "1234";
            Assert.That(changes.Contains("Password"));
        }

        [Test(Description = "")]
        [Repeat(Tests)]
        public void SaveDoesNotThrow()
        {
            var newPw = new Password { RecordID = Guid.NewGuid(), Account = "TestAccount" };
            Assert.DoesNotThrow(() =>viewModel.SavePasswordCommand.Execute(new SCommandArgs(null, null, newPw)));
        }

        [Test(Description = "")]
        [Repeat(Tests)]
        public void Save()
        {
            var newPw = new Password { RecordID = Guid.NewGuid(), Account = "TestAccount" };
            viewModel.SavePasswordCommand.Execute(new SCommandArgs(null, null, newPw));

             var newModel = new SqlitePanelModel("x.db");
            newModel.IsKeyCorrect("1234");

            newModel.Passwords.Contains(newPw);
        }

        #endregion
    }
}