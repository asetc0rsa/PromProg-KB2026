using Hospital.BusinessObjects;
using Hospital.DataLayer;

namespace Hospital.Services;

public class HospitalService
{
    private readonly IPatientDao patients;
    private readonly IDoctorDao doctors;
    private readonly INurseDao nurses;
    private readonly IDepartmentDao departments;
    private readonly IMedicalRecordDao records;

    public HospitalService(string dataDir)
    {
        patients = new PatientDao(dataDir);
        doctors = new DoctorDao(dataDir);
        nurses = new NurseDao(dataDir);
        departments = new DepartmentDao(dataDir);
        records = new MedicalRecordDao(dataDir);
        SeedData();
    }

    public List<Department> GetDepartments()
    {
        return departments.GetAll();
    }

    public List<StaffDto> GetStaff()
    {
        var result = doctors.GetAll()
            .Select(d => new StaffDto(d.Id, d.FullName, "doctor", d.DepartmentId, d.Specialization))
            .ToList();
        result.AddRange(nurses.GetAll()
            .Select(n => new StaffDto(n.Id, n.FullName, "nurse", null, n.Role)));
        return result;
    }

    public List<RecordDto> GetRecords()
    {
        return records.GetAll()
            .OrderByDescending(r => r.AdmissionDate)
            .Select(ToDto)
            .ToList();
    }

    public RecordDto Admit(AdmissionRequest request)
    {
        var doctor = doctors.GetById(request.DoctorId)
            ?? throw new HospitalException("Дежурный врач не найден");

        var patient = new Patient
        {
            FirstName = (request.FirstName ?? "").Trim(),
            LastName = (request.LastName ?? "").Trim(),
            Patronymic = (request.Patronymic ?? "").Trim(),
            BirthDate = request.BirthDate,
            Gender = request.Gender ?? "",
            Phone = (request.Phone ?? "").Trim(),
            Address = (request.Address ?? "").Trim()
        };
        Ensure(patient);

        var record = new MedicalRecord
        {
            PatientId = patient.Id,
            AdmissionDoctorId = doctor.Id,
            AdmissionDiagnosis = (request.Diagnosis ?? "").Trim()
        };
        Ensure(record);

        patients.Save(patient);
        records.Save(record);
        return ToDto(record);
    }

    public RecordDto Refer(Guid recordId, ReferRequest request)
    {
        var record = GetRecord(recordId);
        var department = departments.GetById(request.DepartmentId)
            ?? throw new HospitalException("Отделение не найдено");
        var doctor = doctors.GetById(request.DoctorId)
            ?? throw new HospitalException("Врач не найден");
        if (doctor.DepartmentId != department.Id)
            throw new HospitalException("Врач не работает в выбранном отделении");

        record.Refer(department.Id, doctor.Id);
        records.Save(record);
        return ToDto(record);
    }

    public RecordDto AddPrescription(Guid recordId, PrescriptionRequest request)
    {
        var record = GetRecord(recordId);
        var prescription = CreatePrescription(request);
        Ensure(prescription);

        record.AddPrescription(prescription);
        records.Save(record);
        return ToDto(record);
    }

    public RecordDto Perform(Guid recordId, Guid prescriptionId, PerformRequest request)
    {
        var record = GetRecord(recordId);
        var prescription = record.Treatment.Prescriptions.FirstOrDefault(p => p.Id == prescriptionId)
            ?? throw new HospitalException("Назначение не найдено");
        var staff = FindStaff(request.StaffId);

        prescription.Perform(staff);
        records.Save(record);
        return ToDto(record);
    }

    public DischargeSummary Discharge(Guid recordId)
    {
        var record = GetRecord(recordId);
        record.Discharge();
        records.Save(record);
        return BuildSummary(record);
    }

    public DischargeSummary GetSummary(Guid recordId)
    {
        var record = GetRecord(recordId);
        if (!record.IsDischarged)
            throw new HospitalException("Пациент еще не выписан");
        return BuildSummary(record);
    }

    private static Prescription CreatePrescription(PrescriptionRequest request)
    {
        var name = (request.Name ?? "").Trim();
        return request.Kind switch
        {
            "injection" => new InjectionPrescription { Name = name, Quantity = request.Quantity ?? 0 },
            "tablet" => new TabletPrescription { Name = name, Dosage = (request.Dosage ?? "").Trim(), Days = request.Days ?? 0 },
            "diagnostic" => new DiagnosticPrescription { Name = name },
            "preventive" => new PreventivePrescription { Name = name },
            _ => throw new HospitalException("Неизвестный тип назначения")
        };
    }

    private static void Ensure(BusinessObject item)
    {
        if (!item.IsValid())
            throw new HospitalException(string.Join("; ", item.BrokenRules.Select(r => r.Error)));
    }

    private MedicalRecord GetRecord(Guid id)
    {
        return records.GetById(id) ?? throw new HospitalException("ЭМК не найдена");
    }

    private Staff FindStaff(Guid id)
    {
        Staff? staff = doctors.GetById(id);
        staff ??= nurses.GetById(id);
        return staff ?? throw new HospitalException("Сотрудник не найден");
    }

    private string StaffName(Guid id)
    {
        return doctors.GetById(id)?.FullName ?? nurses.GetById(id)?.FullName ?? "";
    }

    private RecordDto ToDto(MedicalRecord record)
    {
        var patient = patients.GetById(record.PatientId)
            ?? throw new HospitalException("Пациент не найден");
        var department = record.DepartmentId.HasValue ? departments.GetById(record.DepartmentId.Value)?.Name : null;
        var doctor = record.AttendingDoctorId.HasValue ? doctors.GetById(record.AttendingDoctorId.Value)?.FullName : null;
        var prescriptions = record.Treatment.Prescriptions
            .Select(p => new PrescriptionDto(
                p.Id, p.Category, p.Name, p.Details, p.RequiresNurse, p.IsDone, p.DoneDate,
                p.PerformedById.HasValue ? StaffName(p.PerformedById.Value) : null))
            .ToList();

        return new RecordDto(
            record.Id, patient.FullName, patient.BirthDate, record.AdmissionDate, record.AdmissionDiagnosis,
            record.DepartmentId, department, doctor, record.IsDischarged, record.DischargeDate, prescriptions);
    }

    private DischargeSummary BuildSummary(MedicalRecord record)
    {
        var patient = patients.GetById(record.PatientId)
            ?? throw new HospitalException("Пациент не найден");
        var department = departments.GetById(record.DepartmentId!.Value)?.Name ?? "";
        var doctor = doctors.GetById(record.AttendingDoctorId!.Value)?.FullName ?? "";
        var discharge = record.DischargeDate!.Value;
        var performed = record.Treatment.Prescriptions
            .Where(p => p.IsDone)
            .Select(p => $"{p.DoneDate:dd.MM.yyyy HH:mm} — {p.Category}: {p.Name} {p.Details}".Trim())
            .ToList();

        return new DischargeSummary(
            patient.FullName, patient.BirthDate, patient.Gender, patient.Address, patient.Phone,
            record.AdmissionDate, discharge, (discharge.Date - record.AdmissionDate.Date).Days + 1,
            record.AdmissionDiagnosis, department, doctor, performed);
    }

    private void SeedData()
    {
        if (departments.GetAll().Count > 0)
            return;

        var cardiology = new Department { Name = "Кардиологическое" };
        var surgery = new Department { Name = "Хирургическое" };
        var neurology = new Department { Name = "Неврологическое" };
        departments.Save(cardiology);
        departments.Save(surgery);
        departments.Save(neurology);

        doctors.Save(new Doctor { LastName = "Петров", FirstName = "Иван", Specialization = "Дежурный врач" });
        doctors.Save(new Doctor { LastName = "Орлова", FirstName = "Мария", Specialization = "Дежурный врач" });
        doctors.Save(new Doctor { LastName = "Кузнецов", FirstName = "Сергей", Specialization = "Кардиолог", DepartmentId = cardiology.Id });
        doctors.Save(new Doctor { LastName = "Волкова", FirstName = "Анна", Specialization = "Хирург", DepartmentId = surgery.Id });
        doctors.Save(new Doctor { LastName = "Соколов", FirstName = "Дмитрий", Specialization = "Невролог", DepartmentId = neurology.Id });

        nurses.Save(new Nurse { LastName = "Иванова", FirstName = "Елена" });
        nurses.Save(new Nurse { LastName = "Смирнова", FirstName = "Ольга" });
    }
}
