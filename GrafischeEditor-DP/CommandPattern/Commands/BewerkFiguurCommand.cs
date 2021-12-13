using System.Drawing;

namespace GrafischeEditor_DP.CommandPattern.Commands
{
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
            oudePositie = _controller.GetFigure(_id).Positie; // verkrijg huidige figuur voor undo
            _controller.WijzigFiguur(_rectangle, _id); // plaats nieuwe rectangle
        }

        public void Undo()
        {
            _controller.WijzigFiguur(oudePositie, _id); // herstel vorige rectangle
        }
    }
}