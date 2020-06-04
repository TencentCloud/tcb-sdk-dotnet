using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace CloudBase {
  public class Function {
    private Core core;

    public Function(Core core) {
      this.core = core;
    }

    public async Task<FunctionResponse> CallFunctionAsync(string name, object param) {
      if (string.IsNullOrEmpty(name)) {
        throw new CloudBaseException(CloudBaseExceptionCode.EMPTY_PARAM, "函数名不能为空");
      }


      Dictionary<string, dynamic> callParam = new Dictionary<string, dynamic>();
      callParam.Add("function_name", name);
      
      if (param != null) {
        callParam.Add("request_data", JsonConvert.SerializeObject(param));
      }

      FunctionResponse res = await this.core.Request.PostAsync<FunctionResponse>("functions.invokeFunction", callParam);

      return res;
    }
  }
}