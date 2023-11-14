using System.Collections;
using System.Collections.Generic;
using DialogManagement.FileExplorer;
using TMPro;
using UnityEngine;

namespace DialogManagement.CreateApp
{
    public class AppSelector : MonoBehaviour
    {
        [SerializeField] private TMP_InputField pathField;
        
        public async void Select()
        {
            FileExplorerArgs args = new FileExplorerArgs
            {
                DialogType = FileExplorerDialogType.Open,
                Directory = "/",
                Extension = "*"
            };
            
            string path = await DialogManager.Instance.OpenFileDialog<string, FileExplorerArgs>(args);
            if (path != null)
                pathField.text = path;
        }
    }
}