using UnityEngine;

namespace Utility
{
    public class OpenURL : MonoBehaviour
    {
        public void Open(string url)
        {
            Application.OpenURL(url);
        }
    }
}