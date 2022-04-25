using System.Drawing;
using System.Windows.Forms;

namespace GrafischeEditor_DP.DecoratorPattern
{
  internal class BottomLabeledComponent : LabeledComponent
  {
    public BottomLabeledComponent(IComponent component) : base(component)
    {
    }

    public override void Draw(PaintEventArgs e, Rectangle? preview = null)
    {
      base.Draw(e, preview);

      var labelPoint = new PointF(Placement.X, Placement.Y + Placement.Height + 10);
      e.Graphics.DrawString(Text, DrawFont, SolidBrush, labelPoint);
    }
  }
}
