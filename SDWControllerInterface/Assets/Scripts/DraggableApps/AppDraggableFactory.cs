using System;
using AppLayout;
using UnityEngine;

namespace DraggableApps
{
    public class AppDraggableFactory : MonoBehaviour
    {
        #region singleton
        
        private static AppDraggableFactory _instance;
        public static AppDraggableFactory Instance
        {
            get
            {
                if (_instance == null)
                    Logs.Logger.Instance.LogError("AppDraggableManager does not exist - it must.");

                return _instance;
            }
        }
        
        private void Awake()
        {
            if (_instance == null)
                _instance = this;
            else
            {
                Logs.Logger.Instance.LogWarning("Duplicate WidgetManager found in the scene! Deleting the duplicate.");
                Destroy(gameObject);
            }
        }

        #endregion
        
        [SerializeField] private GameObject appDraggablePrefab;
        
        public GameObject CreateAppDraggable(Transform parent, App app, Action<App> onAppDragBegin, Action<App> onAppDragEnd, float size = 1.0f)
        {
            // create app draggable
            GameObject appDraggableObject = Instantiate(appDraggablePrefab, parent);
            AppDraggable appDraggable = appDraggableObject.GetComponent<AppDraggable>();
            appDraggable.Init(app);
            appDraggable.SetSize(size);
            
            // register callback events
            appDraggable.OnDragBegin += onAppDragBegin;
            appDraggable.OnDragEnd += onAppDragEnd;
            return appDraggableObject;
        }
        
        public GameObject CreateGhostApp(Transform parent, App app, Color color, float size = 1.0f)
        {
            // create as usual
            GameObject appDraggableObject = Instantiate(appDraggablePrefab, parent);
            AppDraggable appDraggable = appDraggableObject.GetComponent<AppDraggable>();
            appDraggable.Init(app);
            appDraggable.SetSize(size);
            
            // set colour
            appDraggable.SetColor(color); 
            return appDraggableObject;
        }
    }
}
