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
    

    private void _subscribeStream() {
        // Prints
        H.TradePrintClb.Subscribe(_subscriptionContext, (_, info) => {
            TradeInfo = info;
        });

        // Quotes
        H.AskQuoteClb.Subscribe(_subscriptionContext, (_, info) => {
            // Console.WriteLine(info);
        });
        
        H.BidQuoteClb.Subscribe(_subscriptionContext, (_, info) => {
            // Console.WriteLine(info);
        });
        H.EndQuoteClb.Subscribe(_subscriptionContext, (_, info) => {
            Console.WriteLine(info);
        });
        H.LimitOrderBookClb.Subscribe(_subscriptionContext, (_, info) => {
            Console.WriteLine(info);
        });
        
        // Best
        H.BestAskQuoteClb.Subscribe(_subscriptionContext, (_, info) => {
            Console.WriteLine(info);
        });
        H.BestBidAskQuoteClb.Subscribe(_subscriptionContext, (_, info) => {
            Console.WriteLine(info);
        });
        H.BestBidQuoteClb.Subscribe(_subscriptionContext, (_, info) => {
            Console.WriteLine(info);
        });
        
        // Close
        H.ClosePriceClb.Subscribe(_subscriptionContext, (_, info) => {
            Console.WriteLine(info);
            ClosePriceInfo = info;
        });
        
        // PrintsCond
        H.TradeConditionClb.Subscribe(_subscriptionContext, (_, info) => {
            Console.WriteLine(info);            
        });
        
        // Settlement
        H.SettlementPriceClb.Subscribe(_subscriptionContext, (_, info) => {
            Console.WriteLine(info);
        });
        
        // Open
        H.OpenPriceClb.Subscribe(_subscriptionContext, (_, info) => {
            Console.WriteLine(info);
        });
        
        // MarketMode
        H.MarketModeClb.Subscribe(_subscriptionContext, (_, info) => {
            Console.WriteLine(info);
        });
        
        // HighLow
        H.HighPriceClb.Subscribe(_subscriptionContext, (_, info) => {
            Console.WriteLine(info);
        });
        
        H.LowPriceClb.Subscribe(_subscriptionContext, (_, info) => {
            Console.WriteLine(info);
        });
        
        // TradeVolume
        H.TradeVolumeClb.Subscribe(_subscriptionContext, (_, info) => {
            Console.WriteLine(info);
        });
        
        // ClosingIndicator
        H.ClosingIndicatorClb.Subscribe(_subscriptionContext, (_, info) => {
            Console.WriteLine(info);
        });
        
        // OpeningIndicator
        H.OpeningIndicatorClb.Subscribe(_subscriptionContext, (_, info) => {
            Console.WriteLine(info);
        });
        
        // OpenInterest
        H.OpenInterestClb.Subscribe(_subscriptionContext, (_, info) => {
            Console.WriteLine(info);
        });
        
        // RefData
        H.RefDataClb.Subscribe(_subscriptionContext, (_, info) => {
            Console.WriteLine(info);
        });
        
        // MidPrice
        H.CloseMidPriceClb.Subscribe(_subscriptionContext, (_, info) => {
            Console.WriteLine(info);
        });
        H.MidPriceClb.Subscribe(_subscriptionContext, (_, info) => {
            Console.WriteLine(info);
        });
        
        // HighBidLowAsk
        H.HighBidPriceClb.Subscribe(_subscriptionContext, (_, info) => {
            Console.WriteLine(info);
        });
        H.LowAskPriceClb.Subscribe(_subscriptionContext, (_, info) => {
            Console.WriteLine(info);
        });
        
        // PriceLimit
        H.HighPriceLimitClb.Subscribe(_subscriptionContext, (_, info) => {
            Console.WriteLine(info);
        });
        H.LowPriceLimitClb.Subscribe(_subscriptionContext, (_, info) => {
            Console.WriteLine(info);
        });
        
        // ProjectedSettlement
        H.ProjectedSettlementPriceClb.Subscribe(_subscriptionContext, (_, info) => {
            Console.WriteLine(info);
        });
        
        E.subscribe(
            Symbol.Exchange, Symbol.Name,
            SubscriptionFlags.All, _subscriptionContext
        );
    }

    private void _unsubscribeStream() {
        Connection.Client.Engine.unsubscribe(
            Symbol.Exchange, Symbol.Name
        );
    }

    public void StopStream() {
        _unsubscribeStream();
    }

    private CancellationTokenSource _cts = new();

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
            _isStarted = true;
        }
        
        SpinWait sw = new();

        while (!_cts.Token.IsCancellationRequested) {
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
    private REngine  E => Connection.Client.Engine;
    
    /// <summary>
    /// Tick size of the instrument.
    /// </summary>
    public double? TickSize {
        get => _tickSize;
        set {
            SetField(ref _tickSize, value);
            OnPropertyChanged(nameof(TickValue));
        }
    }

    private double? _tickSize;

    /// <summary>
    /// Point value of the instrument.
    /// </summary>
    public double? PointValue {
        get => _pointValue;
        set {
            SetField(ref _pointValue, value);
            OnPropertyChanged(nameof(TickValue));
        }
    }

    private double? _pointValue;

    /// <summary>
    /// Tick value of the instrument calculated from <see cref="TickSize"/> and <see cref="PointValue"/>.
    /// </summary>
    public double? TickValue =>
        TickSize.HasValue && PointValue.HasValue
            ? TickSize * PointValue
            : null;

    public Task<PriceIncrInfo> PriceIncrInfoTask() {
        var ctx = new ContextOnce();
        var tcs = new TaskCompletionSource<PriceIncrInfo>();
        H.PriceIncrUpdateClb.Subscribe(ctx, (_, info) => {
            switch (info.RpCode) {
                case 0:
                    if (info.Rows.Count > 0) {
                        TickSize = info.Rows[0].PriceIncr;
                    }
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
                    PointValue = info.SinglePointValue;
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
    
    private TradeInfo? _tradeInfo;
    private TradeInfo? TradeInfo {
        get => _tradeInfo;
        set {
            _tradeInfo = value;
            OnPropertyChanged(nameof(LastTradePrice));
        }
    }
    
    [DisplayName("Last Trade Price")]
    public decimal? LastTradePrice {
        get {
            if (TradeInfo == null) return null;
            return (decimal)TradeInfo.Price;
        }
    }
    
    
    private ClosePriceInfo? _closePriceInfo;
    private ClosePriceInfo? ClosePriceInfo {
        get => _closePriceInfo;
        set {
            _closePriceInfo = value;
            OnPropertyChanged(nameof(ClosePrice));
        }
    }

    [DisplayName("Close Price")]
    public decimal? ClosePrice {
        get {
            if (ClosePriceInfo == null) return null;
            return (decimal)ClosePriceInfo.Price;
        }
    }
}