using System;
using System.Drawing;
using System.Linq;
using Newtonsoft.Json;

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
            _original = JsonConvert.SerializeObject(group, new JsonSerializerSettings{TypeNameHandling = TypeNameHandling.Auto});
            _parentGroupId = controller.FindParentGroep(groupId)?.Id;
            _groupId = groupId;
            _controller = controller;
            _newPosition = newPosition;
        }

        public void Execute()
        {
            var figures = _controller.AllFiguresFlattened(_groupId).ToArray();

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

        public void Undo()
        {
            var group = JsonConvert.DeserializeObject<Groep>(_original, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });

            if (_parentGroupId.HasValue)
            {
                var parent = _controller.GetGroep(_parentGroupId.Value);
                parent.Children.Remove(parent.Groepen.First(g => g.Id == _groupId));
                parent.Children.Add(group);
            }
            else
            {
                _controller.RemoveComponent(_groupId);
                _controller.AddGroup(group);
            }
        }
    }
}
