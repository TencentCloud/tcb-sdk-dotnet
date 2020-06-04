namespace CloudBase {
	public class Geo {
    public Point Point(float longitude, float latitude) {
      return new Point(longitude, latitude);
    }

    public MultiPoint MultiPoint(Point[] points) {
      return new MultiPoint(points);
    }

    public LineString LineString(Point[] points) {
      return new LineString(points);
    }

    public MultiLineString MultiLineString(LineString[] lines) {
      return new MultiLineString(lines);
    }

    public Polygon Polygon (LineString[] lines) {
      return new Polygon(lines);
    }

    public MultiPolygon MultiPolygon (Polygon[] polygons) {
      return new MultiPolygon(polygons);
    }
  }
}