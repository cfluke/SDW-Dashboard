using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace FileExplorer
{
    public class FileExplorer : MonoBehaviour
    {
        // default root directory
        private string _currentDirectory = "/";
        
        // undo/redo
        private Stack<string> _backwardHistory;
        private Stack<string> _forwardHistory;

        private FileExplorerViewport _fileExplorerViewport;
        private FileExplorerAddressBar _fileExplorerAddressBar;

        void Start()
        {
            _backwardHistory = new Stack<string>();
            _forwardHistory = new Stack<string>();
            
            // TODO: cache these better
            _fileExplorerViewport = FindObjectOfType<FileExplorerViewport>();
            _fileExplorerAddressBar = FindObjectOfType<FileExplorerAddressBar>();
        }

        public void Refresh()
        {
            UpdateCurrentDirectory(_currentDirectory);
        }

        public void Back()
        {
            if (_backwardHistory.Count == 0) 
                return; // no undo history
            
            // perform undo
            _forwardHistory.Push(_currentDirectory);
            UpdateCurrentDirectory(_backwardHistory.Pop());
        }

        public void Forward()
        {
            if (_forwardHistory.Count == 0) 
                return; // no redo history
            
            // perform redo
            _backwardHistory.Push(_currentDirectory);
            UpdateCurrentDirectory(_forwardHistory.Pop());
        }

        private void UpdateCurrentDirectory(string directory)
        {
            _currentDirectory = directory;
            
            // update GUI
            RefreshAddressBar();
            RefreshFileList();
        }

        private void RefreshAddressBar()
        {
            string[] pathSplit = _currentDirectory.Split(Path.DirectorySeparatorChar);
            _fileExplorerAddressBar.RefreshAddressBar(pathSplit, OnAddressButtonClick);
        }
        
        private void RefreshFileList()
        {
            _fileExplorerViewport.ClearViewport();

            // get files and folders and populate the viewport
            string[] folders = Directory.GetDirectories(_currentDirectory);
            string[] files = Directory.GetFiles(_currentDirectory);
            _fileExplorerViewport.PopulateFolders(folders, OnFolderClick);
            _fileExplorerViewport.PopulateFiles(files, OnFileClick);
        }

        private void OnAddressButtonClick(string directory)
        {
            int index = 1;
            foreach (string historyPath in _backwardHistory)
            {
                if (historyPath == directory)
                    break;
                index++;
            }

            for (int i = 0; i < _backwardHistory.Count - index; i++)
            {
                _forwardHistory.Push(_currentDirectory);
                _currentDirectory = _backwardHistory.Pop();
            }
            UpdateCurrentDirectory(_currentDirectory);
        }

        private void OnFolderClick(string directory)
        {
            // no more redo, append undo history
            _forwardHistory.Clear();
            _backwardHistory.Push(_currentDirectory);
            UpdateCurrentDirectory(directory);
        }

        private void OnFileClick(string directory)
        {
            
        }
    }
}
