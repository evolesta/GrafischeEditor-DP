﻿using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace GrafischeEditor_DP
{
    /// <summary>
    /// 'Receiver' class
    /// </summary>
    public class Controller
    {
        // Variabelen declareren
        private Bestand bestand = new Bestand();
        private IList<IComponent> _componenten = new List<IComponent>();

        // Geeft de actuele lijst met figuren terug
        public IEnumerable<IComponent> GetComponents() { return _componenten; }

        // Geeft een enkel figuur uit de lijst terug
        public Figuur GetFiguur(int id) { return Figuren().FirstOrDefault(f => f.Id == id); }

        // maak nieuw figuur object aan en voeg toe aan de list
        public int NieuwFiguur(Rectangle rectangle, FiguurType soortFiguur)
        {

            var newId = GetNewId();//ids.Max() + 1;
            // voeg nieuw object toe aan de list
            _componenten.Add(new Figuur()
            {
                Id = newId, 
                Naam = "figuur " + newId, 
                Positie = rectangle, 
                Type = soortFiguur, 
                Geselecteerd = false
            });

            return newId;
        }

        private int GetNewId()
        {
            if (_componenten.Count == 0)
                return 1;

            //get all used Ids:
            var ids = new List<int>();
            foreach (var component in _componenten)
                IetsMetIds(ids, component);

            return ids.Max() + 1;
        }

        private void IetsMetIds(List<int> ids, IComponent component)
        {
            ids.Add(component.Id);
            if (component is Groep childGroup)
                ids.AddRange(GetIdsFromGroupRecursive(childGroup));
        }

        private IEnumerable<int> GetIdsFromGroupRecursive(Groep groep)
        {
            var ids = new List<int>();

            foreach (var child in groep.Children)
                IetsMetIds(ids, child);

            return ids;
        }

        // wijzig figuur object in de lijst voor nieuwe positie/grootte
        public void WijzigFiguur(Rectangle rectangle, int id)
        {
            var figuur = Figuren().FirstOrDefault(f => f.Id == id);
            if(figuur is not null)
                figuur.Positie = rectangle; // wijzig rectangle x-y en grootte
        }

        // verwijder figuur object uit de lijst
        public void VerwijderFiguur(int id)
        {
            var figuur = Figuren().FirstOrDefault(f => f.Id == id);
            if(figuur is not null)
                _componenten.Remove(figuur); // verwijder object uit de lijst
        }

        // wijzig de selectie van een figuur
        public void WijzigSelectie(int id)
        {
            // pas boolean waarde aan in object door bitwise X-OR met true
            var component = _componenten.FirstOrDefault(c => c.Id == id);
            if(component is not null)
                component.Geselecteerd ^= true;
        }

        // verwijder alle figuren uit de lijst
        public void ResetComponents()
        {
            _componenten.Clear();
        }

        public void OpenBestand(string Bestandspad)
        {
            ResetComponents(); // leeg lijst met figuren
            _componenten = bestand.Open(Bestandspad); // lees XML bestand en plaats figuren in list
        }

        public void OpslaanBestand(string Bestandspad)
        {
            bestand.Opslaan(Bestandspad, _componenten); // sla huidige list op naar een XML bestand
        }

        public void NieuweGroep()
        {
            var newId = GetNewId();
            _componenten.Add(new Groep { Naam = "groep " + newId, Id = newId});
        }

        private IEnumerable<Figuur> Figuren()
        {
            return _componenten.Where(c => c.ComponentType == ComponentType.Figuur)
                .Select(c => c as Figuur);
        }
    }
}
