namespace Hospital.BusinessObjects;

public class Doctor : Staff
{
    public string Specialization { get; set; } = "";
    public Guid? DepartmentId { get; set; }

    public override string Role => "Врач";
}
