namespace Tresor.Contracts.Utilities
{
    using System.ComponentModel;
    using System.Runtime.Serialization;

    public interface IPassword : INotifyPropertyChanged
    {
        string Account { get; set; }
        string Description { get; set; }
        bool IsDirty { get; set; }
        string Key { get; set; }

        string ToString();

        void BeginEdit();
        void EndEdit();
        void CancelEdit();
    }
}