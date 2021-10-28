using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;

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
        public Figuur GetFiguur(int id)
        {
            var figuur = Figuren().FirstOrDefault(f => f.Id == id);
            if (figuur is not null) 
                return figuur;

            var groepen = Groepen();
            foreach (var subgroep in groepen)
            {
                figuur = GetFiguur(id, subgroep);
                if (figuur is not null)
                    return figuur;
            }

            return null;
        }

        // Geeft een enkel figuur uit de lijst terug
        public Figuur GetFiguur(int id, Groep groep)
        {
            var figuren = groep.Children.Where(c => c.ComponentType == ComponentType.Figuur);
            var figuur = figuren.FirstOrDefault(f => f.Id == id);
            if (figuur is null)
            {
                var groepen = groep.Children.Where(g => g.ComponentType == ComponentType.Groep);
                foreach (var subgroep in groepen)
                {
                    figuur = GetFiguur(id, subgroep as Groep);
                    if (figuur is not null)
                        return figuur as Figuur;
                }
            }

            return null;
        }

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
            else
                foreach (var groep in Groepen())
                    if(VerwijderFiguurUitGroepRecursief(groep, id))
                        return;

        }

        private bool VerwijderFiguurUitGroepRecursief(Groep groep, int id)
        {
            var figuur = groep.Figuren.FirstOrDefault(g => g.Id == id);
            if (figuur is not null)
            {
                groep.Children.Remove(figuur);
                return true;
            }

            return groep.Groepen.Any(subGroep => VerwijderFiguurUitGroepRecursief(subGroep, id));
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
            var groep = new Groep {Naam = "groep " + newId, Id = newId};
            groep.Children = new List<IComponent>();
            groep.Children.AddRange(Figuren().Where(f => f.Geselecteerd));
            RemoveAllSelectedFigures();
            _componenten.Add(groep);
        }

        private void RemoveAllSelectedFigures()
        {
            _componenten = _componenten.Where(c => c.ComponentType == ComponentType.Groep || !c.Geselecteerd).ToList();
            foreach (var groep in Groepen())
            {
                RemoveSelectedFiguresFromGroupRecursive(groep);
            }
        }

        private void RemoveSelectedFiguresFromGroupRecursive(Groep groep)
        {
            groep.Children = groep.Children.Where(c => c.ComponentType == ComponentType.Groep || !c.Geselecteerd).ToList();
            foreach (var subGroep in groep.Groepen) 
                RemoveSelectedFiguresFromGroupRecursive(subGroep);
        }

        private IEnumerable<Figuur> GetAllSelectedFigures()
        {
            var figuren = new List<Figuur>();
            figuren.AddRange(Figuren().Where(f => f.Geselecteerd));
            foreach (var groep in Groepen()) 
                figuren.AddRange(GeselecteerdeFigurenInGroepRecursive(groep));

            return figuren;
        }

        private IEnumerable<Figuur> GeselecteerdeFigurenInGroepRecursive(Groep groep)
        {
            var figuren = groep.Figuren.Where(f => f.Geselecteerd).ToList();
            foreach (var subgroep in groep.Groepen) 
                figuren.AddRange(GeselecteerdeFigurenInGroepRecursive(subgroep));

            return figuren;
        }

        public IEnumerable<Figuur> Figuren()
        {
            return _componenten.Where(c => c.ComponentType == ComponentType.Figuur)
                .Select(c => c as Figuur);
        }

        public IEnumerable<Groep> Groepen()
        {
            return _componenten.Where(c => c.ComponentType == ComponentType.Groep)
                .Select(c => c as Groep);
        }
    }
}
