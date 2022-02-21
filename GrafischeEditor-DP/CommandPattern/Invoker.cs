using System.Collections.Generic;

namespace GrafischeEditor_DP.CommandPattern
{
    public class Invoker
    {
        private ICommand _command; // activeer benodigd command
        private Stack<ICommand> _Undo = new Stack<ICommand>();
        private Stack<ICommand> _Redo = new Stack<ICommand>();
        public bool HasCommand => _command is not null;

        public void SetCommand(ICommand command)
        {
            this._command = command;
        }

        // voer nieuw commando uit - nieuwe actie
        public void Execute()
        {
            _command.Execute();
            _Undo.Push(_command); // voeg nieuw commando toe aan undo stack
            _Redo.Clear(); // leeg redo stack na het toevoegen van een nieuwe actie
            _command = null;
        }

        // undo a command
        public void Undo()
        {
            if (_Undo.Count > 0)
            {
                ICommand cmd = _Undo.Pop(); // verkrijg laatst uitgevoerde command
                cmd.Undo(); // herstel uitgevoerde actie
                _Redo.Push(cmd); // voeg command toe aan redo stack
            }
        }

        // redo a command
        public void Redo()
        {
            if (_Redo.Count > 0)
            {
                ICommand cmd = _Redo.Pop();
                cmd.Execute();
                _Undo.Push(cmd);
            }
        }
    }
}
