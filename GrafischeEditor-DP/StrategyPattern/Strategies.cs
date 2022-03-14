using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrafischeEditor_DP.StrategyPattern
{
    public static class Strategies
    {
        public static EllipseStrategy EllipseStrategy { get; } = new EllipseStrategy();
        public static RectangleStrategy RectangleStrategy { get; } = new RectangleStrategy();
    }
}
