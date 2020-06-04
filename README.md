# CloudBase .NET SDK

## 介绍

CloudBase 提供开发应用所需服务和基础设施。CloudBase .NET SDK 让你可以在客户端访问 CloudBase 的服务。

## 安装

* 打包 SDK

```sh
dotnet pack
```

* 在你的项目里引入 SDK

```
cd your_project

dotnet nuget add source LocalPath // LocalPath是你保存 CloudBase Sdk 的路径

dotnet add package CloudBase
```

## 快速上手

```csharp
using System.Threading.Tasks;
using System.Collections.Generic;
using CloudBase;

// 初始化
CloudBaseApp app = CloudBaseApp.Init("your-env-id", 3000);

// 匿名登录
AuthState state = await app.Auth.GetAuthStateAsync();
if (state == null) {
  await app.Auth.SignInAnonymouslyAsync();
}

// 调用云函数
var param = new Dictionary<string, dynamic>() { {"a", 1}, {"b", 2} };
FunctionResponse res = await app.Function.CallFunctionAsync("sum", param);
```

## 详细文档

* [快速开始](https://docs.cloudbase.net/quick-start/dotnet.html)
* [初始化](https://docs.cloudbase.net/api-reference/dotnet/initialization.html)
* [登录](https://docs.cloudbase.net/api-reference/dotnet/authentication.html)
* [云函数](https://docs.cloudbase.net/api-reference/dotnet/functions.html)
* [数据库](https://docs.cloudbase.net/api-reference/dotnet/database.html)
* [文件存储](https://docs.cloudbase.net/api-reference/dotnet/storage.html)


