using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppManagement
{
    public class AppDraggablePool : MonoBehaviour
    {
        [SerializeField] private GameObject appPrefab;
        [SerializeField] private int poolSize;
        private Queue<GameObject> _objectPool;

        void Start()
        {
            _objectPool = new Queue<GameObject>();

            for (int i = 0; i < poolSize; i++)
            {
                GameObject app = Instantiate(appPrefab, transform);
                app.SetActive(false);
                _objectPool.Enqueue(app);
            }
        }

        public AppDraggable Borrow(App app)
        {
            GameObject appObject = _objectPool.Dequeue();

            // update the AppDraggable icon, title, etc. before enabling
            AppDraggable appDraggable = appObject.GetComponent<AppDraggable>();
            appDraggable.InitAppDraggable(app);

            // enable and borrow
            appObject.SetActive(true);
            return appDraggable;
        }

        public void Return(AppDraggable appDraggable)
        {
            GameObject appObject = appDraggable.gameObject;
            appObject.SetActive(false);
            _objectPool.Enqueue(appObject);
        }
    }
}