using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrafischeEditor_DP.DecoratorPattern
{
    public class LeftLabeledComponent : LabeledComponent
    {
        public LeftLabeledComponent(IComponent component) : base(component)
        {
        }
    }
}
