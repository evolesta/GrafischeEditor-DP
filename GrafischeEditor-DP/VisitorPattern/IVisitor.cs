using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrafischeEditor_DP.VisitorPattern
{
    public interface IVisitor
    {
        void VisitFigure(Figuur element);
        void VisitGroup(Groep element);
    }
}
