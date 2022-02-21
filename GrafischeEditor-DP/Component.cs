using GrafischeEditor_DP.VisitorPattern;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace GrafischeEditor_DP
{

    public interface IComponent {
        public string Naam { get; set; }
        public ComponentType ComponentType { get; }
        public int Id { get; set; }
        public bool Geselecteerd { get; set; }
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
        // globale variabelen voor figuur
        private string naam; // naam van het figuur
        private Rectangle positie; // x-y as en groote figuur
        private bool geselecteerd; // of figuur geselecteerd is

        // getters en setters
        public string Naam { get => naam; set => naam = value; }
        public ComponentType ComponentType => ComponentType.Figuur;
        public int Id { get; set; }
        public Rectangle Positie { get => positie; set => positie = value; }
        public bool Geselecteerd { get => geselecteerd; set => geselecteerd = value; }
        public FiguurType Type { get; set; }

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
        public string Naam { get; set; }
        public ComponentType ComponentType => ComponentType.Groep;
        public int Id { get; set; }
        
        public bool Geselecteerd { get; set; }
        public List<IComponent> Children = new();

        public IEnumerable<Figuur> Figuren => Children.OfType<Figuur>();

        public IEnumerable<Groep> Groepen => Children.OfType<Groep>();

        public void Accept(IVisitor visitor)
        {
            visitor.VisitGroup(this);
        }
    }
}
