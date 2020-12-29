using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace CloudBase
{
  public partial class Query
  {
    protected Core Core;
    protected string Coll;
    private object FieldFilters;
    private List<dynamic> FieldOrders;
    private int LimitCount;
    private int Offset;
    private Dictionary<string, int> Projection;
    public Query(Core core, string coll, object fieldFilters, List<dynamic> fieldOrders, int limitCount, int offset, Dictionary<string, int> projection)
    {
      this.Core = core;
      this.Coll = coll;
      this.FieldFilters = fieldFilters;
      this.FieldOrders = fieldOrders;
      this.LimitCount = limitCount;
      this.Offset = offset;
      this.Projection = projection;
    }

    public Query(Core core, string coll)
    {
      List<dynamic> fieldOrders = new List<dynamic>();
      Dictionary<string, int> projection = new Dictionary<string, int>();
      this.Core = core;
      this.Coll = coll;
      this.FieldFilters = null;
      this.FieldOrders = fieldOrders;
      this.LimitCount = 0;
      this.Offset = 0;
      this.Projection = projection;
    }

    public DbQueryResponse Get()
    {
      Dictionary<string, dynamic> param = new Dictionary<string, dynamic>();
      if (!DbUtils.IsNullObject(this.FieldFilters))
      {
        param["query"] = this.FieldFilters;
      }

      if (!DbUtils.IsNullOrEmptyArray(this.FieldOrders))
      {
        param["order"] = this.FieldOrders;
      }

      if (this.LimitCount != 0)
      {
        param["limit"] = this.LimitCount < 1000 ? this.LimitCount : 1000;
      }

      else
      {
        this.LimitCount = 100;
      }

      if (this.Offset != 0)
      {
        param["offset"] = this.Offset;
      }

      if (!DbUtils.IsNullOrEmptyMap(this.Projection))
      {
        param["projection"] = this.Projection;
      }

      DbQueryResponse res = this.QueryRequest<DbQueryResponse>("database.queryDocument", param);

      return res;
    }

    public DbCountResponse Count()
    {
      Dictionary<string, dynamic> param = new Dictionary<string, dynamic>();

      param["query"] = this.FieldFilters;

      DbCountResponse res = this.QueryRequest<DbCountResponse>("database.countDocument", param);

      return res;
    }

    public DbUpdateResponse Update(object data)
    {
      JObject dataJson = (JObject) (Serializer.EncodeData(data));
      Dictionary<string, dynamic> param = new Dictionary<string, dynamic>();

      param["query"] = this.FieldFilters;
      param["muti"] = true;
      param["merge"] = true;
      param["upsert"] = false;
      param["data"] = dataJson;
      param["interfaceCallSource"] = "BATCH_UPDATE_DOC";

      DbUpdateResponse res = this.QueryRequest<DbUpdateResponse>("database.updateDocument", param);

      return res;
    }

    public DbRemoveResponse Remove()
    {
      Dictionary<string, dynamic> param = new Dictionary<string, dynamic>();

      param["query"] = this.FieldFilters;
      param["multi"] = true;

      DbRemoveResponse res = this.QueryRequest<DbRemoveResponse>("database.deleteDocument", param);

      return res;
    }

    public Query Where(object query)
    {
      JObject queryJson = (JObject) (Serializer.EncodeData(query));
      
      return new Query(this.Core, this.Coll, queryJson, this.FieldOrders, this.LimitCount, this.Offset, this.Projection);
    }

    public Query OrderBy(string fieldPath, string directionStr)
    {
      Dictionary<string, string> newOrder = new Dictionary<string, string>();

      newOrder["field"] = fieldPath;
      newOrder["direction"] = directionStr;
      List<dynamic> newFieldOrders = new List<dynamic>();
      foreach (var item in this.FieldOrders)
      {
        newFieldOrders.Add(item);
      }

      newFieldOrders.Add(newOrder);
      return new Query(this.Core, this.Coll, this.FieldFilters, newFieldOrders, this.LimitCount, this.Offset, this.Projection);
    }

    public Query Limit(int limitCount)
    {
      return new Query(this.Core, this.Coll, this.FieldFilters, this.FieldOrders, limitCount, this.Offset, this.Projection);
    }

    public Query Skip(int offset)
    {
      return new Query(this.Core, this.Coll, this.FieldFilters, this.FieldOrders, this.LimitCount, offset, this.Projection);
    }

    public Query Field(Dictionary<string, bool> projection)
    {
      Dictionary<string, int> newProjection = new Dictionary<string, int>();
      foreach (var item in projection)
      {
        newProjection[item.Key] = (item.Value == true ? 1 : 0);
      }

      return new Query(this.Core, this.Coll, this.FieldFilters, this.FieldOrders, this.LimitCount, this.Offset, newProjection);
    }

    private T QueryRequest<T>(string action, Dictionary<string, dynamic> param)
    {
      param["collectionName"] = this.Coll;
      param["queryType"] = "WHERE";
      param["databaseMidTran"] = true;
      return this.Core.Request.Post<T>(action, param);
    }

  }
}