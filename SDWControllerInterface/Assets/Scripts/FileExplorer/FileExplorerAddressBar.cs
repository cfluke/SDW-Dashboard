using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FileExplorer
{
    public class FileExplorerAddressBar : MonoBehaviour
    {
        [SerializeField] private GameObject addressBarButtonPrefab;
        private Stack<GameObject> _addressBarObjects;

        void Start()
        {
            _addressBarObjects = new Stack<GameObject>();
        }

        public void RefreshAddressBar(string[] directory, Action<string> callback)
        {
            ClearAddressBar();

            string accumulatedPath = string.Empty;
            foreach (string part in directory)
            {
                accumulatedPath = Path.Combine(accumulatedPath, part);
                GameObject addressBarButton = Instantiate(addressBarButtonPrefab, transform);
                _addressBarObjects.Push(addressBarButton);
                
                Button button = addressBarButton.GetComponent<Button>();
                addressBarButton.GetComponentInChildren<TMP_Text>().text = part;
                string pathCopy = accumulatedPath;
                button.onClick.AddListener(() => callback.Invoke(pathCopy));
            }
            
            LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
        }

        private void ClearAddressBar()
        {
            while (_addressBarObjects.Count != 0)
            {
                GameObject addressBarObject = _addressBarObjects.Pop();
                Destroy(addressBarObject);
            }
        }
    }
}