using System.Text.Json;
using Robook.Store;

namespace Robook.UnitTest.DataTypes;

[TestFixture]
[TestOf(typeof(ProtectedString))]
public class ProtectedStringTest {

    [Test]
    public void Test() {
        var ps = new ProtectedString("Hello World");
        Assert.That(ps.Value, Is.Not.EqualTo("Hello World"));
        Assert.That(ps.Decrypted(), Is.EqualTo("Hello World"));
        Assert.That(ps.ToString(), Is.EqualTo(ps.Value));
        Assert.That(Convert.FromBase64String(ps.ToString()), Is.Not.EqualTo("Hello World"));
        Assert.That(Convert.FromBase64String(ps.ToString()), Is.Not.EqualTo(ps.Decrypted()));
        Assert.That(Convert.FromBase64String(ps.ToString()), Is.Not.EqualTo(ps.Value));
    }

    [Test]
    public void TestJson() {
        var ps = new ProtectedString("Hello World");
        var json = JsonSerializer.Serialize(ps);
        Assert.That(json.Contains("Hello World"), Is.False);
        
        var ps2 = JsonSerializer.Deserialize<ProtectedString>(json);
        
        Assert.That(ps.Decrypted(), Is.EqualTo("Hello World"));
        Assert.That(ps2.Decrypted(), Is.EqualTo("Hello World"));
        
        Assert.That(ps.ToString(), Is.EqualTo(ps.Value));
        Assert.That(ps2.ToString(), Is.EqualTo(ps2.Value));
        Assert.That(ps.ToString(), Is.EqualTo(ps2.ToString()));
    }
}