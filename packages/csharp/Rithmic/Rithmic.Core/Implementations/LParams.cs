namespace Rithmic.Core;

/// <summary>
/// Parameters for the Client login method.
/// </summary>
public class LParams : ILParams {
    public bool         PlugInMode               { get; }
    public IConnection? PnlConnection            { get; }
    public IConnection? MarketDataConnection     { get; }
    public IConnection? HistoricalDataConnection { get; }
    public IConnection? TradingSystemConnection  { get; }

    public LParams(
        bool   plugInMode,
        string login,
        string password) {
        //
        PlugInMode = plugInMode;

        PnlConnection            = new Connection(login, password, rapi.ConnectionId.PnL);
        HistoricalDataConnection = new Connection(login, password, rapi.ConnectionId.History);
        MarketDataConnection     = new Connection(login, password, rapi.ConnectionId.MarketData);
        TradingSystemConnection  = new Connection(login, password, rapi.ConnectionId.TradingSystem);
    }

    // ReSharper disable once ConvertToPrimaryConstructor
    public LParams(
        bool         plugInMode,
        IConnection? pnlConnection            = null,
        IConnection? marketDataConnection     = null,
        IConnection? historicalDataConnection = null,
        IConnection? tradingSystemConnection  = null
    ) {
        //
        PlugInMode = plugInMode;

        PnlConnection            = pnlConnection;
        MarketDataConnection     = marketDataConnection;
        TradingSystemConnection  = tradingSystemConnection;
        HistoricalDataConnection = historicalDataConnection;
    }

    public IEnumerable<IConnection> ConnectionsEnumerable {
        get {
            if (MarketDataConnection != null) {
                yield return MarketDataConnection;
            }

            if (HistoricalDataConnection != null) {
                yield return HistoricalDataConnection;
            }

            if (TradingSystemConnection != null) {
                yield return TradingSystemConnection;
            }

            if (PnlConnection != null) {
                yield return PnlConnection;
            }
        }
    }
}