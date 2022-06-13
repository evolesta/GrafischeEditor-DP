using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace GrafischeEditor_DP.StrategyPattern
{
    public class EllipseStrategy : IFigureStrategy
    {
        public void Draw(PaintEventArgs e, bool selected, Rectangle placement)
        {
            Pen pen = GeneratePen(selected); // genereer nieuwe pen
            e.Graphics.DrawEllipse(pen, placement);
        }

        // Methode voor het genereren van een pen om te tekenen
        private Pen GeneratePen(bool selectie)
        {
            Pen pen = new Pen(Color.Black, 2); // maak nieuwe pen object aan

            // check op boolean waarde voor selectie
            if (selectie)
            {
                pen.DashStyle = DashStyle.Dot;
            }

            return pen;
        }
    }
}
