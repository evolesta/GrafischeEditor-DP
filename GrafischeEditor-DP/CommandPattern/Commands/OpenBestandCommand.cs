namespace GrafischeEditor_DP.CommandPattern.Commands
{
    class OpenBestandCommand : ICommand
    {
        private Controller _controller;
        private string _bestandspad;

        public OpenBestandCommand(Controller controller, string bestandspad)
        {
            this._controller = controller;
            this._bestandspad = bestandspad;
        }

        public void Execute()
        {
            _controller.OpenBestand(_bestandspad);
        }

        public void Undo() { } // geen undo voor openen bestand
    }
}