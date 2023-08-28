using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DialogManagement
{
    public class Navbar : MonoBehaviour, IDragHandler
    {
        [SerializeField] private RectTransform dialog;

        public void OnDrag(PointerEventData eventData)
        {
                dialog.position += (Vector3)eventData.delta;
        }
    }
}