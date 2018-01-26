using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Spawn.HDT.DustUtility.Mvvm
{
    public abstract class BindableBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string strPropertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(strPropertyName));
        }

        protected virtual void Set<T>(ref T member, T value, [CallerMemberName] string strPropertyName = null)
        {
            if (!Equals(member, value))
            {
                member = value;

                OnPropertyChanged(strPropertyName);
            }
            else { }
        }
    }
}