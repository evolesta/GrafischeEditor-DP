using System.Drawing;
using System.Windows.Forms;

namespace GrafischeEditor_DP.DecoratorPattern
{
    public class LeftLabeledComponent : LabeledComponent
    {
        public LeftLabeledComponent(IComponent component) : base(component)
        {
        }

        public override void Draw(PaintEventArgs e, Rectangle? preview = null)
        {
          base.Draw(e, preview);
      
          var labelPoint = new PointF(Placement.X - 9 * Text.Length, Placement.Y + Placement.Height / 2);
          e.Graphics.DrawString(Text, DrawFont, SolidBrush, labelPoint);
    }
    }
}
