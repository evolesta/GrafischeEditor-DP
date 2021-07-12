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

        Controller controller = new Controller(); // controller object

        public Tekening()
        {
            InitializeComponent();
        }

        // METHODEN -- //
        // Methode voor het printen van een figuur op het scherm
        private void Draw(Figuur.soortenFiguren type, Rectangle positie, PaintEventArgs e)
        {
            switch (type)
            {
                case Figuur.soortenFiguren.Square:
                    e.Graphics.DrawRectangle(Pens.Black, positie);
                    break;
                case Figuur.soortenFiguren.Ellipse:
                    e.Graphics.DrawEllipse(Pens.Black, positie);
                    break;
            }
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

            if (IsMouseDown)
            {
                endpos = e.Location;
                IsMouseDown = false;
            }

            controller.nieuwFiguur(GetRectangle(), soortFiguur); // maak nieuw figuur aan
            Refresh(); // ververs drawpanel zodat het nieuwe figuur zichtbaar wordt
        }

        private void DrawPanel_Click(object sender, MouseEventArgs e)
        {
            // Alleen uitvoeren als de 'muis' state actief is (geen teken states)
            if (soortFiguur == Figuur.soortenFiguren.Default)
            {
                startpos = e.Location; // leg nieuwe coordinaten vast voor selectie

                // controleren of een figuur op de geklikte positie bestaat
                foreach (var figuur in controller.getFiguren())
                {
                    // controleren of er zich een figuur bevindt in de opgeslagen coordinaten
                    if (figuur.Positie.Contains(startpos))
                    {
                        MessageBox.Show("test");
                    }
                }
            }
        }

        private void DrawPanel_Paint(object sender, PaintEventArgs e)
        {
            // verkrijg lijst met n figuren en print ieder figuur op het scherm
            foreach (var figuur in controller.getFiguren())
            {
                Draw(figuur.Type, figuur.Positie, e);
            }

            // wanneer er nog getekend wordt, teken preview
            if (IsMouseDown)
            {
                Draw(soortFiguur, GetRectangle(), e);
            }
        }
    }
}
