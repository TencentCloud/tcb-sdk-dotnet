using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace CloudBase {
  public class MultiPolygon {

    private List<Polygon> Polygons;
    
    public MultiPolygon(Polygon[] polygons) {
      if (polygons.Length < 1) {
        throw new CloudBaseException(CloudBaseExceptionCode.INVALID_PARAM, "MultiPolygon must contain 1 polygon at least");
      }

      this.Polygons = new List<Polygon>(polygons);
    }

    public JObject ToJSON() {
      JArray polygonArr = new JArray();
      foreach(var polygon in this.Polygons) {
        var polygonJson = polygon.ToJSON();
        polygonArr.Add(polygonJson["coordinates"]);
      }

      JObject param = new JObject();
      param["type"] = "MultiPolygon";
      param["coordinates"] = polygonArr;
      return param;
    }

    static public MultiPolygon FromJSON(JArray coordinates) {
      List<Polygon> polygons = new List<Polygon>();

      foreach(var polygon in coordinates) {
        polygons.Add(Polygon.FromJSON(polygon as JArray));
      }

      return new MultiPolygon(polygons.ToArray());
    }
  };
}