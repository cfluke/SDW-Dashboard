using System;
using DialogManagement;
using DialogManagement.LayoutSelector;
using DiscoveryWall;
using UnityEngine;

namespace AppLayout
{
    public class LayoutSelectorButton : MonoBehaviour
    {
        [SerializeField] private GameObject layoutSelectorPrefab;
        private Monitor _monitor;

        private void Start()
        {
            _monitor = GetComponentInParent<Monitor>();
        }

        public async void OpenLayoutSelector()
        {
            LayoutSelectionArgs args = new LayoutSelectionArgs();
            GameObject layoutPrefab = await DialogManager.Instance.Open<GameObject, LayoutSelectionArgs>(layoutSelectorPrefab, args);
            if (layoutPrefab == null)
                return; // no updating needed, user closed the dialog

            _monitor.SetLayout(layoutPrefab);
        }
    }
}