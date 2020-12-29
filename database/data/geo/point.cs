using Newtonsoft.Json.Linq;

namespace CloudBase
{
  public class Point
  {

    // 纬度 [-90, 90]
    private float Longitude;

    // 经度 [-100, 100]

    private float Latitude;

    public Point(float longitude, float latitude)
    {
      this.Longitude = longitude;
      this.Latitude = latitude;
    }

    public JObject ToJSON()
    {
      JObject param = new JObject();
      
      param["type"] = "Point";
      param["coordinates"] = new JArray() { this.Longitude, this.Latitude };
      return param;
    }

    static public Point FromJSON(JArray coordinates)
    {
      return new Point((float) coordinates[0], (float) coordinates[1]);
    }

  }
}