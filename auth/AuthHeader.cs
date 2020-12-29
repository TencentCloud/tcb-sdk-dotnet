using Newtonsoft.Json.Linq;

namespace CloudBase
{
  public class AuthHeader
  {
    private readonly JObject header;

    public AuthHeader()
    {
      this.header = new JObject();
    }

    public void SetHeader(string key, string value)
    {
      this.header[key] = value;
    }

    public JObject ToJSON()
    {
      return this.header;
    }
  }
}