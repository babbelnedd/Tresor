namespace Tresor.Contracts.Mediator
{
    using Tresor.Contracts.Utilities;

    /// <summary>Sendet ein Passwort.</summary>
    public interface IPasswordSender
    {
        #region Public Methods and Operators

        /// <summary>Sendet ein <see cref="IPassword"/>. </summary>
        /// <param name="password">Das <see cref="IPassword"/> welches verschickt wird.</param>
        void Send(IPassword password);

        #endregion
    }
}