using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;
using static Rithmic.Core.LoggingService;
using com.omnesys.rapi;

namespace Rithmic.Core;

// ReSharper disable once RedundantNameQualifier
[SuppressMessage("ReSharper", "RedundantNameQualifier")]
public class RCallbacksFacade : rapi.RCallbacks, IRCallbacksFacade
{

    public rapi.RCallbacks GetRCallbacks()
    {
        return this;
    }

    private void InvokeCallback<T>(IEventDispatcher<T> dispatcher, T info) => dispatcher.Dispatch(info);

    private void InvokeCallback<T>(IEventDispatcher<T> dispatcher, object context, T info)
    {
        if (context is IContext ctx)
        {
            dispatcher.Dispatch(ctx, info);
        }
        else
        {
            Logger?.LogError("Callback {callbackName} received null context with {info}", dispatcher, info);
        }
    }

    public IEventDispatcher<AlertInfo> AlertDispatcher { get; } =
        new EventDispatcher<AlertInfo>();

    public IEventDispatcher<AggregatorInfo> AggregatorDispatcher { get; } =
        new EventDispatcher<AggregatorInfo>();

    public IEventDispatcher<AskInfo> AskQuoteDispatcher { get; } =
        new EventDispatcher<AskInfo>();

    public IEventDispatcher<AuxRefDataInfo> AuxRefDataDispatcher { get; } =
        new EventDispatcher<AuxRefDataInfo>();

    public IEventDispatcher<BestAskQuoteInfo> BestAskQuoteDispatcher { get; } =
        new EventDispatcher<BestAskQuoteInfo>();

    public IEventDispatcher<BestBidAskQuoteInfo> BestBidAskQuoteDispatcher { get; }
        = new EventDispatcher<BestBidAskQuoteInfo>();

    public IEventDispatcher<BestBidQuoteInfo> BestBidQuoteDispatcher { get; }
        = new EventDispatcher<BestBidQuoteInfo>();

    public IEventDispatcher<BidInfo> BidQuoteDispatcher { get; }
        = new EventDispatcher<BidInfo>();

    public IEventDispatcher<BinaryContractListInfo> BinaryContractListDispatcher { get; }
        = new EventDispatcher<BinaryContractListInfo>();

    public IEventDispatcher<CloseMidPriceInfo> CloseMidPriceDispatcher { get; }
        = new EventDispatcher<CloseMidPriceInfo>();

    public IEventDispatcher<ClosePriceInfo> ClosePriceDispatcher { get; }
        = new EventDispatcher<ClosePriceInfo>();

    public IEventDispatcher<ClosingIndicatorInfo> ClosingIndicatorDispatcher { get; }
        = new EventDispatcher<ClosingIndicatorInfo>();

    public IEventDispatcher<DboInfo> DboDispatcher { get; }
        = new EventDispatcher<DboInfo>();

    public IEventDispatcher<DboBookRebuildInfo> DboBookRebuildDispatcher { get; }
        = new EventDispatcher<DboBookRebuildInfo>();

    public IEventDispatcher<EndQuoteInfo> EndQuoteDispatcher { get; }
        = new EventDispatcher<EndQuoteInfo>();

    public IEventDispatcher<EquityOptionStrategyListInfo> EquityOptionStrategyListDispatcher { get; } =
        new EventDispatcher<EquityOptionStrategyListInfo>();

    public IEventDispatcher<HighPriceInfo> HighPriceDispatcher { get; }
        = new EventDispatcher<HighPriceInfo>();

    public IEventDispatcher<HighBidPriceInfo> HighBidPriceDispatcher { get; }
        = new EventDispatcher<HighBidPriceInfo>();

    public IEventDispatcher<HighPriceLimitInfo> HighPriceLimitDispatcher { get; }
        = new EventDispatcher<HighPriceLimitInfo>();

    public IEventDispatcher<InstrumentByUnderlyingInfo> InstrumentByUnderlyingDispatcher { get; }
        = new EventDispatcher<InstrumentByUnderlyingInfo>();

    public IEventDispatcher<InstrumentSearchInfo> InstrumentSearchDispatcher { get; }
        = new EventDispatcher<InstrumentSearchInfo>();

    public IEventDispatcher<OrderBookInfo> LimitOrderBookDispatcher { get; }
        = new EventDispatcher<OrderBookInfo>();

    public IEventDispatcher<LowPriceInfo> LowPriceDispatcher { get; }
        = new EventDispatcher<LowPriceInfo>();

    public IEventDispatcher<LowAskPriceInfo> LowAskPriceDispatcher { get; }
        = new EventDispatcher<LowAskPriceInfo>();

    public IEventDispatcher<LowPriceLimitInfo> LowPriceLimitDispatcher { get; }
        = new EventDispatcher<LowPriceLimitInfo>();

    public IEventDispatcher<MarketModeInfo> MarketModeDispatcher { get; }
        = new EventDispatcher<MarketModeInfo>();

    public IEventDispatcher<MidPriceInfo> MidPriceDispatcher { get; }
        = new EventDispatcher<MidPriceInfo>();

    public IEventDispatcher<OpenInterestInfo> OpenInterestDispatcher { get; }
        = new EventDispatcher<OpenInterestInfo>();

    public IEventDispatcher<OpenPriceInfo> OpenPriceDispatcher { get; }
        = new EventDispatcher<OpenPriceInfo>();

    public IEventDispatcher<OpeningIndicatorInfo> OpeningIndicatorDispatcher { get; }
        = new EventDispatcher<OpeningIndicatorInfo>();

    public IEventDispatcher<OptionListInfo> OptionListDispatcher { get; }
        = new EventDispatcher<OptionListInfo>();

    public IEventDispatcher<PriceIncrInfo> PriceIncrUpdateDispatcher { get; }
        = new EventDispatcher<PriceIncrInfo>();

    public IEventDispatcher<ProjectedSettlementPriceInfo> ProjectedSettlementPriceDispatcher { get; }
        = new EventDispatcher<ProjectedSettlementPriceInfo>();

    public IEventDispatcher<RefDataInfo> RefDataDispatcher { get; }
        = new EventDispatcher<RefDataInfo>();

    public IEventDispatcher<SettlementPriceInfo> SettlementPriceDispatcher { get; }
        = new EventDispatcher<SettlementPriceInfo>();

    public IEventDispatcher<StrategyInfo> StrategyDispatcher { get; }
        = new EventDispatcher<StrategyInfo>();

    public IEventDispatcher<StrategyListInfo> StrategyListDispatcher { get; }
        = new EventDispatcher<StrategyListInfo>();

    public IEventDispatcher<TradeInfo> TradeConditionDispatcher { get; }
        = new EventDispatcher<TradeInfo>();

    public IEventDispatcher<TradeInfo> TradePrintDispatcher { get; }
        = new EventDispatcher<TradeInfo>();

    public IEventDispatcher<TradeVolumeInfo> TradeVolumeDispatcher { get; }
        = new EventDispatcher<TradeVolumeInfo>();

    public IEventDispatcher<UserDefinedSpreadCreateInfo> UserDefinedSpreadCreateDispatcher { get; }
        = new EventDispatcher<UserDefinedSpreadCreateInfo>();

    public IEventDispatcher<UserListInfo> UserListDispatcher { get; }
        = new EventDispatcher<UserListInfo>();

    public IEventDispatcher<UserProfileInfo> UserProfileDispatcher { get; }
        = new EventDispatcher<UserProfileInfo>();

    public IEventDispatcher<VolumeAtPriceInfo> VolumeAtPriceDispatcher { get; }
        = new EventDispatcher<VolumeAtPriceInfo>();

    public IEventDispatcher<BarInfo> BarDispatcher { get; }
        = new EventDispatcher<BarInfo>();

    public IEventDispatcher<BarReplayInfo> BarReplayDispatcher { get; }
        = new EventDispatcher<BarReplayInfo>();

    public IEventDispatcher<TradeReplayInfo> TradeReplayDispatcher { get; }
        = new EventDispatcher<TradeReplayInfo>();

    public IEventDispatcher<AccountListInfo> AccountListDispatcher { get; }
        = new EventDispatcher<AccountListInfo>();

    public IEventDispatcher<AccountUpdateInfo> AccountUpdateDispatcher { get; }
        = new EventDispatcher<AccountUpdateInfo>();

    public IEventDispatcher<PasswordChangeInfo> PasswordChangeDispatcher { get; }
        = new EventDispatcher<PasswordChangeInfo>();

    public IEventDispatcher<AutoLiquidateInfo> AutoLiquidateDispatcher { get; }
        = new EventDispatcher<AutoLiquidateInfo>();

    public IEventDispatcher<BracketReplayInfo> BracketReplayDispatcher { get; }
        = new EventDispatcher<BracketReplayInfo>();

    public IEventDispatcher<BracketTierModifyInfo> BracketTierModifyDispatcher { get; }
        = new EventDispatcher<BracketTierModifyInfo>();

    public IEventDispatcher<BracketInfo> BracketUpdateDispatcher { get; }
        = new EventDispatcher<BracketInfo>();

    public IEventDispatcher<EasyToBorrowInfo> EasyToBorrowDispatcher { get; }
        = new EventDispatcher<EasyToBorrowInfo>();

    public IEventDispatcher<EasyToBorrowListInfo> EasyToBorrowListDispatcher { get; }
        = new EventDispatcher<EasyToBorrowListInfo>();

    public IEventDispatcher<ExchangeListInfo> ExchangeListDispatcher { get; }
        = new EventDispatcher<ExchangeListInfo>();

    public IEventDispatcher<ExecutionReplayInfo> ExecutionReplayDispatcher { get; }
        = new EventDispatcher<ExecutionReplayInfo>();

    public IEventDispatcher<IbListInfo> IbListDispatcher { get; } =
        new EventDispatcher<IbListInfo>();

    public IEventDispatcher<LineInfo> LineUpdateDispatcher { get; }
        = new EventDispatcher<LineInfo>();

    public IEventDispatcher<OrderReplayInfo> OpenOrderReplayDispatcher { get; }
        = new EventDispatcher<OrderReplayInfo>();

    public IEventDispatcher<OrderHistoryDatesInfo> OrderHistoryDatesDispatcher { get; }
        = new EventDispatcher<OrderHistoryDatesInfo>();

    public IEventDispatcher<OrderReplayInfo> OrderReplayDispatcher { get; }
        = new EventDispatcher<OrderReplayInfo>();

    public IEventDispatcher<PnlReplayInfo> PnlReplayDispatcher { get; }
        = new EventDispatcher<PnlReplayInfo>();

    public IEventDispatcher<PnlInfo> PnlUpdateDispatcher { get; }
        = new EventDispatcher<PnlInfo>();

    public IEventDispatcher<PositionExitInfo> PositionExitDispatcher { get; }
        = new EventDispatcher<PositionExitInfo>();

    public IEventDispatcher<ProductRmsListInfo> ProductRmsListDispatcher { get; }
        = new EventDispatcher<ProductRmsListInfo>();

    public IEventDispatcher<QuoteInfo> QuoteDispatcher { get; }
        = new EventDispatcher<QuoteInfo>();

    public IEventDispatcher<QuoteReport> QuoteReportDispatcher { get; }
        = new EventDispatcher<QuoteReport>();

    public IEventDispatcher<QuoteReplayInfo> QuoteReplayDispatcher { get; }
        = new EventDispatcher<QuoteReplayInfo>();

    public IEventDispatcher<SingleOrderReplayInfo> SingleOrderReplayDispatcher { get; }
        = new EventDispatcher<SingleOrderReplayInfo>();

    public IEventDispatcher<SodReport> SodUpdateDispatcher { get; }
        = new EventDispatcher<SodReport>();

    public IEventDispatcher<TradeRouteInfo> TradeRouteDispatcher { get; }
        = new EventDispatcher<TradeRouteInfo>();

    public IEventDispatcher<TradeRouteListInfo> TradeRouteListDispatcher { get; }
        = new EventDispatcher<TradeRouteListInfo>();

    public IEventDispatcher<OrderBustReport> BustReportDispatcher { get; }
        = new EventDispatcher<OrderBustReport>();

    public IEventDispatcher<OrderCancelReport> CancelReportDispatcher { get; }
        = new EventDispatcher<OrderCancelReport>();

    public IEventDispatcher<OrderFailureReport> FailureReportDispatcher { get; }
        = new EventDispatcher<OrderFailureReport>();

    public IEventDispatcher<OrderFillReport> FillReportDispatcher { get; }
        = new EventDispatcher<OrderFillReport>();

    public IEventDispatcher<OrderModifyReport> ModifyReportDispatcher { get; }
        = new EventDispatcher<OrderModifyReport>();

    public IEventDispatcher<OrderNotCancelledReport> NotCancelledReportDispatcher { get; }
        = new EventDispatcher<OrderNotCancelledReport>();

    public IEventDispatcher<OrderNotModifiedReport> NotModifiedReportDispatcher { get; }
        = new EventDispatcher<OrderNotModifiedReport>();

    public IEventDispatcher<OrderReport> OtherReportDispatcher { get; }
        = new EventDispatcher<OrderReport>();

    public IEventDispatcher<OrderRejectReport> RejectReportDispatcher { get; }
        = new EventDispatcher<OrderRejectReport>();

    public IEventDispatcher<OrderStatusReport> StatusReportDispatcher { get; }
        = new EventDispatcher<OrderStatusReport>();

    public IEventDispatcher<OrderTradeCorrectReport> TradeCorrectReportDispatcher { get; }
        = new EventDispatcher<OrderTradeCorrectReport>();

    public IEventDispatcher<OrderTriggerPulledReport> TriggerPulledReportDispatcher { get; }
        = new EventDispatcher<OrderTriggerPulledReport>();

    public IEventDispatcher<OrderTriggerReport> TriggerReportDispatcher { get; }
        = new EventDispatcher<OrderTriggerReport>();

    public IEventDispatcher<AgreementListInfo> AgreementListDispatcher { get; }
        = new EventDispatcher<AgreementListInfo>();

    public override void AskQuote(AskInfo info) =>
        InvokeCallback(AskQuoteDispatcher, info.Context, info);

    public override void AuxRefData(AuxRefDataInfo info) =>
        InvokeCallback(AuxRefDataDispatcher, info.Context, info);

    public override void BestAskQuote(AskInfo info) =>
        InvokeCallback(BestAskQuoteDispatcher, info.Context, new BestAskQuoteInfo(info));

    public override void BestBidAskQuote(BidInfo binfo, AskInfo ainfo) =>
        InvokeCallback(BestBidAskQuoteDispatcher, binfo.Context, new BestBidAskQuoteInfo(ainfo, binfo));

    public override void BestBidQuote(BidInfo info) =>
        InvokeCallback(BestBidQuoteDispatcher, info.Context, new BestBidQuoteInfo(info));

    public override void BidQuote(BidInfo info) =>
        InvokeCallback(BidQuoteDispatcher, info.Context, info);

    public override void BinaryContractList(BinaryContractListInfo info) =>
        InvokeCallback(BinaryContractListDispatcher, info.Context, info);

    public override void CloseMidPrice(CloseMidPriceInfo info) =>
        InvokeCallback(CloseMidPriceDispatcher, info.Context, info);

    public override void ClosePrice(ClosePriceInfo info) =>
        InvokeCallback(ClosePriceDispatcher, info.Context, info);

    public override void ClosingIndicator(ClosingIndicatorInfo info) =>
        InvokeCallback(ClosingIndicatorDispatcher, info.Context, info);

    public override void Dbo(DboInfo info) =>
        InvokeCallback(DboDispatcher, info.Context, info);

    public override void DboBookRebuild(DboBookRebuildInfo info) =>
        InvokeCallback(DboBookRebuildDispatcher, info.Context, info);

    public override void EndQuote(EndQuoteInfo info) =>
        InvokeCallback(EndQuoteDispatcher, info.Context, info);

    public override void EquityOptionStrategyList(EquityOptionStrategyListInfo info) =>
        InvokeCallback(EquityOptionStrategyListDispatcher, info.Context, info);

    public override void HighPrice(HighPriceInfo info) =>
        InvokeCallback(HighPriceDispatcher, info.Context, info);

    public override void HighBidPrice(HighBidPriceInfo info) =>
        InvokeCallback(HighBidPriceDispatcher, info.Context, info);

    public override void HighPriceLimit(HighPriceLimitInfo info) =>
        InvokeCallback(HighPriceLimitDispatcher, info.Context, info);

    public override void InstrumentByUnderlying(InstrumentByUnderlyingInfo info) =>
        InvokeCallback(InstrumentByUnderlyingDispatcher, info.Context, info);

    public override void InstrumentSearch(InstrumentSearchInfo info) =>
        InvokeCallback(InstrumentSearchDispatcher, info.Context, info);

    public override void LimitOrderBook(OrderBookInfo info) =>
        InvokeCallback(LimitOrderBookDispatcher, info.Context, info);

    public override void LowPrice(LowPriceInfo info) =>
        InvokeCallback(LowPriceDispatcher, info.Context, info);

    public override void LowAskPrice(LowAskPriceInfo info) =>
        InvokeCallback(LowAskPriceDispatcher, info.Context, info);

    public override void LowPriceLimit(LowPriceLimitInfo info) =>
        InvokeCallback(LowPriceLimitDispatcher, info.Context, info);

    public override void MarketMode(MarketModeInfo info) =>
        InvokeCallback(MarketModeDispatcher, info.Context, info);

    public override void MidPrice(MidPriceInfo info) =>
        InvokeCallback(MidPriceDispatcher, info.Context, info);

    public override void OpenInterest(OpenInterestInfo info) =>
        InvokeCallback(OpenInterestDispatcher, info.Context, info);

    public override void OpenPrice(OpenPriceInfo info) =>
        InvokeCallback(OpenPriceDispatcher, info.Context, info);

    public override void OpeningIndicator(OpeningIndicatorInfo info) =>
        InvokeCallback(OpeningIndicatorDispatcher, info.Context, info);

    public override void OptionList(OptionListInfo info) =>
        InvokeCallback(OptionListDispatcher, info.Context, info);

    public override void PriceIncrUpdate(PriceIncrInfo info) =>
        InvokeCallback(PriceIncrUpdateDispatcher, info.Context, info);

    public override void ProjectedSettlementPrice(ProjectedSettlementPriceInfo info) =>
        InvokeCallback(ProjectedSettlementPriceDispatcher, info.Context, info);

    public override void RefData(RefDataInfo info) =>
        InvokeCallback(RefDataDispatcher, info.Context, info);

    public override void SettlementPrice(SettlementPriceInfo info) =>
        InvokeCallback(SettlementPriceDispatcher, info.Context, info);

    public override void Strategy(StrategyInfo info) =>
        InvokeCallback(StrategyDispatcher, info.Context, info);

    public override void StrategyList(StrategyListInfo info) =>
        InvokeCallback(StrategyListDispatcher, info.Context, info);

    public override void TradeCondition(TradeInfo info) =>
        InvokeCallback(TradeConditionDispatcher, info.Context, info);

    public override void TradePrint(TradeInfo info) =>
        InvokeCallback(TradePrintDispatcher, info.Context, info);

    public override void TradeVolume(TradeVolumeInfo info) =>
        InvokeCallback(TradeVolumeDispatcher, info.Context, info);

    public override void UserDefinedSpreadCreate(UserDefinedSpreadCreateInfo info) =>
        InvokeCallback(UserDefinedSpreadCreateDispatcher, info.Context, info);

    public override void UserList(UserListInfo info) =>
        InvokeCallback(UserListDispatcher, info.Context, info);

    public override void UserProfile(UserProfileInfo info) =>
        InvokeCallback(UserProfileDispatcher, info.Context, info);

    public override void VolumeAtPrice(VolumeAtPriceInfo info) =>
        InvokeCallback(VolumeAtPriceDispatcher, info.Context, info);

    public override void Bar(BarInfo info) =>
        InvokeCallback(BarDispatcher, info.Context, info);

    public override void BarReplay(BarReplayInfo info) =>
        InvokeCallback(BarReplayDispatcher, info.Context, info);

    public override void TradeReplay(TradeReplayInfo info) =>
        InvokeCallback(TradeReplayDispatcher, info.Context, info);

    public override void BracketReplay(BracketReplayInfo info) =>
        InvokeCallback(BracketReplayDispatcher, info.Context, info);

    public override void BracketTierModify(BracketTierModifyInfo info) =>
        InvokeCallback(BracketTierModifyDispatcher, info.Context, info);

    public override void EasyToBorrow(EasyToBorrowInfo info) =>
        InvokeCallback(EasyToBorrowDispatcher, info.Context, info);

    public override void EasyToBorrowList(EasyToBorrowListInfo info) =>
        InvokeCallback(EasyToBorrowListDispatcher, info.Context, info);

    public override void ExchangeList(ExchangeListInfo info) =>
        InvokeCallback(ExchangeListDispatcher, info.Context, info);

    public override void ExecutionReplay(ExecutionReplayInfo info) =>
        InvokeCallback(ExecutionReplayDispatcher, info.Context, info);

    public override void IbList(IbListInfo info) =>
        InvokeCallback(IbListDispatcher, info.Context, info);

    public override void LineUpdate(LineInfo info) =>
        InvokeCallback(LineUpdateDispatcher, info.Context, info);

    public override void OpenOrderReplay(OrderReplayInfo info) =>
        InvokeCallback(OpenOrderReplayDispatcher, info.Context, info);

    public override void OrderHistoryDates(OrderHistoryDatesInfo info) =>
        InvokeCallback(OrderHistoryDatesDispatcher, info.Context, info);

    public override void OrderReplay(OrderReplayInfo info) =>
        InvokeCallback(OrderReplayDispatcher, info.Context, info);

    public override void PnlReplay(PnlReplayInfo info) =>
        InvokeCallback(PnlReplayDispatcher, info.Context, info);

    public override void PositionExit(PositionExitInfo info) =>
        InvokeCallback(PositionExitDispatcher, info.Context, info);

    public override void ProductRmsList(ProductRmsListInfo info) =>
        InvokeCallback(ProductRmsListDispatcher, info.Context, info);

    public override void QuoteReplay(QuoteReplayInfo info) =>
        InvokeCallback(QuoteReplayDispatcher, info.Context, info);

    public override void SingleOrderReplay(SingleOrderReplayInfo info) =>
        InvokeCallback(SingleOrderReplayDispatcher, info.Context, info);

    public override void TradeRoute(TradeRouteInfo info) =>
        InvokeCallback(TradeRouteDispatcher, info.Context, info);

    public override void TradeRouteList(TradeRouteListInfo info) =>
        InvokeCallback(TradeRouteListDispatcher, info.Context, info);

    public override void BustReport(OrderBustReport info) =>
        InvokeCallback(BustReportDispatcher, info.Context, info);

    public override void CancelReport(OrderCancelReport info) =>
        InvokeCallback(CancelReportDispatcher, info.Context, info);

    public override void FailureReport(OrderFailureReport info) =>
        InvokeCallback(FailureReportDispatcher, info.Context, info);

    public override void FillReport(OrderFillReport info) =>
        InvokeCallback(FillReportDispatcher, info.Context, info);

    public override void ModifyReport(OrderModifyReport info) =>
        InvokeCallback(ModifyReportDispatcher, info.Context, info);

    public override void NotCancelledReport(OrderNotCancelledReport info) =>
        InvokeCallback(NotCancelledReportDispatcher, info.Context, info);

    public override void NotModifiedReport(OrderNotModifiedReport info) =>
        InvokeCallback(NotModifiedReportDispatcher, info.Context, info);

    public override void OtherReport(OrderReport info) =>
        InvokeCallback(OtherReportDispatcher, info.Context, info);

    public override void RejectReport(OrderRejectReport info) =>
        InvokeCallback(RejectReportDispatcher, info.Context, info);

    public override void StatusReport(OrderStatusReport info) =>
        InvokeCallback(StatusReportDispatcher, info.Context, info);

    public override void TradeCorrectReport(OrderTradeCorrectReport info) =>
        InvokeCallback(TradeCorrectReportDispatcher, info.Context, info);

    public override void TriggerPulledReport(OrderTriggerPulledReport info) =>
        InvokeCallback(TriggerPulledReportDispatcher, info.Context, info);

    public override void TriggerReport(OrderTriggerReport info) =>
        InvokeCallback(TriggerReportDispatcher, info.Context, info);

    public override void AgreementList(AgreementListInfo info) =>
        InvokeCallback(AgreementListDispatcher, info.Context, info);

    public override void Alert(AlertInfo                   info) => InvokeCallback(AlertDispatcher,          info);
    public override void Aggregator(AggregatorInfo         info) => InvokeCallback(AggregatorDispatcher,     info);
    public override void SodUpdate(SodReport               info) => InvokeCallback(SodUpdateDispatcher,      info);
    public override void PnlUpdate(PnlInfo                 info) => InvokeCallback(PnlUpdateDispatcher,      info);
    public override void Quote(QuoteInfo                   info) => InvokeCallback(QuoteDispatcher,          info);
    public override void Quote(QuoteReport                 info) => InvokeCallback(QuoteReportDispatcher,    info);
    public override void AccountList(AccountListInfo       info) => InvokeCallback(AccountListDispatcher,    info);
    public override void AccountUpdate(AccountUpdateInfo   info) => InvokeCallback(AccountUpdateDispatcher,  info);
    public override void PasswordChange(PasswordChangeInfo info) => InvokeCallback(PasswordChangeDispatcher, info);
    public override void AutoLiquidate(AutoLiquidateInfo   info) => InvokeCallback(AutoLiquidateDispatcher,  info);
    public override void BracketUpdate(BracketInfo         info) => InvokeCallback(BracketUpdateDispatcher,  info);
}
