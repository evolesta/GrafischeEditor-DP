using GrafischeEditor_DP.VisitorPattern;

namespace GrafischeEditor_DP.CommandPattern.Commands    
{
    class OpenBestandCommand : ICommand
    {
        private readonly Groep _groep;
        private readonly Controller _controller;
        private readonly string _bestandspad;

        public OpenBestandCommand(Controller controller, string bestandspad)
        {
            _groep = controller.HoofdGroep;
            _controller = controller;
            _bestandspad = bestandspad;
        }

        public void Execute()
        {
            var visitor = new OpenVisitor(_bestandspad, _controller);
            _groep.Accept(visitor);
        }

        public void Undo() { } // geen undo voor openen bestand
    }
}