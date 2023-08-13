using System;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FileExplorer
{
    public class FileExplorerFooter : MonoBehaviour
    {
        [SerializeField] private TMP_Text errorMessageText;
        [SerializeField] private TMP_InputField filenameInputField;
        [SerializeField] private Button confirmButton;

        private string _extension;
        
        public void Init(FileExplorerDialogType dialogType, string extension, Action<string> onInputFieldUpdate)
        {
            SetConfirmButtonText(dialogType);
            filenameInputField.onValueChanged.AddListener(OnInputFieldUpdate);
            filenameInputField.onValueChanged.AddListener(value => onInputFieldUpdate(value));

            _extension = extension;
        }

        public void Rebuild()
        {
            SetFileName(string.Empty);
            SetErrorMessage(string.Empty);
        }

        public void SetFileName(string fileName)
        {
            filenameInputField.text = fileName;
        }

        public void SetErrorMessage(string errorMessage)
        {
            errorMessageText.text = errorMessage;
        }

        private void SetConfirmButtonText(FileExplorerDialogType dialogType)
        {
            TMP_Text confirmButtonText = confirmButton.GetComponentInChildren<TMP_Text>();

            switch (dialogType)
            { 
                default:
                case FileExplorerDialogType.Save:
                    confirmButtonText.text = "Save";
                    filenameInputField.interactable = true;
                    break;
                case FileExplorerDialogType.Open:
                    confirmButtonText.text = "Open";
                    filenameInputField.interactable = false;
                    break;
            }
        }

        private void OnInputFieldUpdate(string value)
        {
            confirmButton.interactable = value != string.Empty;
        }
    }
}