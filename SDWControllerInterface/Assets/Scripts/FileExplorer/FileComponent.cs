using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FileExplorer
{
    public enum FileComponentType
    {
        File,
        Folder
    }
    
    public class FileComponent : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private TMP_Text text;

        [SerializeField] private Sprite folderIcon;
        [SerializeField] private Sprite fileIcon;
        
        public void SetIcon(FileComponentType iconType)
        {
            icon.sprite = iconType switch
            {
                FileComponentType.File => fileIcon,
                FileComponentType.Folder => folderIcon,
                _ => folderIcon
            };
        }
        
        public void SetName(string fileName)
        {
            text.text = fileName;
        }
    }
}