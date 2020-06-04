using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace CloudBase {
 public class DbQueryResponse: Response {
  public JArray Data;
  public int Limit;
  public int Offset;
  public DbQueryResponse(JObject res): base(res) {
   if (DbUtils.IsNullOrEmptyString(this.Code)) {
    this.Data = (JArray)(res["data"]["list"]);
    this.Limit = (int)(res["data"]["limit"]);
    this.Offset = (int)(res["data"]["offset"]);
   }
  }
 };
 public class DbUpdateResponse: Response {
  public string UpsertedId;
  public int Updated;
  public DbUpdateResponse(JObject res): base(res) {
   if (DbUtils.IsNullOrEmptyString(this.Code)) {
    this.Updated = (int)(res["data"]["updated"]);
    this.UpsertedId = (string)(res["data"]["upserted_id"]);
   }
  }
 };
 public class DbRemoveResponse: Response {
  public int Deleted;
  public DbRemoveResponse(JObject res): base(res) {
   if (DbUtils.IsNullOrEmptyString(this.Code)) {
    this.Deleted = (int)(res["data"]["deleted"]);
   }
  }
 };
 public class DbCreateResponse: Response {
  public string Id;
  public DbCreateResponse(JObject res): base(res) {
   if (DbUtils.IsNullOrEmptyString(this.Code)) {
    this.Id = (string)(res["data"]["_id"]);
   }
  }
 };
 public class DbCountResponse: Response {
  public int Total;
  public DbCountResponse(JObject res): base(res) {
   if (DbUtils.IsNullOrEmptyString(this.Code)) {
    this.Total = (int)(res["data"]["total"]);
   }
  }
 };
}