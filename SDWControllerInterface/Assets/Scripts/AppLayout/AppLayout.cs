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

        public Monitor Monitor { get; private set; }
        public AppSerializable[] Apps { get; private set; }

        public void Init()
        {
            int appCount = GetAppCount();
            Apps = new AppSerializable[appCount];
            _appButtons = GetComponentsInChildren<AppButton>();
            Monitor = GetComponentInParent<Monitor>();
        }

        public void Populate(AppSerializable[] apps)
        {
            Init();
            for (int i = 0; i < apps.Length; i++)
            {
                AppSerializable app = apps[i];
                if (app.path.Length > 0)
                    AddApp(i, app);
            }
        }

        public void AddApp(int buttonId, AppSerializable app)
        {
            Apps[buttonId] = app;
            _appButtons[buttonId].ShowAppIcon(app.icon, string.IsNullOrEmpty(app.name) ? app.path : app.name);
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