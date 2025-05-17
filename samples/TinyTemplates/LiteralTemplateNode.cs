using System;
using System.Collections.Generic;
using System.IO;

namespace TinyTemplates;

class LiteralTemplateNode : TemplateNode
{
  readonly string _text;

  public LiteralTemplateNode(string text)
  {
    ArgumentNullException.ThrowIfNull(text);
    _text = text;
  }

  public override void Execute(Stack<object> model, TextWriter output)
  {
    output.Write(_text);
  }
}
