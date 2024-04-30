using System.ComponentModel;
using System.IO;
using System.Text.Json;
using System.Xml.Serialization;

namespace Robook.Data;


public static class JsonSerializer {
    public static void Serialize<T>(T data, string fileName) {
        var jsonData = System.Text.Json.JsonSerializer.Serialize(data);
        File.WriteAllText(fileName, jsonData);
    }

    public static T Deserialize<T>(string fileName, Action<T>? postProcess = null) {
        var jsonData = File.ReadAllText(fileName);
        var   result   = System.Text.Json.JsonSerializer.Deserialize<T>(jsonData);
        postProcess?.Invoke(result);
        return result;
    }
}

public interface IBindingListDataLoader<T> {
    BindingList<T> Load(Action<BindingList<T>>? postProcess = null);
}

public interface IBindingListDataSaver<T> {
    void Save(BindingList<T> data);
}

public interface IBindingListDataHandler<T>: IBindingListDataLoader<T>, IBindingListDataSaver<T> {}

public class LocalStorage<T>: IBindingListDataHandler<T> {
    private readonly string            _filePath;

    public LocalStorage(string filePath) {
        _filePath          = filePath;
    }

    public void Save(BindingList<T> data) {
        JsonSerializer.Serialize(data, _filePath);
    }

    public BindingList<T> Load(Action<BindingList<T>>? postProcess = null) {
        if (!File.Exists(_filePath)) {
            return new BindingList<T>();
        }
        return JsonSerializer.Deserialize(_filePath, postProcess);
    }
}

public class LocalStorageProvider<T> {
    private readonly string _dirPath;

    public LocalStorageProvider(string dirPath) {
        _dirPath = dirPath;
        if (!Directory.Exists(dirPath)) {
            Directory.CreateDirectory(dirPath);
        }
    }

    public LocalStorage<T> CreateLocalStorage(string fileName) {
        var filePath = Path.Combine(_dirPath, fileName);
        return new LocalStorage<T>(filePath);
    }
    
}

public static class LocalStorageFactory {
    public static Func<string, LocalStorage<T>> CreateLocalStorageInstance<T>(string dirPath) {
        if (!Directory.Exists(dirPath)) {
            Directory.CreateDirectory(dirPath);
        }
        return s => {
            var filePath = Path.Combine(dirPath, s);
            return new LocalStorage<T>(filePath);
        };
    }
}