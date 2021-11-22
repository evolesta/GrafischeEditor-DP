using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrafischeEditor_DP.CommandPattern.Commands
{
    internal class NieuweGroepCommand : ICommand
    {
        private readonly Controller _controller;
        private int _id;

        public NieuweGroepCommand(Controller controller)
        {
            _controller = controller;
        }

        public void Execute()
        {
            _id = _controller.NieuweGroep();
        }

        public void Undo()
        {
            throw new NotImplementedException();
            //remove groep met _id;
        }
    }
}
