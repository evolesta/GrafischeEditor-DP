using System;
using System.Drawing;
using GrafischeEditor_DP.VisitorPattern;

namespace GrafischeEditor_DP.CommandPattern.Commands
{
    internal class ResizeFigureCommand : ICommand
    {
        private readonly Figuur _figure;
        private readonly Point _newPosition;
        private readonly Point _oldPosition;

        public ResizeFigureCommand(Figuur figure, Point newPosition)
        {
            _figure = figure;
            _newPosition = newPosition;
        }

        public void Execute()
        {
            var visitor = new ResizeVisitor(_newPosition);
            _figure.Accept(visitor);
        }

        public void Undo()
        {
            throw new NotImplementedException();
        }
    }
}
