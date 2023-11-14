using UnityEngine;

namespace CommandPattern
{
    public class UndoRedo : MonoBehaviour
    {
        void Update()
        {
            if (Input.GetKey(KeyCode.LeftControl))
            {
                // undo: CTRL + Z
                if (Input.GetKeyDown(KeyCode.Z))
                    CommandInvoker.Undo();
        
                // redo: CTRL + Y
                if (Input.GetKeyDown(KeyCode.Y))
                    CommandInvoker.Redo();
            }
        }
    }
}
