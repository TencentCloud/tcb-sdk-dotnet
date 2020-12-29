using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CloudBase
{
  public class Serializer
  {
    public static object EncodeData(object data)
    {
      string dataStr = JsonConvert.SerializeObject(data, Formatting.None, new CustomConverter());

      return JsonConvert.DeserializeObject(dataStr);
    }

    public static object DecodeData(object data)
    {
      return data;
    }

  }

  class CustomConverter : JsonConverter
  {
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
      if (value is QueryCommand || value is LogicCommand)
      {
        var command = value as LogicCommand;

        writer.WriteRawValue(command.ToJSON().ToString());
      }

      else if (value is UpdateCommand)
      {
        var command = value as UpdateCommand;

        writer.WriteRawValue(command.ToJSON().ToString());
      }

      if (value is ServerDate)
      {
        var date = value as ServerDate;

        writer.WriteRawValue(date.ToJSON().ToString());
      }

      if (value is DateTime)
      {
        var datetime = (DateTime) value;
        JObject param = new JObject();

        param["$date"] = (datetime.Ticks - ((new DateTime(1970, 1, 1)).ToLocalTime()).Ticks) / (10000);
        writer.WriteRawValue(param.ToString());
      }

      if (value is RegExp)
      {
        var regexp = value as RegExp;

        writer.WriteRawValue(regexp.ToJSON().ToString());
      }

      if (value is Point)
      {
        var point = value as Point;

        writer.WriteRawValue(point.ToJSON().ToString());
      }

      if (value is MultiPoint)
      {
        var points = value as MultiPoint;

        writer.WriteRawValue(points.ToJSON().ToString());
      }

      if (value is LineString)
      {
        var line = value as LineString;

        writer.WriteRawValue(line.ToJSON().ToString());
      }

      if (value is MultiLineString)
      {
        var lines = value as MultiLineString;

        writer.WriteRawValue(lines.ToJSON().ToString());
      }

      if (value is Polygon)
      {
        var polygon = value as Polygon;

        writer.WriteRawValue(polygon.ToJSON().ToString());
      }

      if (value is MultiPolygon)
      {
        var multiPolygon = value as MultiPolygon;
        
        writer.WriteRawValue(multiPolygon.ToJSON().ToString());
      }

    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
      throw new NotImplementedException("Unnecessary because CanRead is false. The type will skip the converter.");
    }

    public override bool CanRead
    {
      get { return false; }

    }

    public override bool CanConvert(Type objectType)
    {
      return typeof(QueryCommand) == objectType ||
        typeof(LogicCommand) == objectType ||
        typeof(UpdateCommand) == objectType ||
        typeof(ServerDate) == objectType ||
        typeof(DateTime) == objectType ||
        typeof(RegExp) == objectType ||
        typeof(Point) == objectType ||
        typeof(MultiPoint) == objectType ||
        typeof(LineString) == objectType ||
        typeof(MultiLineString) == objectType ||
        typeof(Polygon) == objectType ||
        typeof(MultiPolygon) == objectType;
    }

  }

}