using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppManagement
{
    public class AppBar : MonoBehaviour
    {
        [SerializeField] private RectTransform appBarParent;
        private AppDraggablePool _appPool;
        
        void Start()
        {
            _appPool = FindObjectOfType<AppDraggablePool>();
        }

        public void AddApp(App app)
        {
            AppDraggable appDraggable = _appPool.Borrow(app);
            appDraggable.transform.SetParent(appBarParent);
        }
    }
}