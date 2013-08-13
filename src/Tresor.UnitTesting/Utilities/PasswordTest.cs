namespace Tresor.UnitTesting.Utilities
{
    using System;
    using System.Collections.Generic;

    using NUnit.Framework;

    using global::Tresor.Contracts.Utilities;

    using global::Tresor.Utilities;

    /// <summary>Testet die Klasse Password.</summary>
    [TestFixture(Description = "Testet die Klasse Password.")]
    [Category("UtilitiesTest")]
    public class PasswordTest : Test
    {
        #region Public Methods and Operators

        /// <summary>Prüft ob die Methode CancelEdit das Passworter auf den gespeicherten Stand zurücksetzt.</summary>
        [Test(Description = "Prüft ob die Methode CancelEdit das Passworter auf den gespeicherten Stand zurücksetzt.")]
        [Repeat(Tests)]
        public void CancelEditResetsPassword()
        {
            var key = Guid.NewGuid().ToString();
            var desc = Guid.NewGuid().ToString();
            var acc = Guid.NewGuid().ToString();
            var recordID = Guid.NewGuid();
            var pw = new Password { Account = acc, Description = desc, Key = key, RecordID = recordID };

            pw.BeginEdit();
            pw.Account = "12345.";
            pw.Key = "12345.";
            pw.Description = "12345.";
            pw.RecordID = Guid.NewGuid();
            pw.CancelEdit();

            Assert.That(pw.Account, Is.EqualTo(acc));
            Assert.That(pw.Description, Is.EqualTo(desc));
            Assert.That(pw.Key, Is.EqualTo(key));
            Assert.That(pw.RecordID, Is.EqualTo(recordID));
        }

        /// <summary>Prüft ob Änderungen am Passwort IsDirty auf True setzen.</summary>
        [Test(Description = "Prüft ob Änderungen am Passwort IsDirty auf True setzen.")]
        [Repeat(Tests)]
        public void ChangesSetIsDirtyToTrue()
        {
            var pw = GetRandomPassword();
            pw.BeginEdit();
            pw.IsDirty = true;
            pw.CancelEdit();
            Assert.That(pw.IsDirty, Is.False);
        }

        /// <summary>Testet ob die Methode IsCloneEqual True zurückgibt, wenn die Bearbeitung begonnen wurde es aber keine ausstehenden Änderungen gibt.</summary>
        [Test(Description = "Testet ob die Methode IsCloneEqual True zurückgibt, wenn die Bearbeitung begonnen wurde es aber keine ausstehenden Änderungen gibt.")]
        [Repeat(Tests)]
        public void CloneIsEqualAfterBeginEdit()
        {
            var pw = GetRandomPassword();
            pw.BeginEdit();
            Assert.That(pw.IsCloneEqual(), Is.True);
        }

        /// <summary>Prüft ob die Methode CloneIsEqual False zurückgibt wenn es ausstehende Änderungen gibt.</summary>
        [Test(Description = "Prüft ob die Methode CloneIsEqual False zurückgibt wenn es ausstehende Änderungen gibt.")]
        [Repeat(Tests)]
        public void CloneIsNotEqualAfterChanges()
        {
            var pw = GetRandomPassword();
            pw.BeginEdit();
            pw.Key = Guid.NewGuid().ToString();
            Assert.That(pw.IsCloneEqual(), Is.False);
        }

        /// <summary>Prüft ob CloneIsEqual False zurückgibt nach dem die Methode EndEdit aufgerufen wurde.</summary>
        [Test(Description = "Prüft ob CloneIsEqual False zurückgibt nach dem die Methode EndEdit aufgerufen wurde.")]
        [Repeat(Tests)]
        public void CloneIsNotEqualAfterEndEdit()
        {
            var pw = GetRandomPassword();
            pw.BeginEdit();
            pw.EndEdit();
            Assert.That(pw.IsCloneEqual(), Is.False);
        }

        /// <summary>Prüft ob IsDirty True zurück gibt nachdem die Methode EndEdit aufgerufen wurde.</summary>
        [Test(Description = "Prüft ob IsDirty True zurück gibt nachdem die Methode EndEdit aufgerufen wurde.")]
        [Repeat(Tests)]
        public void IsDirtyAfterEndEdit()
        {
            var pw = GetRandomPassword();
            pw.BeginEdit();
            pw.EndEdit();
            Assert.That(pw.IsDirty);
        }

        /// <summary>Prüft ob die Eigenschaft IsDirty mit CancelEdit auf False gesetzt wurde.</summary>
        [Test(Description = "Prüft ob die Eigenschaft IsDirty mit CancelEdit auf False gesetzt wurde.")]
        [Repeat(Tests)]
        public void IsDirtyIsFalseAfterCancelEdit()
        {
            var pw = GetRandomPassword();
            pw.BeginEdit();
            pw.Account = "1234";
        }

        /// <summary>Prüft ob das Password vom Typ IPassword ist.</summary>
        [Test(Description = "Prüft ob das Password vom Typ IPassword ist.")]
        [Repeat(Tests)]
        public void PasswordIsOfTypeIPassword()
        {
            var pw = GetRandomPassword();
            Assert.That(pw is IPassword);
        }

        /// <summary>Prüft ob alle Eigenschaften PropertyChanged bei Änderungen auslösen.</summary>
        [Test(Description = "Prüft ob alle Eigenschaften PropertyChanged bei Änderungen auslösen.")]
        [Repeat(Tests)]
        public void PropertiesRaisePropertyChanged()
        {
            var pw = GetRandomPassword();
            var changes = new List<string>();
            pw.PropertyChanged += (sender, arguments) => changes.Add(arguments.PropertyName);
            pw.RecordID = Guid.NewGuid();
            pw.Key = "12345.";
            pw.Description = "12345.";
            pw.Account = "12345.";
            pw.IsDirty = !pw.IsDirty;
            pw.IsNew = !pw.IsNew;

            Assert.That(changes.Count == 6);
        }

        /// <summary>Prüft ob die Methode ToString den Account Namen zurückgibt.</summary>
        [Test(Description = "Prüft ob die Methode ToString den Account Namen zurückgibt.")]
        [Repeat(Tests)]
        public void ToStringReturnsAccountName()
        {
            var pw = GetRandomPassword();
            Assert.That(pw.ToString(), Is.EqualTo(pw.Account));
        }

        #endregion

        #region Methods

        /// <summary>Holt ein Passwort mit zufälligen Werten.</summary>
        /// <returns>Ein neues <see cref="Password"/>.</returns>
        private static Password GetRandomPassword()
        {
            var pw = new Password { RecordID = Guid.NewGuid(), Account = Guid.NewGuid().ToString(), Key = Guid.NewGuid().ToString(), Description = Guid.NewGuid().ToString() };
            return pw;
        }

        #endregion
    }
}