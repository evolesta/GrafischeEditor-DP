using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            Font drawFont = new Font("Arial", 16);
            SolidBrush solidBrush = new SolidBrush(Color.Black);
            PointF labelPoint = new PointF()

            e.Graphics.DrawString(Text, drawFont, solidBrush, null);
        }
    }
}
