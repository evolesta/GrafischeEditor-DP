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
        public enum soortenFiguren { Default, Square, Ellipse } // enum voor soorten figuren
        private soortenFiguren type = soortenFiguren.Default; // bepaald soort figuur

        public Figuur(string naam, Rectangle positie, soortenFiguren type, bool selectie)
        {
            this.naam = naam;
            this.positie = positie;
            this.selectie = selectie;
            this.type = type;
        }

        // getters en setters
        public string Naam { get => naam; set => naam = value; }
        public Rectangle Positie { get => positie; set => positie = value; }
        public bool Selectie { get => selectie; set => selectie = value; }
        private soortenFiguren Type { get => type; set => type = value; }
    }
}
