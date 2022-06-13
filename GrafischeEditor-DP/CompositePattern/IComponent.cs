using GrafischeEditor_DP.VisitorPattern;
using System.Drawing;
using System.Windows.Forms;

namespace GrafischeEditor_DP
{

    public interface IComponent {
        public string Name { get; set; }
        public ComponentType ComponentType { get; }
        public Rectangle Placement { get; set; }
        public int Id { get; set; }
        public bool Selected { get; set; }
        void Accept(IVisitor visitor);
        void Draw(PaintEventArgs e, Rectangle? preview = null);

        public IComponent InnerComponent();
    }

    public enum ComponentType
    {
        Groep,
        Figuur
    }

}
