using UnityEngine;
using System.IO;

namespace Utility
{
    public class ImageLoader : MonoBehaviour
    {
        private static ImageLoader _instance;

        public static ImageLoader Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<ImageLoader>();

                    if (_instance == null)
                    {
                        GameObject singleton = new GameObject("ImageLoader");
                        _instance = singleton.AddComponent<ImageLoader>();
                    }
                }

                return _instance;
            }
        }

        // Function to load an image from a file path and convert it to a Sprite
        public Sprite LoadImageAsSprite(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                return null;
            
            if (!File.Exists(filePath))
            {
                Debug.LogError("Image file does not exist at path: " + filePath);
                return null;
            }

            // Load the image file as a Texture2D
            Texture2D texture = LoadTexture(filePath);

            if (texture == null)
            {
                Debug.LogError("Failed to load image from path: " + filePath);
                return null;
            }

            // Create a Sprite from the loaded Texture2D
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
            return sprite;
        }

        // Function to load a Texture2D from a file path
        private Texture2D LoadTexture(string filePath)
        {
            byte[] fileData = File.ReadAllBytes(filePath);
            Texture2D texture = new Texture2D(2, 2); // Create a placeholder texture

            if (texture.LoadImage(fileData))
                return texture;
            return null;
        }
    }
}