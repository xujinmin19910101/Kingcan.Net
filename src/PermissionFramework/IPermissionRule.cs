namespace PermissionFramework;

/// <summary>
/// Optional custom rules for advanced authorization logic such as time windows or resource ownership.
/// </summary>
public interface IPermissionRule
{
    AuthorizationResult Evaluate(AuthorizationRequest request);
}
