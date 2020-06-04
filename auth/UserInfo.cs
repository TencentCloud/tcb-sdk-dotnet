using Newtonsoft.Json.Linq;

namespace CloudBase {
  public class UserInfo {

    // 用户在云开发的唯一ID
    public readonly string Uuid;

    // 用户使用的云开发环境
    public readonly string Env;

    // 用户登录类型
    public readonly string LoginType;

    // 微信(开放平台或公众平台)应用appid
    public readonly string Appid;

    // 当前用户在微信(开放平台或公众平台)应用的openid
    public readonly string Openid;

    // 用户昵称
    public readonly string NickName;

    // 用户性别，male(男)或female(女)
    public readonly string Gender;

    // 用户所在国家
    public readonly string Country;

    // 用户所在省份
    public readonly string Province;

    // 用户所在城市
    public readonly string City;

    // 用户头像链接
    public readonly string AvatarUrl;

    public UserInfo(JObject data) {
      Uuid = (string)data["uuid"];
      Env = (string)data["envName"];
      LoginType = (string)data["loginType"];
      Appid = (string)data["appid"];
      Openid = (string)data["openid"];
      NickName = (string)data["nickName"];
      Gender = (string)data["gender"];
      Country = (string)data["country"];
      Province = (string)data["province"];
      City = (string)data["city"];
      AvatarUrl = (string)data["avatarUrl"];
    }
    
  }
}