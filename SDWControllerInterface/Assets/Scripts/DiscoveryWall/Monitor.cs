using System;
using System.Collections;
using System.Collections.Generic;
using AppManagement;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace DiscoveryWall
{
    public class Monitor : MonoBehaviour
    {
        [SerializeField] private Color defaultColor;
        [SerializeField] private Color onAppHover;
        private Image _monitor;
        private AppDraggablePool _appDraggablePool;
        
        [CanBeNull] public AppDraggable AppDraggable { get; private set; }
        
        void Start()
        {
            _monitor = GetComponent<Image>();
            _appDraggablePool = FindObjectOfType<AppDraggablePool>();
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (AppDraggable)
                return; // already have an app assigned, ignore any other triggers
            
            AppDraggable app = col.GetComponent<AppDraggable>();
            app.OnMonitorEnter(this);
            _monitor.color = onAppHover;
            
        }

        private void OnTriggerExit2D(Collider2D col)
        {
            AppDraggable app = col.GetComponent<AppDraggable>();
            if (AppDraggable && app != AppDraggable)
                return; // if this monitor has an app already but it's not the same as the exiting app, then ignore
            
            app.OnMonitorExit();
            _monitor.color = defaultColor;
        }

        public void NewApp(App app)
        {
            // borrow a new AppDraggable from pool
            AppDraggable appDraggable = _appDraggablePool.Borrow(app);
            SetApp(appDraggable);
        }

        public void SetApp(AppDraggable appDraggable)
        {
            if (AppDraggable)
                Clear();

            AppDraggable = appDraggable;
            if (!AppDraggable) 
                return; // if null, our work is done
            
            // otherwise, update the position and parent of the newly attached AppDraggable
            // TODO: better solution maybe? 
            Transform appTransform = AppDraggable.transform;
            appTransform.SetParent(transform);
            appTransform.position = transform.position;
            _monitor.color = onAppHover;
        }

        public void Clear()
        {
            if (!AppDraggable) 
                return;
            
            _appDraggablePool.Return(AppDraggable); // return to pool
            AppDraggable = null;
            _monitor.color = defaultColor;
        }
    }
}