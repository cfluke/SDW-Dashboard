using UnityEngine;

namespace CommandPattern.Commands
{
    public class AddWidgetCommand : ICommand
    {
        public void Execute()
        {
            Debug.Log("Add Widget");
        }

        public void Undo()
        {
            Debug.Log("Undo Add Widget");
        }
    }
}
