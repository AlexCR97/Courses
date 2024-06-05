namespace AntuDevOps.PointOfSale.Domain.Models;

public readonly record struct TenantId(int Value);

public class Tenant : AggregateRoot<TenantId>
{

    public Tenant(TenantId id, DateTime createdAt, string createdBy, DateTime? lastModifiedAt, string? lastModifiedBy, string name) : base(id, createdAt, createdBy, lastModifiedAt, lastModifiedBy)
    {
        Name = name;
    }

    public string Name { get; private set; }

    public IReadOnlyList<TenantPreference> Preferences => _preferences.ToList();
    private readonly List<TenantPreference> _preferences;

    public static Tenant SignUp(string name)
    {
        return new Tenant(
            default,
            DateTime.UtcNow,
            "Anonymous",
            null,
            null,
            name);
    }

    public TenantPreference GetPreference(string key)
    {
        return GetPreferenceOrDefault(key)
            ?? throw new Exception($@"No such preference with key ""{key}""");
    }

    public TenantPreference? GetPreferenceOrDefault(string key)
    {
        return _preferences.Find(x => x.Key == key);
    }

    public void SetPreference(string key, string? value, string? lastModifiedBy)
    {
        var existingPreference = GetPreferenceOrDefault(key);

        if (existingPreference is not null)
            existingPreference.SetValue(value);
        else
            _preferences.Add(new TenantPreference(key, value));

        LastModifiedAt = DateTime.UtcNow;
        LastModifiedBy = lastModifiedBy;
    }
}

public class TenantPreference
{
    public TenantPreference(string key, string? value)
    {
        Key = key;
        Value = value;
    }

    public string Key { get; }
    public string? Value { get; private set; }

    public void SetValue(string? value)
    {
        Value = value;
    }
}
