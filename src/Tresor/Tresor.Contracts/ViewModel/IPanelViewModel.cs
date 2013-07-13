
namespace Tresor.Contracts.ViewModel
{
    using System.Collections.ObjectModel;

    using Tresor.Contracts.Utilities;

    public interface IPanelViewModel
    {
        bool IsDirty { get; }

         ObservableCollection<IPassword> Passwords { get; }
    }
}