using System.Text.Json;
namespace Storage.Core;


public interface IStorage<T> {
    T?   Load();
    void Save(T? data);
}

public class JsonFileStorage<T>(string filePath) : IStorage<T> {
    // ReSharper disable once MemberCanBePrivate.Global
    public readonly string FilePath = filePath;

    public void Save(T? data) {
        JsonFileHandler.Serialize(FilePath, data);
    }

    public T? Load() {
        if (!File.Exists(FilePath)) {
            return default;
        }

        return JsonFileHandler.Deserialize<T>(FilePath);
    }
}

public static class JsonFileHandler {
    public static void Serialize<T>(string filePath, T? data) {
        var jsonData = JsonSerializer.Serialize(data);
        File.WriteAllText(filePath, jsonData);
    }

    public static T? Deserialize<T>(string fileName) {
        var jsonData = File.Open(fileName, FileMode.Open);
        var result   = JsonSerializer.Deserialize<T>(jsonData);
        return result;
    }
}