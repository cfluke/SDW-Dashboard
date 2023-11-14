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

        private void Start()
        {
            _keckDisplays = new List<KeckDisplay>();
        }

        public void Populate(DiscoveryWallData discoveryWallData)
        {
            foreach (var keckDisplay in discoveryWallData.keckDisplays)
                AddKeckDisplay(keckDisplay);
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
        
        public void Clear()
        {
            foreach (KeckDisplay keckDisplay in _keckDisplays)
                keckDisplay.Clear();
        }

        public void Destroy()
        {
            foreach (KeckDisplay keckDisplay in _keckDisplays)
                Destroy(keckDisplay.gameObject);

            _keckDisplays.Clear();
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

        public DiscoveryWallData GetSerializable()
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