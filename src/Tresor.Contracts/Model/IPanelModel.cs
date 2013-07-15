namespace Tresor.Contracts.Model
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;

    using Tresor.Contracts.Utilities;

    public interface IPanelModel : INotifyPropertyChanged
    {
        bool IsDirty { get; }

        ObservableCollection<IPassword> Passwords { get; }
    }
}