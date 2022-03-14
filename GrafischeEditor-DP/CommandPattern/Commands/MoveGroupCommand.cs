using GrafischeEditor_DP.VisitorPattern;

namespace GrafischeEditor_DP.CommandPattern.Commands
{
    internal class MoveGroupCommand : ICommand
    {
        private readonly int _moveX;
        private readonly int _moveY;
        private readonly Groep _groep;

        public MoveGroupCommand(Controller controller, int groupId, int moveX, int moveY)
        {
            _groep = controller.GetGroep(groupId);
            this._moveX = moveX;
            this._moveY = moveY;
        }

        public void Execute()
        {
            var visitor = new MoveVisitor(_moveX, _moveY);
            _groep.Accept(visitor);
        }

        public void Undo()
        {
            var visitor = new MoveVisitor( - _moveX, - _moveY);
            _groep.Accept(visitor);
        }
    }
}
