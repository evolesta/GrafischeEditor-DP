﻿using System.Collections.Generic;
using System.Drawing;

namespace GrafischeEditor_DP
{
    class Controller
    {
        // Variabelen declareren
        private List<Figuur> Figuren = new List<Figuur>(); // list van Figuur objecten

        // Geeft de actuele lijst met figuren terug
        public List<Figuur> GetFiguren() { return Figuren; }

        // Geeft een enkel figuur uit de lijst terug
        public Figuur GetFiguur(int objIndex) { return Figuren[objIndex]; }

        // maak nieuw figuur object aan en voeg toe aan de list
        public void NieuwFiguur(Rectangle rectangle, Figuur.TekenModus soortFiguur)
        {
            int counter = Figuren.Count + 1; // genereer counter voor unieke object naam
            Figuren.Add(new Figuur("figuur" + counter, rectangle, soortFiguur, false));
        }

        // wijzig figuur object in de lijst voor nieuwe positie/grootte
        public void WijzigFiguur(Rectangle rectangle, int index)
        {
            Figuren[index].Positie = rectangle; // wijzig rectangle x-y en grootte
        }

        // verwijder figuur object uit de lijst
        public void VerwijderFiguur(int index)
        {
            Figuren.RemoveAt(index); // verwijder object uit de lijst
        }

        public void WijzigSelectie(int objIndex)
        {
            // pas boolean waarde aan in object door bitwise X-OR met true
            Figuren[objIndex].Geselecteerd ^= true;
        }
    }
}
