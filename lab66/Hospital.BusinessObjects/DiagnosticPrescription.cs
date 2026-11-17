namespace Hospital.BusinessObjects;

public class DiagnosticPrescription : Prescription
{
    public override string Category => "Диагностика";
    public override string Details => "";
    public override bool RequiresNurse => false;
}
