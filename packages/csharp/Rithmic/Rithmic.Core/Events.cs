namespace Rithmic.Core;

/// <summary>
///     Wraps Ask and Bid info into a single object.
/// </summary>
public class BestBidAskQuoteInfo(rapi.AskInfo askInfo, rapi.BidInfo bidInfo) {
    public rapi.AskInfo AskInfo { get; } = askInfo;
    public rapi.BidInfo BidInfo { get; } = bidInfo;
}

/// <summary>
///     Wraps Ask info to distinguish it from BestAskQuoteInfo.
/// </summary>
public class BestAskQuoteInfo(rapi.AskInfo askInfo) {
    public rapi.AskInfo AskInfo { get; } = askInfo;
}

/// <summary>
///     Wraps Bid info to distinguish it from BestBidQuoteInfo.
/// </summary>
public class BestBidQuoteInfo(rapi.BidInfo bidInfo) {
    public rapi.BidInfo BidInfo { get; } = bidInfo;
}