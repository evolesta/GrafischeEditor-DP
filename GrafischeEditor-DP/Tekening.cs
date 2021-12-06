using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using GrafischeEditor_DP.CommandPattern;
using GrafischeEditor_DP.CommandPattern.Commands;

namespace GrafischeEditor_DP
{
    public partial class Tekening : Form
    {
        private TekenModus _currentMode;

        private Point _mouseDragStartPosition; // X and Y
        private Point _mouseDragEndPosition; // X and Y

        private bool _isMouseDown; // bool wanneer muisknop vastgehouden wordt
        private bool _isMoving; // bool wanneer een bestaand figuur verplaatst wordt
        private bool _isResizing; // bool wanneer een bestaand figuur van grootte veranderd wordt
        private bool _isDrawing ;
        private int _modifyingFigureId = -1; // index waarde van aan te passen object bij resizing + moving
        private Rectangle _modifyingRectangle; // rectangle die verplaatst of vergroot wordt
        private FiguurType _modifyingFigureType;

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
            _isDrawing = IsInTekenModus();
            
            _mouseDragStartPosition = e.Location; // bewaar X Y positie startpunt

            var huidigFiguur = _controller.Figuren().LastOrDefault(f => f.Positie.Contains(e.Location));
            if(huidigFiguur is not null) 
                _modifyingFigureId = huidigFiguur.Id;

            switch (_currentMode)
            {
                case TekenModus.Select:
                    foreach (var figuur in _controller.Figuren()) // doorloop alle figuren
                    {
                        // controleren of figuur zich in muispositie bevindt
                        if (figuur.Positie.Contains(_mouseDragStartPosition))
                        {
                            _isMoving = true; // zet boolean op beweegmodus
                            _modifyingRectangle = _controller.GetFiguur(_modifyingFigureId).Positie; // verkrijg rectangle van object
                            _modifyingFigureType = _controller.GetFiguur(_modifyingFigureId).Type; // verkrijg soort figuur
                        }
                    }
                    break;
                case TekenModus.Resize:
                    foreach (var figuur in _controller.Figuren()) // doorloop alle figuren
                    {
                        // controleren of figuur zich in muispositie bevindt en de state default is
                        if (figuur.Positie.Contains(_mouseDragStartPosition))
                        {
                            _isResizing = true; // zet boolean actief resizing
                            _modifyingRectangle = _controller.GetFiguur(_modifyingFigureId).Positie; // verkrijg rectangle van object
                            _modifyingFigureType = _controller.GetFiguur(_modifyingFigureId).Type; // verkrijg soort figuur
                        }
                    }
                    break;
            }
        }

        private bool IsInTekenModus()
        {
            return _currentMode == TekenModus.Ellipse || _currentMode == TekenModus.Rectangle;
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

            switch (_currentMode)
            {
                case TekenModus.Select:
                    if (_modifyingFigureId >= 0)
                    {
                        if (_mouseDragEndPosition == _mouseDragStartPosition)
                            _controller.WijzigSelectie(_modifyingFigureId);
                        else
                            _invoker.SetCommand(new BewerkFiguurCommand(_controller, MoveRectangle(_modifyingRectangle), _modifyingFigureId));
                    }
                    break;
                case TekenModus.Resize:
                    if (_modifyingFigureId >= 0 && _mouseDragEndPosition != _mouseDragStartPosition)
                        _invoker.SetCommand(new BewerkFiguurCommand(_controller, ResizeRectangle(_modifyingRectangle), _modifyingFigureId));
                    break;
                case TekenModus.Rectangle:
                case TekenModus.Ellipse:
                    if (_mouseDragEndPosition != _mouseDragStartPosition)
                    {
                        int? selectedGroupId = null;
                        if (treeView.SelectedNode is not null)
                            selectedGroupId = ((IComponent)treeView.SelectedNode.Tag).Id;

                        _invoker.SetCommand(new NieuwFiguurCommand(_controller, GetRectangle(),
                            ToFiguurType(_currentMode), selectedGroupId));
                    }

                    break;
                case TekenModus.Verwijder:
                    if (_modifyingFigureId >= 0 && _mouseDragEndPosition == _mouseDragStartPosition)
                        _invoker.SetCommand(new VerwijderFiguurCommand(_controller, _modifyingFigureId));
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
            foreach (var figuur in _controller.Figuren())
            {
                Draw(figuur.Type, figuur.Positie, figuur.Geselecteerd, e);
            }

            foreach (var groep in _controller.Groepen())
            {
                DrawFiguresRecursive(groep, e);
            }

            FillTreeview(); // genereer TreeView met figuren

            // wanneer er nog getekend wordt, teken preview
            if (_isDrawing)
            {
                Draw(ToFiguurType(_currentMode), GetRectangle(), false, e);
            }

            if (_isMoving)
            {
                Draw(_modifyingFigureType, MoveRectangle(_modifyingRectangle), true, e);
            }

            if (_isResizing)
            {
                Draw(_modifyingFigureType, ResizeRectangle(_modifyingRectangle), true, e);
            }
        }

        private void DrawFiguresRecursive(Groep groep, PaintEventArgs e)
        {
            foreach (var figuur in groep.Figuren) 
                Draw(figuur, e);

            foreach (var subGroep in groep.Groepen) 
                DrawFiguresRecursive(subGroep, e);
        }


        private void BestandOpslaan_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog(); // nieuw SaveFile dialog object aanmaken
            dialog.Filter = "XML files (*.xml)|*.xml"; // bestandstype definieren
            
            if (dialog.ShowDialog() == DialogResult.OK) // dialog openen voor opslaglocatie
            {
                _invoker.SetCommand(new OpslaanBestandCommand(_controller, dialog.FileName));
                _invoker.Execute();
            }
        }

        private void OpenBestand_Click(object sender, EventArgs e)
        {
            CheckForUnsavedChanges();

            OpenFileDialog dialog = new OpenFileDialog(); // nieuw openFile dialog object
            dialog.Filter = "XML files (*.xml)|*.xml";

            // als dialog een succesvol pad heeft bestand daadwerkelijk openen
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                _invoker.SetCommand(new OpenBestandCommand(_controller, dialog.FileName));
                _invoker.Execute();
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
            if (treeView.SelectedNode is not null)
                selectedGroupId = ((IComponent)treeView.SelectedNode.Tag).Id;

            _invoker.SetCommand(new NieuweGroepCommand(_controller, selectedGroupId));
            _invoker.Execute();
            FillTreeview();
        }

        private void treeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                _currentComponent = e.Node.Tag as IComponent; // verkrijg object uit Node
                
                var c = sender as Control;

                // genereer toolstripmenu
                var menu = new ContextMenuStrip();
                menu.Items.Add("Verwijderen", null, DeleteContextMenuItemClick);

                if (e.Node.Tag is Groep groep)
                {
                    groep.Geselecteerd = true;
                    menu.Items.Add("Groep toevoegen", null, AddChildGoupMenuItemClick);
                }

                menu.Show(c, e.Location); // toon menu aan gebruiker
            }
        }

        private void AddChildGoupMenuItemClick(object? sender, EventArgs e)
        {
            var command = new NieuweGroepCommand(_controller, _currentComponent.Id);
            _invoker.SetCommand(command);
            _invoker.Execute();
        }

        private void DeleteContextMenuItemClick(object sender, EventArgs e)
        {
            _invoker.SetCommand(new VerwijderFiguurCommand(_controller, _currentComponent.Id));
            _invoker.Execute();
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
        // Methode voor het printen van een figuur op het scherm
        private void Draw(FiguurType type, Rectangle positie, bool selected, PaintEventArgs e)
        {
            Pen pen = GeneratePen(selected); // genereer nieuwe pen

            switch (type)
            {
                case FiguurType.Rectangle:
                    e.Graphics.DrawRectangle(pen, positie);
                    break;
                case FiguurType.Ellipse:
                    e.Graphics.DrawEllipse(pen, positie);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type));
            }
        }

        private void Draw(Figuur figuur, PaintEventArgs e)
        {
            Draw(figuur.Type, figuur.Positie, figuur.Geselecteerd, e);
        }

        // Methode voor het genereren van een pen om te tekenen
        private Pen GeneratePen(bool selectie)
        {
            Pen pen = new Pen(Color.Black, 2); // maak nieuwe pen object aan

            // check op boolean waarde voor selectie
            if (selectie)
            {
                pen.DashStyle = DashStyle.Dot;
            }

            return pen;
        }

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

        // methode om de plaating van het figuur op de x- en y-as opnieuw te berekenen
        private Rectangle MoveRectangle(Rectangle currRectangle)
        {
            var rectangle = new Rectangle();
            rectangle.X = _mouseDragEndPosition.X;
            rectangle.Y = _mouseDragEndPosition.Y;
            rectangle.Width = currRectangle.Width; // waarde veranderd niet bij verplaatsen
            rectangle.Height = currRectangle.Height; // waarde veranderd niet bij verplaatsen
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
                var newNode = new TreeNode { Text = component.Naam, Tag = component };

                if (component is Groep groep)
                    AddChildNodesRecursive(newNode, groep);

                treeView.Nodes.Add(newNode);
            }

            treeView.EndUpdate(); // toon treeview naar GUI

            treeView.ExpandAll();
        }

        private static void AddChildNodesRecursive(TreeNode node, Groep groep)
        {

            // maak voor ieder figuur een node aan in de treeview
            foreach (var component in groep.Children)
            {
                // voeg nieuwe node toe voor een groep of figuur. Bewaar het object in de node voor later gebruik 
                var subNode = new TreeNode { Text = component.Naam, Tag = component };

                if (component is Groep subGroep)
                    AddChildNodesRecursive(subNode, subGroep);

                node.Nodes.Add(subNode);
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
