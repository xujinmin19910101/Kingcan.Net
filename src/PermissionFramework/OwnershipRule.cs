namespace PermissionFramework;

/// <summary>
/// Example rule: requires "resource.owner" permission when user accesses own resource.
/// For non-owner access, requires "resource.manage".
/// </summary>
public sealed class OwnershipRule : IPermissionRule
{
    private readonly Func<UserContext, string?, bool> _isOwner;

    public OwnershipRule(Func<UserContext, string?, bool> isOwner)
    {
        _isOwner = isOwner ?? throw new ArgumentNullException(nameof(isOwner));
    }

    public AuthorizationResult Evaluate(AuthorizationRequest request)
    {
        var requiredPermission = _isOwner(request.User, request.Resource)
            ? new Permission("resource.owner")
            : new Permission("resource.manage");

        if (request.User.HasPermission(requiredPermission))
        {
            return AuthorizationResult.Allow();
        }

        return AuthorizationResult.Deny([requiredPermission], $"Missing ownership-related permission: {requiredPermission}.");
    }
}
