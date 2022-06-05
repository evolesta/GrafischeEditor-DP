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

    public override bool TryGetLabel(LabelDirection direction, out LabeledComponent component)
    {

      if (direction == LabelDirection.Left)
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

    public override LabelDirection Direction => LabelDirection.Left;
  }
}
