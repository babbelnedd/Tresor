namespace Tresor
{
    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.Configuration;

    using Tresor.Contracts.Model;
    using Tresor.Contracts.Model.Application;

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
            container.LoadConfiguration();
            var y = container.Resolve<ISplashModel>("SplashModel");
            var x = container.Resolve<IMainModel>("MainModel");
            var z = container.Resolve<IPasswordModel>("PasswordModel");
        }

        #endregion
    }
}