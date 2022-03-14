using GrafischeEditor_DP.VisitorPattern;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using GrafischeEditor_DP.StrategyPattern;

namespace GrafischeEditor_DP
{

    public interface IComponent {
        public string Name { get; set; }
        public ComponentType ComponentType { get; }
        public int Id { get; set; }
        public bool Selected { get; set; }
        void Accept(IVisitor visitor);
    }

    public enum ComponentType
    {
        Groep,
        Figuur
    }

    /// <summary>
    /// De Component klasse representeert een enkel figuur die getekend kan worden
    /// </summary>
    public class Figuur : IComponent
    {
        public string Name { get; set; }
        public ComponentType ComponentType => ComponentType.Figuur;
        public int Id { get; set; }
        public Rectangle Placement { get; set; }
        public bool Selected { get; set; }
        public IFigureStrategy Strategy { get; set; }


        public void Draw(PaintEventArgs e, Rectangle? preview = null) => Strategy.Draw(e, Selected, preview ?? Placement);

        public void Accept(IVisitor visitor)
        {
            visitor.VisitFigure(this);
        }
    }

    public enum FiguurType
    {
        Rectangle,
        Ellipse
    }
    public class Groep : IComponent
    {
        public string Name { get; set; }
        public ComponentType ComponentType => ComponentType.Groep;
        public int Id { get; set; }
        
        public bool Selected { get; set; }
        public List<IComponent> Children = new();

        public IEnumerable<Figuur> Figuren => Children.OfType<Figuur>();

        public IEnumerable<Groep> Groepen => Children.OfType<Groep>();

        public void Accept(IVisitor visitor)
        {
            visitor.VisitGroup(this);
        }


        public IEnumerable<Figuur> AllFiguresFlattened() =>
            Figuren.Concat(Groepen.SelectMany(g => g.AllFiguresFlattened()));
    }
}
