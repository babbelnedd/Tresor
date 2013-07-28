namespace Tresor.UnitTesting.Tresor.Model
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;

    using NUnit.Framework;

    using global::Tresor.Contracts.Model;

    using global::Tresor.Contracts.Utilities;

    using global::Tresor.Model;

    using global::Tresor.Utilities;

    /// <summary>Tests für das SqlitePanelModel.</summary>
    [TestFixture(Description = "Tests für das SqlitePanelModel.")]
    public class SqlitePanelModelTests : Test
    {
        #region Static Fields

        /// <summary>Der Name der Datenbank.</summary>
        private static string databaseName;

        #endregion

        #region Fields

        /// <summary>Das zu testende Datenmodel.</summary>
        private IPanelModel model;

        #endregion

        #region Public Methods and Operators

        /// <summary>Prüft ob das hinzufügen von Passwörtern nicht zu lange dauert. 10 Passwörter sollten durchschnittlich nicht länger als 1 Sekunde benötigen.</summary>
        [Repeat(Tests)]
        [Test(Description = "Prüft ob das hinzufügen von Passwörtern nicht zu lange dauert. 10 Passwörter sollten durchschnittlich nicht länger als 1 Sekunde benötigen.")]
        public void AddPasswordsIsFastEnough()
        {
            if (File.Exists("swtest.db"))
            {
                File.Delete("swtest.db");
            }

            model = new SqlitePanelModel("sw.db");
            model.IsKeyCorrect("1");

            var passwords = GetSomeRandomPasswords(10);
            var times = new List<long>();

            for (var i = 0; i < 5; i++)
            {
                var sw = StopwatchSavePasswords(model, passwords);
                times.Add(sw.ElapsedMilliseconds);
            }

            var average = times.Average();
            Assert.Less(average, 1000);
        }

        [Test(Description = "Prüft ob die Eigenschaft IsDirty True ist, wenn ein oder mehrere Passwörter IsDirty sind.")]
        [Repeat(Tests)]
        public void IsDirtyIfAnyPasswordIsDirty()
        {
            Assert.That(model.IsDirty, Is.False);
            var pw1 = new Password { RecordID = Guid.NewGuid() };
            var pw2 = new Password { RecordID = Guid.NewGuid() };
            var pw3 = new Password { RecordID = Guid.NewGuid() };
            model.AddPassword(pw1);
            model.AddPassword(pw2);
            model.AddPassword(pw3);
            model.Passwords.First().Key = Guid.NewGuid().ToString();
            Assert.That(model.IsDirty);
        }

        /// <summary>Prüft ob Änderungen am Model PropertyChanged für die Eigenschaft IsDirty auslöst.</summary>
        [Test(Description = "Prüft ob Änderungen am Model PropertyChanged für die Eigenschaft IsDirty auslöst.")]
        [Repeat(Tests)]
        public void ChangesRaiseIsDirtyChanged()
        {
            var changes = new List<string>();
            model.PropertyChanged += (sender, arguments) => changes.Add(arguments.PropertyName);

            model.AddPassword(new Password { RecordID = Guid.NewGuid() });

            Assert.Contains("IsDirty", changes);

            model.Passwords.First().Account = Guid.NewGuid().ToString();

            Assert.IsTrue(changes.Count(c => c == "IsDirty") == 3);
        }

        /// <summary>Prüft ob die Datenbank existiert, nach der erfolgreichen Überprüfung des Schlüssels.</summary>
        [Test(Description = "Prüft ob die Datenbank existiert, nach der erfolgreichen Überprüfung des Schlüssels.")]
        [Repeat(Tests)]
        public void DatabaseExistsAfterKeyCheck()
        {
            // Note: Da jedes mal eine neue Datenbank erzeugt wird, legen wir ein neues Passwort fest.
            model.IsKeyCorrect("1234");
            Assert.IsTrue(File.Exists(databaseName));
        }

        /// <summary>Prüft ob IsKeyCorrect false zurückgibt nachdem das Passwort bestimmt wurde und mit einem anderen Passwort getestet wird.</summary>
        [Test(Description = "Prüft ob IsKeyCorrect false zurückgibt nachdem das Passwort bestimmt wurde und mit einem anderen Passwort getestet wird.")]
        [Repeat(Tests)]
        public void IsKeyCorrect()
        {
            Assert.IsTrue(model.IsKeyCorrect("1"));
            Assert.IsFalse(model.IsKeyCorrect("xxx"));
        }

        /// <summary>Prüft ob die private Methode LoadPasswords alle Passwörter in die Eigenschaft Passwords geladen hat.</summary>
        [Test(Description = "Prüft ob die private Methode LoadPasswords alle Passwörter in die Eigenschaft Passwords geladen hat.")]
        [Repeat(Tests)]
        public void LoadPasswords()
        {
            var passwords = new ObservableCollection<IPassword>();

            for (var i = 0; i < 5; i++)
            {
                passwords.Add(new Password { RecordID = Guid.NewGuid() });
            }

            model.Save(passwords);

            Assert.AreEqual(passwords, model.Passwords);
        }

        /// <summary>Prüft ob das Model null ist, nach der Initalisierung des Models.</summary>
        [Test(Description = "Prüft ob das Model null ist, nach der Initalisierung des Models.")]
        [Repeat(Tests)]
        public void ModelIsNotNullAfterInitialize()
        {
            Assert.IsNotNull(model);
        }

        /// <summary>Prüft ob Passwörter auf ihren Zustand hin überprüft werden und IsDirty richtig gesetzt wird.</summary>
        [Test(Description = "Prüft ob Passwörter auf ihren Zustand hin überprüft werden und IsDirty richtig gesetzt wird.")]
        [Repeat(Tests)]
        public void PasswordsAreObserved()
        {
            var newPassword = new Password { RecordID = Guid.NewGuid() };
            model.AddPassword(newPassword);
            Assert.IsFalse(model.Passwords.First(pw => pw.Equals(newPassword)).IsDirty);
            model.Passwords.First(pw => pw.Equals(newPassword)).Account = "1234";
            Assert.IsTrue(model.Passwords.First(pw => pw.Equals(newPassword)).IsDirty);
        }

        /// <summary>Prüft ob die Eigenschaft Passwords null ist, nach der Initalisierung des Models.</summary>
        [Test(Description = "Prüft ob die Eigenschaft Passwords null ist, nach der Initalisierung des Models.")]
        [Repeat(Tests)]
        public void PasswordsIsNotNullAfterInitialize()
        {
            Assert.IsNotNull(model.Passwords);
        }

        /// <summary>Prüft ob die Eigenschaft IsDirty False ist nach der Initialisierung.</summary>
        [Test(Description = "Prüft ob die Eigenschaft IsDirty False ist nach der Initialisierung.")]
        [Repeat(Tests)]
        public void IsDirtyIsFalseOnStart()
        {
            Assert.That(model.IsDirty, Is.False);
        }

        /// <summary> Initialisiert vor jedem Test die Testumgebung </summary>
        [SetUp]
        public void Setup()
        {
            databaseName = string.Format("{0}.db", Guid.NewGuid());
            model = new SqlitePanelModel(databaseName);
            model.IsKeyCorrect("1");
        }

        #endregion

        #region Methods

        /// <summary>Erzeugt zufällige Passwörter.</summary>
        /// <param name="count">Die Anzahl der Passwörter die erstellt werden sollen.</param>
        /// <returns>Die Auflistung der erzeugten Passwörter.</returns>
        private static IEnumerable<IPassword> GetSomeRandomPasswords(int count)
        {
            var passwords = new List<IPassword>();

            for (var i = 0; i < count; i++)
            {
                var newPassword = new Password { RecordID = Guid.NewGuid(), };
                passwords.Add(newPassword);
            }

            return passwords;
        }

        /// <summary>Misst die Zeit, die benötigt wird, um Passwörter zu speichern.</summary>
        /// <param name="model">Das Model in dem gespeichert werden soll.</param>
        /// <param name="passwords">Die Passwörter welche gespeichert werden sollen.</param>
        /// <returns>Die Stopwatch Klasse welche die Dauer des Vorgangs aufgezeichnet hat.</returns>
        private static Stopwatch StopwatchSavePasswords(IPanelModel model, IEnumerable<IPassword> passwords)
        {
            var sw = new Stopwatch();
            sw.Start();
            model.Save(passwords);
            sw.Stop();
            return sw;
        }

        #endregion
    }
}