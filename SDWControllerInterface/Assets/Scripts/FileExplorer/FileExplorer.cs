using System;
using System.Collections.Generic;
using System.IO;
using DialogManagement;
using UnityEngine;

namespace FileExplorer
{
    public enum FileExplorerDialogType
    {
        Save,
        Open
    }
    
    public class FileExplorer : Dialog<string, FileExplorerArgs>
    {
        private string _currentDirectory = "/";
        private string _selectedFile = string.Empty;
        private string _extension = string.Empty;
        private string[] _folders;
        private string[] _files;
        
        // undo/redo
        private Stack<string> _backwardHistory;
        private Stack<string> _forwardHistory;
        
        // components/subsections of the file explorer to manage
        [SerializeField] private FileExplorerNavbar navbar;
        [SerializeField] private FileExplorerSidebar sidebar;
        [SerializeField] private FileExplorerViewport viewport;
        [SerializeField] private FileExplorerFooter footer;

        public override void Init(FileExplorerArgs parameters)
        {
            _backwardHistory = new Stack<string>();
            _forwardHistory = new Stack<string>();

            _extension = parameters.Extension;

            navbar.Init(OnNavButton);
            viewport.Init(OnFolder, OnFile);
            footer.Init(parameters.DialogType, _extension, OnInputFieldUpdate);

            UpdateDirectory(parameters.Directory);
        }
        
        private bool UpdateDirectory(string directory)
        {
            try
            {
                // these function calls can result in UnauthorizedAccessExceptions
                _folders = Directory.GetDirectories(directory);
                _files = Directory.GetFiles(directory, _extension);
            
                _currentDirectory = directory;
                _selectedFile = string.Empty;

                // update GUI
                navbar.Rebuild(_currentDirectory);
                viewport.Rebuild(_folders, _files);
                footer.Rebuild();
            }
            catch (UnauthorizedAccessException)
            {
                footer.SetErrorMessage("Permission Denied");
                return false;
            }
            return true;
        }

        #region navbar functions
        
        public void OnRefresh()
        {
            UpdateDirectory(_currentDirectory);
        }

        public void OnBack()
        {
            if (_backwardHistory.Count == 0) 
                return; // no undo history
            
            // perform undo
            _forwardHistory.Push(_currentDirectory);
            UpdateDirectory(_backwardHistory.Pop());
        }

        public void OnForward()
        {
            if (_forwardHistory.Count == 0) 
                return; // no redo history
            
            // perform redo
            _backwardHistory.Push(_currentDirectory);
            UpdateDirectory(_forwardHistory.Pop());
        }

        private void OnNavButton(string directory)
        {
            // keep performing back/undo until desired directory is reached
            string prevDirectory = _currentDirectory;
            while (prevDirectory != directory)
            {
                _forwardHistory.Push(prevDirectory);
                prevDirectory = _backwardHistory.Pop();
            }
            
            UpdateDirectory(prevDirectory);
        }

        #endregion

        #region viewport functions

        private void OnFolder(string directory)
        {
            string currentDirectory = _currentDirectory;
            if (UpdateDirectory(directory))
            {
                // no more redo, append undo history
                _forwardHistory.Clear();
                _backwardHistory.Push(currentDirectory);
            }
        }

        private void OnFile(string filename)
        {
            _selectedFile = filename;
            footer.SetFileName(filename);
        }

        #endregion

        #region sidebar functions

        

        #endregion

        #region footer functions

        private void OnInputFieldUpdate(string value)
        {
            _selectedFile = value;
        }

        public async void Confirm()
        {
            bool confirm = await DialogManager.Instance.OpenConfirmDialog<bool, ConfirmDialogArgs>(null);
            
            if (confirm) 
                OnConfirm.Invoke(Path.Combine(_currentDirectory, _selectedFile));
        }
        public void Cancel()
        {
            OnConfirm.Invoke(null);
        }
        
        #endregion
    }
}
