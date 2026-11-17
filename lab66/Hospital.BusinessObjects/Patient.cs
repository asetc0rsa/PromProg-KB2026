namespace Hospital.BusinessObjects;

public class Patient : Person
{
    public string Patronymic { get; set; } = "";
    public DateTime BirthDate { get; set; }
    public string Gender { get; set; } = "";
    public string Phone { get; set; } = "";
    public string Address { get; set; } = "";

    public override string FullName =>
        string.Join(" ", new[] { LastName, FirstName, Patronymic }.Where(x => !string.IsNullOrWhiteSpace(x)));

    protected override void Validate()
    {
        base.Validate();
        if (BirthDate.Date > DateTime.Today || BirthDate.Year < 1900)
            AddRule(new BusinessRule(nameof(BirthDate), "Дата рождения указана неверно"));
        if (string.IsNullOrWhiteSpace(Gender))
            AddRule(new BusinessRule(nameof(Gender), "Пол не указан"));
    }
}
