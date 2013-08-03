namespace Tresor.UnitTesting.Tresor.Model
{
    using System;
    using System.Collections.Generic;
    using System.Data.SQLite;
    using System.Linq;

    using NUnit.Framework;

    using global::Tresor.Contracts.Utilities;

    using global::Tresor.Model;

    /// <summary>Tests für die IUserSettings Implementierung SqliteUserSettings.</summary>
    [TestFixture(Description = "Tests für die IUserSettings Implementierung SqliteUserSettings.")]
    public class SqliteUserSettingsTests : Test
    {
        #region Fields

        /// <summary>Eine Implementierung der Schnittstelle <see cref="IUserSettings"/>.</summary>
        private IDatabase database;

        /// <summary>Eine Instanz der <see cref="SqliteUserSettings"/>.</summary>
        private SqliteUserSettings settings;

        #endregion

        #region Public Methods and Operators

        /// <summary>Prüft ob die Methode Add(string categroy, string key, string value) einen neuen Schlüssel anlegt..</summary>
        [Test(Description = "Prüft ob die Methode Add(string categroy, string key, string value) einen neuen Schlüssel anlegt.")]
        [Repeat(Tests)]
        public void AddCreatesNewKey()
        {
            settings.Add("SomeCategory");
            settings.Add("SomeCategory", "SomeKey", "SomeValue");
            Assert.DoesNotThrow(() => database.Get("SELECT * FROM SomeCategory"));
            var result = database.Get("SELECT * FROM SomeCategory");

            Assert.That(result.Count != 0, "Es wurde kein Schlüssel angelegt.");
            Assert.That(result.Count == 1, "Es wurden zu viele Schlüssel angelegt.");

            Assert.That(result.First()[0], Is.EqualTo("SomeKey"), "Der Rückgabewert ist falsch.");
            Assert.That(result.First()[1], Is.EqualTo("SomeValue"), "Der Rückgabewert ist falsch.");
        }

        /// <summary>Prüft ob die Methode Add(string category, string key, string value) keine Ausnahme wirft, falls die Kategorie bereits existiert.</summary>
        [Test(Description = "Prüft ob die Methode Add(string category, string key, string value) keine Ausnahme wirft, falls die Kategorie bereits existiert.")]
        [Repeat(Tests)]
        public void AddCreatesNewKeyInExistingCategoryDoesNotThrow()
        {
            settings.Add("SomeCatgeory");
            Assert.DoesNotThrow(() => settings.Add("SomeCatgeory", "SomeKey", "SomeValue"));
        }

        /// <summary>Prüft ob die Methode Add eine neue Tabelle anlegt, falls diese noch nicht existiert.</summary>
        [Test(Description = "Prüft ob die Methode Add eine neue Tabelle anlegt, falls diese noch nicht existiert.")]
        [Repeat(Tests)]
        public void AddCreatesNewTable()
        {
            settings.Add("SomeCategory");
            Assert.DoesNotThrow(() => database.Get("SELECT * FROM SomeCategory"));
        }

        /// <summary>Prüft ob die Methode Add eine Ausnahme wirft, falls die Tabelle (Kategorie) bereits existiert.</summary>
        [Test(Description = "Prüft ob die Methode Add eine Ausnahme wirft, falls die Tabelle (Kategorie) bereits existiert.")]
        [Repeat(Tests)]
        public void AddThrowsExceptionIfTableAlreadyExists()
        {
            settings.Add("SomeCategory");
            var msg = Assert.Throws<SQLiteException>(() => settings.Add("SomeCategory"));
            Assert.That(msg.ToString().Contains("table SomeCategory already exists"));
        }

        /// <summary>Prüft ob die Methode Add(string categroy, string key, string value) eine Ausnahme wirft, falls die Kategorie nicht existiert.</summary>
        [Test(Description = "Prüft ob die Methode Add(string categroy, string key, string value) eine Ausnahme wirft, falls die Kategorie nicht existiert.")]
        [Repeat(Tests)]
        public void AddThrowsIfCatgeoryNotExists()
        {
            var msg = Assert.Throws<SQLiteException>(() => settings.Add("SomeCategory", "SomeKey", "SomeValue"));
            Assert.That(msg.ToString().Contains("no such table: SomeCategory"));
        }

        /// <summary>Prüft ob die Methode Read(string category) alle Schlüsselpaare der Kategorie zurückgibt.</summary>
        [Test(Description = "Prüft ob die Methode Read(string category) alle Schlüsselpaare der Kategorie zurückgibt.")]
        [Repeat(Tests)]
        public void ReadGetAllKeys()
        {
            settings.Add("SomeCategory");
            settings.Add("SomeCategory", "SomeKey1", "SomeValue1");
            settings.Add("SomeCategory", "SomeKey2", "SomeValue2");
            settings.Add("SomeCategory", "SomeKey3", "SomeValue3");
            Assert.DoesNotThrow(() => settings.Read("SomeCategory"), "Das Auslesen der Tabelle ist fehlgeschlagen.");

            var result = settings.Read("SomeCategory");
            Assert.That(result.Count == 3);
            Assert.That(result["SomeKey1"] == "SomeValue1");
            Assert.That(result["SomeKey2"] == "SomeValue2");
            Assert.That(result["SomeKey3"] == "SomeValue3");
        }

        /// <summary>Prüft ob die Methode Read(string catgeory, string key) die richtigen Ergebnisse zurückliefert.</summary>
        [Test(Description = "Prüft ob die Methode Read(string catgeory, string key) die richtigen Ergebnisse zurückliefert.")]
        [Repeat(Tests)]
        public void ReadGetsRightValues()
        {
            settings.Add("SomeCategory");
            settings.Add("SomeCategory", "SomeKey", "SomeValue");

            Assert.DoesNotThrow(() => settings.Read("SomeCategory", "SomeKey"));
            var result = settings.Read("SomeCategory", "SomeKey");
            Assert.That(result, Is.EqualTo("SomeValue"));
        }

        /// <summary>Prüft ob die Methode Read(string catgeory, string key) eine Ausnahme wirft falls die Kategorie nicht existiert.</summary>
        [Test(Description = "Prüft ob die Methode Read(string catgeory, string key) eine Ausnahme wirft falls die Kategorie nicht existiert.")]
        [Repeat(Tests)]
        public void ReadThrowsExceptionIfCategoryNotExists()
        {
            var msg = Assert.Throws<SQLiteException>(() => settings.Read("SomeCategoryWhichNotExists", "SomeKey"));
            Assert.That(msg.ToString().Contains("no such table: SomeCategoryWhichNotExists"));
        }

        /// <summary>Prüft ob die Methode Read(string catgeory, string key) eine Ausnahme wirft falls der Schlüssel nicht existiert.</summary>
        [Test(Description = "Prüft ob die Methode Read(string catgeory, string key) eine Ausnahme wirft falls der Schlüssel nicht existiert.")]
        [Repeat(Tests)]
        public void ReadThrowsExceptionIfKeyNotExists()
        {
            settings.Add("SomeCategory");
            var msg = Assert.Throws<InvalidOperationException>(() => settings.Read("SomeCategory", "SomeKey"));
            Assert.That(msg.ToString().Contains("Die Sequenz enthält keine Elemente"));
        }

        /// <summary>Initialisiert vor jedem Test die Testumgebung.</summary>
        [SetUp]
        public void Setup()
        {
            var rnd = Guid.NewGuid() + ".db";
            database = new SqliteDatabase();
            database.SetName(rnd);
            settings = new SqliteUserSettings(database);
        }

        /// <summary>Räumt die Testumgebung nach jedem Durchlauf auf.</summary>
        [TearDown]
        public void TearDown()
        {
            settings = null;
        }

        /// <summary>Prüft ob die Methode Write keine Ausnahme wirft, falls der Schlüssel nicht existiert.</summary>
        [Test(Description = "Prüft ob die Methode Write keine Ausnahme wirft, falls der Schlüssel nicht existiert.")]
        [Repeat(Tests)]
        public void WriteDoesNotThrowIfKeyNotExists()
        {
            settings.Add("SomeCategory");
            Assert.DoesNotThrow(() => settings.Write("SomeCategory", "SomKeyThatNotExists", "SomeValue"));
        }

        /// <summary>Überprüft ob die Methode Write einen Wert überschreibt..</summary>
        [Test(Description = "Überprüft ob die Methode Write einen Wert überschreibt.")]
        [Repeat(Tests)]
        public void WriteOverwritesValue()
        {
            settings.Add("SomeCategory");
            settings.Add("SomeCategory", "SomeKey", "SomeValue");

            settings.Write("SomeCategory", "SomeKey", "SomeNewValue");

            var result = settings.Read("SomeCategory", "SomeKey");

            Assert.That(result == "SomeNewValue");
        }

        /// <summary>Prüft ob die Methode Write eine Ausnahme wirft, falls die Kategorie nicht existiert.</summary>
        [Test(Description = "Prüft ob die Methode Write eine Ausnahme wirft, falls die Kategorie nicht existiert.")]
        [Repeat(Tests)]
        public void WriteThrowsIfCategoryNotExists()
        {
            var msg = Assert.Throws<SQLiteException>(() => settings.Write("SomeCategoryThatNotExists", "SomeKey", "SomeValue"));
            Assert.That(msg.ToString().Contains("no such table: SomeCategoryThatNotExists"));
        }

        /// <summary>Prüft ob die Methode Write(string category, Dictionary pair) mehrere Werte überschreiben kann.</summary>
        [Test(Description = "Prüft ob die Methode Write(string category, Dictionary<string, string> pair) mehrere Werte überschreiben kann.")]
        [Repeat(Tests)]
        public void WriteWorksCorrect()
        {
            settings.Add("SomeCategory");
            settings.Add("SomeCategory", "SomeKey1", "SomeValue");
            settings.Add("SomeCategory", "SomeKey2", "SomeValue");
            settings.Add("SomeCategory", "SomeKey3", "SomeValue");
            settings.Add("SomeCategory", "SomeKey4", "SomeValue");

            var toWrite = new Dictionary<string, string>();
            toWrite.Add("SomeKey1", "SomeNewValue");
            toWrite.Add("SomeKey2", "SomeNewValue");
            toWrite.Add("SomeKey3", "SomeNewValue");
            toWrite.Add("SomeKey4", "SomeNewValue");

            Assert.DoesNotThrow(() => settings.Write("SomeCategory", toWrite));
            var result = settings.Read("SomeCategory");
            Assert.That(result.Count == 4);
            Assert.That(result.All(entry => entry.Value == "SomeNewValue"));
        }

        #endregion
    }
}