using System.Drawing;
using System.Windows.Forms;

namespace GrafischeEditor_DP.DecoratorPattern
{
    public class TopLabeledComponent : LabeledComponent
    {
        public TopLabeledComponent(IComponent component) : base(component)
        {
        }

        public override void Draw(PaintEventArgs e, Rectangle? preview = null)
        {
            base.Draw(e, preview);

            PointF labelPoint = new PointF(Placement.X, Placement.Y - 25);

            e.Graphics.DrawString("Dummy", DrawFont, SolidBrush, labelPoint);
        }
    }
}
