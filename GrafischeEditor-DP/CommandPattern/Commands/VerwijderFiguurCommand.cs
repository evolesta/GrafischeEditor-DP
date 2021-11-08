namespace GrafischeEditor_DP.CommandPattern.Commands
{
    class VerwijderFiguurCommand : ICommand
    {
        private Controller _controller;
        private readonly int _id;
        private Figuur _component;
        private Groep _parent;

        // constructor
        public VerwijderFiguurCommand(Controller controller, int id)
        {
            this._controller = controller;
            _id = id;
        }

        public void Execute()
        {
            this._component = _controller.GetFiguur(_id); // verkrijg oude object voor undo
            _controller.VerwijderFiguur(_id); // verwijder uit list
        }

        public void Undo()
        {
            _controller.NieuwFiguur(_component.Positie, _component.Type); // voeg oude object opnieuw toe aan list
        }
    }
}