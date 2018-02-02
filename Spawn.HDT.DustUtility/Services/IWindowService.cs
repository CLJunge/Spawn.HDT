#region Using
using System;
using System.Windows;
#endregion

namespace Spawn.HDT.DustUtility.Services
{
    public interface IWindowService : IDisposable
    {
        #region Methods
        void Show<T>(int nKey, Window owner = null) where T : Window, new();
        bool ShowDialog<T>(int nKey, Window owner = null) where T : Window, new();
        T GetResult<T>(int nKey = -1);
        bool IsVisible(int nKey = -1);
        void Dispose(int nKey);
        #endregion
    }
}