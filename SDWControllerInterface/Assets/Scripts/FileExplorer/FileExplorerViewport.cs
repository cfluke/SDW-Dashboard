using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace FileExplorer
{
    [RequireComponent(typeof(ScrollRect))]
    [RequireComponent(typeof(FileComponentPool))]
    public class FileExplorerViewport : MonoBehaviour
    {
        private ScrollRect scrollRect;
        private FileComponentPool _fileComponentPool;
        private Stack<FileComponent> _inUse;
        
        // callback function/s
        private Action<string> _onFolder, _onFile;

        public void Init(Action<string> onFolder, Action<string> onFile)
        {
            // cache necessary components
            scrollRect = GetComponent<ScrollRect>();
            _fileComponentPool = GetComponent<FileComponentPool>();
            _fileComponentPool.Init();
            _inUse = new Stack<FileComponent>();
            
            _onFolder = onFolder;
            _onFile = onFile;
        }

        public void Rebuild(string[] folders, string[] files)
        {
            Clear();
            PopulateFolders(folders);
            PopulateFiles(files);
        }
        
        private void PopulateFolders(string[] folderNames)
        {
            foreach (string folderName in folderNames)
            {
                FileComponent newFolder = _fileComponentPool.Borrow(folderName, FileComponentType.Folder, _onFolder);
                _inUse.Push(newFolder);
            }
        }

        private void PopulateFiles(string[] fileNames)
        {
            foreach (string fileName in fileNames)
            {
                FileComponent newFile = _fileComponentPool.Borrow(fileName, FileComponentType.File, _onFile);
                _inUse.Push(newFile);
            }
        }

        private void Clear()
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