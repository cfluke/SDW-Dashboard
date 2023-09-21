using System.Linq;
using DiscoveryWall;
using SerializableData;
using Unity.VisualScripting;
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

        public void Init()
        {
            int appCount = GetAppCount();
            Apps = new AppSerializable[appCount];
            _appButtons = GetComponentsInChildren<AppButton>();
            _monitor = GetComponentInParent<Monitor>();
        }

        public void Populate(AppSerializable[] apps)
        {
            Init();
            for (int i = 0; i < apps.Length; i++)
            {
                AppSerializable app = apps[i];
                if (app.path.Length > 0)
                    AddApp(i, new App(app));
            }
        }

        public void AddApp(int buttonId, App app)
        {
            AppButton appButton = _appButtons.FirstOrDefault(button => button.ID == buttonId);
            if (appButton == null)
            {
                Debug.LogError("AppButton with ID: " + buttonId + " does not exist");
                return;
            }
            
            // calculate the x, y, w, and h according to monitor position/dimensions and the position/dimensions of
            // the button/portion of the screen. For example w = 3840 * 0.5
            Vector2 buttonPosition = appButton.Position;
            Vector2 buttonDimensions = appButton.Dimensions;
            int x = (int)(_monitor.Dimensions.x * buttonPosition.x) + _monitor.Offset.x;
            int y = (int)(_monitor.Dimensions.y * buttonPosition.y) + _monitor.Offset.y;
            int w = (int)(_monitor.Dimensions.x * buttonDimensions.x);
            int h = (int)(_monitor.Dimensions.y * buttonDimensions.y);
            
            // create and set a new AppSerializable in the Apps array, for later saving/serialization
            AppSerializable newApp = new AppSerializable(app.Path, x, y, w, h, app.Name, app.Args, app.IconPath);
            Apps[buttonId] = newApp;
            
            // show the AppIcon in the button
            appButton.AppButtonIcon.Show(app.Icon, string.IsNullOrEmpty(app.Name) ? app.Path : app.Name);
        }
        
        public void Clear()
        {
            foreach (AppButton appButton in _appButtons)
                appButton.AppButtonIcon.Hide();
        }

        public AppLayouts GetLayoutType()
        {
            return layoutType;
        }

        private int GetAppCount()
        {
            switch (layoutType)
            {
                default:
                    return 0;
                case AppLayouts.One:
                    return 1;
                case AppLayouts.OneOne:
                    return 2;
                case AppLayouts.OneTwo:
                case AppLayouts.TwoOne:
                case AppLayouts.OneOneOne:
                    return 3;
                case AppLayouts.TwoTwo:
                    return 4;
            }
        }
    }
}