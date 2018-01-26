using System.Windows;

namespace Spawn.HDT.DustUtility.Services
{
    public interface IDialogService
    {
        bool ShowDialog<T>() where T : Window, new();
        T GetDialogResult<T>();
    }
}