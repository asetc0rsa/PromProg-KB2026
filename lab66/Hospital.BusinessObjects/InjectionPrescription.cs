namespace Hospital.BusinessObjects;

public class InjectionPrescription : Prescription
{
    public int Quantity { get; set; }

    public override string Category => "Инъекция";
    public override string Details => $"количество: {Quantity}";
    public override bool RequiresNurse => true;

    protected override void Validate()
    {
        base.Validate();
        if (Quantity <= 0)
            AddRule(new BusinessRule(nameof(Quantity), "Количество инъекций должно быть больше нуля"));
    }
}
