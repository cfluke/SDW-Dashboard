using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DialogManagement.FileExplorer
{
    public class FileExplorerNavbar : Navbar
    {
        [SerializeField] private RectTransform navbarRoot;
        [SerializeField] private GameObject navButtonPrefab;
        private Stack<GameObject> _navButtons;

        // callback function/s
        private Action<string> _onNavButton;

        public void Init(Action<string> onNavButton)
        {
            _navButtons = new Stack<GameObject>();
            _onNavButton = onNavButton;
        }

        public void Rebuild(string directory)
        {
            Clear();

            // split and iterate over directory string, i.e., "/" -> "/home/" -> "/home/localuser/" -> etc.
            string[] splitDirectory = directory.Split(Path.DirectorySeparatorChar);
            string accumulatedPath = string.Empty;
            foreach (string subdirectory in splitDirectory)
            {
                // instantiate new button and change it's text to the current subdirectory, i.e., "SSALab"
                GameObject navButton = Instantiate(navButtonPrefab, navbarRoot);
                _navButtons.Push(navButton);
                Button button = navButton.GetComponent<Button>();
                navButton.GetComponentInChildren<TMP_Text>().text = subdirectory;
                
                // give the button a callback with the full path as an argument, i.e., "/home/localuser/SSALab/"
                accumulatedPath = Path.Combine(accumulatedPath, subdirectory);
                string pathCopy = accumulatedPath;
                button.onClick.AddListener(() => _onNavButton.Invoke(pathCopy));
            }
            
            // TODO: fix this cringe
            LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
        }

        private void Clear()
        {
            while (_navButtons.Count != 0)
            {
                GameObject navButtons = _navButtons.Pop();
                Destroy(navButtons);
            }
        }
    }
}