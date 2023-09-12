using FileExplorer;
using UnityEngine;
using UnityEngine.UI;
using Utility;

namespace DialogManagement
{
    public class AppIconSelector : MonoBehaviour
    {
        [SerializeField] private Image icon;
        private string _iconPath;

        public string Icon
        {
            get => _iconPath;
            set
            {
                _iconPath = value;
                icon.sprite = ImageLoader.Instance.LoadImageAsSprite(_iconPath);
            }
        }

        public async void SelectIcon()
        {
            FileExplorerArgs args = new FileExplorerArgs
            {
                DialogType = FileExplorerDialogType.Open,
                Directory = "/",
                Extension = "*.png"
            };
            string path = await DialogManager.Instance.OpenFileDialog<string, FileExplorerArgs>(args);
            if (path != null)
                Icon = path;
        }
    }
}