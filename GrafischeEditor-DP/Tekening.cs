using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using GrafischeEditor_DP.CommandPattern;
using GrafischeEditor_DP.CommandPattern.Commands;

namespace GrafischeEditor_DP
{
    public partial class Tekening : Form
    {
        TekenModus HuidigeModus; // defineer het type figuur

        Rectangle rectangle; // tijdelijk figuur
        Point startpos; // start positie X Y rechthoek
        Point endpos; // eind positie X Y rechthoek

        bool IsMouseDown = false; // bool wanneer muisknop vastgehouden wordt
        bool IsMoving = false; // bool wanneer een bestaand figuur verplaatst wordt
        bool IsResizing = false; // bool wanneer een bestaand figuur van grootte veranderd wordt
        private bool IsDrawing ;
        int ModifyingFigureId = -1; // index waarde van aan te passen object bij resizing + moving
        Rectangle ModifyingRectangle; // rectangle die verplaatst of vergroot wordt
        FiguurType ModifyingFigureType;

        Controller controller = new Controller(); // controller object

        private IEnumerable<IComponent> Componenten() => controller.GetComponents();
        // genereer command invoker en receiver objecten
        Invoker invoker = new Invoker();

        private IComponent _currentComponent;

        public Tekening()
        {
            InitializeComponent();
        }

        // METHODEN -- //
        // Methode voor het printen van een figuur op het scherm
        private void Draw(FiguurType type, Rectangle positie, bool selected, PaintEventArgs e)
        {
            Pen pen = generatePen(selected); // genereer nieuwe pen

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
        private Pen generatePen(bool selectie)
        {
            Pen pen = new Pen(Color.Black, 2); // maak nieuwe pen object aan

            // check op boolean waarde voor selectie
            if (selectie)
            {
                pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
            }

            return pen; 
        }

        // Bereken de plaatsing van het figuur adhv start & eind x-y posities
        private Rectangle GetRectangle()
        {
            rectangle = new Rectangle();
            rectangle.X = Math.Min(startpos.X, endpos.X);
            rectangle.Y = Math.Min(startpos.Y, endpos.Y);
            rectangle.Width = Math.Abs(startpos.X - endpos.X);
            rectangle.Height = Math.Abs(startpos.Y - endpos.Y);
            return rectangle;
        }

        // methode om de plaating van het figuur op de x- en y-as opnieuw te berekenen
        private Rectangle MoveRectangle(Rectangle currRectangle)
        {
            rectangle = new Rectangle();
            rectangle.X = endpos.X;
            rectangle.Y = endpos.Y;
            rectangle.Width = currRectangle.Width; // waarde veranderd niet bij verplaatsen
            rectangle.Height = currRectangle.Height; // waarde veranderd niet bij verplaatsen
            return rectangle;
        }

        private Rectangle ResizeRectangle(Rectangle currRectangle)
        {
            rectangle = new Rectangle();
            
            if (endpos.X < currRectangle.X) 
            {
                rectangle.X = endpos.X; 
            } 
            else
            {
                rectangle.X = currRectangle.X; 
            }

            if (endpos.Y < currRectangle.Y) 
            { 
                rectangle.Y = endpos.Y;
            }
            else
            {
                rectangle.Y = currRectangle.Y;
            }

            rectangle.Width = Math.Abs(endpos.X - currRectangle.X);
            rectangle.Height = Math.Abs(endpos.Y - currRectangle.Y);
            return rectangle;
        }

        // controleren of de tekening figuren bevat en vragen of deze opgeslagen moet worden
        private void CheckIfDrawingSaved()
        {
            // controleren of er al figuren getekend zijn
            if (controller.GetComponents().Count() != 0)
            {
                // vraag aan gebruiker of de tekening moet worden opgeslagen
                DialogResult dialog = MessageBox.Show("Wil je de tekening opslaan?", "Tekening opslaan", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                // opslaan bestand aanroepen bij "Ja"
                if (dialog == DialogResult.Yes)
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
            foreach (var component in controller.GetComponents())
            {
                // voeg nieuwe node toe voor een groep of figuur. Bewaar het object in de node voor later gebruik 
                var newNode = new TreeNode{ Text = component.Naam, Tag = component  }; 
                
                if(component is Groep groep)
                    AddChildNodesRecursive(newNode, groep);

                treeView.Nodes.Add(newNode);
            }

            treeView.EndUpdate(); // toon treeview naar GUI

            treeView.ExpandAll();
        }

        private void AddChildNodesRecursive(TreeNode node, Groep groep)
        {

            // maak voor ieder figuur een node aan in de treeview
            foreach (var component in groep.Children)
            {
                // voeg nieuwe node toe voor een groep of figuur. Bewaar het object in de node voor later gebruik 
                var subNode = new TreeNode() {Text = component.Naam, Tag = component};

                foreach (var subGroep in groep.Groepen)
                    AddChildNodesRecursive(subNode, subGroep);

                node.Nodes.Add(subNode);
            }
        }

        // -- Drawpanel mouse actions

        // optie op ellipsen te tekenen
        private void ButtonEllipse_Click(object sender, EventArgs e)
        {
            // verander naar tekencursor & state
            HuidigeModus = TekenModus.Ellipse;
            Cursor = Cursors.Cross;
        }

        // optie om squares te tekenen
        private void ButtonSquare_Click(object sender, EventArgs e)
        {
            HuidigeModus = TekenModus.Rectangle;
            Cursor = Cursors.Cross;
        }

        // opties voor standaard pointer, voor selecties e.d.
        private void ButtonPointer_Click(object sender, EventArgs e)
        {
            HuidigeModus = TekenModus.Select;
            Cursor = Cursors.Default;
        }

        private void ButtonRemove_Click(object sender, EventArgs e)
        {
            HuidigeModus = TekenModus.Verwijder;
            Cursor = Cursors.No;
        }

        private void ButtonResize_Click(object sender, EventArgs e)
        {
            HuidigeModus = TekenModus.Resize;
            Cursor = Cursors.SizeNWSE;
        }

        // aangeroepen als muisknop ingehouden wordt
        private void DrawPanel_MouseDown(object sender, MouseEventArgs e)
        {
            IsMouseDown = true;
            IsDrawing = IsInTekenModus();
            
            startpos = e.Location; // bewaar X Y positie startpunt

            var huidigFiguur = controller.Figuren().LastOrDefault(f => f.Positie.Contains(e.Location));
            if(huidigFiguur is not null) 
                ModifyingFigureId = huidigFiguur.Id;

            switch (HuidigeModus)
            {
                case TekenModus.Select:
                    foreach (var figuur in controller.Figuren()) // doorloop alle figuren
                    {
                        // controleren of figuur zich in muispositie bevindt
                        if (figuur.Positie.Contains(startpos))
                        {
                            IsMoving = true; // zet boolean op beweegmodus
                            ModifyingRectangle = controller.GetFiguur(ModifyingFigureId).Positie; // verkrijg rectangle van object
                            ModifyingFigureType = controller.GetFiguur(ModifyingFigureId).Type; // verkrijg soort figuur
                        }
                    }
                    break;
                case TekenModus.Resize:
                    foreach (var figuur in controller.Figuren()) // doorloop alle figuren
                    {
                        // controleren of figuur zich in muispositie bevindt en de state default is
                        if (figuur.Positie.Contains(startpos))
                        {
                            IsResizing = true; // zet boolean actief resizing
                            ModifyingRectangle = controller.GetFiguur(ModifyingFigureId).Positie; // verkrijg rectangle van object
                            ModifyingFigureType = controller.GetFiguur(ModifyingFigureId).Type; // verkrijg soort figuur
                        }
                    }
                    break;
            }
        }

        private bool IsInTekenModus()
        {
            return HuidigeModus == TekenModus.Ellipse || HuidigeModus == TekenModus.Rectangle;
        }

        private void DrawPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (IsMouseDown) // alleen uitvoeren wanneer muisknop ingehouden wordt
            {
                endpos = e.Location; // eindpositie opslaan in pointer
                Refresh();
            }
        }

        // aangeroepen als muisknop losgelaten wordt
        private void DrawPanel_MouseUp(object sender, MouseEventArgs e)
        {
            IsMouseDown = false;
            endpos = e.Location;
            IsMoving = false;
            IsResizing = false;

            switch (HuidigeModus)
            {
                case TekenModus.Select:
                    if (ModifyingFigureId >= 0)
                    {
                        if (endpos == startpos)
                            controller.WijzigSelectie(ModifyingFigureId);
                        else
                            invoker.SetCommand(new BewerkFiguurCommand(controller, MoveRectangle(ModifyingRectangle), ModifyingFigureId));
                    }
                    break;
                case TekenModus.Resize:
                    if (ModifyingFigureId >= 0 && endpos != startpos)
                        invoker.SetCommand(new BewerkFiguurCommand(controller, ResizeRectangle(ModifyingRectangle), ModifyingFigureId));
                    break;
                case TekenModus.Rectangle:
                case TekenModus.Ellipse:
                    if(endpos != startpos)
                        invoker.SetCommand(new NieuwFiguurCommand(controller, GetRectangle(), ToFiguurType(HuidigeModus)));
                    break;
                case TekenModus.Verwijder:
                    if (ModifyingFigureId >= 0 && endpos == startpos)
                        invoker.SetCommand(new VerwijderFiguurCommand(controller, ModifyingFigureId));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (invoker.HasCommand) 
                invoker.Execute();

            ModifyingFigureId = -1;
            Refresh(); // ververs drawpanel zodat het nieuwe figuur zichtbaar wordt
        }

        private void DrawPanel_Paint(object sender, PaintEventArgs e)
        {
            // verkrijg lijst met n figuren en print ieder figuur op het scherm
            foreach (var figuur in controller.Figuren())
            {
                Draw(figuur.Type, figuur.Positie, figuur.Geselecteerd, e);
            }

            foreach (var groep in controller.Groepen())
            {
                DrawFiguresRecursive(groep, e);
            }

            FillTreeview(); // genereer TreeView met figuren

            // wanneer er nog getekend wordt, teken preview
            if (IsDrawing)
            {
                Draw(ToFiguurType(HuidigeModus), GetRectangle(), false, e);
            }

            if (IsMoving)
            {
                Draw(ModifyingFigureType, MoveRectangle(ModifyingRectangle), true, e);
            }

            if (IsResizing)
            {
                Draw(ModifyingFigureType, ResizeRectangle(ModifyingRectangle), true, e);
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
                invoker.SetCommand(new OpslaanBestandCommand(controller, dialog.FileName));
                invoker.Execute();
            }
        }

        private void OpenBestand_Click(object sender, EventArgs e)
        {
            CheckIfDrawingSaved();

            OpenFileDialog dialog = new OpenFileDialog(); // nieuw openFile dialog object
            dialog.Filter = "XML files (*.xml)|*.xml";

            // als dialog een succesvol pad heeft bestand daadwerkelijk openen
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                invoker.SetCommand(new OpenBestandCommand(controller, dialog.FileName));
                invoker.Execute();
            }

            Refresh(); // herteken het werkveld
        }

        private void NieuwBestand_Click(object sender, EventArgs e)
        {
            CheckIfDrawingSaved(); // controleren of bestaande tekening is opgeslagen

            controller.ResetComponents(); // verwijder alle figuren uit lijst
            Refresh(); // herteken het werkveld
        }

        private void ongedaanMaken_Click(object sender, EventArgs e)
        {
            invoker.Undo();
            Refresh();
            FillTreeview();
        }

        private void opnieuwUitvoeren_Click(object sender, EventArgs e)
        {
            invoker.Redo();
            Refresh();
            FillTreeview();
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

        private void btnNieuweGroep_Click(object sender, EventArgs e)
        {
            invoker.SetCommand(new NieuweGroepCommand(controller));
            invoker.Execute();
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
                menu.Show(c, e.Location); // toon menu aan gebruiker
                
            }
        }

        private void DeleteContextMenuItemClick(object sender, EventArgs e)
        {
            invoker.SetCommand(new VerwijderFiguurCommand(controller, _currentComponent.Id));
            invoker.Execute();
        }

        private void TreeView_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            IComponent component;

            if (e.Node?.Tag is not null && !string.IsNullOrWhiteSpace(e.Label))
            {
                component = e.Node.Tag as IComponent;

                invoker.SetCommand(new RenameCommand(component, e.Label));
                invoker.Execute();

                FillTreeview();
            }
            else
            {
                e.CancelEdit = true;
            }
        }
    }

    public enum TekenModus
    {
        Rectangle,
        Ellipse,
        Select,
        Resize,
        Verwijder
    } // enum voor soorten figuren
}
