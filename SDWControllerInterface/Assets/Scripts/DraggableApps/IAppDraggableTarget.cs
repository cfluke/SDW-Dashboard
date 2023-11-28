using AppLayout;
using JetBrains.Annotations;
using UnityEngine.EventSystems;

namespace DraggableApps
{
    public interface IAppDraggableTarget
    {
        public void OnAppEnter(PointerEventData eventData);
        public void OnAppExit(PointerEventData eventData);
        public void OnAppDrop(App app);
    }
}
