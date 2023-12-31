using System;
using UnityEngine;
using Logger = Logs.Logger;

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
                Logger.Instance.LogError(e.Message);
            }
            
            return null;
        }
    }
}