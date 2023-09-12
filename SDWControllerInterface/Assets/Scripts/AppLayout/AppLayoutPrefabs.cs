using System;
using UnityEngine;

namespace AppLayout
{
    public class AppLayoutPrefabs : MonoBehaviour
    {
        [SerializeField] private GameObject[] appLayouts;
        
        public GameObject GetAppLayoutPrefab(AppLayouts layoutType)
        {
            try
            {
                return appLayouts[(int)layoutType - 1];
            }
            catch (IndexOutOfRangeException e)
            {
                Debug.LogError(e);
            }
            
            return null;
        }
    }
}