using System;
using System.Linq;
using GrafischeEditor_DP.DecoratorPattern;

namespace GrafischeEditor_DP.CommandPattern.Commands
{
  internal class SetLabelCommand : ICommand
  {
    private bool _isNewDirection;
    private string _originalText;

    private readonly string _text;
    private readonly LabelDirection _direction;
    private readonly IComponent _originalComponent;
    private readonly Groep _parent;

    public SetLabelCommand(string text, LabelDirection direction, IComponent originalComponent, Groep parent)
    {
      _text = text;
      _direction = direction;
      _originalComponent = originalComponent;
      _parent = parent;
    }

    public void Execute()
    {
      if (_originalComponent is LabeledComponent labeledComponent &&
          labeledComponent.TryGetLabel(_direction, out var newLabeledComponent))
      {
        _originalText = newLabeledComponent.Text;
      }
      else
      {
        //No label present in the given direction, so we create a new one. 
        newLabeledComponent = _direction switch
        {
          LabelDirection.Left => new LeftLabeledComponent(_originalComponent),
          LabelDirection.Right => new RightLabeledComponent(_originalComponent),
          LabelDirection.Top => new TopLabeledComponent(_originalComponent),
          LabelDirection.Bottom => new BottomLabeledComponent(_originalComponent),
          _ => throw new ArgumentOutOfRangeException()
        };
        _isNewDirection = true;
      }

      newLabeledComponent.Text = _text;

      if (_isNewDirection && _parent.Children.Remove(_originalComponent))
        _parent.Children.Add(newLabeledComponent);
    }

    public void Undo()
    {
      var fromParent = _parent.Children.First(c => c.Id == _originalComponent.Id);

      if (_isNewDirection)
      {
        if (_parent.Children.Remove(fromParent))
        {
          _parent.Children.Add(_originalComponent);
        }
      }
      else
      {
        var label = fromParent as LabeledComponent;
        label!.Text = _originalText;
      }
    }
  }
}
