using UnityEngine;

namespace AppLayout
{
    public class AppSprite
    {
        public Sprite Sprite { get; }
        public Color Colour { get; }

        public AppSprite(Sprite sprite, Color color)
        {
            Sprite = sprite;
            Colour = color;
        }
    }
}