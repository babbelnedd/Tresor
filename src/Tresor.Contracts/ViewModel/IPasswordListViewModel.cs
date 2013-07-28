namespace Tresor.Contracts.ViewModel
{
    using Tresor.Contracts.Mediator;

    /// <summary>Schnittstelle für ein PasswordListViewModel.</summary>
    public interface IPasswordListViewModel : IPasswordReciever, IPasswordSender
    {
    }
}