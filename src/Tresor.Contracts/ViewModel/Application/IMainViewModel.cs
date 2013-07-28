namespace Tresor.Contracts.ViewModel.Application
{
    using Tresor.Contracts.Mediator;

    /// <summary>Schnittstelle für ein MainViewModel.</summary>
    public interface IMainViewModel : IPasswordReciever, IPasswordSender
    {
    }
}