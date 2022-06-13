using System.Drawing;
using System.Windows.Forms;
using GrafischeEditor_DP.StrategyPattern;
using GrafischeEditor_DP.VisitorPattern;

namespace GrafischeEditor_DP
{
  /// <summary>
  /// De Component klasse representeert een enkel figuur die getekend kan worden
  /// </summary>
  public class Figuur : IComponent
  {
    public string Name { get; set; }
    public ComponentType ComponentType => ComponentType.Figuur;
    public int Id { get; set; }
    public Rectangle Placement { get; set; }
    public bool Selected { get; set; }
    public FiguurType FiguurType { get; set; }
    public IFigureStrategy Strategy => FiguurType switch
    {
      FiguurType.Ellipse => Strategies.EllipseStrategy,
      FiguurType.Rectangle => Strategies.RectangleStrategy,
      _ => throw new System.NotImplementedException()
    }; 
	
    public void Draw(PaintEventArgs e, Rectangle? preview = null) => Strategy.Draw(e, Selected, preview ?? Placement);
    public IComponent InnerComponent() => this;

    public void Accept(IVisitor visitor)
    {
      visitor.VisitFigure(this);
    }
  }


  public enum FiguurType
  {
    Rectangle,
    Ellipse
  }
}