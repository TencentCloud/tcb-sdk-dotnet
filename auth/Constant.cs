namespace CloudBase {
  class AuthCacheKey {
    public static readonly string ACCESS_TOKEN = "access_token";
    public static readonly string ACCESS_TOKEN_EXPIRE = "access_token_expire";
    public static readonly string REFRESH_TOKEN = "refresh_token";
    public static readonly string ANONYMOUS_UUID = "anonymous_uuid";
    public static readonly string LOGIN_TYPE = "login_type";
  }

  public class AuthType {
    public static readonly string CUSTOM = "custom";
    public static readonly string ANONYMOUS = "anonymous";
    public static readonly string NO_AUTH = "no_auth";

  }
}