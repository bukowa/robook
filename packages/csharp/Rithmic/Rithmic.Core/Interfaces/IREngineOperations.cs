// ReSharper disable InconsistentNaming

using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
namespace Rithmic.Core;

using rapi;

/// <summary>
/// Interface for providing a Rithmic Engine.
/// </summary>
[SuppressMessage("ReSharper", "RedundantNameQualifier")]
public interface IREngineProvider {
    /// <summary>
    /// Gets or sets the Rithmic Engine.
    /// </summary>
    rapi.REngine? REngine { get; set; }
}

/// <summary>
/// Interface for providing Rithmic Engine operations.
/// Wrapper for the Rithmic API methods.
/// </summary>
public interface IREngineOperations :
    IREngineProvider,
    ICancelAllOrders,
    ICancelOrder,
    ICancelOrderList,
    ICancelQuoteList,
    IChangePassword,
    ICreateUserDefinedSpread,
    IExitPosition,
    IGetAccounts,
    IGetAuxRefData,
    IGetEasyToBorrowList,
    IGetEnvironment,
    IGetEquityOptionStrategyList,
    IGetInstrumentByUnderlying,
    IGetOptionList,
    IGetOrderContext,
    IGetPendingInputSize,
    IGetPriceIncrInfo,
    IGetProductRmsInfo,
    IGetRefData,
    IGetStrategyInfo,
    IGetStrategyList,
    IGetUserProfile,
    IGetVolumeAtPrice,
    IIsThereAnAggregator,
    ILinkOrders,
    IListAgreements,
    IListAssignedUsers,
    IListBinaryContracts,
    IListEnvironments,
    IListExchanges,
    IListIbs,
    IListOrderHistoryDates,
    IListTradeRoutes,
    ILogin,
    ILoginRepository,
    ILogout,
    ILogoutRepository,
    IModifyBracketTier,
    IModifyOrder,
    IModifyOrderRefData,
    IRebuildBook,
    IRebuildDboBook,
    IReplayAllOrders,
    IReplayBars,
    IReplayBrackets,
    IReplayExecutions,
    IReplayHistoricalOrders,
    IReplayOpenOrders,
    IReplayPnl,
    IReplayQuotes,
    IReplaySingleHistoricalOrder,
    IReplaySingleOrder,
    IReplayTrades,
    ISearchInstrument,
    ISendBracketOrder,
    ISendOcaList,
    ISendOrder,
    ISetEnvironmentVariable,
    ISetOrderContext,
    IShutdown,
    ISubmitQuoteList,
    ISubscribe,
    ISubscribeAutoLiquidate,
    ISubscribeBar,
    ISubscribeBracket,
    ISubscribeByUnderlying,
    ISubscribeDbo,
    ISubscribeEasyToBorrow,
    ISubscribeOrder,
    ISubscribePnl,
    ISubscribeTradeRoute,
    IUnsetEnvironmentVariable,
    IUnsubscribe,
    IUnsubscribeAutoLiquidate,
    IUnsubscribeBar,
    IUnsubscribeBracket,
    IUnsubscribeByUnderlying,
    IUnsubscribeDbo,
    IUnsubscribeEasyToBorrow,
    IUnsubscribeOrder,
    IUnsubscribePnl,
    IUnsubscribeTradeRoute;

public interface ICancelAllOrders : IREngineProvider {
    void cancelAllOrders(
        AccountInfo oAccount,
        string      sEntryType,
        string      sTradingAlgorithm
    );
}

public interface ICancelOrder : IREngineProvider {
    void cancelOrder(
        AccountInfo oAccount,
        string      sOrderNum,
        string      sEntryType,
        string      sTradingAlgorithm,
        string      sUserMsg,
        object      oContext
    );
}

public interface ICancelOrderList : IREngineProvider {
    void cancelOrderList(
        ReadOnlyCollection<AccountInfo> oAccountList,
        ReadOnlyCollection<string>      oOrderNumList,
        ReadOnlyCollection<string>      oEntryTypeList,
        ReadOnlyCollection<string>      oTradingAlgorithmList,
        ReadOnlyCollection<string>      oUserMsgList,
        ReadOnlyCollection<object>      oContextList
    );
}

public interface ICancelQuoteList : IREngineProvider {
    void cancelQuoteList(ReadOnlyCollection<QuoteCancelParams> oList);
}

public interface IChangePassword : IREngineProvider {
    void changePassword(
        string sOldPassword,
        string sNewPassword
    );
}

public interface ICreateUserDefinedSpread : IREngineProvider {
    void createUserDefinedSpread(
        AccountInfo                oAccount,
        string                     sExchange,
        string                     sTradeRoute,
        string                     sEntryType,
        string                     sStrategyType,
        ReadOnlyCollection<string> oLegSymbols,
        ReadOnlyCollection<int>    oLegRatios,
        object                     oContext
    );
}

public interface IExitPosition : IREngineProvider {
    void exitPosition(
        AccountInfo oAccount,
        string      sExchange,
        string      sSymbol,
        string      sEntryType,
        string      sTradingAlgorithm,
        object      oContext
    );
}

public interface IGetAccounts : IREngineProvider {
    void getAccounts(string sStatus);
}

public interface IGetAuxRefData : IREngineProvider {
    void getAuxRefData(
        string sExchange,
        string sSymbol,
        object oContext
    );
}

public interface IGetEasyToBorrowList : IREngineProvider {
    void getEasyToBorrowList(object oContext);
}

public interface IGetEnvironment : IREngineProvider {
    void getEnvironment(
        string sKey,
        object oContext
    );
}

public interface IGetEquityOptionStrategyList : IREngineProvider {
    void getEquityOptionStrategyList(
        string sExchange,
        string sUnderlying,
        string sStrategyType,
        string sExpiration,
        object oContext
    );
}

public interface IGetInstrumentByUnderlying : IREngineProvider {
    void getInstrumentByUnderlying(
        string sUnderlying,
        string sExchange,
        string sExpiration,
        object oContext
    );
}

public interface IGetOptionList : IREngineProvider {
    void getOptionList(
        string sExchange,
        string sProduct,
        string sExpirationCCYYMM,
        object oContext
    );
}

public interface IGetOrderContext : IREngineProvider {
    object? getOrderContext(string sOrderNum);
}

public interface IGetPendingInputSize : IREngineProvider {
    long? getPendingInputSize(ConnectionId eConnId);
}

public interface IGetPriceIncrInfo : IREngineProvider {
    void getPriceIncrInfo(
        string sExchange,
        string sSymbol,
        object oContext
    );
}

public interface IGetProductRmsInfo : IREngineProvider {
    void getProductRmsInfo(
        AccountInfo oAccount,
        object      oContext
    );
}

public interface IGetRefData : IREngineProvider {
    void getRefData(
        string sExchange,
        string sSymbol,
        object oContext
    );
}

public interface IGetStrategyInfo : IREngineProvider {
    void getStrategyInfo(
        string sExchange,
        string sSymbol,
        object oContext
    );
}

public interface IGetStrategyList : IREngineProvider {
    void getStrategyList(
        string sExchange,
        string sProduct,
        string sStrategyType,
        string sExpirationCCYYMM,
        object oContext
    );
}

public interface IGetUserProfile : IREngineProvider {
    void getUserProfile(
        ConnectionId eConnId,
        object       oContext
    );
}

public interface IGetVolumeAtPrice : IREngineProvider {
    void getVolumeAtPrice(
        string sExchange,
        string sSymbol,
        object oContext
    );
}

public interface IIsThereAnAggregator : IREngineProvider {
    void isThereAnAggregator();
}

public interface ILinkOrders : IREngineProvider {
    void linkOrders(
        ReadOnlyCollection<AccountInfo> oAccountList,
        ReadOnlyCollection<string>      oOrderNumList
    );
}

public interface IListAgreements : IREngineProvider {
    void listAgreements(
        bool   bAccepted,
        object oContext
    );
}

public interface IListAssignedUsers : IREngineProvider {
    void listAssignedUsers(
        AccountInfo oAccount,
        object      oContext
    );
}

public interface IListBinaryContracts : IREngineProvider {
    void listBinaryContracts(
        string sExchange,
        string sProductCode,
        object oContext
    );
}

public interface IListEnvironments : IREngineProvider {
    void listEnvironments(object oContext);
}

public interface IListExchanges : IREngineProvider {
    void listExchanges(object oContext);
}

public interface IListIbs : IREngineProvider {
    void listIbs(object oContext);
}

public interface IListOrderHistoryDates : IREngineProvider {
    void listOrderHistoryDates(object oContext);
}

public interface IListTradeRoutes : IREngineProvider {
    void listTradeRoutes(object oContext);
}

public interface ILogin : IREngineProvider {
    void login(
        RCallbacksFacade oCallbacksFacade,
        string           sMdEnvKey,
        string           sMdUser,
        string           sMdPassword,
        string           sMdCnnctPt,
        string           sTsEnvKey,
        string           sTsUser,
        string           sTsPassword,
        string           sTsCnnctPt,
        string           sPnlCnnctPt,
        string           sIhEnvKey,
        string           sIhUser,
        string           sIhPassword,
        string           sIhCnnctPt
    );
}

public interface ILoginRepository : IREngineProvider {
    void loginRepository(
        RCallbacksFacade oCallbacksFacade,
        string           sEnvKey,
        string           sUser,
        string           sPassword,
        string           sCnnctPt
    );
}

public interface ILogout : IREngineProvider {
    void logout();
}

public interface ILogoutRepository : IREngineProvider {
    void logoutRepository();
}

public interface IModifyBracketTier : IREngineProvider {
    void modifyBracketTier(
        AccountInfo oAccount,
        string      sOrderNum,
        bool        bTarget,
        int         iOldTickOffset,
        int         iNewTickOffset,
        object      oContext
    );
}

public interface IModifyOrder : IREngineProvider {
    void modifyOrder(ModifyLimitOrderParams                    oParamsIn);
    void modifyOrder(ModifyOrderParams                         oParams);
    void modifyOrder(ModifyStopLimitOrderParams                oParamsIn);
    void modifyOrder(ModifyStopMarketOrderParams               oParamsIn);
    void modifyOrderList(ReadOnlyCollection<ModifyOrderParams> oList);
}

public interface IModifyOrderRefData : IREngineProvider {
    void modifyOrderRefData(
        AccountInfo oAccount,
        string      sOrderNum,
        string      sUserMsg,
        string      sUserTag
    );
}

public interface IRebuildBook : IREngineProvider {
    void rebuildBook(
        string sExchange,
        string sSymbol,
        object oContext
    );
}

public interface IRebuildDboBook : IREngineProvider {
    void rebuildDboBook(
        string sExchange,
        string sSymbol,
        double dPrice,
        object oContext
    );
}

public interface IReplayAllOrders : IREngineProvider {
    void replayAllOrders(
        AccountInfo oAccount,
        int         iStartSsboe,
        int         iEndSsboe,
        object      oContext
    );
}

public interface IReplayBars : IREngineProvider {
    void replayBars(ReplayBarParams oParams);
}

public interface IReplayBrackets : IREngineProvider {
    void replayBrackets(
        AccountInfo oAccount,
        object      oContext
    );
}

public interface IReplayExecutions : IREngineProvider {
    void replayExecutions(
        AccountInfo oAccount,
        int         iStartSsboe,
        int         iEndSsboe,
        object      oContext
    );
}

public interface IReplayHistoricalOrders : IREngineProvider {
    void replayHistoricalOrders(
        AccountInfo oAccount,
        string      sDate,
        object      oContext
    );
}

public interface IReplayOpenOrders : IREngineProvider {
    void replayOpenOrders(
        AccountInfo oAccount,
        object      oContext
    );
}

public interface IReplayPnl : IREngineProvider {
    void replayPnl(
        AccountInfo oAccount,
        object      oContext
    );
}

public interface IReplayQuotes : IREngineProvider {
    void replayQuotes(
        AccountInfo oAccount,
        object      oContext
    );
}

public interface IReplaySingleHistoricalOrder : IREngineProvider {
    void replaySingleHistoricalOrder(
        AccountInfo oAccount,
        string      sOrderNum,
        string      sDate,
        object      oContext
    );
}

public interface IReplaySingleOrder : IREngineProvider {
    void replaySingleOrder(
        AccountInfo oAccount,
        string      sOrderNum,
        object      oContext
    );
}

public interface IReplayTrades : IREngineProvider {
    void replayTrades(
        string sExchange,
        string sSymbol,
        int    iStartSsboe,
        int    iEndSsboe,
        object oContext
    );
}

public interface ISearchInstrument : IREngineProvider {
    void searchInstrument(
        string                         sExchange,
        ReadOnlyCollection<SearchTerm> oTerms,
        object                         oContext
    );
}

public interface ISendBracketOrder : IREngineProvider {
    void sendBracketOrder(
        OrderParams   oEntry,
        BracketParams oBracketParams
    );

    void sendBracketOrder(
        OrderParams        oEntry,
        string             sBracketType,
        string             sOrderOperationType,
        bool               bTrailingStop,
        int                iTrailByPriceId,
        IList<BracketTier> oTargets,
        IList<BracketTier> oStops
    );
}

public interface ISendOcaList : IREngineProvider {
    void sendOcaList(
        string                          sOcaType,
        ReadOnlyCollection<OrderParams> oList
    );
}

public interface ISendOrder : IREngineProvider {
    void sendOrder(LimitOrderParams                    oParamsIn);
    void sendOrder(MarketOrderParams                   oParamsIn);
    void sendOrder(StopLimitOrderParams                oParamsIn);
    void sendOrder(StopMarketOrderParams               oParamsIn);
    void sendOrderList(ReadOnlyCollection<OrderParams> oList);
}

public interface ISetEnvironmentVariable : IREngineProvider {
    void setEnvironmentVariable(
        string sKey,
        string sVariable,
        string sValue
    );
}

public interface ISetOrderContext : IREngineProvider {
    void setOrderContext(
        string sOrderNum,
        object oContext
    );
}

public interface IShutdown : IREngineProvider {
    void shutdown();
}

public interface ISubmitQuoteList : IREngineProvider {
    void submitQuoteList(ReadOnlyCollection<QuoteParams> oList);
}

public interface ISubscribe : IREngineProvider {
    void subscribe(
        string            sExchange,
        string            sSymbol,
        SubscriptionFlags eFlags,
        object            oContext
    );
}

public interface ISubscribeAutoLiquidate : IREngineProvider {
    void subscribeAutoLiquidate(AccountInfo oAccount);
}

public interface ISubscribeBar : IREngineProvider {
    void subscribeBar(BarParams oParams);
}

public interface ISubscribeBracket : IREngineProvider {
    void subscribeBracket(AccountInfo oAccount);
}

public interface ISubscribeByUnderlying : IREngineProvider {
    void subscribeByUnderlying(
        string            sUnderlying,
        string            sExchange,
        string            sExpiration,
        SubscriptionFlags eFlags,
        object            oContext
    );
}

public interface ISubscribeDbo : IREngineProvider {
    void subscribeDbo(
        string sExchange,
        string sSymbol,
        double dPrice,
        object oContext
    );
}

public interface ISubscribeEasyToBorrow : IREngineProvider {
    void subscribeEasyToBorrow(object oContext);
}

public interface ISubscribeOrder : IREngineProvider {
    void subscribeOrder(AccountInfo oAccount);
}

public interface ISubscribePnl : IREngineProvider {
    void subscribePnl(AccountInfo oAccount);
}

public interface ISubscribeTradeRoute : IREngineProvider {
    void subscribeTradeRoute(
        string sFcmId,
        string sIbId,
        object oContext
    );
}

public interface IUnsetEnvironmentVariable : IREngineProvider {
    void unsetEnvironmentVariable(
        string sKey,
        string sVariable
    );
}

public interface IUnsubscribe : IREngineProvider {
    void unsubscribe(
        string sExchange,
        string sSymbol
    );
}

public interface IUnsubscribeAutoLiquidate : IREngineProvider {
    void unsubscribeAutoLiquidate(AccountInfo oAccount);
}

public interface IUnsubscribeBar : IREngineProvider {
    void unsubscribeBar(BarParams oParams);
}

public interface IUnsubscribeBracket : IREngineProvider {
    void unsubscribeBracket(AccountInfo oAccount);
}

public interface IUnsubscribeByUnderlying : IREngineProvider {
    void unsubscribeByUnderlying(
        string sUnderlying,
        string sExchange,
        string sExpiration
    );
}

public interface IUnsubscribeDbo : IREngineProvider {
    void unsubscribeDbo(
        string sExchange,
        string sSymbol,
        double dPrice,
        object oContext
    );
}

public interface IUnsubscribeEasyToBorrow : IREngineProvider {
    void unsubscribeEasyToBorrow();
}

public interface IUnsubscribeOrder : IREngineProvider {
    void unsubscribeOrder(AccountInfo oAccount);
}

public interface IUnsubscribePnl : IREngineProvider {
    void unsubscribePnl(AccountInfo oAccount);
}

public interface IUnsubscribeTradeRoute : IREngineProvider {
    void unsubscribeTradeRoute(
        string sFcmId,
        string sIbId
    );
}