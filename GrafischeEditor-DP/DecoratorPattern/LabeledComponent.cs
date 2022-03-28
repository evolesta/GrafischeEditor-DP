using GrafischeEditor_DP.VisitorPattern;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GrafischeEditor_DP.DecoratorPattern
{
    public abstract class LabeledComponent : IComponent
    {
        private readonly IComponent component;

        public LabeledComponent(IComponent component)
        {
            this.component = component;
        }

        public string Name { get => component.Name; set => component.Name = value; }

        public string Text { get; set; }

        public ComponentType ComponentType => component.ComponentType;

        public int Id { get => component.Id; set => component.Id = value; }
        public bool Selected { get => component.Selected; set => component.Selected = value; }
        public Rectangle Placement { get => component.Placement; set => component.Placement = value; }

        public void Accept(IVisitor visitor) => component.Accept(visitor);

        public virtual void Draw(PaintEventArgs e, Rectangle? preview = null) => component.Draw(e, preview);
    }
}
