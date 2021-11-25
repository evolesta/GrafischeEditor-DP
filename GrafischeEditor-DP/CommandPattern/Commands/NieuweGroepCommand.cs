namespace GrafischeEditor_DP.CommandPattern.Commands
{
    internal class NieuweGroepCommand : ICommand
    {
        private readonly Controller _controller;
        private int _id;
        private int? _parentGroupId;

        public NieuweGroepCommand(Controller controller, int? parentGroupId)
        {
            _controller = controller;
            _parentGroupId = parentGroupId;
        }

        public void Execute()
        {
            _id = _controller.NieuweGroep(_parentGroupId);
        }

        public void Undo()
        {
            _controller.VerwijderGroep(_id);
        }
    }
}
