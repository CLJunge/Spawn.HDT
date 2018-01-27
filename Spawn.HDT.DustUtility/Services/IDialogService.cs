using System;
using System.Windows;

namespace Spawn.HDT.DustUtility.Services
{
    public interface IDialogService : IDisposable
    {
        bool ShowDialog<T>(Window owner = null) where T : Window, new();
        T GetDialogResult<T>();
    }
}