namespace Store.UnitTest;

[TestFixture]
public class Tests {
    private string _tempDir = Path.Join(Path.GetTempPath(), Guid.NewGuid().ToString());
    private string NewFilePath => Path.Join(_tempDir, Guid.NewGuid().ToString());

    [SetUp]
    public void Setup() {
        Directory.CreateDirectory(_tempDir);
        Console.WriteLine(_tempDir);
    }

    private struct Data {
        public string Name { get; set; }
        public int    Age  { get; set; }
    }

    [Test]
    public void TestJsonFileHandler() {
        // path to which we serialize
        var filePath = NewFilePath;

        // data to serialize
        var data = new Data { Name = "John", Age = 30 };

        // save
        JsonFileHandler.Serialize(filePath, data);

        // load
        var loadedData = JsonFileHandler.Deserialize<Data>(filePath);

        // assert
        Assert.That(loadedData.Name, Is.EqualTo(data.Name));
        Assert.That(loadedData.Age,  Is.EqualTo(data.Age));
    }
}