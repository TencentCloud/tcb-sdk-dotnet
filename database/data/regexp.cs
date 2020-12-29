using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace CloudBase
{
  public class RegExp
  {
    private string Regexp;
    private string Options;
    public RegExp(string regexp, string options)
    {
      this.Regexp = regexp;
      this.Options = options;
    }

    public JObject ToJSON()
    {
      JObject param = new JObject();
      
      param["$regexp"] = this.Regexp;
      param["$options"] = this.Options;
      return param;
    }

  }
}