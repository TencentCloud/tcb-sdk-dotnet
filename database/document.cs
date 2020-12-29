using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace CloudBase
{
  public partial class Document
  {
    private Core Core;
    private string Id;
    private string Coll;
    private Dictionary<string, int> Projection;
    public Document(Core core, string coll, string docId, Dictionary<string, int> projection)
    {
      this.Core = core;
      this.Id = docId;
      this.Coll = coll;
      this.Projection = projection;
    }

    public DbCreateResponse Create(object data)
    {
      JObject dataJson = (JObject) (Serializer.EncodeData(data));
      Dictionary<string, dynamic> param = new Dictionary<string, dynamic>();

      param["data"] = dataJson;
      if (!DbUtils.IsNullOrEmptyString(this.Id))
      {
        param["_id"] = this.Id;
      }

      DbCreateResponse res = this.DocRequest<DbCreateResponse>("database.addDocument", param);

      return res;
    }

    public DbUpdateResponse Set(object data)
    {
      JObject dataJson = (JObject) (Serializer.EncodeData(data));
      Dictionary<string, dynamic> query = new Dictionary<string, dynamic>();
      Dictionary<string, dynamic> param = new Dictionary<string, dynamic>();

      query["_id"] = this.Id;
      param["query"] = query;
      param["data"] = dataJson;
      param["multi"] = false;
      param["merge"] = false;
      param["upsert"] = true;
      param["interfaceCallSource"] = "SINGLE_SET_DOC";

      DbUpdateResponse res = this.DocRequest<DbUpdateResponse>("database.updateDocument", param);

      return res;
    }

    public DbUpdateResponse Update(object data)
    {
      JObject dataJson = (JObject) (Serializer.EncodeData(data));
      Dictionary<string, dynamic> query = new Dictionary<string, dynamic>();
      Dictionary<string, dynamic> param = new Dictionary<string, dynamic>();

      query["_id"] = this.Id;
      param["query"] = query;
      param["data"] = dataJson;
      param["multi"] = false;
      param["merge"] = true;
      param["upsert"] = false;
      param["interfaceCallSource"] = "SINGLE_UPDATE_DOC";
      
      DbUpdateResponse res = this.DocRequest<DbUpdateResponse>("database.updateDocument", param);

      return res;
    }

    public DbRemoveResponse Remove()
    {
      Dictionary<string, dynamic> query = new Dictionary<string, dynamic>();
      Dictionary<string, dynamic> param = new Dictionary<string, dynamic>();

      query["_id"] = this.Id;
      param["query"] = query;
      param["multi"] = false;

      DbRemoveResponse res = this.DocRequest<DbRemoveResponse>("database.deleteDocument", param);

      return res;
    }

    public DbQueryResponse Get()
    {
      Dictionary<string, dynamic> query = new Dictionary<string, dynamic>();
      Dictionary<string, dynamic> param = new Dictionary<string, dynamic>();

      query["_id"] = this.Id;
      param["query"] = query;
      param["multi"] = false;
      param["projection"] = this.Projection;

      DbQueryResponse res = this.DocRequest<DbQueryResponse>("database.queryDocument", param);

      return res;
    }

    public Document Field(Dictionary<string, bool> projection)
    {
      Dictionary<string, int> newProjection = new Dictionary<string, int>();
      
      return new Document(this.Core, this.Coll, this.Id, newProjection);
    }

    private T DocRequest<T>(string action, Dictionary<string, dynamic> param)
    {
      param["collectionName"] = this.Coll;
      param["queryType"] = "WHERE";
      param["databaseMidTran"] = true;
      return this.Core.Request.Post<T>(action, param);
    }

  }
}