namespace Rithmic.Core;

/// <summary>
/// Interface for creating an instance of IREngineOperations.
/// </summary>
public interface IREngineOperationsFactory
{
    /// <summary>
    /// Method to create an instance of IREngineOperations.
    /// </summary>
    abstract static IREngineOperations Create(
        rapi.REngine                  rEngine,
        ICancelAllOrders?             cancelAllOrders             = null,
        ICancelOrder?                 cancelOrder                 = null,
        ICancelOrderList?             cancelOrderList             = null,
        ICancelQuoteList?             cancelQuoteList             = null,
        IChangePassword?              changePassword              = null,
        ICreateUserDefinedSpread?     createUserDefinedSpread     = null,
        IExitPosition?                exitPosition                = null,
        IGetAccounts?                 getAccounts                 = null,
        IGetAuxRefData?               getAuxRefData               = null,
        IGetEasyToBorrowList?         getEasyToBorrowList         = null,
        IGetEnvironment?              getEnvironment              = null,
        IGetEquityOptionStrategyList? getEquityOptionStrategyList = null,
        IGetInstrumentByUnderlying?   getInstrumentByUnderlying   = null,
        IGetOptionList?               getOptionList               = null,
        IGetOrderContext?             getOrderContext             = null,
        IGetPendingInputSize?         getPendingInputSize         = null,
        IGetPriceIncrInfo?            getPriceIncrInfo            = null,
        IGetProductRmsInfo?           getProductRmsInfo           = null,
        IGetRefData?                  getRefData                  = null,
        IGetStrategyInfo?             getStrategyInfo             = null,
        IGetStrategyList?             getStrategyList             = null,
        IGetUserProfile?              getUserProfile              = null,
        IGetVolumeAtPrice?            getVolumeAtPrice            = null,
        IIsThereAnAggregator?         isThereAnAggregator         = null,
        ILinkOrders?                  linkOrders                  = null,
        IListAgreements?              listAgreements              = null,
        IListAssignedUsers?           listAssignedUsers           = null,
        IListBinaryContracts?         listBinaryContracts         = null,
        IListEnvironments?            listEnvironments            = null,
        IListExchanges?               listExchanges               = null,
        IListIbs?                     listIbs                     = null,
        IListOrderHistoryDates?       listOrderHistoryDates       = null,
        IListTradeRoutes?             listTradeRoutes             = null,
        ILogin?                       login                       = null,
        ILoginRepository?             loginRepository             = null,
        ILogout?                      logout                      = null,
        ILogoutRepository?            logoutRepository            = null,
        IModifyBracketTier?           modifyBracketTier           = null,
        IModifyOrder?                 modifyOrder                 = null,
        IModifyOrderRefData?          modifyOrderRefData          = null,
        IRebuildBook?                 rebuildBook                 = null,
        IRebuildDboBook?              rebuildDboBook              = null,
        IReplayAllOrders?             replayAllOrders             = null,
        IReplayBars?                  replayBars                  = null,
        IReplayBrackets?              replayBrackets              = null,
        IReplayExecutions?            replayExecutions            = null,
        IReplayHistoricalOrders?      replayHistoricalOrders      = null,
        IReplayOpenOrders?            replayOpenOrders            = null,
        IReplayPnl?                   replayPnl                   = null,
        IReplayQuotes?                replayQuotes                = null,
        IReplaySingleHistoricalOrder? replaySingleHistoricalOrder = null,
        IReplaySingleOrder?           replaySingleOrder           = null,
        IReplayTrades?                replayTrades                = null,
        ISearchInstrument?            searchInstrument            = null,
        ISendBracketOrder?            sendBracketOrder            = null,
        ISendOcaList?                 sendOcaList                 = null,
        ISendOrder?                   sendOrder                   = null,
        ISetEnvironmentVariable?      setEnvironmentVariable      = null,
        ISetOrderContext?             setOrderContext             = null,
        IShutdown?                    shutdown                    = null,
        ISubmitQuoteList?             submitQuoteList             = null,
        ISubscribe?                   subscribe                   = null,
        ISubscribeAutoLiquidate?      subscribeAutoLiquidate      = null,
        ISubscribeBar?                subscribeBar                = null,
        ISubscribeBracket?            subscribeBracket            = null,
        ISubscribeByUnderlying?       subscribeByUnderlying       = null,
        ISubscribeDbo?                subscribeDbo                = null,
        ISubscribeEasyToBorrow?       subscribeEasyToBorrow       = null,
        ISubscribeOrder?              subscribeOrder              = null,
        ISubscribePnl?                subscribePnl                = null,
        ISubscribeTradeRoute?         subscribeTradeRoute         = null,
        IUnsetEnvironmentVariable?    unsetEnvironmentVariable    = null,
        IUnsubscribe?                 unsubscribe                 = null,
        IUnsubscribeAutoLiquidate?    unsubscribeAutoLiquidate    = null,
        IUnsubscribeBar?              unsubscribeBar              = null,
        IUnsubscribeBracket?          unsubscribeBracket          = null,
        IUnsubscribeByUnderlying?     unsubscribeByUnderlying     = null,
        IUnsubscribeDbo?              unsubscribeDbo              = null,
        IUnsubscribeEasyToBorrow?     unsubscribeEasyToBorrow     = null,
        IUnsubscribeOrder?            unsubscribeOrder            = null,
        IUnsubscribePnl?              unsubscribePnl              = null,
        IUnsubscribeTradeRoute?       unsubscribeTradeRoute       = null
    );
}
