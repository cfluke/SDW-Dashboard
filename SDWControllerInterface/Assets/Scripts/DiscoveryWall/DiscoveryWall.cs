using System;
using System.Collections.Generic;
using SerializableData;
using UnityEngine;
using Logger = Logs.Logger;

namespace DiscoveryWall
{
    public class DiscoveryWall : MonoBehaviour
    {
        [SerializeField] private GameObject discoveryWallPlaceholder;
        [SerializeField] private GameObject keckDisplayPrefab;
        private List<KeckDisplay> _keckDisplays;

        #region singleton
        
        private static DiscoveryWall _instance;
        public static DiscoveryWall Instance
        {
            get
            {
                if (_instance == null)
                    Logger.Instance.LogError("WidgetManager does not exist - it must.");

                return _instance;
            }
        }
        
        private void Awake()
        {
            if (_instance == null)
                _instance = this;
            else
            {
                Logger.Instance.LogWarning("Duplicate WidgetManager found in the scene! Deleting the duplicate.");
                Destroy(gameObject);
            }
        }

        #endregion
        
        private void Start()
        {
            _keckDisplays = new List<KeckDisplay>();
        }

        public void AddKeckDisplay(KeckDisplayData keckDisplayData)
        {
            GameObject keckDisplayObject = Instantiate(keckDisplayPrefab, transform);
            KeckDisplay keckDisplay = keckDisplayObject.GetComponent<KeckDisplay>();
            keckDisplay.Init(keckDisplayData);
            Logger.Instance.LogSuccess("New KeckDisplay: " + keckDisplayData.id);
            
            // remember KeckDisplay
            _keckDisplays.Add(keckDisplay);
            
            // deactivate "Awaiting KeckDisplays..." placeholder
            discoveryWallPlaceholder.SetActive(false);
        }

        public void Destroy()
        {
            foreach (KeckDisplay keckDisplay in _keckDisplays)
                Destroy(keckDisplay.gameObject);

            _keckDisplays.Clear();
        }

        // TODO: finish this
        private void SortKeckDisplays()
        {
            foreach (KeckDisplay keckDisplay in _keckDisplays)
            {
                
            }
        }

        public void StartApps()
        {
            foreach (KeckDisplay keckDisplay in _keckDisplays)
                keckDisplay.ToggleApps(true);
        }

        public void StopApps()
        {
            foreach (KeckDisplay keckDisplay in _keckDisplays)
                keckDisplay.ToggleApps(false);
        }

        public DiscoveryWallData Serialize()
        {
            List<KeckDisplayData> k = new List<KeckDisplayData>();
            foreach (KeckDisplay keckDisplay in _keckDisplays)
                k.Add(keckDisplay.GetSerializable());
            return new DiscoveryWallData
            {
                keckDisplays = k.ToArray()
            };
        }
    }
}