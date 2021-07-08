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
        private enum States { Default, Square, Ellipse } // bepaalt de tekenoptie adhv constanten
        private States TekenState = States.Default; // zet standaard state

        Rectangle rectangle;
        Point startpos; // start positie X Y rechthoek
        Point endpos; // eind positie X Y rechthoek

        bool IsMouseDown = false; // bool wanneer muisknop vastgehouden wordt

        public Tekening()
        {
            InitializeComponent();
        }

        // optie op ellipsen te tekenen
        private void ButtonEllipse_Click(object sender, EventArgs e)
        {
            // verander naar tekencursor & state
            TekenState = States.Ellipse;
            Cursor = Cursors.Cross;
        }

        // optie om squares te tekenen
        private void ButtonSquare_Click(object sender, EventArgs e)
        {
            TekenState = States.Square;
            Cursor = Cursors.Cross;
        }

        // opties voor standaard pointer, voor selecties e.d.
        private void ButtonPointer_Click(object sender, EventArgs e)
        {
            TekenState = States.Default;
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
            if (IsMouseDown) // alleen uitvoeren wanneer muis ingehouden wordt
            {
                endpos = e.Location; 
                Refresh(); 
            }
        }

        // aangeroepen als muisknop losgelaten wordt
        private void DrawPanel_MouseUp(object sender, MouseEventArgs e)
        {
            IsMouseDown = false; // 

            if (IsMouseDown)
            {
                endpos = e.Location;
                IsMouseDown = false;
            }
        }

        private void DrawPanel_Paint(object sender, PaintEventArgs e)
        {
            // Print het figuur
            switch (TekenState)
            {
                case States.Square:
                    e.Graphics.DrawRectangle(Pens.Black, GetRectangle());
                    break;

                case States.Ellipse:
                    e.Graphics.DrawEllipse(Pens.Black, GetRectangle());
                    break;
            }
        }

        // Genereer het figuur
        private Rectangle GetRectangle()
        {
            rectangle = new Rectangle();
            rectangle.X = Math.Min(startpos.X, endpos.X);
            rectangle.Y = Math.Min(startpos.Y, endpos.Y);
            rectangle.Width = Math.Abs(startpos.X - endpos.X);
            rectangle.Height = Math.Abs(startpos.Y - endpos.Y);
            return rectangle;
        }
    }
}
