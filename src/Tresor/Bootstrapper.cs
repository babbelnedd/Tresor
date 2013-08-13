namespace Tresor
{
    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.Configuration;

    /// <summary>Statischer Bootstrapper der die Anwendung verkabelt.</summary>
    internal class Bootstrapper
    {
        #region Fields

        /// <summary>Der IoC Container.</summary>
        private readonly IUnityContainer container = new UnityContainer();

        #endregion

        #region Methods

        /// <summary>Lädt die Anwendung.</summary>
        internal void LoadApplication()
        {
            // Wann soll das / die Passwort(er) abgefragt werden? 
            // Eine Datenbank die mehrere Kategorien hat. Und dann können nur einzeln Datenbanken geladen werden.
            // Nachträglich eine Datenbank laden geht über das Dateimenü. 
            // Beim intialen sowie dem nachträglichen Laden erscheint ein Dialog in der Mitte der Anwendung der das Passwort für die gewählte Datenbank einliest.

            container.LoadConfiguration();

            // 1. ViewModels instanzieren (Models + Datenbanken + UserSettings + Aggregator?? werden durch CI instanziert)
            // 2. Fertig?
        }

        #endregion
    }
}