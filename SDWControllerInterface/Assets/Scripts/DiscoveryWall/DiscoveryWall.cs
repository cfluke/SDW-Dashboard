using System.Collections.Generic;
using SerializableData;
using UnityEngine;

namespace DiscoveryWall
{
    public class DiscoveryWall : MonoBehaviour
    {
        [SerializeField] private KeckDisplay[] keckDisplays;

        public void Populate(DiscoveryWallSerializable discoveryWallData)
        {
            // TODO: dynamic instantiation of KeckDisplays
            // the following function assumes the KeckDisplay[] array is populated from the inspector
            // however, if we make it so clients can communicate their existence, then we could make them communicate their KeckDisplay info too
            // then, using that KeckDisplay info, we dynamically instantiate KeckDisplay prefabs, making the UI more robust
            // however, this also means more moving parts, such as network comms and config files

            for (int i = 0; i < keckDisplays.Length; i++)
            {
                KeckDisplay keckDisplay = keckDisplays[i];
                KeckDisplaySerializable keckDisplayData = discoveryWallData.keckDisplays[i];
                keckDisplay.Populate(keckDisplayData);
            }
        }
        
        public void Clear()
        {
            foreach (KeckDisplay keckDisplay in keckDisplays)
                keckDisplay.Clear();
        }

        public DiscoveryWallSerializable GetSerializable()
        {
            List<KeckDisplaySerializable> k = new List<KeckDisplaySerializable>();
            foreach (KeckDisplay keckDisplay in keckDisplays)
                k.Add(keckDisplay.GetSerializable());
            return new DiscoveryWallSerializable(k);
        }
    }
}