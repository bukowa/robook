using System.Collections.Concurrent;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using com.omnesys.omne.om;
using com.omnesys.rapi;
using Rithmic;

namespace Robook.Data;

public class Subscriber {
    public string                  Id    { get; set; } = Guid.NewGuid().ToString();
    public ConcurrentQueue<object> Queue { get; }      = new();
}

public partial class Subscription : INotifyPropertyChanged {
    /// <summary>
    /// Unique identifier for the subscription.
    /// </summary>
    public string Id { get; set; } = Guid.NewGuid().ToString();

    #region Connection

    private Connection? _connection;

    [JsonIgnore]
    public Connection? Connection {
        get => _connection;
        set {
            _connection = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(ConnectionId));
        }
    }

    private readonly string? _connectionId;

    public string? ConnectionId {
        // deserialization
        get => Connection?.Id ?? _connectionId;
        init => _connectionId = value;
    }

    #endregion

    #region Symbol

    private Symbol? _symbol;

    [JsonIgnore]
    public Symbol? Symbol {
        get => _symbol;
        set {
            _symbol = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(SymbolId));
        }
    }

    private readonly string? _symbolId;

    public string? SymbolId {
        // deserialization
        get => Symbol?.Id ?? _symbolId;
        init => _symbolId = value;
    }

    #endregion

    [Browsable(false)] public ConcurrentQueue<object> Queue { get; } = new();

    [Browsable(false)] public ConcurrentBag<Subscriber> Subscribers { get; } = new();


    private readonly IContext _subscriptionContext = new Context();

    public Task GatherDataAsync() {
        return Task.WhenAll([
            PriceIncrInfoTask(),
            RefDataTask()
        ]);
    }

    private void _subscribeStream() {
        // Prints
        H.TradePrintClb.Subscribe(_subscriptionContext, (_, info) => { Console.WriteLine(info); });

        // Quotes
        H.AskQuoteClb.Subscribe(_subscriptionContext, (_, info) => { Console.WriteLine(info); });
        H.BidQuoteClb.Subscribe(_subscriptionContext, (_, info) => { Console.WriteLine(info); });

        // OrderBook
        H.EndQuoteClb.Subscribe(_subscriptionContext, (_,       info) => { Console.WriteLine(info); });
        H.LimitOrderBookClb.Subscribe(_subscriptionContext, (_, info) => { Console.WriteLine(info); });

        // Best
        H.BestAskQuoteClb.Subscribe(_subscriptionContext, (_, info) => {
            BestAskInfo = info.AskInfo;
            Console.WriteLine(info);
        });
        H.BestBidAskQuoteClb.Subscribe(_subscriptionContext, (_, info) => {
            BestAskInfo = info.AskInfo;
            BestBidInfo = info.BidInfo;
            Console.WriteLine(info);
        });
        H.BestBidQuoteClb.Subscribe(_subscriptionContext, (_, info) => {
            BestBidInfo = info.BidInfo;
            Console.WriteLine(info);
        });

        // Close
        H.ClosePriceClb.Subscribe(_subscriptionContext, (_, info) => {
            ClosePriceInfo = info;
            Console.WriteLine(info);
        });

        // PrintsCond
        H.TradeConditionClb.Subscribe(_subscriptionContext, (_, info) => { Console.WriteLine(info); });

        // Settlement
        H.SettlementPriceClb.Subscribe(_subscriptionContext, (_, info) => {
            SettlementPriceInfo = info;
            Console.WriteLine(info);
        });

        // Open
        H.OpenPriceClb.Subscribe(_subscriptionContext, (_, info) => {
            OpenPriceInfo = info;
            Console.WriteLine(info);
        });

        // MarketMode
        H.MarketModeClb.Subscribe(_subscriptionContext, (_, info) => {
            MarketModeInfo = info;
            Console.WriteLine(info);
        });

        // HighLow
        H.HighPriceClb.Subscribe(_subscriptionContext, (_, info) => {
            HighPriceInfo = info;
            Console.WriteLine(info);
        });

        H.LowPriceClb.Subscribe(_subscriptionContext, (_, info) => {
            LowPriceInfo = info;
            Console.WriteLine(info);
        });

        // TradeVolume
        H.TradeVolumeClb.Subscribe(_subscriptionContext, (_, info) => {
            TradeVolumeInfo = info;
            Console.WriteLine(info);
        });

        // ClosingIndicator
        H.ClosingIndicatorClb.Subscribe(_subscriptionContext, (_, info) => {
            ClosingIndicatorInfo = info;
            Console.WriteLine(info);
        });

        // OpeningIndicator
        H.OpeningIndicatorClb.Subscribe(_subscriptionContext, (_, info) => {
            OpeningIndicatorInfo = info;
            Console.WriteLine(info);
        });

        // OpenInterest
        H.OpenInterestClb.Subscribe(_subscriptionContext, (_, info) => {
            OpenInterestInfo = info;
            Console.WriteLine(info);
        });

        // RefData
        H.RefDataClb.Subscribe(_subscriptionContext, (_, info) => {
            RefDataInfo = info;
            Console.WriteLine(info);
        });

        // MidPrice
        H.CloseMidPriceClb.Subscribe(_subscriptionContext, (_, info) => {
            CloseMidPriceInfo = info;
            Console.WriteLine(info);
        });
        H.MidPriceClb.Subscribe(_subscriptionContext, (_, info) => {
            MidPriceInfo = info;
            Console.WriteLine(info);
        });

        // HighBidLowAsk
        H.HighBidPriceClb.Subscribe(_subscriptionContext, (_, info) => { Console.WriteLine(info); });
        H.LowAskPriceClb.Subscribe(_subscriptionContext, (_,  info) => { Console.WriteLine(info); });

        // PriceLimit
        H.HighPriceLimitClb.Subscribe(_subscriptionContext, (_, info) => { Console.WriteLine(info); });
        H.LowPriceLimitClb.Subscribe(_subscriptionContext, (_,  info) => { Console.WriteLine(info); });

        // ProjectedSettlement
        H.ProjectedSettlementPriceClb.Subscribe(_subscriptionContext, (_, info) => { Console.WriteLine(info); });
        E.subscribe(
            Symbol.Exchange, Symbol.Name,
            SubscriptionFlags.All, _subscriptionContext
        );
    }

    private void _unsubscribeStream() {
        E.unsubscribe(
            Symbol.Exchange, Symbol.Name
        );
    }

    public void StopStream() {
        _unsubscribeStream();
    }

    public CancellationTokenSource Cts = new();

    private          bool   _isStreamStarted = false;
    private readonly object _streamLock      = new();

    public void StartStream() {
        _subscribeStream();
    }

    private          bool   _isStarted = false;
    private readonly object _startLock = new object();

    public void Start() {
        lock (_startLock) {
            if (_isStarted) return;
            Cts        = new CancellationTokenSource();
            _isStarted = true;
        }

        SpinWait sw = new();

        while (!Cts.Token.IsCancellationRequested) {
            while (Queue.TryDequeue(out var o)) {
                foreach (var subscriber in Subscribers) {
                    subscriber.Queue.Enqueue(o);
                }
            }

            sw.SpinOnce();
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null) {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null) {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }

    private RHandler H => Connection.Client.RHandler;
    private REngine  E => Connection.Client.REngine;

    // /// <summary>
    // /// Tick size of the instrument.
    // /// </summary>
    // public double? TickSize {
    //     get => _tickSize;
    //     set {
    //         SetField(ref _tickSize, value);
    //         OnPropertyChanged(nameof(TickValue));
    //     }
    // }
    //
    // private double? _tickSize;
    //
    // /// <summary>
    // /// Point value of the instrument.
    // /// </summary>
    // public double? PointValue {
    //     get => _pointValue;
    //     set {
    //         SetField(ref _pointValue, value);
    //         OnPropertyChanged(nameof(TickValue));
    //     }
    // }
    //
    // private double? _pointValue;
    //
    // /// <summary>
    // /// Tick value of the instrument calculated from <see cref="TickSize"/> and <see cref="PointValue"/>.
    // /// </summary>
    // public double? TickValue =>
    //     TickSize.HasValue && PointValue.HasValue
    //         ? TickSize * PointValue
    //         : null;

    public Task<PriceIncrInfo> PriceIncrInfoTask() {
        var ctx = new ContextOnce();
        var tcs = new TaskCompletionSource<PriceIncrInfo>();
        H.PriceIncrUpdateClb.Subscribe(ctx, (_, info) => {
            switch (info.RpCode) {
                case 0:
                    PriceIncrInfo = info;
                    tcs.SetResult(info);
                    break;
                default:
                    tcs.SetException(new OMException(info.RpCode, null));
                    break;
            }
        });
        E.getPriceIncrInfo(Symbol.Exchange, Symbol.Name, ctx);
        return tcs.Task;
    }

    public Task<RefDataInfo> RefDataTask() {
        var ctx = new ContextOnce();
        var tcs = new TaskCompletionSource<RefDataInfo>();
        H.RefDataClb.Subscribe(ctx, (_, info) => {
            switch (info.RpCode) {
                case 0:
                    RefDataInfo = info;
                    tcs.SetResult(info);
                    break;
                default:
                    tcs.SetException(new OMException(info.RpCode, null));
                    break;
            }
        });
        E.getRefData(Symbol.Exchange, Symbol.Name, ctx);
        return tcs.Task;
    }
}

public partial class Subscription {
    #region ClosePrice

    private ClosePriceInfo? _closePriceInfo;

    private ClosePriceInfo? ClosePriceInfo {
        get => _closePriceInfo;
        set => SetField(ref _closePriceInfo, value);
    }

    [DisplayName("Close Price")] public decimal? ClosePrice => (decimal?)ClosePriceInfo?.Price;

    #endregion

    #region BestAskQuote

    private AskInfo? _bestAskQuote;

    private AskInfo? BestAskInfo {
        get => _bestAskQuote;
        set => SetField(ref _bestAskQuote, value);
    }

    [DisplayName("Best Ask")] public decimal? BestAsk => (decimal?)BestAskInfo?.Price;

    [DisplayName("Best Ask Size")] public int? BestAskSize => (int?)BestAskInfo?.Size;

    #endregion

    #region BestBidQuote

    private BidInfo? _bestBidQuote;

    private BidInfo? BestBidInfo {
        get => _bestBidQuote;
        set => SetField(ref _bestBidQuote, value);
    }

    [DisplayName("Best Bid")] public decimal? BestBid => (decimal?)BestBidInfo?.Price;

    [DisplayName("Best Bid Size")] public int? BestBidSize => (int?)BestBidInfo?.Size;

    #endregion

    #region SettlementPrice

    private SettlementPriceInfo? _settlementPriceInfo;

    private SettlementPriceInfo? SettlementPriceInfo {
        get => _settlementPriceInfo;
        set => SetField(ref _settlementPriceInfo, value);
    }

    [DisplayName("Settlement Price")] public decimal? SettlementPrice => (decimal?)SettlementPriceInfo?.Price;

    #endregion

    #region OpenPrice

    private OpenPriceInfo? _openPriceInfo;

    private OpenPriceInfo? OpenPriceInfo {
        get => _openPriceInfo;
        set => SetField(ref _openPriceInfo, value);
    }

    [DisplayName("Open Price")]
    public decimal? OpenPrice {
        get {
            if (OpenPriceInfo == null) return null;
            return (decimal)OpenPriceInfo.Price;
        }
    }

    #endregion

    #region MarketMode

    private MarketModeInfo? _marketModeInfo;

    private MarketModeInfo? MarketModeInfo {
        get => _marketModeInfo;
        set => SetField(ref _marketModeInfo, value);
    }

    [DisplayName("Market Mode")] public string? MarketMode => MarketModeInfo?.MarketMode;

    #endregion

    #region HighPrice

    private HighPriceInfo? _highPriceInfo;

    private HighPriceInfo? HighPriceInfo {
        get => _highPriceInfo;
        set => SetField(ref _highPriceInfo, value);
    }

    [DisplayName("High Price")]
    public decimal? HighPrice {
        get {
            if (HighPriceInfo == null) return null;
            return (decimal)HighPriceInfo.Price;
        }
    }

    #endregion

    #region LowPrice

    private LowPriceInfo? _lowPriceInfo;

    private LowPriceInfo? LowPriceInfo {
        get => _lowPriceInfo;
        set => SetField(ref _lowPriceInfo, value);
    }

    [DisplayName("Low Price")]
    public decimal? LowPrice {
        get {
            if (LowPriceInfo == null) return null;
            return (decimal)LowPriceInfo.Price;
        }
    }

    #endregion

    #region TradeVolume

    private TradeVolumeInfo? _tradeVolumeInfo;

    private TradeVolumeInfo? TradeVolumeInfo {
        get => _tradeVolumeInfo;
        set => SetField(ref _tradeVolumeInfo, value);
    }

    [DisplayName("Trade Volume")] public long? TradeVolume => TradeVolumeInfo?.TotalVolume;

    #endregion

    #region ClosingIndicator

    private ClosingIndicatorInfo? _closingIndicatorInfo;

    private ClosingIndicatorInfo? ClosingIndicatorInfo {
        get => _closingIndicatorInfo;
        set => SetField(ref _closingIndicatorInfo, value);
    }

    [DisplayName("Closing Indicator Price")]
    public decimal? ClosingIndicator {
        get {
            if (ClosingIndicatorInfo == null || double.IsNaN(ClosingIndicatorInfo.Price)) return null;
            return (decimal)ClosingIndicatorInfo.Price;
        }
    }

    #endregion

    #region OpeningIndicator

    private OpeningIndicatorInfo? _openingIndicatorInfo;

    private OpeningIndicatorInfo? OpeningIndicatorInfo {
        get => _openingIndicatorInfo;
        set => SetField(ref _openingIndicatorInfo, value);
    }

    [DisplayName("Opening Indicator Price")]
    public decimal? OpeningIndicator {
        get {
            if (OpeningIndicatorInfo == null || double.IsNaN(OpeningIndicatorInfo.Price)) return null;
            return (decimal)OpeningIndicatorInfo.Price;
        }
    }

    #endregion

    #region OpenInterest

    private OpenInterestInfo? _openInterestInfo;

    private OpenInterestInfo? OpenInterestInfo {
        get => _openInterestInfo;
        set => SetField(ref _openInterestInfo, value);
    }

    [DisplayName("Open Interest")] public long? OpenInterest => OpenInterestInfo?.Quantity;

    #endregion

    #region RefData PriceIncr

    private RefDataInfo? _refDataInfo;

    private RefDataInfo? RefDataInfo {
        get => _refDataInfo;
        set => SetField(ref _refDataInfo, value);
    }

    private PriceIncrInfo? _priceIncrInfo;

    private PriceIncrInfo? PriceIncrInfo {
        get => _priceIncrInfo;
        set => SetField(ref _priceIncrInfo, value);
    }

    [DisplayName("Tick Size")] public decimal? TickSize => (decimal?)PriceIncrInfo?.Rows?[0].PriceIncr;

    [DisplayName("Point Value")] public decimal? PointValue => (decimal?)RefDataInfo?.SinglePointValue;

    [DisplayName("Tick Value")]
    public decimal? TickValue {
        get => TickSize * PointValue;
        init {
            PropertyChanged += (s, e) => {
                if (e.PropertyName == nameof(TickSize) || e.PropertyName == nameof(PointValue)) {
                    OnPropertyChanged();
                }
            };
        }
    }

    #endregion

    #region MidPrice

    private MidPriceInfo? _midPriceInfo;

    private MidPriceInfo? MidPriceInfo {
        get => _midPriceInfo;
        set => SetField(ref _midPriceInfo, value);
    }

    [DisplayName("Last Mid Price")]
    public decimal? MidPrice {
        get {
            if (MidPriceInfo == null || double.IsNaN(MidPriceInfo.LastPrice)) return null;
            return (decimal)MidPriceInfo.LastPrice;
        }
    }

    [DisplayName("Low Mid Price")]
    public decimal? LowMidPrice {
        get {
            if (MidPriceInfo == null || double.IsNaN(MidPriceInfo.LowPrice)) return null;
            return (decimal)MidPriceInfo.LowPrice;
        }
    }

    [DisplayName("High Mid Price")]
    public decimal? HighMidPrice {
        get {
            if (MidPriceInfo == null || double.IsNaN(MidPriceInfo.HighPrice)) return null;
            return (decimal)MidPriceInfo.HighPrice;
        }
    } 

    #endregion

    #region CloseMidPrice

    private CloseMidPriceInfo? _closeMidPriceInfo;

    private CloseMidPriceInfo? CloseMidPriceInfo {
        get => _closeMidPriceInfo;
        set => SetField(ref _closeMidPriceInfo, value);
    }

    [DisplayName("Close Mid Price")] public decimal? CloseMidPrice {
        get
        {
            if (CloseMidPriceInfo == null || double.IsNaN(CloseMidPriceInfo.ClosePrice)) return null;
            return (decimal)CloseMidPriceInfo.ClosePrice;
        }
    }

    [DisplayName("Close Mid Price Date")]
    public string? CloseMidPriceDate {
        get {
            if (CloseMidPriceInfo == null) return null;
            return CloseMidPriceInfo.ClosePriceDate;
        }
    }

    #endregion
}