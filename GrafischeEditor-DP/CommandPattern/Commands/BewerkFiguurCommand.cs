using GrafischeEditor_DP.VisitorPattern;
using System.Drawing;

namespace GrafischeEditor_DP.CommandPattern.Commands
{
    class BewerkFiguurCommand : ICommand
    {
        private Rectangle _rectangle;
        private Rectangle oudePositie;
        private Figuur _figuur;
        
        // constructor
        public BewerkFiguurCommand(Controller controller, Rectangle rectangle, int id)
        {
            this._rectangle = rectangle;
            this._figuur = controller.GetComponent(id) as Figuur;
        }

        public void Execute()
        {
            oudePositie = _figuur.Positie; // verkrijg huidige figuur voor undo

            var figuurVisitor = new MoveVisitor(_rectangle);
            _figuur.Accept(figuurVisitor);
        }

        public void Undo()
        {
            var figuurVisitor = new MoveVisitor(oudePositie);
            _figuur.Accept(figuurVisitor);
        }
    }
}