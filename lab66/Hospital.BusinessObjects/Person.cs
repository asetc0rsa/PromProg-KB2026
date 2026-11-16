namespace Hospital.BusinessObjects;

public abstract class Person : BusinessObject
{
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";

    public virtual string FullName => $"{LastName} {FirstName}".Trim();

    protected override void Validate()
    {
        if (string.IsNullOrWhiteSpace(FirstName))
            AddRule(new BusinessRule(nameof(FirstName), "Имя не указано"));
        if (string.IsNullOrWhiteSpace(LastName))
            AddRule(new BusinessRule(nameof(LastName), "Фамилия не указана"));
    }
}
