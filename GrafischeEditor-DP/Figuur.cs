using System.Drawing;

namespace GrafischeEditor_DP
{
    /// <summary>
    /// De Figuur klasse representeert een enkel figuur die getekend kan worden
    /// </summary>
    public class Figuur
    {
        // globale variabelen voor figuur
        private string naam; // naam van het figuur
        private Rectangle positie; // x-y as en groote figuur
        private bool geselecteerd; // of figuur geselecteerd is
        private TekenModus modus = TekenModus.Select; // bepaalt huidige modus

        // getters en setters
        public string Naam { get => naam; set => naam = value; }
        public Rectangle Positie { get => positie; set => positie = value; }
        public bool Geselecteerd { get => geselecteerd; set => geselecteerd = value; }
        public TekenModus Type { get => modus; set => modus = value; }

        public enum TekenModus
        {
            Select, 
            Resize, 
            Square, 
            Ellipse,
            Verwijder
        } // enum voor soorten figuren
    }
}
