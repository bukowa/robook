using System.ComponentModel;
using System.Runtime.CompilerServices;
using com.omnesys.omne.om;
using com.omnesys.rapi;

namespace Rithmic.Symbol;

using Core;

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


    private bool _gatheredInformation;

    /// <summary>
    /// Get information about the instrument like tick size, point value, etc.
    /// Anything that is kind of static and doesn't change often.
    /// </summary>
    public async Task GetInformationAsync(IRithmicService service) {
        if (_gatheredInformation) {
            return;
        }

        await GetRefDataAsync(service);
        await GetPriceIncrInfoAsync(service);
        _gatheredInformation = true;
    }

    public Task<PriceIncrInfo> GetPriceIncrInfoAsync(IRithmicService service) {
        var ctx = new ContextOnce();
        var tcs = new TaskCompletionSource<PriceIncrInfo>();
        service.RCallbacksFacade.PriceIncrUpdateDispatcher.RegisterHandler(ctx, (_, info) => {
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
        service.REngineOperations?.REngine.getPriceIncrInfo(Exchange, Name, ctx);
        return tcs.Task;
    }

    public Task<RefDataInfo> GetRefDataAsync(IRithmicService service) {
        var ctx = new ContextOnce();
        var tcs = new TaskCompletionSource<RefDataInfo>();
        service.RCallbacksFacade.RefDataDispatcher.RegisterHandler(ctx, (_, info) => {
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
        service.REngineOperations?.REngine.getRefData(Exchange, Name, ctx);
        return tcs.Task;
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