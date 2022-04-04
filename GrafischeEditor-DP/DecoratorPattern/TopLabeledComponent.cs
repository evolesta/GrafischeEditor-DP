using System.Drawing;
using System.Windows.Forms;

namespace GrafischeEditor_DP.DecoratorPattern
{
    public class TopLabeledComponent<T> : LabeledComponent<T> where T : IComponent
    {
        public TopLabeledComponent(T component) : base(component)
        {
        }

        public override void Draw(PaintEventArgs e, Rectangle? preview = null)
        {
            base.Draw(e, preview);

            Font drawFont = new Font("Arial", 16);
            SolidBrush solidBrush = new SolidBrush(Color.Black);
            PointF labelPoint = new PointF(Placement.X, Placement.Y + 10);

            e.Graphics.DrawString("Dummy", drawFont, solidBrush, labelPoint);
        }
    }
}
