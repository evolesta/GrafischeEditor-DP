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
        public IComponent GetComponent(int id)
        {
            var component = _componenten.FirstOrDefault(f => f.Id == id);
            if (component is not null) 
                return component;

            var groepen = Groepen();
            foreach (var groep in groepen)
            {
                component = GetComponent(id, groep);
                if (component is not null)
                    return component;
            }

            return null;
        }

        // Geeft een enkel figuur uit de lijst terug
        public IComponent GetComponent(int id, Groep groep)
        {
            var component = groep.Children.FirstOrDefault(f => f.Id == id);
            if (component is not null)
                return component;
            else
            {
                var groepen = groep.Children.Where(g => g.ComponentType == ComponentType.Groep);
                foreach (var subgroep in groepen)
                {
                    component = GetComponent(id, subgroep as Groep);
                    if (component is not null)
                        return component;
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

        public int? SelectedGroupId()
        {
            foreach (var groep in Groepen())
            {
                if (groep.Geselecteerd)
                    return groep.Id;
            }

            int? id = null;

            foreach (var groep in Groepen())
            {
                 id = SelectedSubgroupIdRecursive(groep);
            }

            return id;
        }

        private int? SelectedSubgroupIdRecursive(Groep groep)
        {
            foreach (var subGroep in groep.Groepen)
            {
                if (subGroep.Geselecteerd)
                    return subGroep.Id;
            }

            int? id = null;

            foreach (var subGroep in groep.Groepen)
            {
                id = SelectedSubgroupIdRecursive(subGroep);
            }

            return id;
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

        public Groep GetGroep(int groupId)
        {
            var component = GetComponent(groupId);
            return component as Groep;
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
            var figuur = GetFigure(id);
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

        public void SelectFigure(int id)
        {
            if (GetFigure(id) is { } figure)
            {
                ClearSelection();
                figure.Geselecteerd = true;
            }
        }

        public Figuur GetFigure(int id)
        {
            var component = GetComponent(id);
            return component as Figuur;
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
        
        public IEnumerable<Figuur> Figuren()
        {
            return _componenten.Where(c => c.ComponentType == ComponentType.Figuur)
                .Select(c => c as Figuur);
        }

        public IEnumerable<Figuur> AllFiguresFlattened(int? groupId = null)
        {
            IEnumerable<Figuur> figures = new List<Figuur>();

            if (groupId is null)
            {
                figures = Figuren();
                foreach (var groep in Groepen())
                {
                    figures = AddFiguresRecursive(figures, groep);
                }
            }
            else
            {
                var group = GetGroep(groupId.Value);
                figures = AddFiguresRecursive(figures, group);
            }


            return figures;
        }

        private static IEnumerable<Figuur> AddFiguresRecursive(IEnumerable<Figuur> figures, Groep groep)
        {
            var childFigures = groep.Children
                .Where(c => c.ComponentType == ComponentType.Figuur)
                .Select(c => c as Figuur);

            figures = figures.Union(childFigures);

            return groep.Children
                .Where(c => c.ComponentType == ComponentType.Groep)
                .Aggregate(figures, (current, component) => 
                    AddFiguresRecursive(current, component as Groep));
        }

        public IEnumerable<Groep> Groepen()
        {
            return _componenten.Where(c => c.ComponentType == ComponentType.Groep)
                .Select(c => c as Groep);
        }

        public void ClearSelection()
        {
            foreach (var component in _componenten)
            {
                component.Geselecteerd = false;
                if (component is Groep groep)
                    ClearSelectionInGroup(groep);
            }
        }

        private void ClearSelectionInGroup(Groep groep)
        {
            foreach (var component in groep.Children)
            {
                component.Geselecteerd = false;
                if (component is Groep subGroep)
                    ClearSelectionInGroup(subGroep);
            }
        }

        public void SelectGroupRecursive(Groep groep)
        {
            foreach (var component in groep.Children)
            {
                component.Geselecteerd = true;
                if (component is Groep subGroep)
                    SelectGroupRecursive(subGroep);
            }

        }
    }
}
