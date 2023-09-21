using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Utility
{
    public class TabNavigator : MonoBehaviour
    {
        EventSystem system;

        private bool IsShiftHeld => Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

        private void Start()
        {
            system = EventSystem.current;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
                FocusNextSelectable();
        }

        private void FocusNextSelectable()
        {
            var nextSelectable = GetNextSelectable();
            if (!nextSelectable)
                return;

            system.SetSelectedGameObject(nextSelectable.gameObject);
        }

        private Selectable GetNextSelectable()
        {
            if (!system.currentSelectedGameObject)
                return null;

            var currentSelectable = system.currentSelectedGameObject.GetComponent<Selectable>();
            if (!currentSelectable)
                return null;

            return IsShiftHeld 
                ? currentSelectable.FindSelectableOnLeft() == null ? currentSelectable.FindSelectableOnUp() : currentSelectable.FindSelectableOnLeft()
                : currentSelectable.FindSelectableOnRight() == null ? currentSelectable.FindSelectableOnDown() : currentSelectable.FindSelectableOnRight();
        }
    }
}