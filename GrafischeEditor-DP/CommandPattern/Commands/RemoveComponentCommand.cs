namespace GrafischeEditor_DP.CommandPattern.Commands
{
    internal class RemoveComponentCommand : ICommand
    {
        private readonly Controller _controller;
        private readonly int _id;
        private IComponent _component;
        private readonly Groep _parent;

        // constructor
        public RemoveComponentCommand(Controller controller, int id)
        {
            _controller = controller;
            _id = id;
            _parent = _controller.FindParentGroep(id);
        }

        public void Execute()
        {
            this._component = _controller.GetFigure(_id); // verkrijg oude object voor undo
            _controller.RemoveComponent(_id); // verwijder uit list
        }

        public void Undo()
        {
            if (_component is Figuur figuur)
                _controller.NieuwFiguur(figuur.Positie, figuur.Type, _parent?.Id); // voeg oude object opnieuw toe aan list
            else
                if(_parent is not null)
                    _parent.Children.Add(_component);
                else
                    _controller.AddGroup(_component as Groep);
        }
    }
}