using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace CloudBase {
  public class Auth {
    private Core core;
    private AuthCache cache;

    private Task refreshAccessTokenTask;

    public Auth(Core core) {
      this.core = core;
      this.cache = new AuthCache(core);
      this.core.setAuthInstance(this);
    }

    // 自定义登录
    public async Task<AuthState> SignInWithTicketAsync(string ticket) {
      if (string.IsNullOrEmpty(ticket)) {
        throw new CloudBaseException(CloudBaseExceptionCode.EMPTY_PARAM, "登录票据不能为空");
      }

      string refreshToken = cache.GetStore(cache.RefreshTokenKey);
      Dictionary<string, dynamic> param = new Dictionary<string, dynamic> ();
      param.Add("ticket", ticket);
      param.Add("refresh_token", refreshToken);

      AuthResponse res = await this.core.Request.PostWithoutAuthAsync<AuthResponse>("auth.signInWithTicket", param);

      if (!string.IsNullOrEmpty(res.Code)) {
        throw new CloudBaseException(res.Code, res.Message);
      }

      if (!res.Data.ContainsKey("refresh_token")) {
        throw new CloudBaseException(CloudBaseExceptionCode.AUTH_FAILED, "自定义登录失败");
      }

      string newRefreshToken = (string)res.Data["refresh_token"];
      await this.SetRefreshTokenAsync(newRefreshToken);
      await this.RefreshAccessTokenAsync();
      cache.SetStore(cache.LoginTypeKey, AuthType.CUSTOM);

      string accessToken = cache.GetStore(cache.AccessTokenKey);
      AuthState authState = new AuthState(AuthType.CUSTOM, newRefreshToken, accessToken);
      return authState;
    }

    // 匿名登录
    public async Task<AuthState> SignInAnonymouslyAsync() {
      // 如果本地存有uuid则匿名登录时传给server
      string uuid = cache.GetStore(cache.AnonymousUuidKey);
      string refreshToken = cache.GetStore(cache.RefreshTokenKey);
      
      Dictionary<string, dynamic> param = new Dictionary<string, dynamic>();
      param.Add("anonymous_uuid", uuid);
      param.Add("refresh_token", refreshToken);


      AuthResponse res = await this.core.Request.PostWithoutAuthAsync<AuthResponse>("auth.signInAnonymously", param);
      if (!string.IsNullOrEmpty(res.Code)) {
        throw new CloudBaseException(res.Code, res.Message);
      }

      if (res.Data["refresh_token"] == null && res.Data["uuid"] == null) {
        throw new CloudBaseException(
          CloudBaseExceptionCode.AUTH_FAILED,
          "匿名登录失败"
        );
      }

      string newUuid = (string)res.Data["uuid"];
      string newRefreshToken = (string)res.Data["refresh_token"];
      cache.SetStore(cache.AnonymousUuidKey, newUuid);
      cache.SetStore(cache.LoginTypeKey, AuthType.ANONYMOUS);
      await this.SetRefreshTokenAsync(newRefreshToken);
      await this.RefreshAccessTokenAsync();

      string accessToken = cache.GetStore(cache.AccessTokenKey);
      return (new AuthState(AuthType.ANONYMOUS, newRefreshToken, accessToken));
    }

    // 匿名账号数据迁移到正式账号
    public async Task<AuthState> linkAndRetrieveDataWithTicket(string ticket) {
      string uuid = cache.GetStore(cache.AnonymousUuidKey);
      string refreshToken = cache.GetStore(cache.RefreshTokenKey);
      Dictionary<string, dynamic> param = new Dictionary<string, dynamic>();
      param.Add("anonymous_uuid", uuid);
      param.Add("refresh_token", refreshToken);
      param.Add("ticket", ticket);
      AuthResponse res = await this.core.Request.PostWithoutAuthAsync<AuthResponse>("auth.linkAndRetrieveDataWithTicket", param);

      if (!string.IsNullOrEmpty(res.Code)) {
        throw new CloudBaseException(res.Code, res.Message);
      }

      if (res.Data["refresh_token"] == null) {
        throw new CloudBaseException(CloudBaseExceptionCode.LINK_AND_RETRIEVE_DATA_FAILED, "匿名转化失败");
      }

      // 转正后清除本地保存的匿名uuid
      await cache.RemoveStoreAsync(cache.AnonymousUuidKey);
      string newRefreshToken = (string)res.Data["refresh_token"];
      await SetRefreshTokenAsync(newRefreshToken);
      await RefreshAccessTokenAsync();

      string accessToken = cache.GetStore(cache.AccessTokenKey);
      AuthState authState = new AuthState(AuthType.CUSTOM, newRefreshToken, accessToken);
      return authState;
    }

    public async Task SignOutAsync() {
      AuthState state = await this.GetAuthStateAsync();
      if (state != null && state.AuthType == AuthType.ANONYMOUS) {
        throw new CloudBaseException(CloudBaseExceptionCode.SIGN_OUT_FAILED, "匿名用户不支持登出操作");
      }

      if (string.IsNullOrEmpty(state.RefreshToken)) {
        // 没有refreshToken, 不需要执行登出操作
        return;
      }

      Dictionary<string, dynamic> param = new Dictionary<string, dynamic>();
      param.Add("refresh_token", state.RefreshToken);
      AuthResponse res = await this.core.Request.PostAsync<AuthResponse>("auth.logout", param);

      if (!string.IsNullOrEmpty(res.Code)) {
        throw new CloudBaseException(res.Code, res.Message);
      }

      await cache.RemoveStoreAsync(cache.RefreshTokenKey);
      await cache.RemoveStoreAsync(cache.AccessTokenKey);
      await cache.RemoveStoreAsync(cache.AccessTokenExpireKey);
    }

    public async Task<AuthState> GetAuthStateAsync() {
      await this.RefreshAccessTokenAsync();

      string accessToken = cache.GetStore(cache.AccessTokenKey);
      if (!string.IsNullOrEmpty(accessToken)) {
        string authType = cache.GetStore(cache.LoginTypeKey);
        string refreshToken = cache.GetStore(cache.RefreshTokenKey);
        return (new AuthState(authType, refreshToken, accessToken));
      } else {
        return null;
      }
    }

    public async Task<UserInfo> GetUserInfoAsync() {
      Dictionary<string, dynamic> param = new Dictionary<string, dynamic>();
      UserInfoResponse res = await this.core.Request.PostAsync<UserInfoResponse>("auth.getUserInfo", param);

      if (!string.IsNullOrEmpty(res.Code)) {
        throw new CloudBaseException(res.Code, res.Message);
      }

      return (new UserInfo(res.Data));
    }

    private async Task SetRefreshTokenAsync(string refreshToken) {
      // refresh token设置前，先清掉 access token
      await cache.RemoveStoreAsync(cache.AccessTokenKey);
      await cache.RemoveStoreAsync(cache.AccessTokenExpireKey);
      cache.SetStore(cache.RefreshTokenKey, refreshToken);
    }

    public async Task RefreshAccessTokenAsync() {
      if (this.refreshAccessTokenTask == null) {
        this.refreshAccessTokenTask = this.InternalRefreshAccessTokenAsync();
      }

      try {
        await this.refreshAccessTokenTask;
      } catch(CloudBaseException e) {
        Console.WriteLine(e);
      } finally {
        this.refreshAccessTokenTask = null;
      }
    }

    private async Task InternalRefreshAccessTokenAsync() {
      await cache.RemoveStoreAsync(cache.AccessTokenKey);
      await cache.RemoveStoreAsync(cache.AccessTokenExpireKey);
      string authType = cache.GetStore(cache.LoginTypeKey);
      string refreshToken = cache.GetStore(cache.RefreshTokenKey);
      if (string.IsNullOrEmpty(refreshToken)) {
        throw new CloudBaseException(CloudBaseExceptionCode.NOT_LOGIN, "未登录CloudBase");
      }

      Dictionary<string, dynamic> param = new Dictionary<string, dynamic>();
      param.Add("refresh_token", refreshToken);
      if (AuthType.ANONYMOUS == authType) {
        string uuid = cache.GetStore(cache.AnonymousUuidKey);
        param.Add("anonymous_uuid", uuid);
      }
      AuthResponse res = await this.core.Request.PostWithoutAuthAsync<AuthResponse>("auth.getJwt", param);

      if (!string.IsNullOrEmpty(res.Code)) {
        throw new CloudBaseException(res.Code, res.Message);
      }

      if(res.Data.ContainsKey("access_token")) {
        cache.SetStore(cache.AccessTokenKey, res.Data["access_token"].ToString());
        // 本地时间可能没有同步
        DateTime now = DateTime.Now;
        now.AddMilliseconds((double)res.Data["access_token_expire"]);
        cache.SetStore(cache.AccessTokenExpireKey, now.Millisecond.ToString());
      }

      // 匿名登录refresh_token过期情况下返回refresh_token
      // 此场景下使用新的refresh_token获取access_token
      if (res.Data.ContainsKey("refresh_token")) {
        await this.SetRefreshTokenAsync((string)res.Data["refresh_token"]);
        await this.InternalRefreshAccessTokenAsync();
      }

    }

    public async Task<string> GetAccessTokenAsync() {
      string accessToken = cache.GetStore(cache.AccessTokenKey);

      return accessToken;
    }

    public AuthHeader GetAuthHeader() {
      string accessToken = cache.GetStore(cache.AccessTokenKey);
      string refreshToken = cache.GetStore(cache.RefreshTokenKey);

      AuthHeader header = new AuthHeader();
      header.SetHeader("x-cloudbase-credentials", accessToken + "/@@/" + refreshToken);

      return header;
    }
  }
}
