using System.Threading.Tasks;
using FileExplorer;
using UnityEngine;

namespace DialogManagement
{
    public class DialogManager : MonoBehaviour
    {
        [SerializeField] private RectTransform dialogsRoot;
        [SerializeField] private GameObject fileExplorerDialogPrefab;
        [SerializeField] private GameObject confirmDialogPrefab;

        public async Task<T> Open<T, TParams>(GameObject dialogPrefab, TParams parameters) where TParams : class
        {
            // create and initialise dialog object
            GameObject dialogObject = Instantiate(dialogPrefab, dialogsRoot);
            Dialog<T, TParams> dialog = dialogObject.GetComponent<Dialog<T, TParams>>();
            dialog.Init(parameters);

            // register OnConfirm behaviour
            TaskCompletionSource<T> tcs = new TaskCompletionSource<T>();
            dialog.OnConfirm += result =>
            {
                tcs.SetResult(result);
                Destroy(dialogObject);
            };

            return await tcs.Task;
        }
        
        public async Task<T> OpenFileDialog<T, TParams>(TParams parameters) where TParams : class
        {
            return await Open<T, TParams>(fileExplorerDialogPrefab, parameters);
        }
        
        public async Task<T> OpenConfirmDialog<T, TParams>(TParams parameters) where TParams : class
        {
            return await Open<T, TParams>(confirmDialogPrefab, parameters);
        }
    }
}