using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AppLayout
{
    public class AppButtonIcon : MonoBehaviour
    {
        [SerializeField] private Image fileSprite;
        [SerializeField] private Image appSprite;
        [SerializeField] private TMP_Text appName;
        [SerializeField] private TMP_Text appExtension;

        public void Show(string path)
        {
            string extension = Path.GetExtension(path);
            appExtension.text = extension;

            AppSpriteSelector spriteSelector = FindObjectOfType<AppSpriteSelector>();
            AppSprite sprite = spriteSelector.GetSprite(extension);
            appSprite.sprite = sprite.Sprite;
            fileSprite.color = sprite.Colour;
            
            string fileName = Path.GetFileNameWithoutExtension(path);
            appName.text = fileName;
            
            // finally, enable
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}