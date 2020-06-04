using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace CloudBase {
  public class MultiLineString {

    private List<LineString> Lines;
    
    public MultiLineString(LineString[] lines) {
      if (lines.Length < 1) {
        throw new CloudBaseException(CloudBaseExceptionCode.INVALID_PARAM, "MultiLineString must contain 1 linestring at least");
      }

      this.Lines = new List<LineString>(lines);
    }

    public JObject ToJSON() {
      JArray lineArr = new JArray();
      foreach(var line in this.Lines) {
        var lineJson = line.ToJSON();
        lineArr.Add(lineJson["coordinates"]);
      }

      JObject param = new JObject();
      param["type"] = "MultiLineString";
      param["coordinates"] = lineArr;
      return param;
    }

    static public MultiLineString FromJSON(JArray coordinates) {
      List<LineString> lines = new List<LineString>();

      foreach(var line in coordinates) {
        lines.Add(LineString.FromJSON(line as JArray));
      }

      return new MultiLineString(lines.ToArray());
    }
  };
}