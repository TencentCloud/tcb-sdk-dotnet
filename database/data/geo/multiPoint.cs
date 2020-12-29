using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace CloudBase
{
  public class MultiPoint
  {

    private List<Point> Points;

    public MultiPoint(Point[] points)
    {
      if (points.Length < 1)
      {
        throw new CloudBaseException(CloudBaseExceptionCode.INVALID_PARAM, "MultiPoint must contain 1 point at least");
      }

      this.Points = new List<Point>(points);
    }

    public JObject ToJSON()
    {
      JArray pointArr = new JArray();
      foreach (var point in this.Points)
      {
        var pointJson = point.ToJSON();
        pointArr.Add(pointJson["coordinates"]);
      }

      JObject param = new JObject();
      
      param["type"] = "MultiPoint";
      param["coordinates"] = pointArr;
      return param;
    }

    static public MultiPoint FromJSON(JArray coordinates)
    {
      List<Point> points = new List<Point>();

      foreach (var point in coordinates)
      {
        points.Add(Point.FromJSON(point as JArray));
      }

      return new MultiPoint(points.ToArray());
    }

  }
}