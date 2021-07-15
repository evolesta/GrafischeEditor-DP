using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GrafischeEditor_DP
{
    public partial class Tekening : Form
    {
        Figuur.TekenModus soortFiguur; // defineer het type figuur

        Rectangle rectangle; // tijdelijk figuur
        Point startpos; // start positie X Y rechthoek
        Point endpos; // eind positie X Y rechthoek

        bool IsMouseDown = false; // bool wanneer muisknop vastgehouden wordt
        bool IsMoving = false; // bool wanneer een bestaand figuur verplaatst wordt
        bool IsResizing = false; // bool wanneer een bestaand figuur van grootte veranderd wordt
        int ModifyingFigureIndex; // index waarde van aan te passen object bij resizing + moving
        Rectangle ModifyingRectangle; // rectangle die verplaatst of vergroot wordt
        Figuur.TekenModus ModifyingFigureType;

        Controller controller = new Controller(); // controller object

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

        // -- Drawpanel mouse actions

        // optie op ellipsen te tekenen
        private void ButtonEllipse_Click(object sender, EventArgs e)
        {
            // verander naar tekencursor & state
            soortFiguur = Figuur.TekenModus.Ellipse;
            Cursor = Cursors.Cross;
        }

        // optie om squares te tekenen
        private void ButtonSquare_Click(object sender, EventArgs e)
        {
            soortFiguur = Figuur.TekenModus.Square;
            Cursor = Cursors.Cross;
        }

        // opties voor standaard pointer, voor selecties e.d.
        private void ButtonPointer_Click(object sender, EventArgs e)
        {
            soortFiguur = Figuur.TekenModus.Default;
            Cursor = Cursors.Default;
        }

        private void ButtonRemove_Click(object sender, EventArgs e)
        {
            // to do
        }

        private void ButtonResize_Click(object sender, EventArgs e)
        {
            soortFiguur = Figuur.TekenModus.Resize;
            Cursor = Cursors.SizeNWSE;
        }

        // aangeroepen als muisknop ingehouden wordt
        private void DrawPanel_MouseDown(object sender, MouseEventArgs e)
        {
            IsMouseDown = true;
            startpos = e.Location; // bewaar X Y positie startpunt

            switch (soortFiguur)
            {
                case Figuur.TekenModus.Default:
                    foreach (var figuur in controller.getFiguren().ToList()) // doorloop alle figuren
                    {
                        // controleren of figuur zich in muispositie bevindt en de state default is
                        if (figuur.Positie.Contains(startpos))
                        {
                            IsMoving = true; // zet boolean op beweegmodus
                            ModifyingFigureIndex = controller.getFiguren().IndexOf(figuur); // verkrijg figuur index uit lijst
                            ModifyingRectangle = controller.getFiguur(ModifyingFigureIndex).Positie; // verkrijg rectangle van object
                            ModifyingFigureType = controller.getFiguur(ModifyingFigureIndex).Type; // verkrijg soort figuur
                        }
                    }
                    break;
                case Figuur.TekenModus.Resize:
                    foreach (var figuur in controller.getFiguren().ToList()) // doorloop alle figuren
                    {
                        // controleren of figuur zich in muispositie bevindt en de state default is
                        if (figuur.Positie.Contains(startpos))
                        {
                            IsResizing = true; // zet boolean actief resizing
                            ModifyingFigureIndex = controller.getFiguren().IndexOf(figuur); // verkrijg figuur index uit lijst
                            ModifyingRectangle = controller.getFiguur(ModifyingFigureIndex).Positie; // verkrijg rectangle van object
                            ModifyingFigureType = controller.getFiguur(ModifyingFigureIndex).Type; // verkrijg soort figuur
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

            if (IsMoving)
            {
                IsMoving = false;
                controller.wijzigFiguur(MoveRectangle(ModifyingRectangle), ModifyingFigureIndex); // wijzig huidig object
            }
            else if (IsResizing)
            {
                IsResizing = false;
                controller.wijzigFiguur(ResizeRectangle(ModifyingRectangle), ModifyingFigureIndex);
            }
            else if (soortFiguur != Figuur.TekenModus.Default && soortFiguur != Figuur.TekenModus.Resize) // sla figuur alleen op in een tekenstate (square of ellipse)
            {
                controller.nieuwFiguur(GetRectangle(), soortFiguur); // maak nieuw figuur aan
            }
           
            Refresh(); // ververs drawpanel zodat het nieuwe figuur zichtbaar wordt
        }

        private void DrawPanel_Paint(object sender, PaintEventArgs e)
        {
            // verkrijg lijst met n figuren en print ieder figuur op het scherm
            foreach (var figuur in controller.getFiguren())
            {
                Draw(figuur.Type, figuur.Positie, figuur.Selectie, e);
            }

            // wanneer er nog getekend wordt, teken preview
            if (IsMouseDown)
            {
                Draw(soortFiguur, GetRectangle(), false, e);
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
    }
}
