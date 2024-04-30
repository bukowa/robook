using System.ComponentModel;
using System.IO;
using System.Text.Json;
using System.Xml.Serialization;

namespace Robook.Data;

public enum SerializationType {
    Xml,
    Json
}

public static class Serializer {
    public static void Serialize<T>(T data, string fileName, SerializationType serializationType) {
        switch (serializationType) {
            case SerializationType.Xml:
                var xmlSerializer = new XmlSerializer(typeof(T));
                using (TextWriter writer = new StreamWriter(fileName)) {
                    xmlSerializer.Serialize(writer, data);
                }

                break;
            case SerializationType.Json:
                var jsonData = JsonSerializer.Serialize(data);
                File.WriteAllText(fileName, jsonData);
                break;
            default:
                throw new ArgumentException("Unsupported serialization type.");
        }
    }

    public static T Deserialize<T>(string fileName, SerializationType serializationType) {
        switch (serializationType) {
            case SerializationType.Xml:
                var xmlSerializer = new XmlSerializer(typeof(T));
                using (TextReader reader = new StreamReader(fileName)) {
                    return (T)xmlSerializer.Deserialize(reader);
                }
            case SerializationType.Json:
                var jsonData = File.ReadAllText(fileName);
                return JsonSerializer.Deserialize<T>(jsonData);
            default:
                throw new ArgumentException("Unsupported serialization type.");
        }
    }
}

public interface IDataLoader<T> {
    BindingList<T> Load();
}

public interface IDataSaver<T> {
    void Save(BindingList<T> data);
}

public interface IDataHandler<T>: IDataLoader<T>, IDataSaver<T> {}

public class LocalStorage<T>: IDataHandler<T> {
    private readonly string            _filePath;
    private readonly SerializationType _serializationType;

    public LocalStorage(string filePath) {
        _filePath          = filePath;
        _serializationType = SerializationType.Json;
    }

    public void Save(BindingList<T> data) {
        Serializer.Serialize(data, _filePath, _serializationType);
    }

    public BindingList<T> Load() {
        if (!File.Exists(_filePath)) {
            return new BindingList<T>();
        }
        return Serializer.Deserialize<BindingList<T>>(_filePath, _serializationType);
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