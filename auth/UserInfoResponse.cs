using Newtonsoft.Json.Linq;

namespace CloudBase {
  class UserInfoResponse : Response {
    public readonly JObject Data;

    public UserInfoResponse(JObject res) : base(res) {
      if(string.IsNullOrEmpty(base.Code)) {
        this.Data = res["data"] as JObject;
      }
    }
  }
}