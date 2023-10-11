using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utility;

namespace AppLayout
{
    public class AppButtonIcon : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private TMP_Text appName;
        [SerializeField] Sprite defaultFileSprite;

        public void Show(Sprite sprite, string title)
        {
            appName.text = title;
            if (sprite != null)
            {
                icon.sprite = sprite;
            }
            else
            {
                icon.sprite = defaultFileSprite;
            }
            
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}