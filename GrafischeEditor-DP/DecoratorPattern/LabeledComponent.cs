using GrafischeEditor_DP.VisitorPattern;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace GrafischeEditor_DP.DecoratorPattern
{
    public abstract class LabeledComponent : IComponent
    {

      protected Font DrawFont = new("Arial", 12);
      protected SolidBrush SolidBrush = new(Color.Black);


    protected readonly IComponent _component;

        public LabeledComponent(IComponent component)
        {
            _component = component switch
            {
                Figuur or Groep or LabeledComponent => component,
                _ => throw new InvalidOperationException()
                
            };
        }

        public string Name { get => _component.Name; set => _component.Name = value; }

        public string Text { get; set; } 

        public ComponentType ComponentType => _component.ComponentType;

        public int Id { get => _component.Id; set => _component.Id = value; }
        public bool Selected { get => _component.Selected; set => _component.Selected = value; }
        public Rectangle Placement { get => _component.Placement; set => _component.Placement = value; }

        public void Accept(IVisitor visitor) => _component.Accept(visitor);

        public virtual void Draw(PaintEventArgs e, Rectangle? preview = null) => _component.Draw(e, preview);
        public IComponent InnerComponent() => _component;
    
        public abstract bool TryGetLabel(LabelDirection direction, out LabeledComponent component);
    }
}
