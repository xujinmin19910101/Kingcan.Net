# Kingcan.Net - C# 权限框架示例

这是一个轻量级 C# 权限框架，可用于实现：

- 基于权限点（Permission）的授权
- 基于角色（Role）的权限聚合
- 用户直接授权 + 角色授权
- `All/Any` 两种权限评估模式
- 可插拔的自定义规则（`IPermissionRule`）

## 核心结构

- `Permission`：权限值对象（如 `order.read`）
- `Role`：角色，包含多个权限
- `UserContext`：用户上下文，包含角色和直接权限
- `AuthorizationService`：授权入口
- `IPermissionRule`：规则扩展点
- `OwnershipRule`：资源所有权规则示例

## 使用示例

```csharp
using PermissionFramework;

var adminRole = new Role("admin", new[]
{
    new Permission("order.read"),
    new Permission("order.write"),
    new Permission("resource.manage")
});

var user = new UserContext("u-1001");
user.AddRole(adminRole);
user.Grant(new Permission("resource.owner"));

var service = new AuthorizationService(new[]
{
    new OwnershipRule((u, resource) => resource == u.UserId)
});

var request = new AuthorizationRequest(
    User: user,
    RequiredPermissions: new[] { new Permission("order.read") },
    EvaluationMode: PermissionEvaluationMode.All,
    Resource: "u-1001");

var result = service.Authorize(request);
Console.WriteLine(result.IsAllowed); // True
```

## 可扩展方向

- 增加策略（Policy）注册器
- 对接 ASP.NET Core `IAuthorizationHandler`
- 增加缓存与审计日志
- 支持租户隔离（多租户权限空间）
