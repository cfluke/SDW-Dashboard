using UnityEngine;

namespace Widgets
{
    public class PlaybackWidget : MonoBehaviour
    {
        private DiscoveryWall.DiscoveryWall _discoveryWall;
    
        void Start()
        {
            _discoveryWall = FindObjectOfType<DiscoveryWall.DiscoveryWall>();
        }

        public void StartApps()
        {
            _discoveryWall.StartApps();
        }

        public void StopApps()
        {
            _discoveryWall.StopApps();
        }
    }
}