using System;
using UnityEngine;

namespace DialogManagement.FileExplorer
{
    public class FileExplorerSidebar : MonoBehaviour
    {
        private Func<string, bool> _updateDirectoryCallback;

        public void Init(Func<string, bool> updateDirectory)
        {
            _updateDirectoryCallback = updateDirectory;
        }
        
        public void NavigateTo(string path)
        {
            _updateDirectoryCallback(path);
        }
    }
}