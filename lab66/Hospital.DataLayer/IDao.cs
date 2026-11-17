using Hospital.BusinessObjects;

namespace Hospital.DataLayer;

public interface IDao<T> where T : BusinessObject
{
    List<T> GetAll();
    T? GetById(Guid id);
    void Save(T item);
    void Delete(Guid id);
}

public interface IPatientDao : IDao<Patient>
{
}

public interface IDoctorDao : IDao<Doctor>
{
}

public interface INurseDao : IDao<Nurse>
{
}

public interface IDepartmentDao : IDao<Department>
{
}

public interface IMedicalRecordDao : IDao<MedicalRecord>
{
}
