#region Using
using System;
using System.Windows;
#endregion

namespace Spawn.HDT.DustUtility.Services
{
    public interface IDialogService : IDisposable
    {
        #region Methods
        bool ShowDialog<T>(Window owner = null) where T : Window, new();
        T GetDialogResult<T>();
        #endregion
    }
}