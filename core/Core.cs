namespace CloudBase
{
  public class Core
  {

    public readonly string Env;

    public readonly int Timeout;

    public readonly Request Request;

    public Auth Auth;

    public Core(string env, int timeout)
    {
      this.Env = env;
      this.Timeout = timeout;
      this.Request = new Request(this);
    }

    public void SetAuthInstance(Auth auth)
    {
      this.Auth = auth;
    }
  }
}