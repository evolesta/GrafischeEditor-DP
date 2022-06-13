using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using GrafischeEditor_DP.VisitorPattern;

namespace GrafischeEditor_DP
{
  public class Groep : IComponent
  {
    public string Name { get; set; }
    public ComponentType ComponentType => ComponentType.Groep;
    public int Id { get; set; }
        
    public bool Selected { get; set; }
    public List<IComponent> Children = new();

    public IEnumerable<IComponent> Figuren => Children
      .Where(c => c.ComponentType == ComponentType.Figuur);

    public IEnumerable<IComponent> Groepen => Children
      .Where(c => c.ComponentType == ComponentType.Groep);

    public Rectangle Placement { get => GetRectangle(); set => throw new InvalidOperationException(); }

    private Rectangle GetRectangle()
    {
      var figures = AllFiguresFlattened().ToArray();
      if(!figures.Any())
        return Rectangle.Empty;


      var X = figures.Min(f => f.Placement.X);
      var Y = figures.Min(f => f.Placement.Y);
      var Width = figures.Max(f => f.Placement.X + f.Placement.Width) - X;
      var Heigth = figures.Max(f => f.Placement.Y + f.Placement.Height) - Y;

      return new Rectangle(X, Y, Width, Heigth);
    }

    public void Accept(IVisitor visitor)
    {
      visitor.VisitGroup(this);
    }


    public IEnumerable<IComponent> AllFiguresFlattened() =>
      Figuren.Concat(Groepen.Select(c => c.InnerComponent() as Groep).SelectMany(g => g.AllFiguresFlattened()));
        
    public void Draw(PaintEventArgs e, Rectangle? preview = null)
    {
      //if (Selected)
      //{
      var pen = new Pen(Color.Green);
      pen.DashStyle = DashStyle.Dot;
      e.Graphics.DrawRectangle(pen, preview ?? Placement);
      //}

      foreach (var component in Children)
        component.Draw(e, preview);
    }

    public IComponent InnerComponent() => this;
  }
}