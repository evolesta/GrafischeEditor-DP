using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms.Design;

namespace GrafischeEditor_DP
{

    public interface IComponent {
        public string Naam { get; set; }
        public ComponentType ComponentType { get; }
        public int Id { get; set; }
        public bool Geselecteerd { get; set; }
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

        public void NieuwComponent(IComponent groep)
        {
            Children.Add(groep);
        }

        public void VerwijderComponent(int index)
        {
            Children.RemoveAt(index);
        }
    }
}
