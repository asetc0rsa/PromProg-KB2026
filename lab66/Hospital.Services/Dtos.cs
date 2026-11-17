namespace Hospital.Services;

public record AdmissionRequest(string FirstName, string LastName, string Patronymic, DateTime BirthDate, string Gender, string Phone, string Address, Guid DoctorId, string Diagnosis);

public record ReferRequest(Guid DepartmentId, Guid DoctorId);

public record PrescriptionRequest(string Kind, string Name, int? Quantity, string? Dosage, int? Days);

public record PerformRequest(Guid StaffId);

public record StaffDto(Guid Id, string Name, string Kind, Guid? DepartmentId, string Specialization);

public record PrescriptionDto(Guid Id, string Category, string Name, string Details, bool RequiresNurse, bool IsDone, DateTime? DoneDate, string? PerformedBy);

public record RecordDto(Guid Id, string PatientName, DateTime BirthDate, DateTime AdmissionDate, string Diagnosis, Guid? DepartmentId, string? Department, string? Doctor, bool IsDischarged, DateTime? DischargeDate, List<PrescriptionDto> Prescriptions);

public record DischargeSummary(string PatientName, DateTime BirthDate, string Gender, string Address, string Phone, DateTime AdmissionDate, DateTime DischargeDate, int Days, string Diagnosis, string Department, string Doctor, List<string> Performed);
