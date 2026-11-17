namespace Hospital.BusinessObjects;

public class PreventivePrescription : Prescription
{
    public override string Category => "Профилактика";
    public override string Details => "";
    public override bool RequiresNurse => false;
}
