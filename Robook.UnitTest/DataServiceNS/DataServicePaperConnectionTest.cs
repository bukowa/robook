using System.Collections.Concurrent;
using BenchmarkDotNet.Attributes;
using com.omnesys.rapi;
using Rithmic;
using Robook.DataServiceNS;
using Robook.SymbolNS;

namespace Robook.UnitTest.DataServiceNS;

[TestFixture]
[TestOf(typeof(DataService))]
public class DataServicePaperConnectionTest {
    /// <summary>
    /// The client to use for the tests
    /// </summary>
    /// 
    public static Client client;

    public static TestConnection testConnection;

    /// <summary>
    /// Prepare the client for the tests.
    /// </summary>
    [OneTimeSetUp]
    public void OneTimeSetUp() {
        testConnection = new TestConnection();
        client         = testConnection.NewPaperReadyClient();
    }

    /// <summary>
    /// Shutdown the client after the tests.
    /// </summary>
    [OneTimeTearDown]
    public void OneTimeTearDown() {
        client.REngine.shutdown();
    }
    
    [Test]
    public async Task TestReplay() {
        var queue = new ConcurrentQueue<BarInfo>();
        var start = new DateTime(2023, 10, 1).ToUniversalTime();
        var end   = new DateTime(2023, 12, 6).ToUniversalTime();

        var exchangeHist = "CME";
        var symbolHist   = "NQ";
        var typeHist     = BarType.Minute;
        var periodHist   = 5;

        var symbol = new Symbol(exchangeHist, symbolHist);
        symbol.SetClient(client);
        var tds    = new DataService(symbol);
        await tds.ReplayBars(start, end, periodHist, typeHist, queue);
        
        Assert.That(queue.Count, Is.GreaterThan(0));
        var data = queue.ToArray();
        Assert.That(data[0].StartSsboe, Is.GreaterThan(0));

        var bars2 = data.Select(
            b => new BarWithDate() { BarInfo = b }).ToList();
        
        // time of the first is sunday 22:00
        Assert.That(bars2.First().start.Month,     Is.EqualTo(10));
        Assert.That(bars2.First().start.Day,       Is.EqualTo(1));
        Assert.That(bars2.First().start.DayOfWeek, Is.EqualTo(DayOfWeek.Sunday));
        Assert.That(bars2.First().start.Hour,      Is.EqualTo(22));
        Assert.That(bars2.First().start.Minute,    Is.EqualTo(0));

        // DST +1
        // time of the last is tuesday 22:00
        // its a 1-hour break on weekdays
        Assert.That(bars2.Last().start.Month,     Is.EqualTo(12));
        Assert.That(bars2.Last().start.Day,       Is.EqualTo(5));
        Assert.That(bars2.Last().start.DayOfWeek, Is.EqualTo(DayOfWeek.Tuesday));
        Assert.That(bars2.Last().end.Hour,        Is.EqualTo(22));
        Assert.That(bars2.Last().end.Minute,      Is.EqualTo(0));
        
        for (var i = 1; i < bars2.Count; i++)
            // make sure that start time of the current bar
            // is not equal t o the start time of the previous bar
            Assert.That(
                bars2[i].BarInfo.StartSsboe,
                Is.Not.EqualTo(bars2[i - 1].BarInfo.StartSsboe
                ),
                $"Equal {i}: {bars2[i].BarInfo.StartSsboe} {bars2[i - 1].BarInfo.StartSsboe}");

        // todo make sure the start time fo the next bar
        // has 5 min difference with the start time of the current bar
        // factor in the hour change

        var sdt = new DateTime(2023, 10, 1, 22, 0, 0).ToUniversalTime();

        for (var i = 0; i < bars2.Count; i++) {
            var bar = bars2[i];

            var areEqual = sdt.Minute == bar.start.Minute;
            if (!areEqual)
                // debug
                Assert.That(areEqual, Is.True, $"Not equal `{sdt.Minute}` `{bar.start.Minute}` `{sdt}` `{bar.start}`");
            sdt = sdt.AddMinutes(5);

            // 24 / 11 / 2023 closed at 12:15 (CST = utc -6) => 18:15 utc
            if (bar.end.Month == 11 && bar.end.Day == 24 && bar.end.Hour == 18 && bar.end.Minute == 15)
                // reset sdt minute to 0
                sdt = new DateTime(2023, 10, 1, 22, 0, 0).ToUniversalTime();
        }
    }

    public static readonly DateTime UnixEpochDate = new(1970, 1, 1);

    public class BarWithDate {
        public DateTime? _end;

        public DateTime? _start;
        public BarInfo   BarInfo { get; set; }

        public DateTime start {
            get {
                if (_start == null) _start = DateTimeOffset.FromUnixTimeSeconds(BarInfo.StartSsboe).DateTime;
                return _start.Value;
            }
        }

        public DateTime end {
            get {
                if (_end == null) _end = DateTimeOffset.FromUnixTimeSeconds(BarInfo.EndSsboe).DateTime;
                return _end.Value;
            }
        }
    }

}