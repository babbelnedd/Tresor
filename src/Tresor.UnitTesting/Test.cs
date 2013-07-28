namespace Tresor.UnitTesting
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using NUnit.Framework;

    /// <summary>Basisklasse für Tests.</summary>
    public abstract class Test
    {
        #region Constants

        /// <summary>Die Anzahl der Durchläufe pro Test.</summary>
        public const int Tests = 3;


        #endregion

        #region Constructors and Destructors

        /// <summary>Finalisiert eine Instanz der <see cref="Test"/> Klasse.</summary>
        ~Test()
        {
            DeleteOldFiles();
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>Initialisiert die Testumgebung.</summary>
        [SetUp]
        public virtual void Initalize()
        {
        }

        /// <summary>Macht alle Änderungen, die für die Testumgebung nötig sind, rückgängig.</summary>
        [TearDown]
        public virtual void Teardown()
        {
        }

        #endregion

        #region Methods

        /// <summary>Löscht alle Dateien vom Letzten Test.</summary>
        private void DeleteOldFiles()
        {
            var path = AppDomain.CurrentDomain.BaseDirectory;
            var files = (List<string>)Directory.GetFiles(path).Where(file => file.EndsWith(".db"));
            files.AddRange(Directory.GetFiles(path).Where(file => file.EndsWith(".db-journal")));
            files.ForEach(File.Delete);
        }

        #endregion
    }
}