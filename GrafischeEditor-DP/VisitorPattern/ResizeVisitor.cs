using System;
using System.Drawing;

namespace GrafischeEditor_DP.VisitorPattern
{
    internal class ResizeVisitor : IVisitor
    {
        private readonly Point _newPosition;

        public ResizeVisitor(Point newPosition)
        {
            _newPosition = newPosition;
        }

        public void VisitFigure(Figuur element)
        {
            element.Placement = ResizeRectangle(element.Placement);
        }

        public void VisitGroup(Groep element)
        {
            throw new NotImplementedException();
        }

        private Rectangle ResizeRectangle(Rectangle currRectangle)
        {
            var rectangle = new Rectangle();

            if (_newPosition.X < currRectangle.X)
            {
                rectangle.X = _newPosition.X;
            }
            else
            {
                rectangle.X = currRectangle.X;
            }

            if (_newPosition.Y < currRectangle.Y)
            {
                rectangle.Y = _newPosition.Y;
            }
            else
            {
                rectangle.Y = currRectangle.Y;
            }

            rectangle.Width = Math.Abs(_newPosition.X - currRectangle.X);
            rectangle.Height = Math.Abs(_newPosition.Y - currRectangle.Y);
            return rectangle;
        }
    }
}
