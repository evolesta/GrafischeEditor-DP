using System;
using System.Linq;
using GrafischeEditor_DP.DecoratorPattern;

namespace GrafischeEditor_DP.CommandPattern.Commands
{
  internal class SetLabelCommand : ICommand
  {
    private readonly bool _hasExistingLabelInDirection;
    private readonly string _originalText;
    private readonly string _text;
    private readonly LabelDirection _direction;
    private readonly IComponent _originalComponent;
    private readonly Groep _parent;

    public SetLabelCommand(string text, LabelDirection direction, IComponent originalComponent, Groep parent)
    {
      _text = text;
      _direction = direction;
      if (originalComponent is LabeledComponent labeledComponent &&
          labeledComponent.TryGetLabel(_direction, out var originalLabel))
      {
        _hasExistingLabelInDirection = true;
        _originalText = originalLabel.Text;
        _originalComponent = originalLabel;
      }
      else
      {
        _originalComponent = originalComponent;
      }
      _parent = parent;
    }

    public void Execute()
    {
      if (_hasExistingLabelInDirection)
      {
        (_originalComponent as LabeledComponent)!.Text = _text;
      }
      else
      {
        // No label present in the given direction, so we create a new one ...
        LabeledComponent newLabel = _direction switch
        {
          LabelDirection.Left => new LeftLabeledComponent(_originalComponent),
          LabelDirection.Right => new RightLabeledComponent(_originalComponent),
          LabelDirection.Top => new TopLabeledComponent(_originalComponent),
          LabelDirection.Bottom => new BottomLabeledComponent(_originalComponent),
          _ => throw new ArgumentOutOfRangeException()
        };
        newLabel.Text = _text;

        // ... and replace the original in the containing group. 
        if (_parent.Children.Remove(_originalComponent))
          _parent.Children.Add(newLabel);
      }
    }

    public void Undo()
    {
      var fromParent = _parent.Children.First(c => c.Id == _originalComponent.Id);

      if (_hasExistingLabelInDirection)
      {
        (_originalComponent as LabeledComponent)!.Text = _originalText;
      }
      else
      {
        if (_parent.Children.Remove(fromParent))
          _parent.Children.Add(_originalComponent);
      }
    }
  }
}
