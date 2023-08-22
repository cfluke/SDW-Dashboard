using System;
using UnityEngine;

namespace AppLayout
{
    [Serializable]
    public class AppSpriteSettings
    {
        public Sprite sprite;
        public Color colour;
        
        [Header("Possible Extensions")]
        public string[] extensions;
    }
    
    public class AppSpriteSelector : MonoBehaviour
    {
        [SerializeField] private AppSpriteSettings executable;
        [SerializeField] private AppSpriteSettings text;
        [SerializeField] private AppSpriteSettings library;
        [SerializeField] private AppSpriteSettings config;
        [SerializeField] private AppSpriteSettings empty;

        public AppSprite GetSprite(string extension)
        {
            switch (extension)
            {
                case ".exe":
                    return new AppSprite(executable.sprite, executable.colour);
                case ".txt":
                    return new AppSprite(text.sprite, text.colour);
                case ".dll":
                    return new AppSprite(library.sprite, library.colour);
                default:
                    return new AppSprite(empty.sprite, empty.colour);
            }
        }
    }
}