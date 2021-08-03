﻿using System.Collections.Generic;
using System.Drawing;

namespace GrafischeEditor_DP
{
    class Controller
    {
        // Variabelen declareren
        private List<Figuur> Figuren = new List<Figuur>(); // list van Figuur objecten
        private Bestand bestand = new Bestand(); // nieuw object voor bestand klasse

        // Geeft de actuele lijst met figuren terug
        public List<Figuur> GetFiguren() { return Figuren; }

        // Geeft een enkel figuur uit de lijst terug
        public Figuur GetFiguur(int objIndex) { return Figuren[objIndex]; }

        // maak nieuw figuur object aan en voeg toe aan de list
        public void NieuwFiguur(Rectangle rectangle, Figuur.TekenModus soortFiguur)
        {
            int counter = Figuren.Count + 1; // genereer counter voor unieke object naam

            // voeg nieuw object toe aan de list
            Figuren.Add(new Figuur() { Naam = "figuur" + counter, Positie = rectangle, Type = soortFiguur, Geselecteerd = false });
            
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

        // wijzig de selectie van een figuur
        public void WijzigSelectie(int objIndex)
        {
            // pas boolean waarde aan in object door bitwise X-OR met true
            Figuren[objIndex].Geselecteerd ^= true;
        }

        // verwijder alle figuren uit de lijst
        public void ResetFiguren()
        {
            Figuren.Clear();
        }

        // open bestand
        public void OpenBestand(string Bestandspad)
        {
            ResetFiguren(); // leeg huidige lijst
            Figuren = bestand.Open(Bestandspad); // lees XML bestand en plaats figuren in lijst
        }

        // sla bestand op
        public void OpslaanBestand(string Bestandspad)
        {
            bestand.Opslaan(Bestandspad, Figuren); // sla huidige lijst op naar bestand
        }
    }
}
