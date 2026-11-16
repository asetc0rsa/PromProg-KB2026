using Hospital.BusinessObjects;

namespace Hospital.DataLayer;

public interface IDao<T> where T : BusinessObject
{
    List<T> GetAll();
    T? GetById(Guid id);
    void Save(T item);
    void Delete(Guid id);
}
