using System;
using System.Windows.Forms;
using GrafischeEditor_DP.DecoratorPattern;

namespace GrafischeEditor_DP
{
  internal class EditLabelForm : Form
  {
    private readonly IComponent _originalComponent;
    private readonly Groep _parent;

    public EditLabelForm(IComponent originalComponent, Groep parent)
    {
      _originalComponent = originalComponent;
      _parent = parent;

      Text = "Label toevoegen";

      DirectionSelector = new ComboBox { Text = "selecteer een optie", Left = 50, Top = 20 };
      DirectionSelector.Items.Add("links");
      DirectionSelector.Items.Add("rechts");
      DirectionSelector.Items.Add("boven");
      DirectionSelector.Items.Add("onder");

      TextBox = new TextBox { Text = "Labeltekst", Left = 50, Top = 50 };

      SubmitButton = new Button() { Text = "Toevoegen", Left = 50, Top = 80 };
      SubmitButton.Click += SubmitLabelButtonClick;

      Controls.Add(DirectionSelector);
      Controls.Add(TextBox);
      Controls.Add(SubmitButton);

      AcceptButton = SubmitButton;
    }

    private void SubmitLabelButtonClick(object? sender, EventArgs e)
    {
      var dir = (LabelDirection)DirectionSelector.SelectedIndex;

      LabeledComponent labeledComponent = dir switch
      {
        LabelDirection.Left => new LeftLabeledComponent(_originalComponent),
        LabelDirection.Right => new RightLabeledComponent(_originalComponent),
        LabelDirection.Top => new TopLabeledComponent(_originalComponent),
        LabelDirection.Bottom => new BottomLabeledComponent(_originalComponent),
        _ => throw new ArgumentOutOfRangeException()
      };

      labeledComponent.Text = TextBox.Text;

      if(_parent.Children.Remove(_originalComponent))
        _parent.Children.Add(labeledComponent);

      Close();
    }


    public ComboBox DirectionSelector { get; set; }
    public TextBox TextBox { get; set; }
    public Button SubmitButton { get; set; }

    
  }

  public enum LabelDirection
  {
    Left, Right, Top, Bottom
  }
}
