using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace CloudBase
{
  public class LogicCommand
  {
    private List<List<dynamic>> Actions;
    public LogicCommand(List<List<dynamic>> actions, List<dynamic> step)
    {
      this.Actions = actions;
      actions.Add(step);
    }

    public LogicCommand And(LogicCommand[] expressions)
    {
      return this.LogicOp(LogicCommandLiteral.AND, new List<LogicCommand>(expressions));
    }

    public LogicCommand And(LogicCommand expression)
    {
      List<LogicCommand> expressions = new List<LogicCommand>();

      expressions.Add(expression);
      return this.LogicOp(LogicCommandLiteral.AND, expressions);
    }

    public LogicCommand Or(LogicCommand[] expressions)
    {
      return this.LogicOp(LogicCommandLiteral.OR, new List<LogicCommand>(expressions));
    }

    public LogicCommand Or(LogicCommand expression)
    {
      List<LogicCommand> expressions = new List<LogicCommand>();

      expressions.Add(expression);
      return this.LogicOp(LogicCommandLiteral.OR, expressions);
    }

    public JObject ToJSON()
    {
      JObject jsonMap = new JObject();

      jsonMap["_actions"] = (JArray) (Serializer.EncodeData(this.Actions));
      return jsonMap;
    }

    private LogicCommand LogicOp(string operation, List<LogicCommand> expressions)
    {
      List<dynamic> step = new List<dynamic>();

      step.Add("$" + operation);
      foreach (var expression in expressions)
      {
        step.Add(expression);
      }

      List<List<dynamic>> actions = new List<List<dynamic>>();

      LogicCommand command = new LogicCommand(actions, step);
      
      return command;
    }

  }
}