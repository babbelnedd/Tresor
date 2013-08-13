namespace Tresor.Model.Application
{
    using Tresor.Contracts.Model.Application;
    using Tresor.Contracts.Utilities;

    /// <summary>Model für das SplashViewModel.</summary>
    public class SplashModel : ISplashModel
    {
        private readonly IDatabase database;

        /// <summary>Initialisiert eine neue Instanz der <see cref="SplashModel"/> Klasse.</summary>
        /// <param name="database">Die Datenbank, welche das Model benutzen soll.</param>
        public SplashModel(IDatabase database)
        {
            this.database = database;
        }
    }
}