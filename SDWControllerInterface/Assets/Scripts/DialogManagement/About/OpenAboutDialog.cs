using UnityEngine;

namespace DialogManagement.About
{
    public class OpenAboutDialog : MonoBehaviour
    {
        [SerializeField] private GameObject aboutDialogPrefab;
        
        public async void Open()
        {
            AboutDialogArgs args = new AboutDialogArgs();
            await DialogManager.Instance.Open<object, AboutDialogArgs>(aboutDialogPrefab, args);
        }
    }
}