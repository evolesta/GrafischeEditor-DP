using System;
using System.Windows.Forms;
using GrafischeEditor_DP.CommandPattern;
using GrafischeEditor_DP.CommandPattern.Commands;
using GrafischeEditor_DP.DecoratorPattern;

namespace GrafischeEditor_DP
{
  internal class EditLabelForm : Form
  {
    private readonly IComponent _originalComponent;
    private readonly Groep _parent;
    private readonly Invoker _invoker;

    public EditLabelForm(IComponent originalComponent, Groep parent, Invoker invoker)
    {
      _originalComponent = originalComponent;
      _parent = parent;
      _invoker = invoker;

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
      var cmd = new SetLabelCommand(TextBox.Text, dir, _originalComponent, _parent);
      _invoker.SetCommand(cmd);
      _invoker.Execute();

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
