using Robook.DataServiceNS;

namespace Robook.UnitTest.DataServiceNS;

public class DataServiceUnitTest {
    [Test]
    public void TestBestFilesForOpt() {
        // generate list of files
        var fl = new List<DataService.History.File>() {
            new() {
                Start = new(2000, 1, 1),
                End   = new(2000, 1, 2),
            },
            new() {
                Start = new(2000, 1, 1),
                End   = new(2000, 1, 5),
            },
            new() {
                Start = new(2000, 1, 3),
                End   = new(2000, 1, 4),
            },
            new() {
                Start = new(2000, 1, 6),
                End   = new(2000, 1, 12),
            },
        };

        // assert function
        Action<DataService.History.Opts, DataService.History.File> assert =
            (_opts, _file) => {
                var _selectedFile = DataService.BestFilesForOpt(fl, _opts).First().file;
                Assert.That(_selectedFile, Is.EqualTo(_file), $"" +
                                                              $"_file.Start: ({_file.Start})\n" +
                                                              $"_selectedFile.Start ({_selectedFile.Start})\n\n" +
                                                              $"_file.End ({_file.End})\n" +
                                                              $"_selectedFile.End ({_selectedFile.End})");
            };

        // test
        var opt1 = new DataService.History.Opts() {
            Start = new(2000, 1, 1),
            End   = new(2000, 1, 2),
        };
        assert(opt1, fl[0]);

        // test
        var opt2 = new DataService.History.Opts() {
            Start = new(2000, 1, 1),
            End   = new(2000, 1, 3),
        };
        assert(opt2, fl[1]);

        // test
        var opt3 = new DataService.History.Opts() {
            Start = new(2000, 1, 3),
            End   = new(2000, 1, 4),
        };
        assert(opt3, fl[1]);

        // test
        var opt4 = new DataService.History.Opts() {
            Start = new(2000, 1, 3),
            End   = new(2000, 1, 5),
        };
        assert(opt4, fl[1]);

        // test
        var opt5 = new DataService.History.Opts() {
            Start = new(2000, 1, 3),
            End   = new(2000, 1, 6),
        };
        assert(opt5, fl[1]);

        // test
        var opt6 = new DataService.History.Opts() {
            Start = new(2000, 1, 3),
            End   = new(2000, 1, 10),
        };
        assert(opt6, fl[3]);

        // test
        var opt7 = new DataService.History.Opts() {
            Start = new(1999, 12, 31),
            End   = new(2000, 1, 1),
        };
        assert(opt7, fl[0]);

        // test
        var opt8 = new DataService.History.Opts() {
            Start = new(1999, 12, 31),
            End   = new(2000, 1, 11),
        };
        assert(opt8, fl[3]);
        
        // test
        var opt9 = new DataService.History.Opts() {
            Start = new(2021, 1, 1),
            End   = new(2021, 1, 11),
        };
        assert(opt9, fl[3]);
    }
}