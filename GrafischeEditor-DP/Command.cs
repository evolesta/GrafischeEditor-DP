using System.Collections.Generic;
using System.Drawing;

namespace GrafischeEditor_DP
{
    /// <summary>
    /// 'Command' interface declares a method for executing a command
    /// </summary>
    public interface ICommand
    {
        void Execute(); // declareer een methode voor het uitvoeren van een command
        void Undo(); // declareer een methode voor het terugdraaien van een command
    }

    /// <summary>
    /// 'Concrete commmand' for adding a new figure
    /// </summary>
    class NieuwFiguurCommand : ICommand
    {
        private Controller _controller;
        private Rectangle _rectangle;
        private FiguurType _soortFiguur;
        private int _id;

        // constructor
        public NieuwFiguurCommand(Controller controller, Rectangle rectangle, FiguurType soortFiguur)
        {
            this._controller = controller;
            this._rectangle = rectangle;
            this._soortFiguur = soortFiguur;
        }

        public void Execute()
        {
            _id = _controller.NieuwFiguur(_rectangle, _soortFiguur);
        }

        public void Undo()
        {
            _controller.VerwijderFiguur(_id); // verwijder object uit list
        }
    }

    class BewerkFiguurCommand : ICommand
    {
        private Controller _controller;
        private Rectangle _rectangle;
        private Rectangle oudePositie;
        private int _id;
        
        // constructor
        public BewerkFiguurCommand(Controller controller, Rectangle rectangle, int id)
        {
            this._controller = controller;
            this._rectangle = rectangle;
            this._id = id;
        }

        public void Execute()
        {
            oudePositie = _controller.GetFiguur(_id).Positie; // verkrijg huidige figuur voor undo
            _controller.WijzigFiguur(_rectangle, _id); // plaats nieuwe rectangle
        }

        public void Undo()
        {
            _controller.WijzigFiguur(oudePositie, _id); // herstel vorige rectangle
        }
    }

    class VerwijderFiguurCommand : ICommand
    {
        private Controller _controller;
        private readonly int _id;
        private Figuur _component;
        private Groep _parent;

        // constructor
        public VerwijderFiguurCommand(Controller controller, int id)
        {
            this._controller = controller;
            _id = id;
        }

        public void Execute()
        {
            this._component = _controller.GetFiguur(_id); // verkrijg oude object voor undo
            _controller.VerwijderFiguur(_id); // verwijder uit list
        }

        public void Undo()
        {
            _controller.NieuwFiguur(_component.Positie, _component.Type); // voeg oude object opnieuw toe aan list
        }
    }

    class OpenBestandCommand : ICommand
    {
        private Controller _controller;
        private string _bestandspad;

        public OpenBestandCommand(Controller controller, string bestandspad)
        {
            this._controller = controller;
            this._bestandspad = bestandspad;
        }

        public void Execute()
        {
            _controller.OpenBestand(_bestandspad);
        }

        public void Undo() { } // geen undo voor openen bestand
    }

    class OpslaanBestandCommand : ICommand
    {
        private Controller _controller;
        private string _bestandspad;

        public OpslaanBestandCommand(Controller controller, string bestandspad)
        {
            this._controller = controller;
            this._bestandspad = bestandspad;
        }

        public void Execute()
        {
            _controller.OpslaanBestand(_bestandspad);
        }

        public void Undo() { } // geen undo voor opslaan bestand
    }

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
