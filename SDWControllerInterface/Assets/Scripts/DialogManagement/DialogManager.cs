using System.Threading.Tasks;
using UnityEngine;

namespace DialogManagement
{
    public class DialogManager : MonoBehaviour
    {
        [SerializeField] private RectTransform dialogsRoot;
        [SerializeField] private GameObject appCreationDialog;
        [SerializeField] private GameObject fileExplorerDialogPrefab;
        [SerializeField] private GameObject confirmDialogPrefab;
        
        #region singleton
        
        private static DialogManager _instance;

        public static DialogManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<DialogManager>();
                    if (_instance == null)
                        Debug.LogError("DialogManager doesn't exist - it should");
                }
                return _instance;
            }
        }
        
        #endregion
        
        private void Awake()
        {
            // ensure there is only one instance
            if (_instance != null && _instance != this)
                Destroy(gameObject);
        }
        
        /// <param name="dialogPrefab">the prefab of the Dialog that you want to open</param>
        /// <param name="args">a custom class defined by you, for sending arguments through</param>
        /// <typeparam name="T">the return type of the Dialog, i.e., a Confirm Dialog would use bool</typeparam>
        /// <typeparam name="TParams">a custom class type defined by you, for sending arguments through</typeparam>
        /// <returns>whatever your Dialog returns on Confirm/Cancel, same type as parameter 'T'</returns>
        public async Task<T> Open<T, TParams>(GameObject dialogPrefab, TParams args) where TParams : class
        {
            // create and initialise dialog object
            GameObject dialogObject = Instantiate(dialogPrefab, dialogsRoot);
            Dialog<T, TParams> dialog = dialogObject.GetComponent<Dialog<T, TParams>>();
            dialog.Init(args);

            // register OnConfirm behaviour
            TaskCompletionSource<T> tcs = new TaskCompletionSource<T>();
            dialog.OnConfirm += result =>
            {
                tcs.SetResult(result);
                Destroy(dialogObject);
            };

            return await tcs.Task;
        }
        
        public async Task<T> OpenAppCreationDialog<T, TParams>(TParams parameters) where TParams : class
        {
            return await Open<T, TParams>(appCreationDialog, parameters);
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