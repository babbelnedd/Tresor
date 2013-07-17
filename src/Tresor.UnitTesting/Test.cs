namespace Tresor.UnitTesting
{
    using System.IO;

    using NUnit.Framework;

    using global::Tresor.Utilities;

    /// <summary>Basisklasse für Tests.</summary>
    public abstract class Test
    {
        #region Öffentliche Methoden und Operatoren

        /// <summary>Initialisiert die Testumgebung.</summary>
        [SetUp]
        public virtual void Initalize()
        {
            DeleteStorageFolder();
        }

        /// <summary>Macht alle Änderungen, die für die Testumgebung nötig sind, rückgängig.</summary>
        [TearDown]
        public virtual void Teardown()
        {
        }

        #endregion

        #region Methoden

        /// <summary>Löscht den für Testzwecke aufgesetzen Ordner in dem alle relevanten Appdaten liegen.</summary>
        private static void DeleteStorageFolder()
        {
            var storageFolder = Path.GetDirectoryName(AppSettings.GetStorageFile());

            if (!string.IsNullOrEmpty(storageFolder) && Directory.Exists(storageFolder))
            {
                Directory.Delete(storageFolder);
            }
        }

        #endregion
    }
}