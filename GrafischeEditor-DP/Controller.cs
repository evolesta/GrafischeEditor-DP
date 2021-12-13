using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using GrafischeEditor_DP.Bestand;

namespace GrafischeEditor_DP
{
    /// <summary>
    /// 'Receiver' class
    /// </summary>
    public class Controller
    {
        // Variabelen declareren
        private IList<IComponent> _componenten = new List<IComponent>();

        // Geeft de actuele lijst met figuren terug
        public IEnumerable<IComponent> GetComponents() { return _componenten; }

        // Geeft een enkel figuur uit de lijst terug
        public Figuur GetFigure(int id)
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

        public Groep GetGroep(int id, Groep ancestor = null)
        {

            IEnumerable<Groep> groepen;
            if (ancestor is null)
                groepen = Groepen();
            else
                groepen = ancestor.Groepen;

            var groep = groepen.FirstOrDefault(f => f.Id == id);
            if (groep is not null)
                return groep;

            foreach (var subgroep in groepen)
            {
                groep = GetGroep(id, subgroep);
                if (groep is not null)
                    return groep;
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

        public Groep FindParentGroep(int childId, Groep ancestor = null)
        {
            IComponent child;
            IEnumerable<Groep> groepen;
            if (ancestor is null)
            {
                child = _componenten.FirstOrDefault(c => c.Id == childId);
                if (child is not null)
                    return null;

                groepen = Groepen();
            }
            else
            {
                child = ancestor.Children.FirstOrDefault(c => c.Id == childId);
                if (child is not null)
                    return ancestor;

                groepen = ancestor.Groepen;
            }
            

            foreach (var subgroep in groepen)
            {
                var parent = FindParentGroep(childId, subgroep);
                if (parent is not null)
                    return parent;
            }

            return null;
        }

        // maak nieuw figuur object aan en voeg toe aan de list
        public int NieuwFiguur(Rectangle rectangle, FiguurType soortFiguur, int? parentGroupId)
        {
            var newId = GetNewId();

            // voeg nieuw object toe aan de list
            var figuur = new Figuur()
            {
                Id = newId, 
                Naam = "figuur " + newId, 
                Positie = rectangle, 
                Type = soortFiguur, 
                Geselecteerd = false
            };

            if (parentGroupId is null)
            {
                _componenten.Add(figuur);
            }
            else
            {
                var parent = GetGroep(parentGroupId.Value);
                parent.Children.Add(figuur);
            }

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

        public void RemoveComponent(int id)
        {
            var component = _componenten.FirstOrDefault(f => f.Id == id);
            if(component is not null)
                _componenten.Remove(component);
            else
                foreach (var groep in Groepen())
                    // If we find the component, no need to proceed to remaining groups, so return:
                    if (RemoveComponentFromGroupRecursive(groep, id))
                        return;
        }

        private static bool RemoveComponentFromGroupRecursive(Groep groep, int id)
        {
            var component = groep.Children.FirstOrDefault(c => c.Id == id);
            if (component is not null)
            {
                groep.Children.Remove(component);
                return true;
            }

            return groep.Groepen.Any(subGroup => RemoveComponentFromGroupRecursive(subGroup, id));
        }

        // wijzig de selectie van een figuur
        public void WijzigSelectie(int id)
        {
            var component = GetFigure(id) as IComponent;
            component ??= GetGroep(id);
            if(component is not null)
                // pas boolean waarde aan in object door bitwise X-OR met true
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
            _componenten = BestandIo.Open(Bestandspad); // lees XML bestand en plaats figuren in list
        }

        public void OpslaanBestand(string Bestandspad)
        {
            BestandIo.Opslaan(Bestandspad, _componenten); // sla huidige list op naar een XML bestand
        }

        public int NieuweGroep(int? parentGroupId = null)
        {
            var newId = GetNewId();
            var groep = new Groep {Naam = "groep " + newId, Id = newId};
            groep.Children = new List<IComponent>();
            groep.Children.AddRange(Figuren().Where(f => f.Geselecteerd));
            RemoveAllSelectedFigures();

            if (parentGroupId is null)
            {
                _componenten.Add(groep);
            }
            else
            {
                var parent = GetGroep(parentGroupId.Value);
                parent.Children.Add(groep);
            }

            return newId;
        }

        public void AddGroup(Groep group)
        {
            _componenten.Add(group);
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

        public IEnumerable<Figuur> AllFiguresFlattened()
        {
            var figures = Figuren();
            foreach (var groep in Groepen())
            {
                AddFiguresRecursive(figures, groep);
            }

            return figures;
        }

        private static void AddFiguresRecursive(IEnumerable<Figuur> figures, Groep groep)
        {
            figures.Concat(groep.Children.Where(c => c.ComponentType == ComponentType.Figuur).Select(c => c as Figuur));
            foreach (var component in groep.Children.Where(c => c.ComponentType == ComponentType.Groep))
            {
                AddFiguresRecursive(figures, component as Groep);
            }
        }

        public IEnumerable<Groep> Groepen()
        {
            return _componenten.Where(c => c.ComponentType == ComponentType.Groep)
                .Select(c => c as Groep);
        }
    }
}
