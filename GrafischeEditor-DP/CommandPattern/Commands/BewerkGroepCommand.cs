using System.Drawing;

namespace GrafischeEditor_DP.CommandPattern.Commands
{
    internal class BewerkGroepCommand : ICommand
    {
        private readonly Controller _controller;
        private int _groupId;
        private int _moveX;
        private int _moveY;

        public BewerkGroepCommand(Controller controller, int groupId, int moveX, int moveY)
        {
            _controller = controller;
            this._groupId = groupId;
            this._moveX = moveX;
            this._moveY = moveY;
        }

        public void Execute()
        {
            var groep = _controller.GetGroep(_groupId);
            MoveAllFiguresInGroupRecursive(groep);

        }

        private void MoveAllFiguresInGroupRecursive(Groep groep)
        {
            foreach (var figuur in groep.Figuren)
            {
                figuur.Positie = new Rectangle
                {
                    X = figuur.Positie.X + _moveX,
                    Y = figuur.Positie.Y + _moveY,
                    Height = figuur.Positie.Height,
                    Width = figuur.Positie.Width
                };
            }
            foreach (var subGroep in groep.Groepen)
            {
                MoveAllFiguresInGroupRecursive(subGroep);

            }
        }

        public void Undo()
        {
            var groep = _controller.GetGroep(_groupId);
            MoveAllFiguresInGroupBackRecursive(groep);
        }

        private void MoveAllFiguresInGroupBackRecursive(Groep groep)
        {
            foreach (var figuur in groep.Figuren)
            {
                figuur.Positie = new Rectangle
                {
                    X = figuur.Positie.X - _moveX,
                    Y = figuur.Positie.Y - _moveY,
                    Height = figuur.Positie.Height,
                    Width = figuur.Positie.Width
                };
            }
            foreach (var subGroep in groep.Groepen)
            {
                MoveAllFiguresInGroupRecursive(subGroep);

            }
        }
    }
}
