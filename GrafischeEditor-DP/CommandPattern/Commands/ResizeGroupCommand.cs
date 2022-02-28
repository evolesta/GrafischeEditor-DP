using System;
using System.Drawing;
using System.Linq;
using GrafischeEditor_DP.VisitorPattern;
using Newtonsoft.Json;

namespace GrafischeEditor_DP.CommandPattern.Commands
{
    internal class ResizeGroupCommand : ICommand
    {
        private readonly string _original;
        private readonly int? _parentGroupId;
        private readonly int _groupId;
        private readonly Controller _controller;
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
            var visitor = new ResizeVisitor(_newPosition);
            var group = _controller.GetGroep(_groupId);
            group.Accept(visitor);
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
