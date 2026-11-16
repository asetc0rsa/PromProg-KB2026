namespace Hospital.BusinessObjects;

public class BusinessRule
{
    public string Property { get; }
    public string Error { get; }

    public BusinessRule(string property, string error)
    {
        Property = property;
        Error = error;
    }
}
