using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrafischeEditor_DP.DecoratorPattern
{
    public class LeftLabeledComponent<T> : LabeledComponent<T> where T : IComponent
    {
        public LeftLabeledComponent(T component) : base(component)
        {
        }
    }
}
