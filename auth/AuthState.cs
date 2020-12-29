namespace CloudBase
{
  public class AuthState
  {
    public readonly string AuthType;
    public readonly string RefreshToken;
    public readonly string AccessToken;

    public AuthState(string authType, string refreshToken, string accessToken)
    {
      this.AuthType = authType;
      this.RefreshToken = refreshToken;
      this.AccessToken = accessToken;
    }
  }
}