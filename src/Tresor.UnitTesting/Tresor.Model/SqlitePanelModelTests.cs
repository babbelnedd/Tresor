namespace Tresor.UnitTesting.Tresor.Model
{
    using System;
    using System.IO;

    using NUnit.Framework;

    using global::Tresor.Contracts.Model;

    using global::Tresor.Model;

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
            model.IsKeyCorrect("1234");
            Assert.IsTrue(model.IsKeyCorrect("1234"));
            Assert.IsFalse(model.IsKeyCorrect("xxx"));
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
            databaseName = string.Format("{0}.db", Guid.NewGuid().ToString());
            model = new SqlitePanelModel(databaseName);
        }

        #endregion
    }
}