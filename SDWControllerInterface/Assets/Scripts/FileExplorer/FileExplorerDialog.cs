using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FileExplorer
{
    public enum FileExplorerDialogType
    {
        Save,
        Open
    }
    
    public class FileExplorerDialog : MonoBehaviour
    {
        [SerializeField] private RectTransform dialogPanel;
        [SerializeField] private GameObject fileExplorerPrefab;

        public void SaveDialog()
        {
            GameObject fileExplorerObject = Instantiate(fileExplorerPrefab, dialogPanel);
            FileExplorer fileExplorer = fileExplorerObject.GetComponentInChildren<FileExplorer>();
            fileExplorer.Init(OnConfirm, FileExplorerDialogType.Save, "/", "*.sdw");
        }

        public void OpenDialog()
        {
            GameObject fileExplorerObject = Instantiate(fileExplorerPrefab, dialogPanel);
            FileExplorer fileExplorer = fileExplorerObject.GetComponentInChildren<FileExplorer>();
            fileExplorer.Init(OnConfirm, FileExplorerDialogType.Open, "/", "*.sdw");
        }
        
        private void OnConfirm(string directory)
        {
            // do whatever with the directory
        }
    }
}