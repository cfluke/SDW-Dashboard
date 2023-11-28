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

        private void Start()
        {
            // cache the initial AppLayout child (which will be 1 (One))
            _layout = GetComponentInChildren<AppLayout.AppLayout>();
        }

        private void OnTransformChildrenChanged()
        {
            // cache the new AppLayout child whenever it is replaced
            _layout = GetComponentInChildren<AppLayout.AppLayout>();
        }

        public void Init(MonitorData monitorData)
        {
            Dimensions = new Vector2Int(monitorData.w, monitorData.h);
            Offset = new Vector2Int(monitorData.x, monitorData.y);

            AppLayoutPrefabs appLayouts = FindObjectOfType<AppLayoutPrefabs>();
            if (Enum.TryParse(monitorData.layout, out AppLayouts appLayoutType))
            {
                if (appLayoutType == AppLayouts.None)
                    return; // no need to populate anything
                
                GameObject layoutPrefab = appLayouts.GetAppLayoutPrefab(appLayoutType);
                AppLayout.AppLayout layout = SetLayout(layoutPrefab);
                layout.Populate(monitorData.apps);
            }
        }

        public AppLayout.AppLayout SetLayout(GameObject layoutPrefab)
        {
            if (transform.childCount > 1)
                Destroy(transform.GetChild(0).gameObject); // destroy old layout
            
            GameObject layout = Instantiate(layoutPrefab, transform);
            layout.transform.SetAsFirstSibling();

            AppLayout.AppLayout appLayout = layout.GetComponent<AppLayout.AppLayout>();
            appLayout.Init();
            return appLayout;
        }

        public MonitorData GetSerializable()
        {
            string layoutType = AppLayouts.None.ToString();
            AppData[] apps = Array.Empty<AppData>();

            if (_layout != null)
            {
                layoutType = _layout.GetLayoutType().ToString();
                apps = _layout.Apps;
            }
            
            return new MonitorData
            {
                x = Offset.x,
                y = Offset.y,
                w = Dimensions.x,
                h = Dimensions.y,
                layout = layoutType,
                apps = apps
            };
        }

        public AppData[] GetSerializableApps()
        {
            return _layout != null ? _layout.Apps : Array.Empty<AppData>();
        }
    }
}
