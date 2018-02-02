#region Using
using System;
using System.Windows;
#endregion

namespace Spawn.HDT.DustUtility.Services
{
    public interface IWindowService : IDisposable
    {
        #region Methods
        void Show<T>(Window owner = null) where T : Window, new();
        bool ShowDialog<T>(Window owner = null) where T : Window, new();
        T GetResult<T>();
        #endregion
    }
}