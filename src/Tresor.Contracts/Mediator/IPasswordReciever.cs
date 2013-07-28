namespace Tresor.Contracts.Mediator
{
    using Tresor.Contracts.Utilities;

    /// <summary>Ein Reciever welcher ein IPassword empfängt.</summary>
    public interface IPasswordReciever
    {
        #region Public Methods and Operators

        /// <summary>Empfängt ein <see cref="IPassword"/>.</summary>
        /// <param name="password">Das <see cref="IPassword"/> welches empfangen wird.</param>
        void Recieve(IPassword password);

        #endregion
    }
}