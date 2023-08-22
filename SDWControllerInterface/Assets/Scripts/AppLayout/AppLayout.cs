using DiscoveryWall;
using UnityEngine;

namespace AppLayout
{
    public class AppLayout : MonoBehaviour
    {
        private AppButton[] _appButtons;
        
        private void Start()
        {
            _appButtons = GetComponentsInChildren<AppButton>();
        }
        
        public void AddToMonitor(string path, float x, float y, float w, float h)
        {
            Monitor monitor = GetComponentInParent<Monitor>();
            monitor.AddApp(path, x, y, w, h);
        }
        
        public void Clear()
        {
            foreach (AppButton appButton in _appButtons)
                appButton.Clear();
        }
    }
}