using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace CloudBase
{
  public class ServerDate
  {
    private int Offset;
    public ServerDate()
    {
      this.Offset = 0;
    }

    public ServerDate(int offset)
    {
      this.Offset = offset;
    }

    public JObject ToJSON()
    {
      JObject param = new JObject();
      
      param["$date"] = this.Offset;
      return param;
    }

  }
}