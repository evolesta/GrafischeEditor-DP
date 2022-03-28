using System.Drawing;
using System.Windows.Forms;

namespace GrafischeEditor_DP.StrategyPattern
{
    public interface IFigureStrategy
    {
        void Draw(PaintEventArgs paintEventArgs, bool selected, Rectangle placement);
    }
}
