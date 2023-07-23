using DiscoveryWall;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace AppManagement
{
    [RequireComponent(typeof(Image))]
    public class AppDraggable : MonoBehaviour, IDragHandler, IEndDragHandler
    {
        [SerializeField] private TMP_Text appTitle;
        
        public App App { get; private set; }
        [CanBeNull] private Monitor _monitor;
        [CanBeNull] private Monitor _prevMonitor;

        public void InitAppDraggable(App app)
        {
            App = app;
            appTitle.text = App.appName;
        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.position += (Vector3)eventData.delta;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            // free previous monitor if there was one
            if (_prevMonitor)
                _prevMonitor.SetApp(null);
            
            // on drop, we're either on a monitor or the draggable can be deleted/re-used
            if (!_monitor)
            {
                // delete/re-use
                Debug.Log("Delete this AppDraggable");
                return;
            }
            
            // assign and snap to monitor
            _prevMonitor = _monitor;
            _monitor.SetApp(this);
        }

        public void OnMonitorEnter(Monitor monitor)
        {
            _monitor = monitor;
        }

        public void OnMonitorExit()
        {
            _monitor = null;
        }
    }
}