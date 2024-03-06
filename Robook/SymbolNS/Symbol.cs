using System.ComponentModel;
using System.Runtime.CompilerServices;
using com.omnesys.omne.om;
using com.omnesys.rapi;
using Rithmic;
using Robook.OrderBookNS;

namespace Robook.SymbolNS;

/// <summary>
/// Represents a single instrument on a single exchange.
/// </summary>
public class Symbol : INotifyPropertyChanged {
    /// <summary>
    /// Constructor.
    /// </summary>
    public Symbol(string exchange, string name) {
        Exchange = exchange;
        Name     = name;
    }

    /// <summary>
    /// Name of the instrument.
    /// </summary>
    public readonly string Name;

    /// <summary>
    /// Exchange on which the instrument is traded.
    /// </summary>
    public readonly string Exchange;

    /// <summary>
    /// Client that is used to interact with the instrument.
    /// </summary>
    public Client Client { get; private set; }

    /// <summary>
    /// Sets the client that is used to interact with the instrument.
    /// </summary>
    /// <param name="client"></param>
    public void SetClient(Client client) {
        Client = client;
        NotifyPropertyChanged(nameof(Client));
    }

    /// <summary>
    /// Represents the best bid price of the instrument.
    /// </summary>
    public double? BestBid {
        get => _bestBid;
        set => SetProperty(ref _bestBid, value);
    }

    private double? _bestBid;

    /// <summary>
    /// Represents the best ask price of the instrument.
    /// </summary>
    public double? BestAsk {
        get => _bestAsk;
        set => SetProperty(ref _bestAsk, value);
    }

    private double? _bestAsk;


    /// <summary>
    /// Tick size of the instrument.
    /// </summary>
    public double? TickSize {
        get => _tickSize;
        set => SetProperty(ref _tickSize, value);
    }

    private double? _tickSize;

    /// <summary>
    /// Point value of the instrument.
    /// </summary>
    public double? PointValue {
        get => _pointValue;
        set => SetProperty(ref _pointValue, value);
    }

    private double? _pointValue;

    /// <summary>
    /// Tick value of the instrument calculated from <see cref="TickSize"/> and <see cref="PointValue"/>.
    /// </summary>
    public double? TickValue =>
        TickSize.HasValue && PointValue.HasValue
            ? TickSize * PointValue
            : null;

    public RHandler? H => Client.RHandler;
    public REngine?  E => Client.Engine;

    public Task<PriceIncrInfo> GetPriceIncrInfoAsync() {
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
        E.getPriceIncrInfo(Exchange, Name, ctx);
        return tcs.Task;
    }

    public Task<RefDataInfo> GetRefDataAsync() {
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
        E.getRefData(Exchange, Name, ctx);
        return tcs.Task;
    }

    public Task<OrderBookInfo> RebuildBookAsync() {
        var ctx = new ContextOnce();
        var tcs = new TaskCompletionSource<OrderBookInfo>();
        H.LimitOrderBookClb.Subscribe(ctx, (_, info) => {
            switch (info.RpCode) {
                case 0:
                    tcs.SetResult(info);
                    break;
                default:
                    tcs.SetException(new OMException(info.RpCode, null));
                    break;
            }
        });
        E.rebuildBook(Exchange, Name, ctx);
        return tcs.Task;
    }

    private bool _gatheredInformation = false;

    /// <summary>
    /// Get information about the instrument like tick size, point value, etc.
    /// Anything that is kind of static and doesn't change often.
    /// </summary>
    public async Task GetInformationAsync() {
        if (_gatheredInformation) {
            return;
        }
        try {
            await GetRefDataAsync();
            await GetPriceIncrInfoAsync();
            _gatheredInformation = true;
        }
        catch (Exception e) {
            throw;
        }
    }
    
    /// <summary>
    /// Context used to subscribe to updates about the instrument.
    /// Can be used to unsubscribe from updates or to subscribe additional handlers.
    /// </summary>
    public Context SubscriptionContext { get; } = new();
    
    /// <summary>
    /// Subscribe to updates about the instrument.
    /// </summary>
    /// todo should resubscribe if client/rengine is changed
    public void SubscribeToUpdates() {
        _subscribeToUpdates();
    }
    
    private decimal[] _orderBookAsks;
    private decimal[] _orderBookBids;
    
    private void _subscribeToUpdates() {
        
        // Prints
        Client.RHandler.TradePrintClb.Subscribe(SubscriptionContext, (_, info) => {
            
        });

        // Quotes
        Client.RHandler.AskQuoteClb.Subscribe(SubscriptionContext, (_, info) => {
            Console.WriteLine(info);
        });
        
        Client.RHandler.BidQuoteClb.Subscribe(SubscriptionContext, (_, info) => {
            Console.WriteLine(info);
        });
        Client.RHandler.EndQuoteClb.Subscribe(SubscriptionContext, (_, info) => {
            Console.WriteLine(info);
        });
        Client.RHandler.LimitOrderBookClb.Subscribe(SubscriptionContext, (_, info) => {
            Console.WriteLine(info);
        });
        
        // Best
        Client.RHandler.BestAskQuoteClb.Subscribe(SubscriptionContext, (_, info) => {
            Console.WriteLine(info);
        });
        Client.RHandler.BestBidAskQuoteClb.Subscribe(SubscriptionContext, (_, info) => {
            Console.WriteLine(info);
        });
        Client.RHandler.BestBidQuoteClb.Subscribe(SubscriptionContext, (_, info) => {
            Console.WriteLine(info);
        });
        
        // Close
        Client.RHandler.ClosePriceClb.Subscribe(SubscriptionContext, (_, info) => {
            Console.WriteLine(info);
        });
        
        // PrintsCond
        Client.RHandler.TradeConditionClb.Subscribe(SubscriptionContext, (_, info) => {
            Console.WriteLine(info);            
        });
        
        // Settlement
        Client.RHandler.SettlementPriceClb.Subscribe(SubscriptionContext, (_, info) => {
            Console.WriteLine(info);
        });
        
        // Open
        Client.RHandler.OpenPriceClb.Subscribe(SubscriptionContext, (_, info) => {
            Console.WriteLine(info);
        });
        
        // MarketMode
        Client.RHandler.MarketModeClb.Subscribe(SubscriptionContext, (_, info) => {
            Console.WriteLine(info);
        });
        
        // HighLow
        Client.RHandler.HighPriceClb.Subscribe(SubscriptionContext, (_, info) => {
            Console.WriteLine(info);
        });
        
        Client.RHandler.LowPriceClb.Subscribe(SubscriptionContext, (_, info) => {
            Console.WriteLine(info);
        });
        
        // TradeVolume
        Client.RHandler.TradeVolumeClb.Subscribe(SubscriptionContext, (_, info) => {
            Console.WriteLine(info);
        });
        
        // ClosingIndicator
        Client.RHandler.ClosingIndicatorClb.Subscribe(SubscriptionContext, (_, info) => {
            Console.WriteLine(info);
        });
        
        // OpeningIndicator
        Client.RHandler.OpeningIndicatorClb.Subscribe(SubscriptionContext, (_, info) => {
            Console.WriteLine(info);
        });
        
        // OpenInterest
        Client.RHandler.OpenInterestClb.Subscribe(SubscriptionContext, (_, info) => {
            Console.WriteLine(info);
        });
        
        // RefData
        Client.RHandler.RefDataClb.Subscribe(SubscriptionContext, (_, info) => {
            Console.WriteLine(info);
        });
        
        // MidPrice
        Client.RHandler.CloseMidPriceClb.Subscribe(SubscriptionContext, (_, info) => {
            Console.WriteLine(info);
        });
        Client.RHandler.MidPriceClb.Subscribe(SubscriptionContext, (_, info) => {
            Console.WriteLine(info);
        });
        
        // HighBidLowAsk
        Client.RHandler.HighBidPriceClb.Subscribe(SubscriptionContext, (_, info) => {
            Console.WriteLine(info);
        });
        Client.RHandler.LowAskPriceClb.Subscribe(SubscriptionContext, (_, info) => {
            Console.WriteLine(info);
        });
        
        // PriceLimit
        Client.RHandler.HighPriceLimitClb.Subscribe(SubscriptionContext, (_, info) => {
            Console.WriteLine(info);
        });
        Client.RHandler.LowPriceLimitClb.Subscribe(SubscriptionContext, (_, info) => {
            Console.WriteLine(info);
        });
        
        // ProjectedSettlement
        Client.RHandler.ProjectedSettlementPriceClb.Subscribe(SubscriptionContext, (_, info) => {
            Console.WriteLine(info);
        });
        
        Client?.Engine.subscribe(Exchange, Name, SubscriptionFlags.All, SubscriptionContext);
    }

    #region INotifyPropertyChanged

    /// <summary>
    /// Event that is raised when a property changes.
    /// </summary>
    public event PropertyChangedEventHandler? PropertyChanged;

    private void SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = "") {
        if (!EqualityComparer<T>.Default.Equals(field, value)) {
            field = value;
            NotifyPropertyChanged(propertyName);
        }
    }

    private void NotifyPropertyChanged([CallerMemberName] String propertyName = "") {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    #endregion
}