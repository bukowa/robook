using System.Collections.Concurrent;
using com.omnesys.rapi;
using Microsoft.Extensions.Logging;

namespace Rithmic;

public interface IContext {
    public string  Key   { get; set; }
    public object? Value { get; set; }
    public bool    Once  { get; set; }
}

/// <summary>
///     Represents contextual information associated with an event handler.
/// </summary>
public class Context : IContext {
    /// <summary>
    ///     Gets or sets the key associated with the context.
    /// </summary>
    public string Key { get; set; }

    /// <summary>
    ///     Gets or sets the optional value associated with the context.
    /// </summary>
    public object? Value { get; set; }

    /// <summary>
    ///     Indicates whether the event handler should be removed after the first invocation.
    /// </summary>
    public bool Once { get; set; }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Rithmic.IContext"/> class with the specified key and optional value.
    /// </summary>
    /// <param name="value">The optional value associated with the context.</param>
    public Context(object? value = null) {
        Key   = Guid.NewGuid().ToString();
        Value = value;
    }
}

/// <summary>
///    Represents contextual information associated with an event handler.
///    The event handler will be removed after the first invocation.
/// </summary>
public class ContextOnce : Context {
    /// <summary>
    ///    Initializes a new instance of the <see cref="ContextOnce"/>.
    /// </summary>
    /// <param name="value"></param>
    public ContextOnce(object? value = null) :
        base(value) {
        Once = true;
    }
}

/// <summary>
///    Stores event handlers associated with a specific event.
/// </summary>
/// <typeparam name="T"></typeparam>
public class CallbackManager<T> : ConcurrentDictionary<IContext, List<Action<IContext, T>>> {
    /// <summary>
    /// Adds a new event handler to the dictionary.
    /// </summary>
    /// <param name="ctx"> Context associated with the event handler. </param>
    /// <param name="callback"> Event handler to add. </param>
    public void Subscribe(IContext ctx, Action<IContext, T> callback) {
        this.TryAdd(ctx, new List<Action<IContext, T>> { });
        this[ctx].Add(callback);
    }

    /// <summary>
    ///     Removes the event handler associated with the specified context.
    /// </summary>
    /// <param name="ctx"> Context associated with the event handler. </param>
    public void Unsubscribe(IContext? ctx) {
        TryRemove(ctx, out _);
    }

    /// <summary>
    ///     Removes the event handler associated with the specified key.
    /// </summary>
    /// <param name="key"> Context key associated with the event handler. </param>
    public void Unsubscribe(string key) {
        foreach (var ctx in this.Keys) {
            if (ctx.Key == key) {
                Unsubscribe(ctx);
            }
        }
    }

    /// <summary>
    ///     Invokes the event handler associated with the specified context.
    /// </summary>
    /// <param name="ctx"> Context associated with the event handler. </param>
    /// <param name="info"> Event information. </param>
    public void Invoke(IContext ctx, T info) {
        if (TryGetValue(ctx, out var callback)) {
            foreach (var cb in callback) {
                cb(ctx, info);
            }

            if (ctx.Once) {
                Unsubscribe(ctx);
            }
        }
    }

    /// <summary>
    ///     Invokes all event handlers for the specified event information.
    ///     Passes new <see cref="Context"/> for each event handler with the specified key.
    /// </summary>
    /// <param name="info"></param>
    public void Invoke(T info) {
        foreach (var kvp in this) {
            foreach (var cb in kvp.Value) {
                cb(kvp.Key, info);
            }
        }
    }
}

/// <summary>
///     Wraps Ask and Bid info into a single object.
/// </summary>
public class BestBidAskQuoteInfo {
    public AskInfo AskInfo { get; }
    public BidInfo BidInfo { get; }

    public BestBidAskQuoteInfo(AskInfo askInfo, BidInfo bidInfo) {
        AskInfo = askInfo;
        BidInfo = bidInfo;
    }
}

/// <summary>
///     Implements a handler for AdmCallbacks events with support for dynamic event registration and invocation.
/// </summary>
public class AdmHandler : AdmCallbacks {
    public CallbackManager<AlertInfo>           AlertClb           { get; } = new();
    public CallbackManager<EnvironmentListInfo> EnvironmentListClb { get; } = new();
    public CallbackManager<EnvironmentInfo>     EnvironmentClb     { get; } = new();

    public override void Alert(AlertInfo info) =>
        AlertClb.Invoke(info);

    public virtual void EnvironmentList(EnvironmentListInfo info) =>
        EnvironmentListClb.Invoke(info);

    public virtual void Environment(EnvironmentInfo oInfo) =>
        EnvironmentClb.Invoke(oInfo);
}

/// <summary>
///     Represents a handler for Rithmic events with support for dynamic event registration and invocation.
/// </summary>
/// public class Handler : RCallbacks
public partial class RHandler : RCallbacks {
    public ILogger? Logger;

    public RHandler(ILogger? logger = null) {
        if (logger == null)
            logger = LoggerFactory.Create(builder => {
                builder.AddSimpleConsole();
                builder.AddDebug();
                builder.SetMinimumLevel(LogLevel.Information);
            }).CreateLogger("RHandler");
        this.Logger = logger;
    }

    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Error,
        Message = "Callback `{callbackName}` received null context with `{info}`",
        EventName = "CallbackNullContext")]
    public partial void CallbackNullContext(string callbackName, object? info);


    private void InvokeCallback<T>(CallbackManager<T> clb, T info) => clb.Invoke(info);

    private void InvokeCallback<T>(CallbackManager<T> clb, object context, T info) {
        if (context is IContext ctx) {
            clb.Invoke(ctx, info);
        }
        else {
            CallbackNullContext(clb.ToString() ?? string.Empty, info);
        }
    }

    public CallbackManager<AlertInfo>                    AlertClb                     { get; } = new();
    public CallbackManager<AggregatorInfo>               AggregatorClb                { get; } = new();
    public CallbackManager<AskInfo>                      AskQuoteClb                  { get; } = new();
    public CallbackManager<AuxRefDataInfo>               AuxRefDataClb                { get; } = new();
    public CallbackManager<AskInfo>                      BestAskQuoteClb              { get; } = new();
    public CallbackManager<BestBidAskQuoteInfo>          BestBidAskQuoteClb           { get; } = new();
    public CallbackManager<BidInfo>                      BestBidQuoteClb              { get; } = new();
    public CallbackManager<BidInfo>                      BidQuoteClb                  { get; } = new();
    public CallbackManager<BinaryContractListInfo>       BinaryContractListClb        { get; } = new();
    public CallbackManager<CloseMidPriceInfo>            CloseMidPriceClb             { get; } = new();
    public CallbackManager<ClosePriceInfo>               ClosePriceClb                { get; } = new();
    public CallbackManager<ClosingIndicatorInfo>         ClosingIndicatorClb          { get; } = new();
    public CallbackManager<DboInfo>                      DboClb                       { get; } = new();
    public CallbackManager<DboBookRebuildInfo>           DboBookRebuildClb            { get; } = new();
    public CallbackManager<EndQuoteInfo>                 EndQuoteClb                  { get; } = new();
    public CallbackManager<EquityOptionStrategyListInfo> EquityOptionStrategyListClb  { get; } = new();
    public CallbackManager<HighPriceInfo>                HighPriceClb                 { get; } = new();
    public CallbackManager<HighBidPriceInfo>             HighBidPriceClb              { get; } = new();
    public CallbackManager<HighPriceLimitInfo>           HighPriceLimitClb            { get; } = new();
    public CallbackManager<InstrumentByUnderlyingInfo>   InstrumentByUnderlyingClb    { get; } = new();
    public CallbackManager<InstrumentSearchInfo>         InstrumentSearchClb          { get; } = new();
    public CallbackManager<OrderBookInfo>                LimitOrderBookClb            { get; } = new();
    public CallbackManager<LowPriceInfo>                 LowPriceClb                  { get; } = new();
    public CallbackManager<LowAskPriceInfo>              LowAskPriceClb               { get; } = new();
    public CallbackManager<LowPriceLimitInfo>            LowPriceLimitClb             { get; } = new();
    public CallbackManager<MarketModeInfo>               MarketModeClb                { get; } = new();
    public CallbackManager<MidPriceInfo>                 MidPriceClb                  { get; } = new();
    public CallbackManager<OpenInterestInfo>             OpenInterestClb              { get; } = new();
    public CallbackManager<OpenPriceInfo>                OpenPriceClb                 { get; } = new();
    public CallbackManager<OpeningIndicatorInfo>         OpeningIndicatorClb          { get; } = new();
    public CallbackManager<OptionListInfo>               OptionListCallbacks          { get; } = new();
    public CallbackManager<PriceIncrInfo>                PriceIncrUpdateClb           { get; } = new();
    public CallbackManager<ProjectedSettlementPriceInfo> ProjectedSettlementPriceClb  { get; } = new();
    public CallbackManager<RefDataInfo>                  RefDataClb                   { get; } = new();
    public CallbackManager<SettlementPriceInfo>          SettlementPriceClb           { get; } = new();
    public CallbackManager<StrategyInfo>                 StrategyCallbacks            { get; } = new();
    public CallbackManager<StrategyListInfo>             StrategyListCallbacks        { get; } = new();
    public CallbackManager<TradeInfo>                    TradeConditionClb            { get; } = new();
    public CallbackManager<TradeInfo>                    TradePrintClb                { get; } = new();
    public CallbackManager<TradeVolumeInfo>              TradeVolumeClb               { get; } = new();
    public CallbackManager<UserDefinedSpreadCreateInfo>  UserDefinedSpreadCreateClb   { get; } = new();
    public CallbackManager<UserListInfo>                 UserListCallbacks            { get; } = new();
    public CallbackManager<UserProfileInfo>              UserProfileCallbacks         { get; } = new();
    public CallbackManager<VolumeAtPriceInfo>            VolumeAtPriceCallbacks       { get; } = new();
    public CallbackManager<BarInfo>                      BarClb                       { get; } = new();
    public CallbackManager<BarReplayInfo>                BarReplayClb                 { get; } = new();
    public CallbackManager<TradeReplayInfo>              TradeReplayClb               { get; } = new();
    public CallbackManager<AccountListInfo>              AccountListClb               { get; } = new();
    public CallbackManager<AccountUpdateInfo>            AccountUpdateCallbacks       { get; } = new();
    public CallbackManager<PasswordChangeInfo>           PasswordChangeCallbacks      { get; } = new();
    public CallbackManager<AutoLiquidateInfo>            AutoLiquidateCallbacks       { get; } = new();
    public CallbackManager<BracketReplayInfo>            BracketReplayCallbacks       { get; } = new();
    public CallbackManager<BracketTierModifyInfo>        BracketTierModifyCallbacks   { get; } = new();
    public CallbackManager<BracketInfo>                  BracketUpdateCallbacks       { get; } = new();
    public CallbackManager<EasyToBorrowInfo>             EasyToBorrowCallbacks        { get; } = new();
    public CallbackManager<EasyToBorrowListInfo>         EasyToBorrowListCallbacks    { get; } = new();
    public CallbackManager<ExchangeListInfo>             ExchangeListCallbacks        { get; } = new();
    public CallbackManager<ExecutionReplayInfo>          ExecutionReplayCallbacks     { get; } = new();
    public CallbackManager<IbListInfo>                   IbListCallbacks              { get; } = new();
    public CallbackManager<LineInfo>                     LineUpdateCallbacks          { get; } = new();
    public CallbackManager<OrderReplayInfo>              OpenOrderReplayCallbacks     { get; } = new();
    public CallbackManager<OrderHistoryDatesInfo>        OrderHistoryDatesCallbacks   { get; } = new();
    public CallbackManager<OrderReplayInfo>              OrderReplayCallbacks         { get; } = new();
    public CallbackManager<PnlReplayInfo>                PnlReplayCallbacks           { get; } = new();
    public CallbackManager<PnlInfo>                      PnlUpdateCallbacks           { get; } = new();
    public CallbackManager<PositionExitInfo>             PositionExitCallbacks        { get; } = new();
    public CallbackManager<ProductRmsListInfo>           ProductRmsListCallbacks      { get; } = new();
    public CallbackManager<QuoteInfo>                    QuoteCallbacks               { get; } = new();
    public CallbackManager<QuoteReport>                  QuoteReportCallbacks         { get; } = new();
    public CallbackManager<QuoteReplayInfo>              QuoteReplayCallbacks         { get; } = new();
    public CallbackManager<SingleOrderReplayInfo>        SingleOrderReplayCallbacks   { get; } = new();
    public CallbackManager<SodReport>                    SodUpdateCallbacks           { get; } = new();
    public CallbackManager<TradeRouteInfo>               TradeRouteCallbacks          { get; } = new();
    public CallbackManager<TradeRouteListInfo>           TradeRouteListCallbacks      { get; } = new();
    public CallbackManager<OrderBustReport>              BustReportCallbacks          { get; } = new();
    public CallbackManager<OrderCancelReport>            CancelReportCallbacks        { get; } = new();
    public CallbackManager<OrderFailureReport>           FailureReportCallbacks       { get; } = new();
    public CallbackManager<OrderFillReport>              FillReportCallbacks          { get; } = new();
    public CallbackManager<OrderModifyReport>            ModifyReportCallbacks        { get; } = new();
    public CallbackManager<OrderNotCancelledReport>      NotCancelledReportCallbacks  { get; } = new();
    public CallbackManager<OrderNotModifiedReport>       NotModifiedReportCallbacks   { get; } = new();
    public CallbackManager<OrderReport>                  OtherReportCallbacks         { get; } = new();
    public CallbackManager<OrderRejectReport>            RejectReportCallbacks        { get; } = new();
    public CallbackManager<OrderStatusReport>            StatusReportCallbacks        { get; } = new();
    public CallbackManager<OrderTradeCorrectReport>      TradeCorrectReportCallbacks  { get; } = new();
    public CallbackManager<OrderTriggerPulledReport>     TriggerPulledReportCallbacks { get; } = new();
    public CallbackManager<OrderTriggerReport>           TriggerReportCallbacks       { get; } = new();
    public CallbackManager<AgreementListInfo>            AgreementListCallbacks       { get; } = new();

    public override void AskQuote(AskInfo info) =>
        InvokeCallback(AskQuoteClb, info.Context, info);

    public override void AuxRefData(AuxRefDataInfo info) =>
        InvokeCallback(AuxRefDataClb, info.Context, info);

    public override void BestAskQuote(AskInfo info) =>
        InvokeCallback(BestAskQuoteClb, info.Context, info);

    public override void BestBidAskQuote(BidInfo binfo, AskInfo ainfo) =>
        InvokeCallback(BestBidAskQuoteClb, binfo.Context, new BestBidAskQuoteInfo(ainfo, binfo));

    public override void BestBidQuote(BidInfo info) =>
        InvokeCallback(BestBidQuoteClb, info.Context, info);

    public override void BidQuote(BidInfo info) =>
        InvokeCallback(BidQuoteClb, info.Context, info);

    public override void BinaryContractList(BinaryContractListInfo info) =>
        InvokeCallback(BinaryContractListClb, info.Context, info);

    public override void CloseMidPrice(CloseMidPriceInfo info) =>
        InvokeCallback(CloseMidPriceClb, info.Context, info);

    public override void ClosePrice(ClosePriceInfo info) =>
        InvokeCallback(ClosePriceClb, info.Context, info);

    public override void ClosingIndicator(ClosingIndicatorInfo info) =>
        InvokeCallback(ClosingIndicatorClb, info.Context, info);

    public override void Dbo(DboInfo info) =>
        InvokeCallback(DboClb, info.Context, info);

    public override void DboBookRebuild(DboBookRebuildInfo info) =>
        InvokeCallback(DboBookRebuildClb, info.Context, info);

    public override void EndQuote(EndQuoteInfo info) =>
        InvokeCallback(EndQuoteClb, info.Context, info);

    public override void EquityOptionStrategyList(EquityOptionStrategyListInfo info) =>
        InvokeCallback(EquityOptionStrategyListClb, info.Context, info);

    public override void HighPrice(HighPriceInfo info) =>
        InvokeCallback(HighPriceClb, info.Context, info);

    public override void HighBidPrice(HighBidPriceInfo info) =>
        InvokeCallback(HighBidPriceClb, info.Context, info);

    public override void HighPriceLimit(HighPriceLimitInfo info) =>
        InvokeCallback(HighPriceLimitClb, info.Context, info);

    public override void InstrumentByUnderlying(InstrumentByUnderlyingInfo info) =>
        InvokeCallback(InstrumentByUnderlyingClb, info.Context, info);

    public override void InstrumentSearch(InstrumentSearchInfo info) =>
        InvokeCallback(InstrumentSearchClb, info.Context, info);

    public override void LimitOrderBook(OrderBookInfo info) =>
        InvokeCallback(LimitOrderBookClb, info.Context, info);

    public override void LowPrice(LowPriceInfo info) =>
        InvokeCallback(LowPriceClb, info.Context, info);

    public override void LowAskPrice(LowAskPriceInfo info) =>
        InvokeCallback(LowAskPriceClb, info.Context, info);

    public override void LowPriceLimit(LowPriceLimitInfo info) =>
        InvokeCallback(LowPriceLimitClb, info.Context, info);

    public override void MarketMode(MarketModeInfo info) =>
        InvokeCallback(MarketModeClb, info.Context, info);

    public override void MidPrice(MidPriceInfo info) =>
        InvokeCallback(MidPriceClb, info.Context, info);

    public override void OpenInterest(OpenInterestInfo info) =>
        InvokeCallback(OpenInterestClb, info.Context, info);

    public override void OpenPrice(OpenPriceInfo info) =>
        InvokeCallback(OpenPriceClb, info.Context, info);

    public override void OpeningIndicator(OpeningIndicatorInfo info) =>
        InvokeCallback(OpeningIndicatorClb, info.Context, info);

    public override void OptionList(OptionListInfo info) =>
        InvokeCallback(OptionListCallbacks, info.Context, info);

    public override void PriceIncrUpdate(PriceIncrInfo info) =>
        InvokeCallback(PriceIncrUpdateClb, info.Context, info);

    public override void ProjectedSettlementPrice(ProjectedSettlementPriceInfo info) =>
        InvokeCallback(ProjectedSettlementPriceClb, info.Context, info);

    public override void RefData(RefDataInfo info) =>
        InvokeCallback(RefDataClb, info.Context, info);

    public override void SettlementPrice(SettlementPriceInfo info) =>
        InvokeCallback(SettlementPriceClb, info.Context, info);

    public override void Strategy(StrategyInfo info) =>
        InvokeCallback(StrategyCallbacks, info.Context, info);

    public override void StrategyList(StrategyListInfo info) =>
        InvokeCallback(StrategyListCallbacks, info.Context, info);

    public override void TradeCondition(TradeInfo info) =>
        InvokeCallback(TradeConditionClb, info.Context, info);

    public override void TradePrint(TradeInfo info) =>
        InvokeCallback(TradePrintClb, info.Context, info);

    public override void TradeVolume(TradeVolumeInfo info) =>
        InvokeCallback(TradeVolumeClb, info.Context, info);

    public override void UserDefinedSpreadCreate(UserDefinedSpreadCreateInfo info) =>
        InvokeCallback(UserDefinedSpreadCreateClb, info.Context, info);

    public override void UserList(UserListInfo info) =>
        InvokeCallback(UserListCallbacks, info.Context, info);

    public override void UserProfile(UserProfileInfo info) =>
        InvokeCallback(UserProfileCallbacks, info.Context, info);

    public override void VolumeAtPrice(VolumeAtPriceInfo info) =>
        InvokeCallback(VolumeAtPriceCallbacks, info.Context, info);

    public override void Bar(BarInfo info) =>
        InvokeCallback(BarClb, info.Context, info);

    public override void BarReplay(BarReplayInfo info) =>
        InvokeCallback(BarReplayClb, info.Context, info);

    public override void TradeReplay(TradeReplayInfo info) =>
        InvokeCallback(TradeReplayClb, info.Context, info);

    public override void BracketReplay(BracketReplayInfo info) =>
        InvokeCallback(BracketReplayCallbacks, info.Context, info);

    public override void BracketTierModify(BracketTierModifyInfo info) =>
        InvokeCallback(BracketTierModifyCallbacks, info.Context, info);

    public override void EasyToBorrow(EasyToBorrowInfo info) =>
        InvokeCallback(EasyToBorrowCallbacks, info.Context, info);

    public override void EasyToBorrowList(EasyToBorrowListInfo info) =>
        InvokeCallback(EasyToBorrowListCallbacks, info.Context, info);

    public override void ExchangeList(ExchangeListInfo info) =>
        InvokeCallback(ExchangeListCallbacks, info.Context, info);

    public override void ExecutionReplay(ExecutionReplayInfo info) =>
        InvokeCallback(ExecutionReplayCallbacks, info.Context, info);

    public override void IbList(IbListInfo info) =>
        InvokeCallback(IbListCallbacks, info.Context, info);

    public override void LineUpdate(LineInfo info) =>
        InvokeCallback(LineUpdateCallbacks, info.Context, info);

    public override void OpenOrderReplay(OrderReplayInfo info) =>
        InvokeCallback(OpenOrderReplayCallbacks, info.Context, info);

    public override void OrderHistoryDates(OrderHistoryDatesInfo info) =>
        InvokeCallback(OrderHistoryDatesCallbacks, info.Context, info);

    public override void OrderReplay(OrderReplayInfo info) =>
        InvokeCallback(OrderReplayCallbacks, info.Context, info);

    public override void PnlReplay(PnlReplayInfo info) =>
        InvokeCallback(PnlReplayCallbacks, info.Context, info);

    public override void PositionExit(PositionExitInfo info) =>
        InvokeCallback(PositionExitCallbacks, info.Context, info);

    public override void ProductRmsList(ProductRmsListInfo info) =>
        InvokeCallback(ProductRmsListCallbacks, info.Context, info);

    public override void QuoteReplay(QuoteReplayInfo info) =>
        InvokeCallback(QuoteReplayCallbacks, info.Context, info);

    public override void SingleOrderReplay(SingleOrderReplayInfo info) =>
        InvokeCallback(SingleOrderReplayCallbacks, info.Context, info);

    public override void TradeRoute(TradeRouteInfo info) =>
        InvokeCallback(TradeRouteCallbacks, info.Context, info);

    public override void TradeRouteList(TradeRouteListInfo info) =>
        InvokeCallback(TradeRouteListCallbacks, info.Context, info);

    public override void BustReport(OrderBustReport info) =>
        InvokeCallback(BustReportCallbacks, info.Context, info);

    public override void CancelReport(OrderCancelReport info) =>
        InvokeCallback(CancelReportCallbacks, info.Context, info);

    public override void FailureReport(OrderFailureReport info) =>
        InvokeCallback(FailureReportCallbacks, info.Context, info);

    public override void FillReport(OrderFillReport info) =>
        InvokeCallback(FillReportCallbacks, info.Context, info);

    public override void ModifyReport(OrderModifyReport info) =>
        InvokeCallback(ModifyReportCallbacks, info.Context, info);

    public override void NotCancelledReport(OrderNotCancelledReport info) =>
        InvokeCallback(NotCancelledReportCallbacks, info.Context, info);

    public override void NotModifiedReport(OrderNotModifiedReport info) =>
        InvokeCallback(NotModifiedReportCallbacks, info.Context, info);

    public override void OtherReport(OrderReport info) =>
        InvokeCallback(OtherReportCallbacks, info.Context, info);

    public override void RejectReport(OrderRejectReport info) =>
        InvokeCallback(RejectReportCallbacks, info.Context, info);

    public override void StatusReport(OrderStatusReport info) =>
        InvokeCallback(StatusReportCallbacks, info.Context, info);

    public override void TradeCorrectReport(OrderTradeCorrectReport info) =>
        InvokeCallback(TradeCorrectReportCallbacks, info.Context, info);

    public override void TriggerPulledReport(OrderTriggerPulledReport info) =>
        InvokeCallback(TriggerPulledReportCallbacks, info.Context, info);

    public override void TriggerReport(OrderTriggerReport info) =>
        InvokeCallback(TriggerReportCallbacks, info.Context, info);

    public override void AgreementList(AgreementListInfo info) =>
        InvokeCallback(AgreementListCallbacks, info.Context, info);

    public override void Alert(AlertInfo                   info) => InvokeCallback(AlertClb,                info);
    public override void Aggregator(AggregatorInfo         info) => InvokeCallback(AggregatorClb,           info);
    public override void SodUpdate(SodReport               info) => InvokeCallback(SodUpdateCallbacks,      info);
    public override void PnlUpdate(PnlInfo                 info) => InvokeCallback(PnlUpdateCallbacks,      info);
    public override void Quote(QuoteInfo                   info) => InvokeCallback(QuoteCallbacks,          info);
    public override void Quote(QuoteReport                 info) => InvokeCallback(QuoteReportCallbacks,    info);
    public override void AccountList(AccountListInfo       info) => InvokeCallback(AccountListClb,          info);
    public override void AccountUpdate(AccountUpdateInfo   info) => InvokeCallback(AccountUpdateCallbacks,  info);
    public override void PasswordChange(PasswordChangeInfo info) => InvokeCallback(PasswordChangeCallbacks, info);
    public override void AutoLiquidate(AutoLiquidateInfo   info) => InvokeCallback(AutoLiquidateCallbacks,  info);
    public override void BracketUpdate(BracketInfo         info) => InvokeCallback(BracketUpdateCallbacks,  info);
}