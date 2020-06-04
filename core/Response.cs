using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace CloudBase {

  public class Response {
    public readonly string Code;
    public readonly string Message;
    public readonly string RequestId;

    public Response(JObject res) {
      if (res.ContainsKey("code")) {
        this.Code = (string)res["code"];
        this.Message = (string)res["message"];
      }
      if (res.ContainsKey("requestId")) {
        this.RequestId = (string)res["requestId"];
      }
    }
  }

}