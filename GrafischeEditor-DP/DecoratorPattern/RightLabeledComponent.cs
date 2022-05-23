using System.Drawing;
using System.Windows.Forms;

namespace GrafischeEditor_DP.DecoratorPattern
{
  internal class RightLabeledComponent : LabeledComponent
  {
    public RightLabeledComponent(IComponent component) : base(component)
    {
    }

    public override void Draw(PaintEventArgs e, Rectangle? preview = null)
    {
      base.Draw(e, preview);

      var labelPoint = new PointF(Placement.X + Placement.Width + 5, Placement.Y + Placement.Height / 2);
      e.Graphics.DrawString(Text, DrawFont, SolidBrush, labelPoint);
    }

    public override bool TryGetLabel(LabelDirection direction, out LabeledComponent component)
    {

      if (direction == LabelDirection.Right)
      {
        component = this;
        return true;
      }
      else
      {
        component = null;
        return _component is LabeledComponent labeledComponent &&
               labeledComponent.TryGetLabel(direction, out component);
      }
    }
  }
}
