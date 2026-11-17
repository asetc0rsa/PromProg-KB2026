using Hospital.BusinessObjects;

namespace Hospital.DataLayer;

public class PatientDao : JsonDao<Patient>, IPatientDao
{
    public PatientDao(string dataDir) : base(Path.Combine(dataDir, "patients.json"))
    {
    }
}

public class DoctorDao : JsonDao<Doctor>, IDoctorDao
{
    public DoctorDao(string dataDir) : base(Path.Combine(dataDir, "doctors.json"))
    {
    }
}

public class NurseDao : JsonDao<Nurse>, INurseDao
{
    public NurseDao(string dataDir) : base(Path.Combine(dataDir, "nurses.json"))
    {
    }
}

public class DepartmentDao : JsonDao<Department>, IDepartmentDao
{
    public DepartmentDao(string dataDir) : base(Path.Combine(dataDir, "departments.json"))
    {
    }
}

public class MedicalRecordDao : JsonDao<MedicalRecord>, IMedicalRecordDao
{
    public MedicalRecordDao(string dataDir) : base(Path.Combine(dataDir, "records.json"))
    {
    }
}
