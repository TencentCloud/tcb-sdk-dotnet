using Newtonsoft.Json.Linq;

namespace CloudBase {
  class AuthResponse : Response {
    public readonly JObject Data;

    public AuthResponse(JObject res) : base(res) {
      if(string.IsNullOrEmpty(base.Code)) {
        this.Data = res;
      }
    }
  }
}