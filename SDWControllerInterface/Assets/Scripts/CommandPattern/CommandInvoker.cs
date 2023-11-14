using System.Collections.Generic;

namespace CommandPattern
{
    public static class CommandInvoker
    {
        private static List<ICommand> _commands = new();
        private static int _currentIndex = -1;

        public static void ExecuteCommand(ICommand command)
        {
            // Clear any commands that were undone after the current index
            _commands.RemoveRange(_currentIndex + 1, _commands.Count - _currentIndex - 1);

            // Execute the new command
            command.Execute();
            _commands.Add(command);
            _currentIndex = _commands.Count - 1;
        }

        public static void Undo()
        {
            if (_currentIndex >= 0)
            {
                _commands[_currentIndex].Undo();
                _currentIndex--;
            }
        }

        public static void Redo()
        {
            if (_currentIndex < _commands.Count - 1)
            {
                _currentIndex++;
                _commands[_currentIndex].Execute();
            }
        }
    }
}
