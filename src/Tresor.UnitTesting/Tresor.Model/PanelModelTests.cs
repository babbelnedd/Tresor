namespace Tresor.UnitTesting.Tresor.Model
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;

    using NUnit.Framework;

    using global::Tresor.Contracts.Utilities;

    using global::Tresor.Model;

    using global::Tresor.Utilities;

    /// <summary>Testklasse für das <see cref="PanelModel"/>.</summary>
    [TestFixture]
    public class PanelModelTests : ModelTest
    {
        #region Öffentliche Methoden und Operatoren

        /// <summary>Prüft ob die Methode AddPassword wirklich ein Passwort hinzufügt.</summary>
        [Test(Description = "Prüft ob die Methode AddPassword wirklich ein Passwort hinzufügt.")]
        [Repeat(Tests)]
        public void AddPasswordIncreaseCount()
        {
            var oldCount = Model.Passwords.Count;
            var newPassword = new Password
                                  {
                                      Account = Guid.NewGuid().ToString(),
                                      Description = Guid.NewGuid().ToString(),
                                      IsDirty = false,
                                      Key = Guid.NewGuid().ToString()
                                  };
            Model.AddPassword(newPassword);
            var newCount = Model.Passwords.Count();
            Assert.IsTrue(oldCount + 1 == newCount, "Es sind nicht mehr Passwörter vorhanden.");
            Assert.Contains(newPassword, Model.Passwords, "Die Auflistung von Passwörtern enthält das neue Passwort nicht.");
        }

        /// <summary>Prüft, ob das Model einen Ordner für die Passwörterverwaltung angelegt hat.</summary>
        [Test(Description = "Prüft, ob das Model einen Ordner für die Passwörterverwaltung angelegt hat.")]
        [Repeat(Tests)]
        public void FolderIsCreated()
        {
            var storageFolder = Path.GetDirectoryName(AppSettings.GetStorageFile());
            Assert.IsNotNull(storageFolder);
            Assert.IsNotEmpty(storageFolder);
            var folderExists = Directory.Exists(storageFolder);
            Assert.True(folderExists);
        }

        /// <summary>Prüft, ob IsDirty korrekt arbeitet.</summary>
        [Test(Description = "Prüft, ob IsDirty korrekt arbeitet.")]
        [Repeat(Tests)]
        public void IsDirtyWorksCorrectly()
        {
            var anyPasswordDirty = Model.Passwords.Any(pwd => pwd.IsDirty);
            Assert.IsTrue(anyPasswordDirty == Model.IsDirty);
        }

        /// <summary>Testet ob die Methode IsKeyCorrect false bei falschem Schlüssel zurückgibt.</summary>
        [Test(Description = "Testet ob die Methode IsKeyCorrect false bei falschem Schlüssel zurückgibt.")]
        [Repeat(Tests)]
        public void IsKeyCorrectReturnsFalseOnWrongKey()
        {
            var newPasswords = new ObservableCollection<IPassword> { new Password { Account = Guid.NewGuid().ToString() } };
            var key = new Random().Next(999999, 99999999).ToString();
            var secondKey = new Random().Next(0, 1000).ToString();

            Model.Save(newPasswords, key);
            Assert.IsFalse(Model.IsKeyCorrect(secondKey));
        }

        /// <summary>Testet ob die Methode IsKeyCorrect true bei richtigem Schlüssel zurückgibt.</summary>
        [Test(Description = "Testet ob die Methode IsKeyCorrect true bei richtigem Schlüssel zurückgibt.")]
        [Repeat(Tests)]
        public void IsKeyCorrectReturnsTrueOnCorrectKey()
        {
            var newPasswords = new ObservableCollection<IPassword> { new Password { Account = Guid.NewGuid().ToString() } };
            var key = new Random().Next(999999, 99999999).ToString();
            Model.Save(newPasswords, key);
            Assert.IsTrue(Model.IsKeyCorrect(key));
        }

        /// <summary>Prüft, dass das Model zu Beginn nicht schmutzig ist.</summary>
        [Test(Description = "Prüft, dass das Model zu Beginn nicht schmutzig ist.")]
        [Repeat(Tests)]
        public void NotDirtyOnStart()
        {
            Model = new PanelModel();
            Assert.IsFalse(Model.IsDirty, "Das Model ist zu Beginn schmutzig.");
        }

        /// <summary>Prüft, dass die Eigenschaft Passwords niemals null zurückgibt.</summary>
        [Test(Description = "Prüft, dass die Eigenschaft Passwords niemals null zurückgibt.")]
        [Repeat(Tests)]
        public void PasswordReturnsNeverNull()
        {
            Assert.IsNotNull(Model.Passwords, "Passwords gibt null zurück.");
        }

        /// <summary>Testet ob das setzen der Eigenschaft Passwords OnPropertyChanged auslöst.</summary>
        [Test(Description = "Testet ob das setzen der Eigenschaft Passwords OnPropertyChanged auslöst.")]
        [Repeat(Tests)]
        public void PasswordsRaiseOnPropertyChanged()
        {
            var changes = new List<string>();
            Model.PropertyChanged += (sender, arguments) => changes.Add(arguments.PropertyName);

            var newPasswords = new ObservableCollection<IPassword> { new Password { Account = Guid.NewGuid().ToString() } };
            Model.Save(newPasswords);

            Assert.IsNotEmpty(changes);
            Assert.IsTrue(changes.Count == 1);
            Assert.IsTrue(changes[0] == "Passwords");
        }

        /// <summary>Prüft, ob PropertyChanged ausgelöst wird, wenn sich ein Passwort geändert hat.</summary>
        [Test(Description = "Prüft, ob PropertyChanged ausgelöst wird, wenn sich ein Passwort geändert hat.")]
        [Repeat(Tests)]
        public void PropertyChangedIfPasswordChanges()
        {
            var testSuccesful = false;
            IPassword password = new Password { Account = "TestAccount", Description = "Desc", IsDirty = false, Key = "StrengGeheim" };
            Model.AddPassword(password);
            var x = 0;
            Model.PropertyChanged += (obj, sender) =>
                {
                    testSuccesful = true;
                    x++;
                };

            var newPassword = Model.Passwords.Single(pwd => pwd == password);
            newPassword.Key = "AnderesPasswort";

            Assert.IsTrue(testSuccesful, "PropertyChanged wurde nicht ausgelöst.");
            Assert.IsTrue(x == 1, "PropertyChanged wurde mehr als einmal ausgelöst.");
        }

        #endregion

        /// <summary>Die Methode Save wirft eine ArgumentNullException falls der Parameter Passwords null ist.</summary>
        [Test(Description = "Die Methode Save wirft eine ArgumentNullException falls der Parameter Passwords null ist.")]
        [Repeat(Tests)]
        public void SaveThrowsArgumentException()
        {
            Assert.Throws<ArgumentNullException>(() => Model.Save(null));
            Assert.Throws<ArgumentNullException>(() => Model.Save(null, "1234"));
        }
    }
}