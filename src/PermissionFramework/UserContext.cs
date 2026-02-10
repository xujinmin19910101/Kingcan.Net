using System.Collections.ObjectModel;

namespace PermissionFramework;

/// <summary>
/// Describes a user and all permissions that can be resolved from direct grants and roles.
/// </summary>
public sealed class UserContext
{
    private readonly HashSet<Role> _roles = [];
    private readonly HashSet<Permission> _directPermissions = [];

    public UserContext(string userId)
    {
        if (string.IsNullOrWhiteSpace(userId))
        {
            throw new ArgumentException("User id cannot be empty.", nameof(userId));
        }

        UserId = userId;
    }

    public string UserId { get; }

    public IReadOnlyCollection<Role> Roles => new ReadOnlyCollection<Role>(_roles.ToList());

    public IReadOnlyCollection<Permission> DirectPermissions => new ReadOnlyCollection<Permission>(_directPermissions.ToList());

    public void AddRole(Role role) => _roles.Add(role);

    public void Grant(Permission permission) => _directPermissions.Add(permission);

    public bool HasPermission(Permission permission)
    {
        if (_directPermissions.Contains(permission))
        {
            return true;
        }

        return _roles.Any(role => role.HasPermission(permission));
    }

    public IReadOnlyCollection<Permission> ResolveAllPermissions()
    {
        var result = new HashSet<Permission>(_directPermissions);

        foreach (var role in _roles)
        {
            foreach (var permission in role.Permissions)
            {
                result.Add(permission);
            }
        }

        return new ReadOnlyCollection<Permission>(result.ToList());
    }
}
