using System;
using System.Collections.Generic;
using SerializableData;
using UnityEngine;

namespace DiscoveryWall
{
    public class DiscoveryWall : MonoBehaviour
    {
        [SerializeField] private GameObject keckDisplayPrefab;
        private List<KeckDisplay> _keckDisplays;

        private void Start()
        {
            _keckDisplays = new List<KeckDisplay>();
        }

        public void Populate(DiscoveryWallSerializable discoveryWallData)
        {
            foreach (var keckDisplay in discoveryWallData.keckDisplays)
                AddKeckDisplay(keckDisplay);
        }

        public void AddKeckDisplay(KeckDisplaySerializable keckDisplayData)
        {
            GameObject keckDisplayObject = Instantiate(keckDisplayPrefab, transform);
            KeckDisplay keckDisplay = keckDisplayObject.GetComponent<KeckDisplay>();
            keckDisplay.Init(keckDisplayData);
            
            // remember KeckDisplay
            _keckDisplays.Add(keckDisplay);
        }
        
        public void Clear()
        {
            foreach (KeckDisplay keckDisplay in _keckDisplays)
                keckDisplay.Clear();
        }

        public DiscoveryWallSerializable GetSerializable()
        {
            List<KeckDisplaySerializable> k = new List<KeckDisplaySerializable>();
            foreach (KeckDisplay keckDisplay in _keckDisplays)
                k.Add(keckDisplay.GetSerializable());
            return new DiscoveryWallSerializable(k);
        }
    }
}