using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using GrafischeEditor_DP.CommandPattern;
using GrafischeEditor_DP.CommandPattern.Commands;
using GrafischeEditor_DP.VisitorPattern;
using Newtonsoft.Json;

namespace GrafischeEditor_DP
{
    public partial class Tekening : Form
    {
        private static readonly Figuur _preview = new();

        private TekenModus _currentMode;

        private Point _mouseDragStartPosition; // X and Y
        private Point _mouseDragEndPosition; // X and Y

        private bool _isMouseDown; // bool wanneer muisknop vastgehouden wordt
        private bool _isMoving; // bool wanneer een bestaand figuur verplaatst wordt
        private bool _isResizing; // bool wanneer een bestaand figuur van grootte veranderd wordt
        private bool _isDrawing ;
        private int _modifyingFigureId = -1; // index waarde van aan te passen object bij resizing + moving

        private readonly Controller _controller = new(); // controller object

        private IEnumerable<IComponent> Componenten() => _controller.GetComponents();
        private readonly Invoker _invoker = new();

        private IComponent _currentComponent;

        public Tekening()
        {
            InitializeComponent();
        }


        #region EVENTHANDLERS

        // -- Drawpanel mouse actions

        // optie op ellipsen te tekenen
        private void ButtonEllipse_Click(object sender, EventArgs e)
        {
            // verander naar tekencursor & state
            _currentMode = TekenModus.Ellipse;
            Cursor = Cursors.Cross;
        }

        // optie om squares te tekenen
        private void ButtonSquare_Click(object sender, EventArgs e)
        {
            _currentMode = TekenModus.Rectangle;
            Cursor = Cursors.Cross;
        }

        // opties voor standaard pointer, voor selecties e.d.
        private void ButtonPointer_Click(object sender, EventArgs e)
        {
            _currentMode = TekenModus.Select;
            Cursor = Cursors.Default;
        }

        private void ButtonRemove_Click(object sender, EventArgs e)
        {
            _currentMode = TekenModus.Verwijder;
            Cursor = Cursors.No;
        }

        private void ButtonResize_Click(object sender, EventArgs e)
        {
            _currentMode = TekenModus.Resize;
            Cursor = Cursors.SizeNWSE;
        }

        // aangeroepen als muisknop ingehouden wordt
        private void DrawPanel_MouseDown(object sender, MouseEventArgs e)
        {
            _isMouseDown = true;
            _isDrawing = IsInDrawingMode() && _mouseDragStartPosition != _mouseDragEndPosition;
            
            _mouseDragStartPosition = e.Location; // bewaar X Y positie startpunt

            var figures = _controller.GetAllFiguresFlattened();
            var huidigFiguur = figures.LastOrDefault(f => f.Placement.Contains(e.Location));
            if(huidigFiguur is not null) 
                _modifyingFigureId = huidigFiguur.Id;

            switch (_currentMode)
            {
                case TekenModus.Select:
                    foreach (var figuur in _controller.GetAllFiguresFlattened()) // doorloop alle figuren
                    {
                        if (figuur.Placement.Contains(_mouseDragStartPosition))
                        {
                            _isMoving = true; // zet boolean op beweegmodus
                        }
                    }
                    break;
                case TekenModus.Resize:
                    foreach (var figuur in _controller.GetAllFiguresFlattened()) // doorloop alle figuren
                    {
                        // controleren of figuur zich in muispositie bevindt en de state default is
                        if (figuur.Placement.Contains(_mouseDragStartPosition))
                        {
                            _isResizing = true; // zet boolean actief resizing
                        }
                    }
                    break;
                case TekenModus.Rectangle:
                case TekenModus.Ellipse:
                case TekenModus.Verwijder:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private bool IsInDrawingMode()
        {
            return _currentMode is TekenModus.Ellipse or TekenModus.Rectangle;
        }

        private void DrawPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isMouseDown) // alleen uitvoeren wanneer muisknop ingehouden wordt
            {
                _mouseDragEndPosition = e.Location; // eindpositie opslaan in pointer
                Refresh();
            }
        }

        // aangeroepen als muisknop losgelaten wordt
        private void DrawPanel_MouseUp(object sender, MouseEventArgs e)
        {
            _isMouseDown = false;
            _mouseDragEndPosition = e.Location;
            _isMoving = false;
            _isResizing = false; 
            _isDrawing = false;

            var selectedGroupId = _controller.SelectedGroupId();

            switch (_currentMode)
            {
                case TekenModus.Select:
                    if (_modifyingFigureId >= 0)
                    {
                        if (_mouseDragEndPosition == _mouseDragStartPosition)
                            _controller.SelectFigure(_modifyingFigureId);
                        else
                        {
                            if (selectedGroupId.HasValue)
                            {
                                var moveX = _mouseDragEndPosition.X - _mouseDragStartPosition.X;
                                var moveY = _mouseDragEndPosition.Y - _mouseDragStartPosition.Y;
                                _invoker.SetCommand(new MoveGroupCommand(_controller, selectedGroupId.Value, moveX, moveY));
                            }
                            else
                            {
                                var moveRight = _mouseDragEndPosition.X - _mouseDragStartPosition.X;
                                var moveDown = _mouseDragEndPosition.Y - _mouseDragStartPosition.Y;
                                _invoker.SetCommand(new MoveFigureCommand(_controller, moveRight, moveDown, _modifyingFigureId));
                            }


                        }
                    }
                    break;
                case TekenModus.Resize:
                    if (_mouseDragEndPosition != _mouseDragStartPosition)
                    {
                        if (selectedGroupId.HasValue)
                        {
                            _invoker.SetCommand(new ResizeGroupCommand(selectedGroupId.Value, _controller, _mouseDragEndPosition));
                        }
                        else if (_modifyingFigureId >= 0)
                        {
                            var figure = _controller.GetFigure(_modifyingFigureId);
                            _invoker.SetCommand(new ResizeFigureCommand(figure, _mouseDragEndPosition));
                        }
                    }
                    break;
                case TekenModus.Rectangle:
                case TekenModus.Ellipse:
                    if (_mouseDragEndPosition != _mouseDragStartPosition)
                    {
                        _invoker.SetCommand(new NieuwFiguurCommand(_controller, GetRectangle(),
                            ToFiguurType(_currentMode), selectedGroupId));
                    }

                    break;
                case TekenModus.Verwijder:
                    if (_modifyingFigureId >= 0 && _mouseDragEndPosition == _mouseDragStartPosition)
                        _invoker.SetCommand(new RemoveComponentCommand(_controller, _modifyingFigureId));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (_invoker.HasCommand) 
                _invoker.Execute();

            _modifyingFigureId = -1;

            Refresh(); // ververs drawpanel zodat het nieuwe figuur zichtbaar wordt
        }

        private void DrawPanel_Paint(object sender, PaintEventArgs e)
        {
            // verkrijg lijst met n figuren en print ieder figuur op het scherm
            foreach (var component in _controller.GetComponents())
            {
                if(component is not null)
                    component.Draw(e);
            }

            FillTreeview(); // genereer TreeView met figuren

            // wanneer er nog getekend wordt, teken preview
            if (_isDrawing)
            {
                _preview.FiguurType = ToFiguurType(_currentMode); 
                _preview.Placement = GetRectangle();
                _preview.Draw(e);
            }

            if (_isMoving)
            {   
                var figure = _controller.GetFigure(_modifyingFigureId);
                var preview = MoveRectangle(figure.Placement);
                figure.Draw(e, preview);    
            }

            if (_isResizing)
            {
                var figure = _controller.GetFigure(_modifyingFigureId);
                var preview = ResizeRectangle(figure.Placement);
                figure.Draw(e, preview);
            }
        }


        private void BestandOpslaan_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog(); // nieuw SaveFile dialog object aanmaken
            dialog.Filter = "JSON files (*.json)|*.json"; // bestandstype definieren
            
            if (dialog.ShowDialog() == DialogResult.OK) // dialog openen voor opslaglocatie
            {
                var visitor = new OpslaanVisitor(dialog.FileName);
                _controller.HoofdGroep.Accept(visitor);
            }
        }

        private void OpenBestand_Click(object sender, EventArgs e)
        {
            CheckForUnsavedChanges();

            OpenFileDialog dialog = new OpenFileDialog(); // nieuw openFile dialog object
            dialog.Filter = "JSON files (*.json)|*.json";

            // als dialog een succesvol pad heeft bestand daadwerkelijk openen
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                var json = File.ReadAllText(dialog.FileName);
                var groep = JsonConvert.DeserializeObject<Groep>(json, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                _controller.HoofdGroep = groep;
            }

            Refresh(); // herteken het werkveld
        }

        private void NieuwBestand_Click(object sender, EventArgs e)
        {
            CheckForUnsavedChanges(); // controleren of bestaande tekening is opgeslagen

            _controller.ResetComponents(); // verwijder alle figuren uit lijst
            Refresh(); // herteken het werkveld
        }

        private void ongedaanMaken_Click(object sender, EventArgs e)
        {
            _invoker.Undo();
            Refresh();
            FillTreeview();
        }

        private void opnieuwUitvoeren_Click(object sender, EventArgs e)
        {
            _invoker.Redo();
            Refresh();
            FillTreeview();
        }

        private void btnNieuweGroep_Click(object sender, EventArgs e)
        {
            int? selectedGroupId = null;
            if (treeView.SelectedNode?.Tag is Groep)
                selectedGroupId = ((IComponent)treeView.SelectedNode.Tag).Id;

            _invoker.SetCommand(new NieuweGroepCommand(_controller, selectedGroupId));
            _invoker.Execute();
            FillTreeview();
        }

        private void treeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            _currentComponent = e.Node.Tag as IComponent; // verkrijg object uit Node
            var treeView = sender as TreeView;
            treeView.SelectedNode = e.Node;

            switch (e.Button)
            {
                case MouseButtons.Right:
                {
                    // genereer toolstripmenu
                    var menu = new ContextMenuStrip();
                    menu.Items.Add("Verwijderen", null, DeleteContextMenuItemClick);

                    if (e.Node.Tag is Groep)
                    {
                        menu.Items.Add("Groep toevoegen", null, AddChildGroupMenuItemClick);
                    }

                    menu.Items.Add("Bijschriften", null, LabelsMenuItemClick);

                    menu.Show(treeView, e.Location); // toon menu aan gebruiker
                    break;
                }
                case MouseButtons.Left:
                    _controller.ClearSelection();
                    _currentComponent.Selected = true;
                    if(_currentComponent is Groep groep)
                        _controller.SelectGroupRecursive(groep);
                    Refresh();
                    FillTreeview();
                    break;
                default:
                    break;
            }
        }

        private void LabelsMenuItemClick(object? sender, EventArgs e)
        {
          var parent = _controller.FindParentGroep(_currentComponent.Id);
          var form = new EditLabelForm(_currentComponent, parent);
          form.Closed += (_, _) => Refresh();
          form.ShowDialog();
        }

        private void AddChildGroupMenuItemClick(object? sender, EventArgs e)
        {
            var command = new NieuweGroepCommand(_controller, _currentComponent.Id);
            _invoker.SetCommand(command);
            _invoker.Execute();
            FillTreeview();
        }

        private void DeleteContextMenuItemClick(object sender, EventArgs e)
        {
            _invoker.SetCommand(new RemoveComponentCommand(_controller, _currentComponent.Id));
            _invoker.Execute();
            Refresh();
            FillTreeview();

        }

        private void TreeView_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            IComponent component;

            if (e.Node?.Tag is not null && !string.IsNullOrWhiteSpace(e.Label))
            {
                component = e.Node.Tag as IComponent;

                _invoker.SetCommand(new RenameCommand(component, e.Label));
                _invoker.Execute();

                FillTreeview();
            }
            else
            {
                e.CancelEdit = true;
            }
        }

        #endregion



        // METHODEN -- //

        // Bereken de plaatsing van het figuur adhv start & eind x-y posities
        private Rectangle GetRectangle()
        {
            var rectangle = new Rectangle();
            rectangle.X = Math.Min(_mouseDragStartPosition.X, _mouseDragEndPosition.X);
            rectangle.Y = Math.Min(_mouseDragStartPosition.Y, _mouseDragEndPosition.Y);
            rectangle.Width = Math.Abs(_mouseDragStartPosition.X - _mouseDragEndPosition.X);
            rectangle.Height = Math.Abs(_mouseDragStartPosition.Y - _mouseDragEndPosition.Y);
            return rectangle;
        }

        // methode om de plaatsing van het figuur op de x- en y-as opnieuw te berekenen
        private Rectangle MoveRectangle(Rectangle rectangle)
        {
            var moveRight = _mouseDragEndPosition.X - _mouseDragStartPosition.X;
            rectangle.X += moveRight;
            var moveDown = _mouseDragEndPosition.Y - _mouseDragStartPosition.Y;
            rectangle.Y += moveDown;
            return rectangle;
        }

        private Rectangle ResizeRectangle(Rectangle currRectangle)
        {
            var rectangle = new Rectangle();

            if (_mouseDragEndPosition.X < currRectangle.X)
            {
                rectangle.X = _mouseDragEndPosition.X;
            }
            else
            {
                rectangle.X = currRectangle.X;
            }

            if (_mouseDragEndPosition.Y < currRectangle.Y)
            {
                rectangle.Y = _mouseDragEndPosition.Y;
            }
            else
            {
                rectangle.Y = currRectangle.Y;
            }

            rectangle.Width = Math.Abs(_mouseDragEndPosition.X - currRectangle.X);
            rectangle.Height = Math.Abs(_mouseDragEndPosition.Y - currRectangle.Y);
            return rectangle;
        }

        // controleren of de tekening figuren bevat en vragen of deze opgeslagen moet worden
        private void CheckForUnsavedChanges()
        {
            // controleren of er al figuren zijn
            if (Componenten().Count() != 0)
            {
                // vraag aan gebruiker of de tekening moet worden opgeslagen
                var saveDialogResult = MessageBox.Show("Wil je de tekening opslaan?", "Tekening opslaan", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                // opslaan bestand aanroepen bij "Ja"
                if (saveDialogResult == DialogResult.Yes)
                {
                    BestandOpslaan_Click(null, null); // bestand opslaan
                }
            }
        }

        // TreeView voorzien van figuren + groepen
        public void FillTreeview()
        {
            treeView.BeginUpdate();
            treeView.Nodes.Clear(); // leeg TreeView om nieuwe view te genereren

            // maak voor ieder figuur een node aan in de treeview
            foreach (var component in _controller.GetComponents())
            {
                // voeg nieuwe node toe voor een groep of figuur. Bewaar het object in de node voor later gebruik 
                var newNode = new TreeNode { Text = component.Name, Tag = component };

                if (component is Groep groep)
                    AddChildNodesRecursive(newNode, groep, treeView);

                treeView.Nodes.Add(newNode);
                if (component.Selected)
                    treeView.SelectedNode = newNode;
            }

            treeView.EndUpdate(); // toon treeview naar GUI

            treeView.ExpandAll();
        }

        private static void AddChildNodesRecursive(TreeNode node, Groep groep, TreeView treeView)
        {

            // maak voor ieder figuur een node aan in de treeview
            foreach (var component in groep.Children)
            {
                // voeg nieuwe node toe voor een groep of figuur. Bewaar het object in de node voor later gebruik 
                var subNode = new TreeNode { Text = component.Name, Tag = component };

                if (component is Groep subGroep)
                    AddChildNodesRecursive(subNode, subGroep, treeView);

                node.Nodes.Add(subNode);
                if (!groep.Selected && component.Selected)
                    treeView.SelectedNode = subNode;
            }
        }

        private static FiguurType ToFiguurType(TekenModus modus)
        {
            return modus switch
            {
                TekenModus.Rectangle => FiguurType.Rectangle,
                TekenModus.Ellipse => FiguurType.Ellipse,
                _ => throw new ArgumentOutOfRangeException(nameof(modus), modus, null)
            };
        }
    }

    public enum TekenModus
    {
        Rectangle,
        Ellipse,
        Select,
        Resize,
        Verwijder
    } 
}
