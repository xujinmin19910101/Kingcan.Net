using System.Collections.ObjectModel;

namespace PermissionFramework;

/// <summary>
/// Core service for evaluating permissions and invoking custom authorization rules.
/// </summary>
public sealed class AuthorizationService
{
    private readonly List<IPermissionRule> _rules = [];

    public AuthorizationService(IEnumerable<IPermissionRule>? rules = null)
    {
        if (rules is null)
        {
            return;
        }

        _rules.AddRange(rules);
    }

    public void AddRule(IPermissionRule rule)
    {
        ArgumentNullException.ThrowIfNull(rule);
        _rules.Add(rule);
    }

    public AuthorizationResult Authorize(AuthorizationRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (request.RequiredPermissions.Count == 0)
        {
            return AuthorizationResult.Allow();
        }

        var missing = FindMissingPermissions(request);
        var hasPermissionByMode = request.EvaluationMode switch
        {
            PermissionEvaluationMode.All => missing.Count == 0,
            PermissionEvaluationMode.Any => missing.Count < request.RequiredPermissions.Count,
            _ => false
        };

        if (!hasPermissionByMode)
        {
            return AuthorizationResult.Deny(missing, "Base permission check failed.");
        }

        foreach (var rule in _rules)
        {
            var result = rule.Evaluate(request);
            if (!result.IsAllowed)
            {
                return result;
            }
        }

        return AuthorizationResult.Allow();
    }

    private static IReadOnlyCollection<Permission> FindMissingPermissions(AuthorizationRequest request)
    {
        var missing = request.RequiredPermissions
            .Where(required => !request.User.HasPermission(required))
            .ToList();

        return new ReadOnlyCollection<Permission>(missing);
    }
}
