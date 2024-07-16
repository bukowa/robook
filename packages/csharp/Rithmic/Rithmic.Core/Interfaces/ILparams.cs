namespace Rithmic.Core;

/// <summary>
/// Login parameters.
/// </summary>
public interface ILParams {
    /// <summary>
    /// Determines whether the client is in plug-in mode.
    /// </summary>
    bool PlugInMode { get; }

    /// <summary>
    /// Connections to the Rithmic trading platform.
    /// </summary>
    #region IConnection
    
    IConnection? PnlConnection            { get; }
    IConnection? MarketDataConnection     { get; }
    IConnection? HistoricalDataConnection { get; }
    IConnection? TradingSystemConnection  { get; }

    #endregion

    /// <summary>
    /// Enumerates all connections.
    /// </summary>
    IEnumerable<IConnection> ConnectionsEnumerable { get; }
}