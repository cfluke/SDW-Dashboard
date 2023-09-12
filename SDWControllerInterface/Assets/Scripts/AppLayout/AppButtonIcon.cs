using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utility;

namespace AppLayout
{
    public class AppButtonIcon : MonoBehaviour
    {
        [SerializeField] private Image fileSprite;
        [SerializeField] private TMP_Text appName;

        public void Show(string path, string name)
        {
            Sprite sprite = ImageLoader.Instance.LoadImageAsSprite(path);
            if (sprite != null)
                fileSprite.sprite = sprite;
            
            appName.text = name;
            
            // finally, enable
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}