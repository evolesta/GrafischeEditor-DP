using System.Collections.Generic;
using System.Drawing;

namespace GrafischeEditor_DP
{

    public interface IFiguur { }

    /// <summary>
    /// De Figuur klasse representeert een enkel figuur die getekend kan worden
    /// </summary>
    public class Figuur : IFiguur
    {
        // globale variabelen voor figuur
        private string naam; // naam van het figuur
        private Rectangle positie; // x-y as en groote figuur
        private bool geselecteerd; // of figuur geselecteerd is

        // getters en setters
        public string Naam { get => naam; set => naam = value; }
        public Rectangle Positie { get => positie; set => positie = value; }
        public bool Geselecteerd { get => geselecteerd; set => geselecteerd = value; }
        public FiguurType Type { get; set; }
    }

    public enum FiguurType
    {
        Rectangle,
        Ellipse
    }

    public class CompositeFiguur : IFiguur
    {
        public IEnumerable<Figuur> Children { get; set; }
    }
}
