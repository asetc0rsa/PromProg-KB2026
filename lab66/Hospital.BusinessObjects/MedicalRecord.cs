namespace Hospital.BusinessObjects;

public class MedicalRecord : BusinessObject
{
    public Guid PatientId { get; set; }
    public Guid AdmissionDoctorId { get; set; }
    public DateTime AdmissionDate { get; set; } = DateTime.Now;
    public string AdmissionDiagnosis { get; set; } = "";
    public Guid? DepartmentId { get; set; }
    public Guid? AttendingDoctorId { get; set; }
    public Treatment Treatment { get; set; } = new();
    public DateTime? DischargeDate { get; set; }

    public bool IsDischarged => DischargeDate.HasValue;

    public void Refer(Guid departmentId, Guid doctorId)
    {
        EnsureOpen();
        DepartmentId = departmentId;
        AttendingDoctorId = doctorId;
    }

    public void AddPrescription(Prescription prescription)
    {
        EnsureOpen();
        if (AttendingDoctorId == null)
            throw new HospitalException("Сначала направьте пациента в отделение");
        Treatment.Prescriptions.Add(prescription);
    }

    public void Discharge()
    {
        EnsureOpen();
        if (AttendingDoctorId == null)
            throw new HospitalException("Пациент не был направлен в отделение");
        if (!Treatment.IsCompleted)
            throw new HospitalException("Не все назначения выполнены");
        DischargeDate = DateTime.Now;
    }

    protected override void Validate()
    {
        if (string.IsNullOrWhiteSpace(AdmissionDiagnosis))
            AddRule(new BusinessRule(nameof(AdmissionDiagnosis), "Диагноз при поступлении не указан"));
    }

    private void EnsureOpen()
    {
        if (IsDischarged)
            throw new HospitalException("Пациент уже выписан");
    }
}
