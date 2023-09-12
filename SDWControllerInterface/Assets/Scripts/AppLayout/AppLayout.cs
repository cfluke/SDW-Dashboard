using System;
using DiscoveryWall;
using SerializableData;
using UnityEngine;

namespace AppLayout
{
    public enum AppLayouts
    {
        None,
        One,
        OneOne,
        OneTwo,
        TwoOne,
        TwoTwo,
        OneOneOne,
    }
    
    public class AppLayout : MonoBehaviour
    {
        [SerializeField] private AppLayouts layoutType;
        private AppButton[] _appButtons;
        private Monitor _monitor;
        
        public AppSerializable[] Apps { get; private set; }
        
        public void Populate(AppSerializable[] apps)
        {
            _appButtons = GetComponentsInChildren<AppButton>();
            _monitor = GetComponentInParent<Monitor>();
            
            Apps = new AppSerializable[apps.Length];
            for (int i = 0; i < apps.Length; i++)
            {
                AppSerializable app = apps[i];
                if (app.path.Length > 0)
                {
                    Apps[i] = app;
                    _appButtons[i].ShowAppIcon(app.path);
                }
            }
        }

        public void AddApp(int buttonId, string path, float x, float y, float w, float h)
        {
            _monitor ??= GetComponentInParent<Monitor>();
            _appButtons ??= GetComponentsInChildren<AppButton>();
            Apps ??= new AppSerializable[_appButtons.Length];

            int xVal = (int)(x * _monitor.Dimensions.x) + _monitor.Offset.x;
            int yVal = (int)(y * _monitor.Dimensions.y) + _monitor.Offset.y;
            int width = (int)(w * _monitor.Dimensions.x);
            int height = (int)(h * _monitor.Dimensions.y);
            
            AppSerializable appSerializable = new AppSerializable(path, xVal, yVal, width, height);
            Apps[buttonId] = appSerializable;
            _appButtons[buttonId].ShowAppIcon(path);
        }
        
        public void Clear()
        {
            foreach (AppButton appButton in _appButtons)
                appButton.Clear();
        }

        public AppLayouts GetLayoutType()
        {
            return layoutType;
        }
    }
}