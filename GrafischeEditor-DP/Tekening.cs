using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace GrafischeEditor_DP
{
    public partial class Tekening : Form
    {
        Figuur.TekenModus HuidigeModus; // defineer het type figuur

        Rectangle rectangle; // tijdelijk figuur
        Point startpos; // start positie X Y rechthoek
        Point endpos; // eind positie X Y rechthoek

        bool IsMouseDown = false; // bool wanneer muisknop vastgehouden wordt
        bool IsMoving = false; // bool wanneer een bestaand figuur verplaatst wordt
        bool IsResizing = false; // bool wanneer een bestaand figuur van grootte veranderd wordt
        int ModifyingFigureIndex = -1; // index waarde van aan te passen object bij resizing + moving
        Rectangle ModifyingRectangle; // rectangle die verplaatst of vergroot wordt
        Figuur.TekenModus ModifyingFigureType;

        Controller controller = new Controller(); // controller object
        // genereer command invoker en receiver objecten
        Invoker invoker = new Invoker();

        public Tekening()
        {
            InitializeComponent();
        }

        // METHODEN -- //
        // Methode voor het printen van een figuur op het scherm
        private void Draw(Figuur.TekenModus type, Rectangle positie, bool selected, PaintEventArgs e)
        {
            Pen pen = generatePen(selected); // genereer nieuwe pen

            switch (type)
            {
                case Figuur.TekenModus.Square:
                    e.Graphics.DrawRectangle(pen, positie);
                    break;
                case Figuur.TekenModus.Ellipse:
                    e.Graphics.DrawEllipse(pen, positie);
                    break;
            }
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
            if (controller.GetFiguren().Count() != 0)
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

        // -- Drawpanel mouse actions

        // optie op ellipsen te tekenen
        private void ButtonEllipse_Click(object sender, EventArgs e)
        {
            // verander naar tekencursor & state
            HuidigeModus = Figuur.TekenModus.Ellipse;
            Cursor = Cursors.Cross;
        }

        // optie om squares te tekenen
        private void ButtonSquare_Click(object sender, EventArgs e)
        {
            HuidigeModus = Figuur.TekenModus.Square;
            Cursor = Cursors.Cross;
        }

        // opties voor standaard pointer, voor selecties e.d.
        private void ButtonPointer_Click(object sender, EventArgs e)
        {
            HuidigeModus = Figuur.TekenModus.Select;
            Cursor = Cursors.Default;
        }

        private void ButtonRemove_Click(object sender, EventArgs e)
        {
            HuidigeModus = Figuur.TekenModus.Verwijder;
            Cursor = Cursors.No;
        }

        private void ButtonResize_Click(object sender, EventArgs e)
        {
            HuidigeModus = Figuur.TekenModus.Resize;
            Cursor = Cursors.SizeNWSE;
        }

        // aangeroepen als muisknop ingehouden wordt
        private void DrawPanel_MouseDown(object sender, MouseEventArgs e)
        {
            IsMouseDown = true;
            startpos = e.Location; // bewaar X Y positie startpunt

            var huidigFiguur = controller.GetFiguren().LastOrDefault(f => f.Positie.Contains(e.Location));
            ModifyingFigureIndex = controller.GetFiguren().IndexOf(huidigFiguur);

            switch (HuidigeModus)
            {
                case Figuur.TekenModus.Select:
                    foreach (var figuur in controller.GetFiguren().ToList()) // doorloop alle figuren
                    {
                        // controleren of figuur zich in muispositie bevindt
                        if (figuur.Positie.Contains(startpos))
                        {
                            IsMoving = true; // zet boolean op beweegmodus
                            ModifyingRectangle = controller.GetFiguur(ModifyingFigureIndex).Positie; // verkrijg rectangle van object
                            ModifyingFigureType = controller.GetFiguur(ModifyingFigureIndex).Type; // verkrijg soort figuur
                        }
                    }
                    break;
                case Figuur.TekenModus.Resize:
                    foreach (var figuur in controller.GetFiguren().ToList()) // doorloop alle figuren
                    {
                        // controleren of figuur zich in muispositie bevindt en de state default is
                        if (figuur.Positie.Contains(startpos))
                        {
                            IsResizing = true; // zet boolean actief resizing
                            ModifyingRectangle = controller.GetFiguur(ModifyingFigureIndex).Positie; // verkrijg rectangle van object
                            ModifyingFigureType = controller.GetFiguur(ModifyingFigureIndex).Type; // verkrijg soort figuur
                        }
                    }
                    break;
            }
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
                case Figuur.TekenModus.Select:
                    if (ModifyingFigureIndex >= 0)
                    {
                        if (endpos == startpos)
                            controller.WijzigSelectie(ModifyingFigureIndex);
                        else
                            invoker.SetCommand(new BewerkFiguurCommand(controller, MoveRectangle(ModifyingRectangle), ModifyingFigureIndex));
                            invoker.Execute();
                    }
                    break;
                case Figuur.TekenModus.Resize:
                    if (ModifyingFigureIndex >= 0 && endpos != startpos)
                        invoker.SetCommand(new BewerkFiguurCommand(controller, ResizeRectangle(ModifyingRectangle), ModifyingFigureIndex));
                        invoker.Execute();
                    break;
                case Figuur.TekenModus.Square:
                case Figuur.TekenModus.Ellipse:
                    invoker.SetCommand(new NieuwFiguurCommand(controller, GetRectangle(), HuidigeModus));
                    invoker.Execute();
                    break;
                case Figuur.TekenModus.Verwijder:
                    if (ModifyingFigureIndex >= 0 && endpos == startpos)
                        invoker.SetCommand(new VerwijderFiguurCommand(controller, ModifyingFigureIndex));
                        invoker.Execute();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            ModifyingFigureIndex = -1;
            Refresh(); // ververs drawpanel zodat het nieuwe figuur zichtbaar wordt
        }

        private void DrawPanel_Paint(object sender, PaintEventArgs e)
        {
            // verkrijg lijst met n figuren en print ieder figuur op het scherm
            foreach (var figuur in controller.GetFiguren())
            {
                Draw(figuur.Type, figuur.Positie, figuur.Geselecteerd, e);
            }

            // wanneer er nog getekend wordt, teken preview
            if (IsMouseDown)
            {
                Draw(HuidigeModus, GetRectangle(), false, e);
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

            controller.ResetFiguren(); // verwijder alle figuren uit lijst
            Refresh(); // herteken het werkveld
        }

        private void ongedaanMaken_Click(object sender, EventArgs e)
        {
            invoker.Undo();
            Refresh();
        }

        private void opnieuwUitvoeren_Click(object sender, EventArgs e)
        {
            invoker.Redo();
            Refresh();
        }
    }
}
