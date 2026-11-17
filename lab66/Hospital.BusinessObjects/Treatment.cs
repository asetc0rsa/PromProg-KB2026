namespace Hospital.BusinessObjects;

public class Treatment : BusinessObject
{
    public List<Prescription> Prescriptions { get; set; } = new();

    public bool IsCompleted => Prescriptions.All(p => p.IsDone);

    protected override void Validate()
    {
        foreach (var prescription in Prescriptions)
        {
            if (prescription.IsValid())
                continue;
            foreach (var rule in prescription.BrokenRules)
                AddRule(rule);
        }
    }
}
