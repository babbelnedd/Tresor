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
    public class SqlitePanelModelTests
    {
        #region Konstanten und Felder

        /// <summary>Der Name der Datenbank.</summary>
        private static string databaseName;

        /// <summary>Das zu testende Datenmodel.</summary>
        private IPanelModel model;

        #endregion

        #region Öffentliche Methoden und Operatoren

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

        [Test(Description = "Prüft ob die Datenbank existiert, nach der erfolgreichen .")]
        [Repeat(20)]
        public void DatabaseExistsAfterKeyCheck()
        {
            // Note: Da jedes mal eine neue Datenbank erzeugt wird, legen wir ein neues Passwort fest.
            model.IsKeyCorrect("1234");
            Assert.IsTrue(File.Exists(databaseName));
        }

        [Test(Description = "Prüft ob IsKeyCorrect false zurückgibt nachdem das Passwort bestimmt wurde und mit einem anderen Passwort getestet wird.")]
        [Repeat(20)]
        public void IsKeyCorrect()
        {
            Assert.IsTrue(model.IsKeyCorrect("1"));
            Assert.IsFalse(model.IsKeyCorrect("xxx"));
        }

        [Test(Description = "Prüft ob die private Methode LoadPasswords alle Passwörter in die Eigenschaft Passwords geladen hat.")]
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

        [Test(Description = "Prüft ob Passwörter auf ihren Zustand hin überprüft werden und IsDirty richtig gesetzt wird.")]
        public void PasswordsAreObserved()
        {
            var newPassword = new Password { RecordID = Guid.NewGuid() };
            model.AddPassword(newPassword);
            Assert.IsFalse(model.Passwords.First(pw => pw.Equals(newPassword)).IsDirty);
            model.Passwords.First(pw => pw.Equals(newPassword)).Account = "1234";
            Assert.IsTrue(model.Passwords.First(pw => pw.Equals(newPassword)).IsDirty);
        }

        [Test(Description = "Prüft ob das Model null ist, nach der Initalisierung des Models.")]
        [Repeat(20)]
        public void ModelIsNotNullAfterInitialize()
        {
            Assert.IsNotNull(model);
        }

        [Test(Description = "Prüft ob die Eigenschaft Passwords null ist, nach der Initalisierung des Models.")]
        [Repeat(20)]
        public void PasswordsIsNotNullAfterInitialize()
        {
            Assert.IsNotNull(model.Passwords);
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

        #region Methoden

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

        [Test(Description = "Prüft ob Änderungen am Model PropertyChanged für die Eigenschaft IsDirty auslöst.")]
        public void ChangesRaiseIsDirtyChanged()
        {
            var changes = new List<string>();
            model.PropertyChanged += (sender, arguments) => changes.Add(arguments.PropertyName);

            model.AddPassword(new Password { RecordID = Guid.NewGuid() });

            Assert.Contains("IsDirty", changes);

            model.Passwords.First().Account = Guid.NewGuid().ToString();

            Assert.IsTrue(changes.Count(c => c == "IsDirty") == 2);
        }
    }
}