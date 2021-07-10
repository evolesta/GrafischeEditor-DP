using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;

namespace GrafischeEditor_DP
{
    class Controller
    {
        // Variabelen declareren
        private List<Figuur> Figuren = new List<Figuur>(); // list van Figuur objecten

        // maak nieuw figuur object aan en voeg toe aan de list
        public void nieuwFiguur(Rectangle rectangle, Figuur.soortenFiguren soortFiguur)
        {
            Figuren.Add(new Figuur("figuur" + Figuren.Count+1, rectangle, soortFiguur, false));
        }

        // Geeft de actuele lijst met figuren terug
        public List<Figuur> getFiguren() { return Figuren; }
    }
}
