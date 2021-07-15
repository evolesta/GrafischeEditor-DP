using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;

namespace GrafischeEditor_DP
{
    class Figuur
    {
        // globale variabelen voor figuur
        private string naam; // naam van het figuur
        private Rectangle positie; // x-y as en groote figuur
        private bool selectie; // of figuur geselecteerd is
        public enum TekenModus { Default, Resize, Square, Ellipse } // enum voor soorten figuren
        private TekenModus modus = TekenModus.Default; // bepaald soort figuur

        public Figuur(string naam, Rectangle positie, TekenModus modus, bool selectie)
        {
            this.naam = naam;
            this.positie = positie;
            this.selectie = selectie;
            this.modus = modus;
        }

        // getters en setters
        public string Naam { get => naam; set => naam = value; }
        public Rectangle Positie { get => positie; set => positie = value; }
        public bool Selectie { get => selectie; set => selectie = value; }
        public TekenModus Type { get => modus; set => modus = value; }
    }
}
