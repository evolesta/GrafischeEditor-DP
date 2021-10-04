using System.Collections.Generic;
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
        private List<Figuur> Figuren = new List<Figuur>(); // list van Component objecten
        private Bestand bestand = new Bestand();
        private IList<IComponent> _componenten = new List<IComponent>();

        // Geeft de actuele lijst met figuren terug
        public IEnumerable<IComponent> GetComponents() { return _componenten; }

        // Geeft een enkel figuur uit de lijst terug
        public Figuur GetFiguur(int objIndex) { return Figuren[objIndex]; }

        // maak nieuw figuur object aan en voeg toe aan de list
        public void NieuwFiguur(Rectangle rectangle, FiguurType soortFiguur)
        {

            var newId = GetNewId();//ids.Max() + 1;
            // voeg nieuw object toe aan de list
            Figuren.Add(new Figuur()
            {
                Id = newId, 
                Naam = "figuur " + newId, 
                Positie = rectangle, 
                Type = soortFiguur, 
                Geselecteerd = false
            });

        }

        private int GetNewId()
        {
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

        public void OpenBestand(string Bestandspad)
        {
            ResetFiguren(); // leeg lijst met figuren
            Figuren = bestand.Open(Bestandspad); // lees XML bestand en plaats figuren in list
        }

        public void OpslaanBestand(string Bestandspad)
        {
            bestand.Opslaan(Bestandspad, Figuren); // sla huidige list op naar een XML bestand
        }

        public void NieuweGroep(string Naam)
        {
            _componenten.Add(new Groep { Naam = Naam, Id = GetNewId()});
        }
    }
}
