using SerializableData;
using UnityEngine;

namespace Widgets.Playback
{
    public class PlaybackWidget : Widget
    {
        private DiscoveryWall.DiscoveryWall _discoveryWall;
    
        void Start()
        {
            _discoveryWall = DiscoveryWall.DiscoveryWall.Instance;
        }

        public void StartApps()
        {
            _discoveryWall.StartApps();
        }

        public void StopApps()
        {
            _discoveryWall.StopApps();
        }

        public void DisconnectListeners()
        {
            
        }
    }
}