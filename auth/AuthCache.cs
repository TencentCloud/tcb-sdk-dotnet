using System.Collections.Generic;
using System.Threading.Tasks;

// todo: 持久化cache
namespace CloudBase
{
  class AuthCache
  {
    public readonly string AccessTokenKey;
    public readonly string AccessTokenExpireKey;
    public readonly string RefreshTokenKey;
    public readonly string AnonymousUuidKey;
    public readonly string LoginTypeKey;

    private Dictionary<string, string> MCache;

    public AuthCache(Core core)
    {
      string envId = core.Env;

      this.AccessTokenKey = $"{envId}_{AuthCacheKey.ACCESS_TOKEN}";
      this.AccessTokenExpireKey = $"{envId}_{AuthCacheKey.ACCESS_TOKEN_EXPIRE}";
      this.RefreshTokenKey = $"{envId}_{AuthCacheKey.REFRESH_TOKEN}";
      this.AnonymousUuidKey = $"{envId}_{AuthCacheKey.ANONYMOUS_UUID}";
      this.LoginTypeKey = $"{envId}_{AuthCacheKey.LOGIN_TYPE}";

      this.MCache = new Dictionary<string, string>();
    }

    public async Task<string> GetStoreAsync(string key)
    {
      return await Task<string>.Run(() =>
      {
        if (MCache.ContainsKey(key))
        {
          return MCache[key];
        }
        return null;
      });
    }

    public async Task SetStoreAsync(string key, string value)
    {
      await Task.Run(() =>
      {
        MCache[key] = value;
      });
    }

    public async Task RemoveStoreAsync(string key)
    {
      await Task.Run(() =>
      {
        MCache.Remove(key);
      });
    }

    public string GetStore(string key)
    {

      if (this.MCache.ContainsKey(key))
      {
        return this.MCache[key];
      }
      else
      {
        return null;
      }
    }

    public void SetStore(string key, string value)
    {
      this.MCache[key] = value;
    }

    public void RemoveStore(string key)
    {
      this.MCache.Remove(key);
    }
  }
}