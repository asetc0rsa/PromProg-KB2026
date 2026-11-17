namespace Hospital.BusinessObjects;

public class Department : BusinessObject
{
    public string Name { get; set; } = "";

    protected override void Validate()
    {
        if (string.IsNullOrWhiteSpace(Name))
            AddRule(new BusinessRule(nameof(Name), "Название отделения не указано"));
    }
}
