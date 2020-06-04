using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CloudBase {
  public class FunctionResponse : Response {
    public readonly JObject Data;

    public FunctionResponse(JObject res) : base(res) {
      if(string.IsNullOrEmpty(base.Code)) {
        try {
          // 尝试解析响应值
          string responseData = (string)res["data"]["response_data"]; 
          this.Data =  JsonConvert.DeserializeObject(responseData) as JObject;
        } catch (Exception e) {
          this.Data = res["data"] as JObject;
        }
      }
    }
  }
}