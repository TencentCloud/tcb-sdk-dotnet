namespace CloudBase
{
  public class CloudBaseApp
  {

    private static CloudBaseApp Instance;
    private readonly Core Core;
    public readonly Auth Auth;

    public readonly Function Function;

    public readonly Storage Storage;

    public readonly Database Db;

    private CloudBaseApp(string env, int timeout)
    {
      this.Core = new Core(env, timeout);
      this.Auth = new Auth(this.Core);
      this.Function = new Function(this.Core);
      this.Storage = new Storage(this.Core);
      this.Db = new Database(this.Core);
    }

    public static CloudBaseApp Init(string env, int timeout)
    {
      if (Instance == null)
      {
        Instance = new CloudBaseApp(env, timeout);
      }

      return Instance;
    }
  }
}