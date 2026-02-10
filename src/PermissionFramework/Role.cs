using System.Collections.ObjectModel;

namespace PermissionFramework;

/// <summary>
/// Role groups multiple permissions.
/// </summary>
public sealed class Role
{
    private readonly HashSet<Permission> _permissions = [];

    public Role(string name, IEnumerable<Permission>? permissions = null)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Role name cannot be empty.", nameof(name));
        }

        Name = name;

        if (permissions is null)
        {
            return;
        }

        foreach (var permission in permissions)
        {
            _permissions.Add(permission);
        }
    }

    public string Name { get; }

    public IReadOnlyCollection<Permission> Permissions => new ReadOnlyCollection<Permission>(_permissions.ToList());

    public void AddPermission(Permission permission) => _permissions.Add(permission);

    public bool HasPermission(Permission permission) => _permissions.Contains(permission);
}
