namespace Hospital.BusinessObjects;

public class TabletPrescription : Prescription
{
    public string Dosage { get; set; } = "";
    public int Days { get; set; }

    public override string Category => "Таблетки";
    public override string Details => $"дозировка: {Dosage}, дней: {Days}";
    public override bool RequiresNurse => true;

    protected override void Validate()
    {
        base.Validate();
        if (string.IsNullOrWhiteSpace(Dosage))
            AddRule(new BusinessRule(nameof(Dosage), "Дозировка не указана"));
        if (Days <= 0)
            AddRule(new BusinessRule(nameof(Days), "Количество дней должно быть больше нуля"));
    }
}
