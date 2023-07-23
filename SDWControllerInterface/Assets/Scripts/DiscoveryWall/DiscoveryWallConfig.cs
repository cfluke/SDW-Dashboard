using System;
using AppManagement;
using UnityEngine;

namespace DiscoveryWall
{
    [Serializable]
    public class DiscoveryWallConfig
    {
        // 5x KeckDisplays
        public KeckDisplayConfig[] keckDisplays = new KeckDisplayConfig[5];

        /// <param name="keckDisplay">accepts integers 1-5</param>
        /// <param name="monitor1">app to execute on top monitor</param>
        /// <param name="monitor2">app to execute on bottom monitor</param>
        public void SetKeckDisplay(int keckDisplay, App monitor1, App monitor2)
        {
            try
            {
                keckDisplays[keckDisplay - 1] = new KeckDisplayConfig(monitor1, monitor2);
            }
            catch (IndexOutOfRangeException e)
            {
                Debug.LogError(e.Message);
            }
        }
    }
}