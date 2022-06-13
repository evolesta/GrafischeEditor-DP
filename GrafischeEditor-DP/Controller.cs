using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using GrafischeEditor_DP.DecoratorPattern;

namespace GrafischeEditor_DP
{
    /// <summary>
    /// 'Receiver' class
    /// </summary>
    public class Controller
    {
        // Variabelen declareren
        private Groep _hoofdGroep = new Groep();

        public Groep HoofdGroep { get => _hoofdGroep; set => _hoofdGroep = value; }

        // Geeft de actuele lijst met figuren terug
        public IEnumerable<IComponent> GetComponents() { return _hoofdGroep.Children; }

        // Geeft een enkel figuur uit de lijst terug
        public IComponent GetComponent(int id)
        {
            var component = _hoofdGroep.Children.FirstOrDefault(f => f.Id == id);
            if (component is not null) 
                return component;

            var groepen = Groepen();
            foreach (var groep in groepen)
            {
                component = GetComponent(id, groep.InnerComponent() as Groep);
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
                    component = GetComponent(id, subgroep.InnerComponent() as Groep);
                    if (component is not null)
                        return component;
                }
            }

            return null;
        }

        public Groep FindParentGroep(int childId, Groep ancestor = null)
        {
            IComponent topLevelComponent;
            IEnumerable<Groep> groepen;
            if (ancestor is null)
            {
                topLevelComponent = _hoofdGroep.Children.FirstOrDefault(c => c.Id == childId);
                if (topLevelComponent is not null)
                    return HoofdGroep;

                groepen = Groepen().Select(g => g.InnerComponent() as Groep);
            }
            else
            {
                topLevelComponent = ancestor.Children.FirstOrDefault(c => c.Id == childId);
                if (topLevelComponent is not null)
                    return ancestor;

                groepen = ancestor.Groepen.Select(c => c.InnerComponent() as Groep);
            }
            

            foreach (var subgroep in groepen)
            {
                var parent = FindParentGroep(childId, subgroep);
                if (parent is not null)
                    return parent;
            }

            return null;
        }

        public IEnumerable<IComponent> GetAllFiguresFlattened() => _hoofdGroep.AllFiguresFlattened();


        public int? SelectedGroupId()
        {
            var groups = Groepen().Select(g => g.InnerComponent() as Groep);

            foreach (var groep in groups)
            {
                if (groep.Selected)
                    return groep.Id;
            }

            int? id = null;

            foreach (var groep in groups)
            {
                 id = SelectedSubgroupIdRecursive(groep);
            }

            return id;
        }

        private int? SelectedSubgroupIdRecursive(Groep groep)
        {
            foreach (var subGroep in groep.Groepen)
            {
                if (subGroep.Selected)
                    return subGroep.Id;
            }

            int? id = null;

            foreach (var subGroep in groep.Groepen)
            {
                id = SelectedSubgroupIdRecursive(subGroep.InnerComponent() as Groep);
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
                Name = "figuur " + newId, 
                Placement = rectangle, 
                FiguurType = soortFiguur,
                Selected = false
            };

            if (parentGroupId is null)
            {
                _hoofdGroep.Children.Add(figuur);
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
            return component.InnerComponent() as Groep;
        }

        private int GetNewId()
        {
            if (_hoofdGroep.Children.Count == 0)
                return 1;

            //get all used Ids:
            var ids = new List<int>();
            foreach (var component in _hoofdGroep.Children)
                AddIdsFromChildren(ids, component);

            return ids.Max() + 1;
        }

        private void AddIdsFromChildren(List<int> ids, IComponent component)
        {
            ids.Add(component.Id);
            if (component is Groep childGroup)
                ids.AddRange(GetIdsFromGroupRecursive(childGroup));
        }

        private IEnumerable<int> GetIdsFromGroupRecursive(Groep groep)
        {
            var ids = new List<int>();

            foreach (var child in groep.Children)
                AddIdsFromChildren(ids, child);

            return ids;
        }

        public void RemoveComponent(int id)
        {
            var component = _hoofdGroep.Children.FirstOrDefault(f => f.Id == id);
            if(component is not null)
                _hoofdGroep.Children.Remove(component);
            else
                foreach (var groep in Groepen().Select(g => g.InnerComponent() as Groep))
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

            return groep.Groepen.Any(subGroup => RemoveComponentFromGroupRecursive(subGroup.InnerComponent() as Groep, id));
        }

        public void SelectFigure(int id)
        {
            if (GetFigure(id) is { } figure)
            {
                ClearSelection();
                figure.Selected = true;
            }
        }

        public Figuur GetFigure(int id)
        {
            var component = GetComponent(id);
            return component.InnerComponent() as Figuur;
        }

        // verwijder alle figuren uit de lijst
        public void ResetComponents()
        {
            _hoofdGroep.Children.Clear();
        }

        public int NieuweGroep(int? parentGroupId = null)
        {
            var newId = GetNewId();
            var groep = new Groep {Name = "groep " + newId, Id = newId};
            groep.Children = new List<IComponent>();

            if (parentGroupId is null)
            {
                _hoofdGroep.Children.Add(groep);
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
            _hoofdGroep.Children.Add(group);
        }

        public IEnumerable<IComponent> Groepen()
        {
            return _hoofdGroep.Children.Where(c => c.ComponentType == ComponentType.Groep);
        }

        public void ClearSelection()
        {
            foreach (var component in _hoofdGroep.Children)
            {
                component.Selected = false;
                if (component is Groep groep)
                    ClearSelectionInGroup(groep);
            }
        }

        private void ClearSelectionInGroup(Groep groep)
        {
            foreach (var component in groep.Children)
            {
                component.Selected = false;
                if (component is Groep subGroep)
                    ClearSelectionInGroup(subGroep);
            }
        }

        public void SelectGroupRecursive(Groep groep)
        {
            foreach (var component in groep.Children)
            {
                component.Selected = true;
                if (component is Groep subGroep)
                    SelectGroupRecursive(subGroep);
            }

        }
    }
}
