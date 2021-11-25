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
        private readonly int? _parentGroupId;
        private int _id;

        // constructor
        public NieuwFiguurCommand(Controller controller, Rectangle rectangle, FiguurType soortFiguur,
            int? parentGroupId)
        {
            this._controller = controller;
            this._rectangle = rectangle;
            this._soortFiguur = soortFiguur;
            _parentGroupId = parentGroupId;
        }

        public void Execute()
        {
            _id = _controller.NieuwFiguur(_rectangle, _soortFiguur, _parentGroupId);
        }

        public void Undo()
        {
            _controller.VerwijderFiguur(_id); // verwijder object uit list
        }
    }
}