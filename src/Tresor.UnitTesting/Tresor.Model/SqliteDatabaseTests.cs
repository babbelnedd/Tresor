namespace Tresor.UnitTesting.Tresor.Model
{
    using System;
    using System.Data.SQLite;
    using System.Linq;

    using NUnit.Framework;

    using global::Tresor.Model;

    /// <summary>"Tests für die IDatabase Implementierung SqliteDatabase."</summary>
    [TestFixture(Description = "Tests für die IDatabase Implementierung SqliteDatabase.")]
    public class SqliteDatabaseTests : Test
    {
        #region Fields

        /// <summary>Eine Instanz der <see cref="UserSettingsDatabase"/>.</summary>
        private UserSettingsDatabase database;

        #endregion

        #region Properties

        /// <summary>Holt einen zufälligen Namen für eine Datenbank.</summary>
        private string RandomDatabase
        {
            get
            {
                return Guid.NewGuid() + ".db";
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>Prüft ob die Methode CloseConnection keine Ausnahme wirft wenn eine Connection erzeugt wurde.</summary>
        [Test(Description = "Prüft ob die Methode CloseConnection keine Ausnahme wirft wenn eine Connection erzeugt wurde.")]
        [Repeat(Tests)]
        public void CloseConnectionDoesNotThrowIfConnectionIsNotNull()
        {
            database.SetName(RandomDatabase);
            database.OpenConnection();
            Assert.DoesNotThrow(() => database.CloseConnection());
        }

        /// <summary>Prüft ob die Methode CloseConnection keine Ausnahme wirft wenn keine Connection erzeugt wurde.</summary>
        [Test(Description = "Prüft ob die Methode CloseConnection keine Ausnahme wirft wenn keine Connection erzeugt wurde.")]
        [Repeat(Tests)]
        public void CloseConnectionDoesNotThrowIfConnectionIsNull()
        {
            Assert.DoesNotThrow(() => database.CloseConnection());
        }

        /// <summary>Prüft ob die Methode Execute eine Ausnahme wirft, wenn die Verbindung geschlossen oder nicht initialisiert ist.</summary>
        [Test(Description = "Prüft ob die Methode Execute eine Ausnahme wirft, wenn die Verbindung geschlossen oder nicht initialisiert ist.")]
        [Repeat(Tests)]
        public void ExecuteThrowsIfConnectionIsClosed()
        {
            // Connection ist null
            var msg = Assert.Throws<InvalidOperationException>(() => database.Execute("SELECT * FROM Table"));
            Assert.That(msg.ToString().Contains("No connection associated with this command"));

            // Connection ist geschlossen
            database.SetName(RandomDatabase);
            database.OpenConnection();
            database.CloseConnection();
            msg = Assert.Throws<InvalidOperationException>(() => database.Execute("SELECT * FROM Table"));
            Assert.That(msg.ToString().Contains("Database is not open"));
        }

        /// <summary>Prüft ob die Methode Execute eine Ausnahme wirft wenn der CommandText falsche Syntax enthält.</summary>
        [Test(Description = "Prüft ob die Methode Execute eine Ausnahme wirft wenn der CommandText falsche Syntax enthält.")]
        [Repeat(Tests)]
        public void ExecuteThrowsOnWrongSyntax()
        {
            database.SetName(RandomDatabase);
            database.OpenConnection();
            Assert.Throws<SQLiteException>(() => database.Execute("This is some wrong Syntax."));
        }

        /// <summary>Testet ob die Methode Execute funktioniert.</summary>
        [Test(Description = "Testet ob die Methode Execute funktioniert.")]
        [Repeat(Tests)]
        public void ExecuteWorks()
        {
            var rnd = RandomDatabase;
            database.SetName(rnd);
            database.OpenConnection();
            database.Execute("CREATE TABLE IF NOT EXISTS Tabelle(Field NVARCHAR)");
            database.Execute("CREATE TABLE IF NOT EXISTS Tabelle(Field NVARCHAR)");
            database.CloseConnection();
            database.DisposeConnection();

            var con = new SQLiteConnection("Data Source=" + rnd);
            con.Open();
            var com = new SQLiteCommand("CREATE TABLE Tabelle(Field NVARCHAR)", con);
            var msg = Assert.Throws<SQLiteException>(() => com.ExecuteNonQuery());
            Assert.That(msg.ToString().Contains("table Tabelle already exists"));
            con.Close();
            con.Dispose();
        }

        /// <summary>Prüft ob die Methode Get die erwarteten Ergebnisse zurückliefert.</summary>
        [Test(Description = "Prüft ob die Methode Get die erwarteten Ergebnisse zurückliefert.")]
        public void Get()
        {
            database.SetName(RandomDatabase);
            database.OpenConnection();
            database.Execute("CREATE TABLE IF NOT EXISTS Tabelle(Key NVARCHAR, Value NVARCHAR)");
            database.Execute("INSERT INTO Tabelle(Key, Value) VALUES('TestKey1', 'TestValue1')");
            database.Execute("INSERT INTO Tabelle(Key, Value) VALUES('TestKey2', 'TestValue2')");
            database.Execute("INSERT INTO Tabelle(Key, Value) VALUES('TestKey3', 'TestValue3')");

            var result = database.Get("SELECT * FROM Tabelle");
            Assert.That(result.Count == 3);
            result.ForEach(r => Assert.That(r.Count == 2));

            Assert.That(result.First()[0], Is.EqualTo("TestKey1"));
            Assert.That(result.First()[1], Is.EqualTo("TestValue1"));
        }

        /// <summary>Prüft ob die Methode Get eine Ausnahme wirft falls die Connection geschloßen ist.</summary>
        [Test(Description = "Prüft ob die Methode Get eine Ausnahme wirft falls die Connection geschloßen ist.")]
        [Repeat(Tests)]
        public void GetThrowsIfConnectionIsClosed()
        {
            database.SetName(RandomDatabase);
            database.OpenConnection();
            database.CloseConnection();
            var msg = Assert.Throws<InvalidOperationException>(() => database.Get("SELECT * FROM Tabelle"));
            Assert.That(msg.ToString().Contains("Database is not open"));
        }

        /// <summary>Prüft ob die Methode Get eine Ausnahme wirft falls die Connection null ist.</summary>
        [Test(Description = "Prüft ob die Methode Get eine Ausnahme wirft falls die Connection null ist.")]
        [Repeat(Tests)]
        public void GetThrowsIfConnectionIsNull()
        {
            Assert.Throws<InvalidOperationException>(() => database.Get("SELECT * FROM Tabelle"));
        }

        /// <summary>Prüft ob die Methode Get eine Ausnahme wirft falls die Syntax des Commands falsch ist.</summary>
        [Test(Description = "Prüft ob die Methode Get eine Ausnahme wirft falls die Syntax des Commands falsch ist.")]
        [Repeat(Tests)]
        public void GetThrowsOnWrongSyntax()
        {
            database.SetName(RandomDatabase);
            database.OpenConnection();

            var msg = Assert.Throws<SQLiteException>(() => database.Get("This is some wrong Syntax"));
            Assert.That(msg.ToString().Contains("SQL logic error"));

            database.CloseConnection();
            database.DisposeConnection();
        }

        /// <summary>Prüft ob die Methode OpenConnection keine Ausn ahmewirft, wenn ein Name für die Datenbank angegeben wurde.</summary>
        [Test(Description = "Prüft ob die Methode OpenConnection keine Ausnahme wirft, wenn ein Name für die Datenbank angegeben wurde.")]
        [Repeat(Tests)]
        public void OpenConnectionDoesNotThrow()
        {
            database.SetName(RandomDatabase);
            Assert.DoesNotThrow(() => database.OpenConnection());
        }

        /// <summary>Prüft ob die Methode OpenConnection eine Ausnahme wirft, wenn kein Name für die Datenbank angegeben wurde.</summary>
        [Test(Description = "Prüft ob die Methode OpenConnection eine Ausnahme wirft, wenn kein Name für die Datenbank angegeben wurde.")]
        [Repeat(Tests)]
        public void OpenConnectionThrowsOnNoName()
        {
            Assert.Throws<Exception>(() => database.OpenConnection());
        }

        /// <summary>Testet ob die Methode SetName die Eigenschaft Name setzt.</summary>
        [Test(Description = "Testet ob die Methode SetName die Eigenschaft Name setzt.")]
        [Repeat(Tests)]
        public void SetName()
        {
            var rnd = RandomDatabase;
            database.SetName(rnd);
            Assert.That(database.Name, Is.EqualTo(rnd));
        }

        /// <summary>Prüft ob die Methode SetPassword keine Ausnahme wirft.</summary>
        [Test(Description = "Prüft ob die Methode SetPassword keine Ausnahme wirft.")]
        [Repeat(Tests)]
        public void SetPasswordDoesNotThrow()
        {
            Assert.DoesNotThrow(() => database.SetPassword(RandomDatabase));
        }

        /// <summary>Testet ob die Methode SetPath die Eigenschaft Path setzt.</summary>
        [Test(Description = "Testet ob die Methode SetPath die Eigenschaft Path setzt.")]
        [Repeat(Tests)]
        public void SetPath()
        {
            var rnd = Guid.NewGuid().ToString();
            database.SetPath(rnd);
            Assert.That(database.Path, Is.EqualTo(rnd));
        }

        /// <summary>Initialisiert vor jedem Test die Testumgebung.</summary>
        [SetUp]
        public void Setup()
        {
            database = new UserSettingsDatabase();
        }

        #endregion
    }
}