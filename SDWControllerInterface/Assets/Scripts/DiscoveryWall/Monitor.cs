using System;
using AppLayout;
using SerializableData;
using UnityEngine;

namespace DiscoveryWall
{
    public class Monitor : MonoBehaviour
    {
        public Vector2Int Dimensions { get; private set; } = new(3840, 2160); // default to 4k
        public Vector2Int Offset { get; private set; } = Vector2Int.zero;
        
        private AppLayout.AppLayout _layout;

        private void OnTransformChildrenChanged()
        {
            _layout = GetComponentInChildren<AppLayout.AppLayout>();
        }

        public void Init(MonitorSerializable monitorData)
        {
            Dimensions = new Vector2Int(monitorData.w, monitorData.h);
            Offset = new Vector2Int(monitorData.x, monitorData.y);

            AppLayoutPrefabs appLayouts = FindObjectOfType<AppLayoutPrefabs>();
            if (Enum.TryParse(monitorData.layout, out AppLayouts appLayoutType))
            {
                if (appLayoutType == AppLayouts.None)
                    return; // no need to populate anything
                
                GameObject appLayoutPrefab = appLayouts.GetAppLayoutPrefab(appLayoutType);
                GameObject appLayoutObject = Instantiate(appLayoutPrefab, transform);
                appLayoutObject.transform.SetAsFirstSibling();

                AppLayout.AppLayout layout = appLayoutObject.GetComponent<AppLayout.AppLayout>();
                layout.Populate(monitorData.apps);
            }
        }
        
        public void Clear()
        {
            if (_layout != null)
                _layout.Clear();
        }

        public MonitorSerializable GetSerializable()
        {
            string layoutType = AppLayouts.None.ToString();
            AppSerializable[] apps = Array.Empty<AppSerializable>();

            if (_layout != null)
            {
                layoutType = _layout.GetLayoutType().ToString();
                apps = _layout.Apps;
            }
            
            return new MonitorSerializable(Offset.x, Offset.y, Dimensions.x, Dimensions.y, layoutType, apps);
        }

        public AppSerializable[] GetSerializableApps()
        {
            return _layout != null ? _layout.Apps : Array.Empty<AppSerializable>();
        }
    }
}
