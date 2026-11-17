using System.Text.Json.Serialization;

namespace Hospital.BusinessObjects;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "kind")]
[JsonDerivedType(typeof(InjectionPrescription), "injection")]
[JsonDerivedType(typeof(TabletPrescription), "tablet")]
[JsonDerivedType(typeof(DiagnosticPrescription), "diagnostic")]
[JsonDerivedType(typeof(PreventivePrescription), "preventive")]
public abstract class Prescription : BusinessObject
{
    public string Name { get; set; } = "";
    public bool IsDone { get; set; }
    public DateTime? DoneDate { get; set; }
    public Guid? PerformedById { get; set; }

    public abstract string Category { get; }
    public abstract string Details { get; }
    public abstract bool RequiresNurse { get; }

    public void Perform(Staff staff)
    {
        if (IsDone)
            throw new HospitalException("Назначение уже выполнено");
        if (RequiresNurse && staff is not Nurse)
            throw new HospitalException("Медикаментозные назначения выполняет медсестра");
        if (!RequiresNurse && staff is not Doctor)
            throw new HospitalException("Это назначение выполняет врач");
        IsDone = true;
        DoneDate = DateTime.Now;
        PerformedById = staff.Id;
    }

    protected override void Validate()
    {
        if (string.IsNullOrWhiteSpace(Name))
            AddRule(new BusinessRule(nameof(Name), "Название назначения не указано"));
    }
}
