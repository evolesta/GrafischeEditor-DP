using GrafischeEditor_DP.VisitorPattern;

namespace GrafischeEditor_DP.CommandPattern.Commands
{
    internal class MoveFigureCommand : ICommand
    {
        private readonly int _moveX;
        private readonly int _moveY;
        private readonly Figuur _figure;
        
        // constructor
        public MoveFigureCommand(Controller controller, int moveX, int moveY, int id)
        {
            _moveX = moveX;
            _moveY = moveY;
            _figure = controller.GetComponent(id).InnerComponent() as Figuur;
        }

        public void Execute()
        {
            var visitor = new MoveVisitor(_moveX, _moveY);
            _figure.Accept(visitor);
        }

        public void Undo()
        {
            var visitor = new MoveVisitor( - _moveX, - _moveY);
            _figure.Accept(visitor);
        }
    }
}