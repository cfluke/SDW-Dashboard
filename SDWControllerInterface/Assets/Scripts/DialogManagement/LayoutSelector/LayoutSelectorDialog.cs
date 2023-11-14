using AppLayout;
using UnityEngine;

namespace DialogManagement.LayoutSelector
{
    public class LayoutSelectionArgs
    {
    }

    public class LayoutSelectorDialog : Dialog<GameObject, LayoutSelectionArgs>
    {
        private AppLayoutPrefabs _layoutPrefabs;
        private GameObject _appLayoutOutput;
        
        public override void Init(LayoutSelectionArgs parameters)
        {
            _layoutPrefabs = FindObjectOfType<AppLayoutPrefabs>();
        }

        public void SelectLayout(int layoutId)
        {
            _appLayoutOutput = _layoutPrefabs.GetAppLayoutPrefab((AppLayouts)layoutId); 
            Confirm();
        }

        public override void Confirm()
        {
            OnConfirm.Invoke(_appLayoutOutput);
        }

        public override void Cancel()
        {
            OnConfirm.Invoke(null);
        }
    }
}
/*public class LayoutSelectorDialog : MonoBehaviour
{
    [SerializeField] private GameObject layoutSelectionDialog; //Layout selection popup
    private RectTransform _monitor; //Reference to the monitor that was clicked

    //shows layout selection popup
    public void OpenDialog(RectTransform monitor)
    {
        if(monitor != null)
        {
            layoutSelectionDialog.SetActive(true);
            _monitor = monitor;
        }        
    }

    //Sets selected layout to selected monitor
    public void OnSelect(GameObject layoutPrefab)
    {
        if (_monitor != null)
        {
            //Destroys old layout if there was one already selected
            if(_monitor.childCount > 1)
            {
                Destroy(_monitor.GetChild(0).gameObject);
            }

            //Instantiats selected layout onto selected monitor and
            //sets as first child of monitor (so it is displayed under the plus button)
            GameObject layout = Instantiate(layoutPrefab, _monitor);
            layout.transform.SetAsFirstSibling();
            layoutSelectionDialog.SetActive(false);
            _monitor = null;

            AppLayout.AppLayout appLayout = layout.GetComponent<AppLayout.AppLayout>();
            appLayout.Init();
        }
        layoutSelectionDialog.SetActive(false);
    }
}*/
