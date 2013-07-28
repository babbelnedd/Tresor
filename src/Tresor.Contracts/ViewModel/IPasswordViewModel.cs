namespace Tresor.Contracts.ViewModel
{
    using Tresor.Contracts.Mediator;

    /// <summary>Schnittstelle für das <see cref="Tresor.ViewModel.PasswordViewModel"/>.</summary>
    public interface IPasswordViewModel : IPasswordReciever, IPasswordSender
    {
    }
}