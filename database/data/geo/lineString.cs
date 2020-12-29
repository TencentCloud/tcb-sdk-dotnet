using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace CloudBase
{
  public class LineString
  {

    private List<Point> Points;

    public LineString(Point[] points)
    {
      if (points.Length < 2)
      {
        throw new CloudBaseException(CloudBaseExceptionCode.INVALID_PARAM, "LineString must contain 2 points at least");
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
      
      param["type"] = "LineString";
      param["coordinates"] = pointArr;
      return param;
    }

    static public LineString FromJSON(JArray coordinates)
    {
      List<Point> points = new List<Point>();

      foreach (var point in coordinates)
      {
        points.Add(Point.FromJSON(point as JArray));
      }

      return new LineString(points.ToArray());
    }

  }
}