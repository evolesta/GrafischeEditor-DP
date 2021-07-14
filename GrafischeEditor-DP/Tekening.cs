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
        Figuur.soortenFiguren soortFiguur; // defineer het type figuur

        Rectangle rectangle; // tijdelijk figuur
        Point startpos; // start positie X Y rechthoek
        Point endpos; // eind positie X Y rechthoek

        bool IsMouseDown = false; // bool wanneer muisknop vastgehouden wordt
        bool IsMoving = false; // bool wanneer een bestaand figuur verplaatst wordt
        int ModifyingFigureIndex; // index waarde van aan te passen object bij resizing + moving
        Rectangle ModifyingRectangle; // rectangle die verplaatst of vergroot wordt

        Controller controller = new Controller(); // controller object

        public Tekening()
        {
            InitializeComponent();
        }

        // METHODEN -- //
        // Methode voor het printen van een figuur op het scherm
        private void Draw(Figuur.soortenFiguren type, Rectangle positie, bool selected, PaintEventArgs e)
        {
            Pen pen = generatePen(selected); // genereer nieuwe pen

            switch (type)
            {
                case Figuur.soortenFiguren.Square:
                    e.Graphics.DrawRectangle(pen, positie);
                    break;
                case Figuur.soortenFiguren.Ellipse:
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

        // -- Drawpanel mouse actions

        // optie op ellipsen te tekenen
        private void ButtonEllipse_Click(object sender, EventArgs e)
        {
            // verander naar tekencursor & state
            soortFiguur = Figuur.soortenFiguren.Ellipse;
            Cursor = Cursors.Cross;
        }

        // optie om squares te tekenen
        private void ButtonSquare_Click(object sender, EventArgs e)
        {
            soortFiguur = Figuur.soortenFiguren.Square;
            Cursor = Cursors.Cross;
        }

        // opties voor standaard pointer, voor selecties e.d.
        private void ButtonPointer_Click(object sender, EventArgs e)
        {
            soortFiguur = Figuur.soortenFiguren.Default;
            Cursor = Cursors.Default;
        }

        // aangeroepen als muisknop ingehouden wordt
        private void DrawPanel_MouseDown(object sender, MouseEventArgs e)
        {
            IsMouseDown = true;
            startpos = e.Location; // bewaar X Y positie startpunt

            foreach (var figuur in controller.getFiguren().ToList()) // doorloop alle figuren
            {
                // controleren of figuur zich in muispositie bevindt
                if (figuur.Positie.Contains(e.Location))
                {
                    IsMoving = true; // zet boolean op beweegmodus
                    ModifyingFigureIndex = controller.getFiguren().IndexOf(figuur); // verkrijg figuur index uit lijst
                    ModifyingRectangle = controller.getFiguur(ModifyingFigureIndex).Positie; // verkrijg rectangle van object
                }
            }
        }

        private void DrawPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (IsMouseDown || IsMoving) // alleen uitvoeren wanneer muisknop ingehouden wordt
            {
                endpos = e.Location; // eindpositie opslaan in pointer
                Refresh(); 
            }
        }

        // aangeroepen als muisknop losgelaten wordt
        private void DrawPanel_MouseUp(object sender, MouseEventArgs e)
        {
            IsMouseDown = false;  

            if (IsMouseDown)
            {
                endpos = e.Location;
                IsMouseDown = false;
            }

            if (IsMoving)
            {
                endpos = e.Location;
                IsMoving = false;
                controller.wijzigFiguur(MoveRectangle(ModifyingRectangle), ModifyingFigureIndex); // wijzig huidig object
            }
            else
            {
                controller.nieuwFiguur(GetRectangle(), soortFiguur); // maak nieuw figuur aan
            }
           
            Refresh(); // ververs drawpanel zodat het nieuwe figuur zichtbaar wordt
        }

        private void DrawPanel_Click(object sender, MouseEventArgs e)
        {
            // Alleen uitvoeren als de 'muis' state actief is (geen teken states)
            if (soortFiguur == Figuur.soortenFiguren.Default)
            {
                startpos = e.Location; // leg nieuwe coordinaten vast voor selectie

                foreach (var figuur in controller.getFiguren())
                {
                    // controleren of er zich een figuur bevindt in de opgeslagen coordinate
                    if (figuur.Positie.Contains(startpos))
                    {
                        int index = controller.getFiguren().IndexOf(figuur); // verkrijg obj index van list
                        controller.MaakSelectie(index); // pas selectie in object aan
                    }
                }
            }
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
                Draw(Figuur.soortenFiguren.Square, MoveRectangle(ModifyingRectangle), true, e);
            }
        }

        private void ButtonRemove_Click(object sender, EventArgs e)
        {
            // verwijder ieder geselecteerd object uit de controlle klasse
            foreach (var figuur in controller.getFiguren().ToList())
            {
                if (figuur.Selectie) // verwijder alleen de geselecteerde objecten
                {
                    int index = controller.getFiguren().IndexOf(figuur); // verkrijg index uit list
                    controller.verwijderFiguur(index); // verwijder object uit controller list
                }
            }
            Refresh(); // repaint the drawing
        }
    }
}
