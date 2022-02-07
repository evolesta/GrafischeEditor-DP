using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GrafischeEditor_DP.CommandPattern.Commands
{
    internal class ResizeGroupCommand : ICommand
    {
        private string _original;
        private int? _parentGroupId;
        private readonly int _groupId;
        private Controller _controller;
        private readonly Point _newPosition;

        public ResizeGroupCommand(int groupId, Controller controller, Point newPosition)
        {
            var group = controller.GetGroep(groupId);
            _original = JsonSerializer.Serialize(group);
            _parentGroupId = controller.FindParentGroep(groupId)?.Id;
            _groupId = groupId;
            _controller = controller;
            _newPosition = newPosition;
        }

        public void Execute()
        {
            var figures = _controller.AllFiguresFlattened(_groupId);

            var x = figures.Min(f => f.Positie.X);
            var y = figures.Min(f => f.Positie.Y);

            var oldWidth = figures.Max(f => f.Positie.X) - x;
            var oldHeight = figures.Max(f => f.Positie.Y) - y;

            var newWidth = _newPosition.X - x;
            var newHeight = _newPosition.Y - y;

            var oldRectangle = new Rectangle(x, y, oldWidth, oldHeight);
            var newRectangle = new Rectangle(x, y, newWidth, newHeight);

            foreach (var figure in figures)
            {
                //TODO
                throw new NotImplementedException();
            }
        }

        public void Undo()
        {
            var group = JsonSerializer.Deserialize<Groep>(_original);
        }
    }
}
