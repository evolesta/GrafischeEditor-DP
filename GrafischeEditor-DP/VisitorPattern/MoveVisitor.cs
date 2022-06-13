using System.Drawing;
using System.Linq;

namespace GrafischeEditor_DP.VisitorPattern
{
    public class MoveVisitor : IVisitor
    {
        private readonly int _moveX;
        private readonly int _moveY;

        public MoveVisitor(int moveX, int moveY)
        {
            _moveX = moveX;
            _moveY = moveY;
        }

        public void VisitFigure(Figuur element)
        {
            var newX = element.Placement.X + _moveX;
            var newY = element.Placement.Y + _moveY;
            element.Placement = new Rectangle(newX, newY, element.Placement.Width, element.Placement.Height);
        }

        public void VisitGroup(Groep element)
        {
            MoveAllFiguresInGroupRecursive(element);
        }

        private void MoveAllFiguresInGroupRecursive(Groep groep)
        {
            foreach (var figuur in groep.Figuren)
            {
                figuur.Placement = new Rectangle
                {
                    X = figuur.Placement.X + _moveX,
                    Y = figuur.Placement.Y + _moveY,
                    Height = figuur.Placement.Height,
                    Width = figuur.Placement.Width
                };
            }
            foreach (var subGroep in groep.Groepen.Select(c => c.InnerComponent() as Groep))
            {
                MoveAllFiguresInGroupRecursive(subGroep);
            }
        }
    }
}
