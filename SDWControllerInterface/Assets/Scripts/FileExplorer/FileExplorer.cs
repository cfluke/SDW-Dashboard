using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace FileExplorer
{
    public class FileExplorer : MonoBehaviour
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
        
        // callback functions
        private Action<string> _onConfirm;
        
        public void Init(Action<string> onConfirm, FileExplorerDialogType dialogType, string directory, string extension)
        {
            _backwardHistory = new Stack<string>();
            _forwardHistory = new Stack<string>();

            _onConfirm = onConfirm;
            _extension = extension;

            navbar.Init(OnNavButton);
            viewport.Init(OnFolder, OnFile);
            footer.Init(dialogType, extension, OnInputFieldUpdate);

            UpdateDirectory(directory);
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
        
        public void OnConfirm()
        {
            _onConfirm.Invoke(_currentDirectory);
        }

        private void OnInputFieldUpdate(string value)
        {
            _selectedFile = value;
        }

        #endregion
    }
}
