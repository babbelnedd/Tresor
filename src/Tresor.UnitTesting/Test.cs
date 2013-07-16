namespace Tresor.UnitTesting
{
    using System;
    using System.IO;

    using NUnit.Framework;

    using global::Tresor.UnitTesting.Tools;

    /// <summary>Basisklasse für Tests.</summary>
    public abstract class Test
    {
        #region Öffentliche Eigenschaften

        /// <summary>Holt den Pfad des Ordners in dem der <see cref="OriginalFolder"/> temporär gebackupt wird.</summary>
        public string BackupFolder
        {
            get
            {
                return string.Format("{0}\\Tresor.bak", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
            }
        }

        /// <summary>Holt den Pfad des Ordners in dem die Passwörter verwaltet werden.</summary>
        public string OriginalFolder
        {
            get
            {
                return string.Format("{0}\\Tresor", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
            }
        }

        #endregion

        #region Öffentliche Methoden und Operatoren

        /// <summary>Initialisiert die Testumgebung.</summary>
        [SetUp]
        public virtual void Initalize()
        {
            if (!Directory.Exists(OriginalFolder) && Directory.Exists(BackupFolder))
            {
                PasswordStorage.MoveBackupToOrigin();
            }

            PasswordStorage.MoveOriginToBackup();
        }

        /// <summary>Macht alle Änderungen, die für die Testumgebung nötig sind, rückgängig.</summary>
        [TearDown]
        public virtual void Teardown()
        {
            PasswordStorage.MoveBackupToOrigin();
        }

        #endregion
    }
}