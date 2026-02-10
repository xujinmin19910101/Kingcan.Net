namespace PermissionFramework;

/// <summary>
/// Represents a permission in the system, e.g. "order.read".
/// </summary>
public sealed record Permission(string Value)
{
    public override string ToString() => Value;

    public static implicit operator Permission(string value) => new(value);
}
