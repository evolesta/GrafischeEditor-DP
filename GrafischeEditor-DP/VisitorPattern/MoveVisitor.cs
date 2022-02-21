using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace GrafischeEditor_DP.VisitorPattern
{
    public class MoveVisitor : IVisitor
    {
        private Rectangle _newRectangle;

        public MoveVisitor(Rectangle newRectangle)
        {
            _newRectangle = newRectangle;
        }

        public void VisitFigure(Figuur element)
        {
            element.Positie = _newRectangle;
        }

        public void VisitGroup(Groep element)
        {
            throw new NotImplementedException();
        }
    }
}
