using System.Drawing;

namespace GrafischeEditor_DP
{
    class Figuur
    {
        // globale variabelen voor figuur
        private string naam; // naam van het figuur
        private Rectangle positie; // x-y as en groote figuur
        private bool geselecteerd; // of figuur geselecteerd is
        private TekenModus modus = TekenModus.Select; // bepaalt huidige modus

        public Figuur(string naam, Rectangle positie, TekenModus modus, bool selectie)
        {
            this.naam = naam;
            this.positie = positie;
            this.geselecteerd = selectie;
            this.modus = modus;
        }

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
