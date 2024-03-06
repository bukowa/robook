using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Globalization;
using com.omnesys.omne.om;
using com.omnesys.rapi;
using Parquet.Serialization;
using Rithmic;
using Robook.SymbolNS;
using Microsoft.Extensions.Logging;

namespace Robook.DataServiceNS;

public partial class DataService {
    public readonly Symbol   Symbol;
    public          string   DataDir;
    public          ILogger? Logger;

    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Information,
        Message = "Replaying Bars for `{symbol} {exchange} {start} - {end}`",
        EventName = "ReplayingBars")]
    public partial void LogReplayingBars(string symbol, string exchange, DateTime start, DateTime end);

    [LoggerMessage(
        EventId = 2,
        Level = LogLevel.Debug,
        Message = "Processed more than 10000 bars: {processed} for `{symbol} {exchange} {start} - {end}`",
        EventName = "ProcessedMoreThan10000Bars")]
    public partial void LogProcessedMoreThan10000Bars(int processed, string symbol, string exchange, DateTime start,
                                                      DateTime end);

    public DataService(Symbol symbol, string dataDir = "") {
        Symbol      = symbol;
        DataDir     = dataDir;
        this.Logger = Logging.Application.CreateDefaultLogger(typeof(DataService));
    }

    /// <summary>
    ///     Parameters for getting historical data.
    /// </summary>
    public class History {
        public class Bar {
            public int StartSsboe;
            public int StartUsecs;

            public int EndSsboe;
            public int EndUsecs;

            public double OpenPrice;
            public double HighPrice;
            public double LowPrice;
            public double ClosePrice;

            public double SettlementPrice;

            public double Vwap;
            public double VwapLong;
            public long   Volume;
            public int    NumTrades;

            public long BuyerAsAggressorVolume;
            public bool BuyerAsAggressorVolumeFlag;

            public long SellerAsAggressorVolume;
            public bool SellerAsAggressorVolumeFlag;

            public static Bar FromBarInfo(BarInfo bi) {
                return new Bar() {
                    StartSsboe = bi.StartSsboe,
                    StartUsecs = bi.StartUsecs,
                    EndSsboe   = bi.EndSsboe,
                    EndUsecs   = bi.EndUsecs,

                    OpenPrice  = bi.OpenPrice,
                    HighPrice  = bi.HighPrice,
                    LowPrice   = bi.LowPrice,
                    ClosePrice = bi.ClosePrice,

                    Vwap     = bi.Vwap,
                    VwapLong = bi.VwapLong,

                    SettlementPrice = bi.SettlementPrice,

                    Volume    = bi.Volume,
                    NumTrades = bi.NumTrades,

                    BuyerAsAggressorVolumeFlag = bi.BuyerAsAggressorVolumeFlag,
                    BuyerAsAggressorVolume     = bi.BuyerAsAggressorVolume,

                    SellerAsAggressorVolumeFlag = bi.SellerAsAggressorVolumeFlag,
                    SellerAsAggressorVolume     = bi.SellerAsAggressorVolume,
                };
            }

            public static BarInfo ToBarInfo(Bar bar) {
                return new BarInfo() {
                    StartSsboe = bar.StartSsboe,
                    StartUsecs = bar.StartUsecs,
                    EndSsboe   = bar.EndSsboe,
                    EndUsecs   = bar.EndUsecs,

                    OpenPrice  = bar.OpenPrice,
                    HighPrice  = bar.HighPrice,
                    LowPrice   = bar.LowPrice,
                    ClosePrice = bar.ClosePrice,

                    Vwap     = bar.Vwap,
                    VwapLong = bar.VwapLong,

                    SettlementPrice = bar.SettlementPrice,

                    Volume    = bar.Volume,
                    NumTrades = bar.NumTrades,

                    BuyerAsAggressorVolumeFlag = bar.BuyerAsAggressorVolumeFlag,
                    BuyerAsAggressorVolume     = bar.BuyerAsAggressorVolume,

                    SellerAsAggressorVolumeFlag = bar.SellerAsAggressorVolumeFlag,
                    SellerAsAggressorVolume     = bar.SellerAsAggressorVolume,
                };
            }

            public static TradeInfo ToTradeInfo(Bar bar) {
                return new TradeInfo() {
                    Vwap          = bar.Vwap,
                    VwapLong      = bar.VwapLong,
                    Size          = bar.Volume,
                    Price         = bar.OpenPrice,
                    Ssboe         = bar.StartSsboe,
                    Usecs         = bar.StartUsecs,
                    AggressorSide = bar.BuyerAsAggressorVolumeFlag ? "B" : "S",
                };
            }

            public static IEnumerable<Bar> FromBarInfoCollection(IReadOnlyCollection<BarInfo> c) {
                return c.Select(bi => { return Bar.FromBarInfo(bi); });
            }

            public static IEnumerable<BarInfo> ToBarInfoCollection(IReadOnlyCollection<Bar> c) {
                return c.Select(b => { return Bar.ToBarInfo(b); });
            }

            public static IEnumerable<TradeInfo> ToTradeInfoCollection(IReadOnlyCollection<Bar> c) {
                return c.Select(b => { return Bar.ToTradeInfo(b); });
            }
        }

        public class Opts {
            public static readonly string
                DateFormat = "yyyy-MM-dd-HH-mm-ss-fff";

            public DateTime End;
            public DateTime Start;
            public BarType  BarType;
            public double   BarPeriod;

            public int StartSsboe => (int)Start.Subtract(UnixEpochDate).TotalSeconds;
            public int EndSsboe   => (int)End.Subtract(UnixEpochDate).TotalSeconds;
        }

        public class File {
            public DateTime Start;
            public DateTime End;
            public string   Path;

            public int StartSsboe => (int)Start.Subtract(UnixEpochDate).TotalSeconds;
            public int EndSsboe   => (int)End.Subtract(UnixEpochDate).TotalSeconds;
        }
    }

    public string HistoryDirPath(History.Opts opts) =>
        Path.Combine(DataDir, $"{Symbol.Exchange}_{Symbol.Name}_{opts.BarType}_{opts.BarPeriod}");

    public string HistoryFilePath(History.Opts opts) =>
        Path.Combine(HistoryDirPath(opts),
                     $"{opts.Start.ToString(History.Opts.DateFormat)}_{opts.End.ToString(History.Opts.DateFormat)}.parquet");

    public History.File HistoryFileFromFilePath(string filePath) {
        var fileName  = Path.GetFileName(filePath).Replace(".parquet", "");
        var fileSplit = fileName.Split('_');
        var startStr  = fileSplit[0];
        var endStr    = fileSplit[1];
        var start     = DateTime.ParseExact(startStr, History.Opts.DateFormat, CultureInfo.InvariantCulture);
        var end       = DateTime.ParseExact(endStr,   History.Opts.DateFormat, CultureInfo.InvariantCulture);
        return new History.File() {
            Start = start,
            End   = end,
            Path  = filePath,
        };
    }

    public static IEnumerable<(History.File file, int period)> BestFilesForOpt(
        List<History.File> fileList, History.Opts opts) {
        return fileList.Select(f => {
            var start = f.StartSsboe > opts.StartSsboe ? f.StartSsboe : opts.StartSsboe;
            var end   = f.EndSsboe < opts.EndSsboe ? f.EndSsboe : opts.EndSsboe;
            return (f, period: end - start);
        }).OrderByDescending(t => t.period);
    }

    /// <summary>
    ///    Gets historical data. If missing, requests it from the server.
    /// </summary>
    public async Task<IEnumerable<History.Bar>> GetHistory(History.Opts opts) {
        // make sure history directory exists
        var histDir = HistoryDirPath(opts);
        if (!Directory.Exists(histDir)) {
            Directory.CreateDirectory(histDir);
        }

        // get list of files
        var fileList = Directory.GetFiles(HistoryDirPath(opts));

        // if there are no files, request data from the server
        if (fileList.Length == 0) {
            var q = new ConcurrentQueue<BarInfo>();
            await ReplayBars(opts.Start, opts.End, opts.BarPeriod, opts.BarType, q);
            var barInfos = History.Bar.FromBarInfoCollection(q).ToList();
            ParquetSerializer.SerializeAsync(barInfos, HistoryFilePath(opts));
            return barInfos;
        }

        // select best file
        var f = BestFilesForOpt(fileList.Select(HistoryFileFromFilePath).ToList(), opts).First().file;

        // 1. file contains all data from requested period
        if (f.StartSsboe <= opts.StartSsboe && f.EndSsboe >= opts.EndSsboe) {
            var data = await ParquetSerializer.DeserializeAsync<History.Bar>(f.Path);
            return data.Where(b => b.StartSsboe >= opts.StartSsboe && b.EndSsboe <= opts.EndSsboe).ToList();
        }

        // 2. file contains no data from requested period
        if (f.StartSsboe > opts.EndSsboe || f.EndSsboe < opts.StartSsboe) {
            var q = new ConcurrentQueue<BarInfo>();
            await ReplayBars(opts.Start, opts.End, opts.BarPeriod, opts.BarType, q);
            var barInfos = History.Bar.FromBarInfoCollection(q).ToList();
            ParquetSerializer.SerializeAsync(barInfos, HistoryFilePath(opts));
            return barInfos;
        }

        // 3. file contains start but not end of requested period
        if (f.StartSsboe <= opts.StartSsboe && f.EndSsboe < opts.EndSsboe) {
            // read old data and join with new data requested from server
            var start = f.End;
            var end   = opts.End;

            // old data
            var oldBars = await ParquetSerializer.DeserializeAsync<History.Bar>(f.Path);

            // new data
            var q = new ConcurrentQueue<BarInfo>();
            await ReplayBars(start, end, opts.BarPeriod, opts.BarType, q);
            var newBars = History.Bar.FromBarInfoCollection(q).ToList();

            // join
            var data = oldBars.Concat(newBars);
            ParquetSerializer.SerializeAsync(data, HistoryFilePath(opts));
            return data;
        }

        // 4. file contains end but not start of requested period
        if (f.StartSsboe > opts.StartSsboe && f.EndSsboe >= opts.EndSsboe) {
            // read old data and join with new data requested from server
            var start = opts.Start;
            var end   = f.Start;

            // old data
            var oldBars = await ParquetSerializer.DeserializeAsync<History.Bar>(f.Path);

            // new data
            var q = new ConcurrentQueue<BarInfo>();
            await ReplayBars(start, end, opts.BarPeriod, opts.BarType, q);
            var newBars = History.Bar.FromBarInfoCollection(q).ToList();

            // join
            var data = newBars.Concat(oldBars);
            ParquetSerializer.SerializeAsync(data, HistoryFilePath(opts));
            return data;
        }

        // 5. file contains part of requested period
        if (f.StartSsboe > opts.StartSsboe && f.EndSsboe < opts.EndSsboe) {
            // read old data and join with new data requested from server
            var start1 = opts.Start;
            var end1   = f.Start;
            var start2 = f.End;
            var end2   = opts.End;

            // old data
            var oldBars = await ParquetSerializer.DeserializeAsync<History.Bar>(f.Path);

            // new data 1
            var q1 = new ConcurrentQueue<BarInfo>();
            await ReplayBars(start1, end1, opts.BarPeriod, opts.BarType, q1);
            var newBars1 = History.Bar.FromBarInfoCollection(q1).ToList();

            // new data 2
            var q2 = new ConcurrentQueue<BarInfo>();
            await ReplayBars(start2, end2, opts.BarPeriod, opts.BarType, q2);
            var newBars2 = History.Bar.FromBarInfoCollection(q2).ToList();

            // join
            var data = newBars1.Concat(oldBars).Concat(newBars2);
            ParquetSerializer.SerializeAsync(data, HistoryFilePath(opts));
            return data;
        }
        else {
            throw new Exception("Unknown case");
        }
    }

    private static readonly DateTime UnixEpochDate =
        new(1970, 1, 1);

    public async Task ReplayBars(DateTime                 start, DateTime end, double barPeriod, BarType barType,
                                 ConcurrentQueue<BarInfo> queue, Action<BarInfo>? onBar = null) {
        LogReplayingBars(Symbol.Name, Symbol.Exchange, start, end);

        var            processed      = 0;
        BarReplayInfo? lastBarReplay  = null;
        BarInfo?       lastBar        = null;
        var            barTimersSsboe = new List<int>();
        var            barTimersUsecs = new List<int>();
        var            ctx            = new Context();
        
        Symbol.H.BarClb.Subscribe(ctx, (_, info) => {
            // bug in Rithmic API 10002 bars
            if (processed <= 10000) {
                onBar?.Invoke(info);
                lastBar = info;
                barTimersSsboe.Add(info.EndSsboe);
                barTimersUsecs.Add(info.EndUsecs);
                queue.Enqueue(info);
            }
            processed++;
            if (processed > 10000) {
                LogProcessedMoreThan10000Bars(processed, Symbol.Name, Symbol.Exchange, start, end);
            }
        });

        var tcs = new TaskCompletionSource<BarReplayInfo>();
        Symbol.H.BarReplayClb.Subscribe(ctx, (_, info) => {
            lastBarReplay = info;
            tcs.SetResult(info);
        });

        var getBarReplayParams = () =>
            new ReplayBarParams() {
                Context        = ctx,
                Type           = barType,
                SpecifiedRange = barPeriod,
                Symbol         = Symbol.Name,
                Exchange       = Symbol.Exchange,

                SpecifiedMinutes = Convert.ToInt32(barPeriod),
                SpecifiedSeconds = Convert.ToInt32(barPeriod),
                SpecifiedTicks   = Convert.ToInt32(barPeriod),
                SpecifiedVolume  = Convert.ToInt32(barPeriod),
            };

        var replay =
            async (
                DateTime startReplay,
                DateTime endReplay
            ) => {
                var startCcyymmdd = startReplay.ToString("yyyyMMdd");
                var endCcyymmdd   = endReplay.ToString("yyyyMMdd");
                var startSsboe    = (int)startReplay.Subtract(UnixEpochDate).TotalSeconds;
                var endSsboe      = (int)endReplay.Subtract(UnixEpochDate).TotalSeconds;

                processed = 0;
                var replayParams = getBarReplayParams();
                replayParams.StartSsboe    = startSsboe;
                replayParams.EndSsboe      = endSsboe;
                replayParams.StartCcyymmdd = startCcyymmdd;
                replayParams.EndCcyymmdd   = endCcyymmdd;
                replayParams.StartUsecs    = 0;
                replayParams.EndUsecs      = 999999;
                tcs                        = new TaskCompletionSource<BarReplayInfo>();
                Symbol.E.replayBars(replayParams);
                await tcs.Task;
            };

        try {
            do {
                barTimersSsboe.Clear();
                barTimersUsecs.Clear();
                await replay(start, end);

                // we received error
                if (lastBarReplay != null && lastBarReplay.RpCode != 0) {
                    throw new OMException(lastBarReplay.RpCode, null);
                }

                // we received bars
                if (lastBar != null) {
                    start = DateTimeOffset.FromUnixTimeSeconds(barTimersSsboe.Max()).AddMicroseconds(barTimersUsecs.Max()).DateTime;
                }

                // continue replaying
            } while (processed >= 10000);
        }

        finally {
            Symbol.H.BarClb.Unsubscribe(ctx);
            Symbol.H.BarReplayClb.Unsubscribe(ctx);
        }
    }
}