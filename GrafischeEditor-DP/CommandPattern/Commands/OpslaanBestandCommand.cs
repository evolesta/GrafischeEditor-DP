namespace GrafischeEditor_DP.CommandPattern.Commands
{
    class OpslaanBestandCommand : ICommand
    {
        private Controller _controller;
        private string _bestandspad;

        public OpslaanBestandCommand(Controller controller, string bestandspad)
        {
            this._controller = controller;
            this._bestandspad = bestandspad;
        }

        public void Execute()
        {
            _controller.OpslaanBestand(_bestandspad);
        }

        public void Undo() { } // geen undo voor opslaan bestand
    }
}