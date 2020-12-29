using System;

namespace CloudBase
{
  public class CloudBaseException : ApplicationException
  {
    public readonly string Code;

    public CloudBaseException(string code, string message) : base(message)
    {
      this.Code = code;
    }
  }

  class CloudBaseExceptionCode
  {
    static public readonly string EMPTY_PARAM = "EMPTY_PARAM";

    static public readonly string INVALID_PARAM = "INVALID_PARAM";

    static public readonly string NETWORK_ERR = "NETWORK_ERR";

    static public readonly string NULL_RESPONES = "NULL_RESPONES";

    // 响应结果解析失败
    static public readonly string RESPONSE_PRASE_ERR = "RESPONSE_PRASE_ERR";

    static public readonly string IO_ERR = "IO_ERR";

    static public readonly string INVALID_FIELD_PATH = "INVALID_FIELD_PATH";

    static public readonly string FILE_NOT_EXIST = "FILE_NOT_EXIST";

    static public readonly string STORAGE_REQUEST_FAIL = "STORAGE_REQUEST_FAIL";

    static public readonly string DATABASE_REQUEST_FAILED = "DATABASE_REQUEST_FAILED";

    static public readonly string INVALID_CUSTOM_LOGIN_TICKET = "INVALID_CUSTOM_LOGIN_TICKET";

    // 获取授权失败
    static public readonly string AUTH_FAILED = "AUTH_FAILED";

    // 登出失败
    static public readonly string SIGN_OUT_FAILED = "SIGN_OUT_FAILED";

    // 匿名转化失败
    static public readonly string LINK_AND_RETRIEVE_DATA_FAILED = "LINK_AND_RETRIEVE_DATA_FAILED";

    // refreshToken 过期
    static public readonly string REFRESH_TOKEN_EXPIRED = "REFRESH_TOKEN_EXPIRED";

    // 不合法的 refreshToken
    static public readonly string INVALID_REFRESH_TOKEN = "INVALID_REFRESH_TOKEN";

    // accessToken 过期 或 无效
    static public readonly string CHECK_LOGIN_FAILED = "CHECK_LOGIN_FAILED";

    // 没有 refreshToken 信息
    static public readonly string NOT_LOGIN = "NOT_LOGIN";

    // 无效的 GEO 数据
    static public readonly string INVALID_GEO_DAT = "INVALID_GEO_DAT";
  }
}