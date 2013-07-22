namespace Tresor.Contracts.ViewModel
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;

    using Cinch;

    using Tresor.Contracts.Utilities;
    using Tresor.Framework.MVVM;

    public interface IPanelViewModel : INotifyPropertyChanged
    {
        bool IsDirty { get; }

        ObservableCollection<IPassword> Passwords { get; }

        SimpleCommand<object, SCommandArgs> AddPasswordCommand { get; }
    }
}