using System.Text.Json;
using Hospital.BusinessObjects;

namespace Hospital.DataLayer;

public class JsonDao<T> : IDao<T> where T : BusinessObject
{
    private static readonly JsonSerializerOptions Options = new() { WriteIndented = true };

    private readonly string path;
    private readonly object sync = new();

    public JsonDao(string path)
    {
        this.path = path;
        Directory.CreateDirectory(Path.GetDirectoryName(Path.GetFullPath(path))!);
    }

    public List<T> GetAll()
    {
        lock (sync)
            return Read();
    }

    public T? GetById(Guid id)
    {
        lock (sync)
            return Read().FirstOrDefault(x => x.Id == id);
    }

    public void Save(T item)
    {
        lock (sync)
        {
            var items = Read();
            var index = items.FindIndex(x => x.Id == item.Id);
            if (index >= 0)
                items[index] = item;
            else
                items.Add(item);
            Write(items);
        }
    }

    public void Delete(Guid id)
    {
        lock (sync)
        {
            var items = Read();
            items.RemoveAll(x => x.Id == id);
            Write(items);
        }
    }

    private List<T> Read()
    {
        if (!File.Exists(path))
            return new List<T>();
        return JsonSerializer.Deserialize<List<T>>(File.ReadAllText(path), Options) ?? new List<T>();
    }

    private void Write(List<T> items)
    {
        File.WriteAllText(path, JsonSerializer.Serialize(items, Options));
    }
}
