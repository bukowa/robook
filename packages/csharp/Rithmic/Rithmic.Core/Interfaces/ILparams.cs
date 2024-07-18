namespace Rithmic.Core;

/// <summary>
/// Login parameters.
/// </summary>
public interface ILParams
{
    /// <summary>
    /// Determines whether the client is in plug-in mode.
    /// </summary>
    bool PlugInMode { get; }

    /// <summary>
    /// Connections to the Rithmic trading platform.
    /// </summary>
    IConnection? PnlConnection { get; }
    IConnection? MarketDataConnection     { get; }
    IConnection? TradingSystemConnection  { get; }
    IConnection? HistoricalDataConnection { get; }

    /// <summary>
    /// Enumerates all connections.
    /// </summary>
    IEnumerable<IConnection> ConnectionsEnumerable { get; }
}
