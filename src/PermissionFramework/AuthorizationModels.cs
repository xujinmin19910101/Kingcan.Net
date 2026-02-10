namespace PermissionFramework;

public enum PermissionEvaluationMode
{
    Any,
    All
}

public sealed record AuthorizationRequest(
    UserContext User,
    IReadOnlyCollection<Permission> RequiredPermissions,
    PermissionEvaluationMode EvaluationMode = PermissionEvaluationMode.All,
    string? Resource = null);

public sealed record AuthorizationResult(
    bool IsAllowed,
    IReadOnlyCollection<Permission> MissingPermissions,
    string? FailureReason = null)
{
    public static AuthorizationResult Allow() => new(true, Array.Empty<Permission>());

    public static AuthorizationResult Deny(
        IReadOnlyCollection<Permission> missingPermissions,
        string? reason = null)
        => new(false, missingPermissions, reason);
}
