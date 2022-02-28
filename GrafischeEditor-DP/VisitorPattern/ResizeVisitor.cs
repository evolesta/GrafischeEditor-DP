﻿using System;
using System.Drawing;
using System.Linq;

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
            var figures = element.AllFiguresFlattened().ToArray();

            var groupX = figures.Min(f => f.Placement.X);
            var groupY = figures.Min(f => f.Placement.Y);

            var oldGroupWidth = figures.Max(f => f.Placement.X) - groupX;
            var newGroupWidth = _newPosition.X - groupX;
            var ratio = (float)newGroupWidth / oldGroupWidth;

            foreach (var figure in figures)
            {
                var originalRelativeX = figure.Placement.X - groupX;
                var originalRelativeY = figure.Placement.Y - groupY;

                var newRelativeX = (int)Math.Floor(originalRelativeX * ratio);
                var newRelativeY = (int)Math.Floor(originalRelativeY * ratio);

                var newX = groupX + newRelativeX;
                var newY = groupY + newRelativeY;

                var newWidth = (int)Math.Floor(figure.Placement.Width * ratio);
                var newHeight = (int)Math.Floor(figure.Placement.Height * ratio);

                figure.Placement = new Rectangle(newX, newY, newWidth, newHeight);
            }
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
