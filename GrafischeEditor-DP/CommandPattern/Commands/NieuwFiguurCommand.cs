using System.Drawing;

namespace GrafischeEditor_DP.CommandPattern.Commands
{
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
}