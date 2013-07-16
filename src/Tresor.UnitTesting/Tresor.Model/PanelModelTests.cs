namespace Tresor.UnitTesting.Tresor.Model
{
    using System;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;

    using NUnit.Framework;

    using ThirdParty;

    using global::Tresor.Contracts.Model;

    using global::Tresor.Contracts.Utilities;

    using global::Tresor.Model;

    using global::Tresor.Utilities;

    /// <summary>Testklasse für das <see cref="PanelModel"/>.</summary>
    [TestFixture]
    public class PanelModelTests : Test
    {
        #region Konstanten und Felder

        /// <summary>Das <see cref="PanelModel"/>.</summary>
        private IPanelModel model = new PanelModel();

        #endregion

        #region Öffentliche Methoden und Operatoren

        /// <summary>Prüft ob die Methode AddPassword wirklich ein Passwort hinzufügt.</summary>
        [Test(Description = "Prüft ob die Methode AddPassword wirklich ein Passwort hinzufügt.")]
        public void AddPasswordIncreaseCount()
        {
            var oldCount = model.Passwords.Count;
            var newPassword = new Password
                                  {
                                      Account = Guid.NewGuid().ToString(),
                                      Description = Guid.NewGuid().ToString(),
                                      IsDirty = false,
                                      Key = Guid.NewGuid().ToString()
                                  };
            model.AddPassword(newPassword);
            var newCount = model.Passwords.Count();
            Assert.IsTrue(oldCount + 1 == newCount, "Es sind nicht mehr Passwörter vorhanden.");
            Assert.Contains(newPassword, model.Passwords, "Die Auflistung von Passwörtern enthält das neue Passwort nicht.");
        }

        /// <summary>Prüft, ob das Model einen Ordner für die Passwörterverwaltung angelegt hat.</summary>
        [Test(Description = "Prüft, ob das Model einen Ordner für die Passwörterverwaltung angelegt hat.")]
        public void FolderIsCreated()
        {
            if (Directory.Exists(OriginalFolder))
            {
                Directory.Move(OriginalFolder, BackupFolder);
            }

            new PanelModel();
            Assert.True(Directory.Exists(OriginalFolder));

            Directory.Delete(OriginalFolder);
            Directory.Move(BackupFolder, OriginalFolder);
        }

        /// <summary>Prüft, ob IsDirty korrekt arbeitet.</summary>
        [Test(Description = "Prüft, ob IsDirty korrekt arbeitet.")]
        public void IsDirtyWorksCorrectly()
        {
            var anyPasswordDirty = model.Passwords.Any(pwd => pwd.IsDirty);
            Assert.IsTrue(anyPasswordDirty == model.IsDirty);
        }

        /// <summary>Prüft, dass das Model zu Beginn nicht schmutzig ist.</summary>
        [Test(Description = "Prüft, dass das Model zu Beginn nicht schmutzig ist.")]
        public void NotDirtyOnStart()
        {
            model = new PanelModel();
            Assert.IsFalse(model.IsDirty, "Das Model ist zu Beginn schmutzig.");
        }

        /// <summary>Prüft, dass die Eigenschaft Passwords niemals null zurückgibt.</summary>
        [Test(Description = "Prüft, dass die Eigenschaft Passwords niemals null zurückgibt.")]
        public void PasswordReturnsNeverNull()
        {
            Assert.IsNotNull(model.Passwords, "Passwords gibt null zurück.");
        }

        /// <summary>Prüft, ob PropertyChanged ausgelöst wird, wenn sich ein Passwort geändert hat.</summary>
        [Test(Description = "Prüft, ob PropertyChanged ausgelöst wird, wenn sich ein Passwort geändert hat.")]
        public void PropertyChangedIfPasswordChanges()
        {
            var testSuccesful = false;
            IPassword password = new Password { Account = "TestAccount", Description = "Desc", IsDirty = false, Key = "StrengGeheim" };
            model.AddPassword(password);
            var x = 0;
            model.PropertyChanged += (obj, sender) =>
                {
                    testSuccesful = true;
                    x++;
                };

            var newPassword = model.Passwords.Single(pwd => pwd == password);
            newPassword.Key = "AnderesPasswort";

            Assert.IsTrue(testSuccesful, "PropertyChanged wurde nicht ausgelöst.");
            Assert.IsTrue(x == 1, "PropertyChanged wurde mehr als einmal ausgelöst.");
        }

        /// <summary>Prüft, ob Passwörter automatisch geladen werden.</summary>
        [Test(Description = "Prüft, ob Passwörter automatisch geladen werden.")]
        public void LoadPasswordsAutomatically()
        {
            var passwords = new ObservableCollection<IPassword>();
            var rndNumber = new Random().Next(10, 20);
            for (var i = 0; i < rndNumber; i++)
            {
                passwords.Add(new Password { Account = Guid.NewGuid().ToString() });
            }

            var serializer = new GenericCryptoClass();
            serializer.Serialize(OriginalFolder + "\\save.tsr", passwords);

            var model = new PanelModel();
            Assert.IsTrue(model.Passwords.Count == rndNumber);
        }

        #endregion
    }
}