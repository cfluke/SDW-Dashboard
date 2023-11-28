using System.Linq;
using DiscoveryWall;
using SerializableData;
using UnityEngine;
using Logger = Logs.Logger;

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

        public AppData[] Apps { get; private set; }

        public void Awake()
        {
            Init();
        }

        public void Init()
        {
            int appCount = GetAppCount();
            Apps = new AppData[appCount];
            _appButtons = GetComponentsInChildren<AppButton>();
            _monitor = GetComponentInParent<Monitor>();
        }

        public void Populate(AppData[] apps)
        {
            Init();
            for (int i = 0; i < apps.Length; i++)
            {
                AppData app = apps[i];
                AppButton button = _appButtons[i];
                if (app != null)
                    button.AddApp(new App(app));
            }
        }

        public void AddApp(int buttonId, App app)
        {
            AppButton appButton = _appButtons.FirstOrDefault(button => button.ID == buttonId);
            if (appButton == null)
            {
                Logger.Instance.LogError("AppButton with ID: " + buttonId + " does not exist");
                return;
            }
            
            // create and set a new AppSerializable in the Apps array, for later saving/serialization
            AppData newApp = new AppData
            {
                // calculate the x, y, w, and h according to monitor position/dimensions and the position/dimensions of
                // the button/portion of the screen. For example w = 3840 * 0.5
                x = (int)(_monitor.Dimensions.x * appButton.Position.x) + _monitor.Offset.x,
                y = (int)(_monitor.Dimensions.y * appButton.Position.y) + _monitor.Offset.y,
                w = (int)(_monitor.Dimensions.x * appButton.Dimensions.x),
                h = (int)(_monitor.Dimensions.y * appButton.Dimensions.y),
                path = app.Path,
                name = app.Name,
                args = app.Args,
                icon = app.IconPath
            };
            Apps[buttonId] = newApp;
        }

        public void RemoveApp(int buttonId)
        {
            AppButton appButton = _appButtons.FirstOrDefault(button => button.ID == buttonId);
            if (appButton == null)
            {
                Logger.Instance.LogError("AppButton with ID: " + buttonId + " does not exist");
                return;
            }

            Apps[buttonId] = null;
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