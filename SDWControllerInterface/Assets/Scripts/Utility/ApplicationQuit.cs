using DialogManagement;
using DialogManagement.Confirm;
using UnityEngine;

namespace Utility
{
    public class ApplicationQuit : MonoBehaviour
    {
        public async void Quit()
        {
            ConfirmDialogArgs args = new ConfirmDialogArgs
            {
                Message = "Exit the application?"
            };
            
            bool confirm = await DialogManager.Instance.OpenConfirmDialog<bool, ConfirmDialogArgs>(args);
            if (confirm) 
                Application.Quit();
        }
    }
}