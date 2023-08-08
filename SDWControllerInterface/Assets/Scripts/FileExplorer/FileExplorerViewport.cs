using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FileExplorer
{
    [RequireComponent(typeof(FileComponentPool))]
    public class FileExplorerViewport : MonoBehaviour
    {
        [SerializeField] private ScrollRect scrollRect;
        
        private FileComponentPool _fileComponentPool;
        private Stack<FileComponent> _inUse;

        private void Start()
        {
            _fileComponentPool = GetComponent<FileComponentPool>();
            _inUse = new Stack<FileComponent>();
        }
        
        public void PopulateFolders(string[] folderNames, Action<string> callback)
        {
            foreach (string folderName in folderNames)
            {
                FileComponent newFolder = _fileComponentPool.Borrow(folderName, FileComponentType.Folder, callback);
                _inUse.Push(newFolder);
            }
        }

        public void PopulateFiles(string[] fileNames, Action<string> callback)
        {
            foreach (string fileName in fileNames)
            {
                FileComponent newFile = _fileComponentPool.Borrow(fileName, FileComponentType.File, callback);
                _inUse.Push(newFile);
            }
        }

        public void ClearViewport()
        {
            while (_inUse.Count != 0)
            {
                FileComponent fileComponent = _inUse.Pop();
                _fileComponentPool.Return(fileComponent);
            }
            
            // reset scroll rect to top
            scrollRect.verticalNormalizedPosition = 1.0f;
        }
    }
}