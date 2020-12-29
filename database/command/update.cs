using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace CloudBase
{
  public class UpdateCommand
  {
    private List<List<dynamic>> Actions;
    public UpdateCommand(List<List<dynamic>> actions, List<dynamic> step)
    {
      this.Actions = actions;
      actions.Add(step);
    }

    public JObject ToJSON()
    {
      JObject jsonMap = new JObject();

      jsonMap["_actions"] = (JArray) (Serializer.EncodeData(this.Actions));
      return jsonMap;
    }

  }
}