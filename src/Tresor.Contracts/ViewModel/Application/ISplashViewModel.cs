namespace Tresor.Contracts.ViewModel.Application
{
    /// <summary>Schnittstelle für das SplashViewModel.</summary>
    public interface ISplashViewModel
    {
        #region Öffentliche Eigenschaften

        /// <summary>Holt oder setzt die Eingabe der SplashView.</summary>
        string Input { get; set; }

        #endregion
    }
}