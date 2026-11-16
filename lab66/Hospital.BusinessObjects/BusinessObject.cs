using System.Text.Json.Serialization;

namespace Hospital.BusinessObjects;

public abstract class BusinessObject
{
    private readonly List<BusinessRule> brokenRules = new();

    public Guid Id { get; set; } = Guid.NewGuid();

    [JsonIgnore]
    public IReadOnlyList<BusinessRule> BrokenRules => brokenRules;

    protected abstract void Validate();

    protected void AddRule(BusinessRule rule)
    {
        brokenRules.Add(rule);
    }

    public bool IsValid()
    {
        brokenRules.Clear();
        Validate();
        return brokenRules.Count == 0;
    }
}
