using GrafischeEditor_DP.VisitorPattern;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace GrafischeEditor_DP.DecoratorPattern
{
    public abstract class LabeledComponent<T> : IComponent where T : IComponent
    {
        private readonly T _component;

        public LabeledComponent(T component)
        {
            _component = component switch
            {
                Figuur or Groep or LabeledComponent<T> => component,
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
    }
}
