using GrafischeEditor_DP.VisitorPattern;

namespace GrafischeEditor_DP.CommandPattern.Commands
{
    class OpslaanBestandCommand : ICommand
    {
        private readonly Groep _groep;
        private readonly string _bestandspad;

        public OpslaanBestandCommand(Controller controller, string bestandspad)
        {
            _groep = controller.HoofdGroep;
            _bestandspad = bestandspad;
        }

        public void Execute()
        {
            var visitor = new OpslaanVisitor(_bestandspad);
            _groep.Accept(visitor);
        }

        public void Undo() { } // geen undo voor opslaan bestand
    }
}