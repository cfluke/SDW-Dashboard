using DiscoveryWall;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace AppManagement
{
    [RequireComponent(typeof(Image))]
    public class AppDraggable : MonoBehaviour, IDragHandler, IEndDragHandler
    {
        [SerializeField] private TMP_Text appTitle;
        
        //public App App { get; private set; }

        public void InitAppDraggable()
        {
            //App = app;
        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.position += (Vector3)eventData.delta;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            
        }
    }
}