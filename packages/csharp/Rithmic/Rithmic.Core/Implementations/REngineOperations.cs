using System.Collections.ObjectModel;
using com.omnesys.rapi;
namespace Rithmic.Core;

public class REngineOperations : IREngineOperations {

    private REngine? _rEngine;

    /// <summary>
    /// The Rithmic Engine.
    /// </summary>
    public REngine? REngine {
        get => _rEngine;
        set {
            _rEngine                             = value;
            _cancelAllOrders.REngine             = value;
            _cancelOrder.REngine                 = value;
            _cancelOrderList.REngine             = value;
            _cancelQuoteList.REngine             = value;
            _changePassword.REngine              = value;
            _createUserDefinedSpread.REngine     = value;
            _exitPosition.REngine                = value;
            _getAccounts.REngine                 = value;
            _getAuxRefData.REngine               = value;
            _getEasyToBorrowList.REngine         = value;
            _getEnvironment.REngine              = value;
            _getEquityOptionStrategyList.REngine = value;
            _getInstrumentByUnderlying.REngine   = value;
            _getOptionList.REngine               = value;
            _getOrderContext.REngine             = value;
            _getPendingInputSize.REngine         = value;
            _getPriceIncrInfo.REngine            = value;
            _getProductRmsInfo.REngine           = value;
            _getRefData.REngine                  = value;
            _getStrategyInfo.REngine             = value;
            _getStrategyList.REngine             = value;
            _getUserProfile.REngine              = value;
            _getVolumeAtPrice.REngine            = value;
            _isThereAnAggregator.REngine         = value;
            _linkOrders.REngine                  = value;
            _listAgreements.REngine              = value;
            _listAssignedUsers.REngine           = value;
            _listBinaryContracts.REngine         = value;
            _listEnvironments.REngine            = value;
            _listExchanges.REngine               = value;
            _listIbs.REngine                     = value;
            _listOrderHistoryDates.REngine       = value;
            _listTradeRoutes.REngine             = value;
            _login.REngine                       = value;
            _loginRepository.REngine             = value;
            _logout.REngine                      = value;
            _logoutRepository.REngine            = value;
            _modifyBracketTier.REngine           = value;
            _modifyOrder.REngine                 = value;
            _modifyOrderRefData.REngine          = value;
            _rebuildBook.REngine                 = value;
            _rebuildDboBook.REngine              = value;
            _replayAllOrders.REngine             = value;
            _replayBars.REngine                  = value;
            _replayBrackets.REngine              = value;
            _replayExecutions.REngine            = value;
            _replayHistoricalOrders.REngine      = value;
            _replayOpenOrders.REngine            = value;
            _replayPnl.REngine                   = value;
            _replayQuotes.REngine                = value;
            _replaySingleHistoricalOrder.REngine = value;
            _replaySingleOrder.REngine           = value;
            _replayTrades.REngine                = value;
            _searchInstrument.REngine            = value;
            _sendBracketOrder.REngine            = value;
            _sendOcaList.REngine                 = value;
            _sendOrder.REngine                   = value;
            _setEnvironmentVariable.REngine      = value;
            _setOrderContext.REngine             = value;
            _shutdown.REngine                    = value;
            _submitQuoteList.REngine             = value;
            _subscribe.REngine                   = value;
            _subscribeAutoLiquidate.REngine      = value;
            _subscribeBar.REngine                = value;
            _subscribeBracket.REngine            = value;
            _subscribeByUnderlying.REngine       = value;
            _subscribeDbo.REngine                = value;
            _subscribeEasyToBorrow.REngine       = value;
            _subscribeOrder.REngine              = value;
            _subscribePnl.REngine                = value;
            _subscribeTradeRoute.REngine         = value;
            _unsetEnvironmentVariable.REngine    = value;
            _unsubscribe.REngine                 = value;
            _unsubscribeAutoLiquidate.REngine    = value;
            _unsubscribeBar.REngine              = value;
            _unsubscribeBracket.REngine          = value;
            _unsubscribeByUnderlying.REngine     = value;
            _unsubscribeDbo.REngine              = value;
            _unsubscribeEasyToBorrow.REngine     = value;
            _unsubscribeOrder.REngine            = value;
            _unsubscribePnl.REngine              = value;
            _unsubscribeTradeRoute.REngine       = value;
        }
    }

    public REngineOperations(
        REngine?                       rEngine,
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
    ) {
        REngine                      = rEngine;
        _cancelAllOrders             = cancelAllOrders ?? new CancelAllOrders(rEngine);
        _cancelOrder                 = cancelOrder ?? new CancelOrder(rEngine);
        _cancelOrderList             = cancelOrderList ?? new CancelOrderList(rEngine);
        _cancelQuoteList             = cancelQuoteList ?? new CancelQuoteList(rEngine);
        _changePassword              = changePassword ?? new ChangePassword(rEngine);
        _createUserDefinedSpread     = createUserDefinedSpread ?? new CreateUserDefinedSpread(rEngine);
        _exitPosition                = exitPosition ?? new ExitPosition(rEngine);
        _getAccounts                 = getAccounts ?? new GetAccounts(rEngine);
        _getAuxRefData               = getAuxRefData ?? new GetAuxRefData(rEngine);
        _getEasyToBorrowList         = getEasyToBorrowList ?? new GetEasyToBorrowList(rEngine);
        _getEnvironment              = getEnvironment ?? new GetEnvironment(rEngine);
        _getEquityOptionStrategyList = getEquityOptionStrategyList ?? new GetEquityOptionStrategyList(rEngine);
        _getInstrumentByUnderlying   = getInstrumentByUnderlying ?? new GetInstrumentByUnderlying(rEngine);
        _getOptionList               = getOptionList ?? new GetOptionList(rEngine);
        _getOrderContext             = getOrderContext ?? new GetOrderContext(rEngine);
        _getPendingInputSize         = getPendingInputSize ?? new GetPendingInputSize(rEngine);
        _getPriceIncrInfo            = getPriceIncrInfo ?? new GetPriceIncrInfo(rEngine);
        _getProductRmsInfo           = getProductRmsInfo ?? new GetProductRmsInfo(rEngine);
        _getRefData                  = getRefData ?? new GetRefData(rEngine);
        _getStrategyInfo             = getStrategyInfo ?? new GetStrategyInfo(rEngine);
        _getStrategyList             = getStrategyList ?? new GetStrategyList(rEngine);
        _getUserProfile              = getUserProfile ?? new GetUserProfile(rEngine);
        _getVolumeAtPrice            = getVolumeAtPrice ?? new GetVolumeAtPrice(rEngine);
        _isThereAnAggregator         = isThereAnAggregator ?? new IsThereAnAggregator(rEngine);
        _linkOrders                  = linkOrders ?? new LinkOrders(rEngine);
        _listAgreements              = listAgreements ?? new ListAgreements(rEngine);
        _listAssignedUsers           = listAssignedUsers ?? new ListAssignedUsers(rEngine);
        _listBinaryContracts         = listBinaryContracts ?? new ListBinaryContracts(rEngine);
        _listEnvironments            = listEnvironments ?? new ListEnvironments(rEngine);
        _listExchanges               = listExchanges ?? new ListExchanges(rEngine);
        _listIbs                     = listIbs ?? new ListIbs(rEngine);
        _listOrderHistoryDates       = listOrderHistoryDates ?? new ListOrderHistoryDates(rEngine);
        _listTradeRoutes             = listTradeRoutes ?? new ListTradeRoutes(rEngine);
        _login                       = login ?? new Login(rEngine);
        _loginRepository             = loginRepository ?? new LoginRepository(rEngine);
        _logout                      = logout ?? new Logout(rEngine);
        _logoutRepository            = logoutRepository ?? new LogoutRepository(rEngine);
        _modifyBracketTier           = modifyBracketTier ?? new ModifyBracketTier(rEngine);
        _modifyOrder                 = modifyOrder ?? new ModifyOrder(rEngine);
        _modifyOrderRefData          = modifyOrderRefData ?? new ModifyOrderRefData(rEngine);
        _rebuildBook                 = rebuildBook ?? new RebuildBook(rEngine);
        _rebuildDboBook              = rebuildDboBook ?? new RebuildDboBook(rEngine);
        _replayAllOrders             = replayAllOrders ?? new ReplayAllOrders(rEngine);
        _replayBars                  = replayBars ?? new ReplayBars(rEngine);
        _replayBrackets              = replayBrackets ?? new ReplayBrackets(rEngine);
        _replayExecutions            = replayExecutions ?? new ReplayExecutions(rEngine);
        _replayHistoricalOrders      = replayHistoricalOrders ?? new ReplayHistoricalOrders(rEngine);
        _replayOpenOrders            = replayOpenOrders ?? new ReplayOpenOrders(rEngine);
        _replayPnl                   = replayPnl ?? new ReplayPnl(rEngine);
        _replayQuotes                = replayQuotes ?? new ReplayQuotes(rEngine);
        _replaySingleHistoricalOrder = replaySingleHistoricalOrder ?? new ReplaySingleHistoricalOrder(rEngine);
        _replaySingleOrder           = replaySingleOrder ?? new ReplaySingleOrder(rEngine);
        _replayTrades                = replayTrades ?? new ReplayTrades(rEngine);
        _searchInstrument            = searchInstrument ?? new SearchInstrument(rEngine);
        _sendBracketOrder            = sendBracketOrder ?? new SendBracketOrder(rEngine);
        _sendOcaList                 = sendOcaList ?? new SendOcaList(rEngine);
        _sendOrder                   = sendOrder ?? new SendOrder(rEngine);
        _setEnvironmentVariable      = setEnvironmentVariable ?? new SetEnvironmentVariable(rEngine);
        _setOrderContext             = setOrderContext ?? new SetOrderContext(rEngine);
        _shutdown                    = shutdown ?? new Shutdown(rEngine);
        _submitQuoteList             = submitQuoteList ?? new SubmitQuoteList(rEngine);
        _subscribe                   = subscribe ?? new Subscribe(rEngine);
        _subscribeAutoLiquidate      = subscribeAutoLiquidate ?? new SubscribeAutoLiquidate(rEngine);
        _subscribeBar                = subscribeBar ?? new SubscribeBar(rEngine);
        _subscribeBracket            = subscribeBracket ?? new SubscribeBracket(rEngine);
        _subscribeByUnderlying       = subscribeByUnderlying ?? new SubscribeByUnderlying(rEngine);
        _subscribeDbo                = subscribeDbo ?? new SubscribeDbo(rEngine);
        _subscribeEasyToBorrow       = subscribeEasyToBorrow ?? new SubscribeEasyToBorrow(rEngine);
        _subscribeOrder              = subscribeOrder ?? new SubscribeOrder(rEngine);
        _subscribePnl                = subscribePnl ?? new SubscribePnl(rEngine);
        _subscribeTradeRoute         = subscribeTradeRoute ?? new SubscribeTradeRoute(rEngine);
        _unsetEnvironmentVariable    = unsetEnvironmentVariable ?? new UnsetEnvironmentVariable(rEngine);
        _unsubscribe                 = unsubscribe ?? new Unsubscribe(rEngine);
        _unsubscribeAutoLiquidate    = unsubscribeAutoLiquidate ?? new UnsubscribeAutoLiquidate(rEngine);
        _unsubscribeBar              = unsubscribeBar ?? new UnsubscribeBar(rEngine);
        _unsubscribeBracket          = unsubscribeBracket ?? new UnsubscribeBracket(rEngine);
        _unsubscribeByUnderlying     = unsubscribeByUnderlying ?? new UnsubscribeByUnderlying(rEngine);
        _unsubscribeDbo              = unsubscribeDbo ?? new UnsubscribeDbo(rEngine);
        _unsubscribeEasyToBorrow     = unsubscribeEasyToBorrow ?? new UnsubscribeEasyToBorrow(rEngine);
        _unsubscribeOrder            = unsubscribeOrder ?? new UnsubscribeOrder(rEngine);
        _unsubscribePnl              = unsubscribePnl ?? new UnsubscribePnl(rEngine);
        _unsubscribeTradeRoute       = unsubscribeTradeRoute ?? new UnsubscribeTradeRoute(rEngine);
    }

    private readonly ICancelAllOrders             _cancelAllOrders;
    private readonly ICancelOrder                 _cancelOrder;
    private readonly ICancelOrderList             _cancelOrderList;
    private readonly ICancelQuoteList             _cancelQuoteList;
    private readonly IChangePassword              _changePassword;
    private readonly ICreateUserDefinedSpread     _createUserDefinedSpread;
    private readonly IExitPosition                _exitPosition;
    private readonly IGetAccounts                 _getAccounts;
    private readonly IGetAuxRefData               _getAuxRefData;
    private readonly IGetEasyToBorrowList         _getEasyToBorrowList;
    private readonly IGetEnvironment              _getEnvironment;
    private readonly IGetEquityOptionStrategyList _getEquityOptionStrategyList;
    private readonly IGetInstrumentByUnderlying   _getInstrumentByUnderlying;
    private readonly IGetOptionList               _getOptionList;
    private readonly IGetOrderContext             _getOrderContext;
    private readonly IGetPendingInputSize         _getPendingInputSize;
    private readonly IGetPriceIncrInfo            _getPriceIncrInfo;
    private readonly IGetProductRmsInfo           _getProductRmsInfo;
    private readonly IGetRefData                  _getRefData;
    private readonly IGetStrategyInfo             _getStrategyInfo;
    private readonly IGetStrategyList             _getStrategyList;
    private readonly IGetUserProfile              _getUserProfile;
    private readonly IGetVolumeAtPrice            _getVolumeAtPrice;
    private readonly IIsThereAnAggregator         _isThereAnAggregator;
    private readonly ILinkOrders                  _linkOrders;
    private readonly IListAgreements              _listAgreements;
    private readonly IListAssignedUsers           _listAssignedUsers;
    private readonly IListBinaryContracts         _listBinaryContracts;
    private readonly IListEnvironments            _listEnvironments;
    private readonly IListExchanges               _listExchanges;
    private readonly IListIbs                     _listIbs;
    private readonly IListOrderHistoryDates       _listOrderHistoryDates;
    private readonly IListTradeRoutes             _listTradeRoutes;
    private readonly ILogin                       _login;
    private readonly ILoginRepository             _loginRepository;
    private readonly ILogout                      _logout;
    private readonly ILogoutRepository            _logoutRepository;
    private readonly IModifyBracketTier           _modifyBracketTier;
    private readonly IModifyOrder                 _modifyOrder;
    private readonly IModifyOrderRefData          _modifyOrderRefData;
    private readonly IRebuildBook                 _rebuildBook;
    private readonly IRebuildDboBook              _rebuildDboBook;
    private readonly IReplayAllOrders             _replayAllOrders;
    private readonly IReplayBars                  _replayBars;
    private readonly IReplayBrackets              _replayBrackets;
    private readonly IReplayExecutions            _replayExecutions;
    private readonly IReplayHistoricalOrders      _replayHistoricalOrders;
    private readonly IReplayOpenOrders            _replayOpenOrders;
    private readonly IReplayPnl                   _replayPnl;
    private readonly IReplayQuotes                _replayQuotes;
    private readonly IReplaySingleHistoricalOrder _replaySingleHistoricalOrder;
    private readonly IReplaySingleOrder           _replaySingleOrder;
    private readonly IReplayTrades                _replayTrades;
    private readonly ISearchInstrument            _searchInstrument;
    private readonly ISendBracketOrder            _sendBracketOrder;
    private readonly ISendOcaList                 _sendOcaList;
    private readonly ISendOrder                   _sendOrder;
    private readonly ISetEnvironmentVariable      _setEnvironmentVariable;
    private readonly ISetOrderContext             _setOrderContext;
    private readonly IShutdown                    _shutdown;
    private readonly ISubmitQuoteList             _submitQuoteList;
    private readonly ISubscribe                   _subscribe;
    private readonly ISubscribeAutoLiquidate      _subscribeAutoLiquidate;
    private readonly ISubscribeBar                _subscribeBar;
    private readonly ISubscribeBracket            _subscribeBracket;
    private readonly ISubscribeByUnderlying       _subscribeByUnderlying;
    private readonly ISubscribeDbo                _subscribeDbo;
    private readonly ISubscribeEasyToBorrow       _subscribeEasyToBorrow;
    private readonly ISubscribeOrder              _subscribeOrder;
    private readonly ISubscribePnl                _subscribePnl;
    private readonly ISubscribeTradeRoute         _subscribeTradeRoute;
    private readonly IUnsetEnvironmentVariable    _unsetEnvironmentVariable;
    private readonly IUnsubscribe                 _unsubscribe;
    private readonly IUnsubscribeAutoLiquidate    _unsubscribeAutoLiquidate;
    private readonly IUnsubscribeBar              _unsubscribeBar;
    private readonly IUnsubscribeBracket          _unsubscribeBracket;
    private readonly IUnsubscribeByUnderlying     _unsubscribeByUnderlying;
    private readonly IUnsubscribeDbo              _unsubscribeDbo;
    private readonly IUnsubscribeEasyToBorrow     _unsubscribeEasyToBorrow;
    private readonly IUnsubscribeOrder            _unsubscribeOrder;
    private readonly IUnsubscribePnl              _unsubscribePnl;
    private readonly IUnsubscribeTradeRoute       _unsubscribeTradeRoute;

    public void cancelAllOrders(
        AccountInfo oAccount,
        string      sEntryType,
        string      sTradingAlgorithm) =>
        _cancelAllOrders.cancelAllOrders(oAccount, sEntryType, sTradingAlgorithm);

    public void cancelOrder(
        AccountInfo oAccount,
        string      sOrderNum,
        string      sEntryType,
        string      sTradingAlgorithm,
        string      sUserMsg,
        object      oContext
    ) =>
        _cancelOrder.cancelOrder(oAccount, sOrderNum, sEntryType, sTradingAlgorithm, sUserMsg, oContext);

    public void cancelOrderList(
        ReadOnlyCollection<AccountInfo> oAccountList,
        ReadOnlyCollection<string>      oOrderNumList,
        ReadOnlyCollection<string>      oEntryTypeList,
        ReadOnlyCollection<string>      oTradingAlgorithmList,
        ReadOnlyCollection<string>      oUserMsgList,
        ReadOnlyCollection<object>      oContextList
    ) =>
        _cancelOrderList.cancelOrderList(oAccountList, oOrderNumList, oEntryTypeList, oTradingAlgorithmList, oUserMsgList, oContextList);


    public void cancelQuoteList(ReadOnlyCollection<QuoteCancelParams> oList) =>
        _cancelQuoteList.cancelQuoteList(oList);

    public void changePassword(
        string sOldPassword,
        string sNewPassword
    ) =>
        _changePassword.changePassword(sOldPassword, sNewPassword);

    public void createUserDefinedSpread(
        AccountInfo                oAccount,
        string                     sExchange,
        string                     sTradeRoute,
        string                     sEntryType,
        string                     sStrategyType,
        ReadOnlyCollection<string> oLegSymbols,
        ReadOnlyCollection<int>    oLegRatios,
        object                     oContext
    ) =>
        _createUserDefinedSpread.createUserDefinedSpread
            (oAccount, sExchange, sTradeRoute, sEntryType, sStrategyType, oLegSymbols, oLegRatios, oContext);

    public void exitPosition(
        AccountInfo oAccount,
        string      sExchange,
        string      sSymbol,
        string      sEntryType,
        string      sTradingAlgorithm,
        object      oContext
    ) =>
        _exitPosition.exitPosition(oAccount, sExchange, sSymbol, sEntryType, sTradingAlgorithm, oContext);

    public void getAccounts(string sStatus) =>
        _getAccounts.getAccounts(sStatus);

    public void getAuxRefData(
        string sExchange,
        string sSymbol,
        object oContext
    ) =>
        _getAuxRefData.getAuxRefData(sExchange, sSymbol, oContext);

    public void getEasyToBorrowList(object oContext) =>
        _getEasyToBorrowList.getEasyToBorrowList(oContext);

    public void getEnvironment(
        string sKey,
        object oContext
    ) =>
        _getEnvironment.getEnvironment(sKey, oContext);

    public void getEquityOptionStrategyList(
        string sExchange,
        string sUnderlying,
        string sStrategyType,
        string sExpiration,
        object oContext
    ) =>
        _getEquityOptionStrategyList.getEquityOptionStrategyList(sExchange, sUnderlying, sStrategyType, sExpiration, oContext);

    public void getInstrumentByUnderlying(
        string sUnderlying,
        string sExchange,
        string sExpiration,
        object oContext
    ) =>
        _getInstrumentByUnderlying.getInstrumentByUnderlying(sUnderlying, sExchange, sExpiration, oContext);

    public void getOptionList(
        string sExchange,
        string sProduct,
        string sExpirationCCYYMM,
        object oContext
    ) =>
        _getOptionList.getOptionList(sExchange, sProduct, sExpirationCCYYMM, oContext);

    public object? getOrderContext(string sOrderNum) =>
        _getOrderContext.getOrderContext(sOrderNum);

    public long? getPendingInputSize(ConnectionId eConnId) =>
        _getPendingInputSize.getPendingInputSize(eConnId);

    public void getPriceIncrInfo(
        string sExchange,
        string sSymbol,
        object oContext
    ) =>
        _getPriceIncrInfo.getPriceIncrInfo(sExchange, sSymbol, oContext);

    public void getProductRmsInfo(
        AccountInfo oAccount,
        object      oContext
    ) =>
        _getProductRmsInfo.getProductRmsInfo(oAccount, oContext);

    public void getRefData(
        string sExchange,
        string sSymbol,
        object oContext
    ) =>
        _getRefData.getRefData(sExchange, sSymbol, oContext);

    public void getStrategyInfo(
        string sExchange,
        string sSymbol,
        object oContext
    ) =>
        _getStrategyInfo.getStrategyInfo(sExchange, sSymbol, oContext);

    public void getStrategyList(
        string sExchange,
        string sProduct,
        string sStrategyType,
        string sExpirationCCYYMM,
        object oContext
    ) =>
        _getStrategyList.getStrategyList(sExchange, sProduct, sStrategyType, sExpirationCCYYMM, oContext);

    public void getUserProfile(
        ConnectionId eConnId,
        object       oContext
    ) =>
        _getUserProfile.getUserProfile(eConnId, oContext);

    public void getVolumeAtPrice(
        string sExchange,
        string sSymbol,
        object oContext
    ) =>
        _getVolumeAtPrice.getVolumeAtPrice(sExchange, sSymbol, oContext);

    public void isThereAnAggregator() =>
        _isThereAnAggregator.isThereAnAggregator();

    public void linkOrders(
        ReadOnlyCollection<AccountInfo> oAccountList,
        ReadOnlyCollection<string>      oOrderNumList
    ) =>
        _linkOrders.linkOrders(oAccountList, oOrderNumList);

    public void listAgreements(
        bool   bAccepted,
        object oContext
    ) =>
        _listAgreements.listAgreements(bAccepted, oContext);

    public void listAssignedUsers(
        AccountInfo oAccount,
        object      oContext
    ) =>
        _listAssignedUsers.listAssignedUsers(oAccount, oContext);

    public void listBinaryContracts(
        string sExchange,
        string sProductCode,
        object oContext
    ) =>
        _listBinaryContracts.listBinaryContracts(sExchange, sProductCode, oContext);

    public void listEnvironments(object oContext) =>
        _listEnvironments.listEnvironments(oContext);

    public void listExchanges(object oContext) =>
        _listExchanges.listExchanges(oContext);

    public void listIbs(object oContext) =>
        _listIbs.listIbs(oContext);

    public void listOrderHistoryDates(object oContext) =>
        _listOrderHistoryDates.listOrderHistoryDates(oContext);

    public void listTradeRoutes(object oContext) =>
        _listTradeRoutes.listTradeRoutes(oContext);

    public void login(
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
    ) =>
        _login.login
        (
            oCallbacksFacade,
            sMdEnvKey,
            sMdUser,
            sMdPassword,
            sMdCnnctPt,
            sTsEnvKey,
            sTsUser,
            sTsPassword,
            sTsCnnctPt,
            sPnlCnnctPt,
            sIhEnvKey,
            sIhUser,
            sIhPassword,
            sIhCnnctPt
        );

    public void loginRepository(
        RCallbacksFacade oCallbacksFacade,
        string           sEnvKey,
        string           sUser,
        string           sPassword,
        string           sCnnctPt
    ) =>
        _loginRepository.loginRepository(oCallbacksFacade, sEnvKey, sUser, sPassword, sCnnctPt);

    public void logout() =>
        _logout.logout();

    public void logoutRepository() =>
        _logoutRepository.logoutRepository();

    public void modifyBracketTier(
        AccountInfo oAccount,
        string      sOrderNum,
        bool        bTarget,
        int         iOldTickOffset,
        int         iNewTickOffset,
        object      oContext
    ) =>
        _modifyBracketTier.modifyBracketTier(oAccount, sOrderNum, bTarget, iOldTickOffset, iNewTickOffset, oContext);

    public void modifyOrder(ModifyLimitOrderParams oParamsIn) =>
        _modifyOrder.modifyOrder(oParamsIn);

    public void modifyOrder(ModifyOrderParams oParams) =>
        _modifyOrder.modifyOrder(oParams);

    public void modifyOrder(ModifyStopLimitOrderParams oParamsIn) =>
        _modifyOrder.modifyOrder(oParamsIn);

    public void modifyOrder(ModifyStopMarketOrderParams oParamsIn) =>
        _modifyOrder.modifyOrder(oParamsIn);

    public void modifyOrderList(ReadOnlyCollection<ModifyOrderParams> oList) =>
        _modifyOrder.modifyOrderList(oList);

    public void modifyOrderRefData(
        AccountInfo oAccount,
        string      sOrderNum,
        string      sUserMsg,
        string      sUserTag
    ) =>
        _modifyOrderRefData.modifyOrderRefData(oAccount, sOrderNum, sUserMsg, sUserTag);

    public void rebuildBook(
        string sExchange,
        string sSymbol,
        object oContext
    ) =>
        _rebuildBook.rebuildBook(sExchange, sSymbol, oContext);

    public void rebuildDboBook(
        string sExchange,
        string sSymbol,
        double dPrice,
        object oContext
    ) =>
        _rebuildDboBook.rebuildDboBook(sExchange, sSymbol, dPrice, oContext);

    public void replayAllOrders(
        AccountInfo oAccount,
        int         iStartSsboe,
        int         iEndSsboe,
        object      oContext
    ) =>
        _replayAllOrders.replayAllOrders(oAccount, iStartSsboe, iEndSsboe, oContext);

    public void replayBars(ReplayBarParams oParams) =>
        _replayBars.replayBars(oParams);

    public void replayBrackets(
        AccountInfo oAccount,
        object      oContext
    ) =>
        _replayBrackets.replayBrackets(oAccount, oContext);

    public void replayExecutions(
        AccountInfo oAccount,
        int         iStartSsboe,
        int         iEndSsboe,
        object      oContext
    ) =>
        _replayExecutions.replayExecutions(oAccount, iStartSsboe, iEndSsboe, oContext);

    public void replayHistoricalOrders(
        AccountInfo oAccount,
        string      sDate,
        object      oContext
    ) =>
        _replayHistoricalOrders.replayHistoricalOrders(oAccount, sDate, oContext);

    public void replayOpenOrders(
        AccountInfo oAccount,
        object      oContext
    ) =>
        _replayOpenOrders.replayOpenOrders(oAccount, oContext);

    public void replayPnl(
        AccountInfo oAccount,
        object      oContext
    ) =>
        _replayPnl.replayPnl(oAccount, oContext);

    public void replayQuotes(
        AccountInfo oAccount,
        object      oContext
    ) =>
        _replayQuotes.replayQuotes(oAccount, oContext);

    public void replaySingleHistoricalOrder(
        AccountInfo oAccount,
        string      sOrderNum,
        string      sDate,
        object      oContext
    ) =>
        _replaySingleHistoricalOrder.replaySingleHistoricalOrder(oAccount, sOrderNum, sDate, oContext);

    public void replaySingleOrder(
        AccountInfo oAccount,
        string      sOrderNum,
        object      oContext
    ) =>
        _replaySingleOrder.replaySingleOrder(oAccount, sOrderNum, oContext);

    public void replayTrades(
        string sExchange,
        string sSymbol,
        int    iStartSsboe,
        int    iEndSsboe,
        object oContext
    ) =>
        _replayTrades.replayTrades(sExchange, sSymbol, iStartSsboe, iEndSsboe, oContext);

    public void searchInstrument(
        string                         sExchange,
        ReadOnlyCollection<SearchTerm> oTerms,
        object                         oContext
    ) =>
        _searchInstrument.searchInstrument(sExchange, oTerms, oContext);

    public void sendBracketOrder(
        OrderParams   oEntry,
        BracketParams oBracketParams
    ) =>
        _sendBracketOrder.sendBracketOrder(oEntry, oBracketParams);

    public void sendBracketOrder(
        OrderParams        oEntry,
        string             sBracketType,
        string             sOrderOperationType,
        bool               bTrailingStop,
        int                iTrailByPriceId,
        IList<BracketTier> oTargets,
        IList<BracketTier> oStops
    ) =>
        _sendBracketOrder.sendBracketOrder(oEntry, sBracketType, sOrderOperationType, bTrailingStop, iTrailByPriceId, oTargets, oStops);

    public void sendOcaList(
        string                          sOcaType,
        ReadOnlyCollection<OrderParams> oList
    ) =>
        _sendOcaList.sendOcaList(sOcaType, oList);

    public void sendOrder(LimitOrderParams oParamsIn) =>
        _sendOrder.sendOrder(oParamsIn);

    public void sendOrder(MarketOrderParams oParamsIn) =>
        _sendOrder.sendOrder(oParamsIn);

    public void sendOrder(StopLimitOrderParams oParamsIn) =>
        _sendOrder.sendOrder(oParamsIn);

    public void sendOrder(StopMarketOrderParams oParamsIn) =>
        _sendOrder.sendOrder(oParamsIn);

    public void sendOrderList(ReadOnlyCollection<OrderParams> oList) =>
        _sendOrder.sendOrderList(oList);

    public void setEnvironmentVariable(
        string sKey,
        string sVariable,
        string sValue
    ) =>
        _setEnvironmentVariable.setEnvironmentVariable(sKey, sVariable, sValue);

    public void setOrderContext(
        string sOrderNum,
        object oContext
    ) =>
        _setOrderContext.setOrderContext(sOrderNum, oContext);

    public void shutdown() =>
        _shutdown.shutdown();

    public void submitQuoteList(ReadOnlyCollection<QuoteParams> oList) =>
        _submitQuoteList.submitQuoteList(oList);

    public void subscribe(
        string            sExchange,
        string            sSymbol,
        SubscriptionFlags eFlags,
        object            oContext
    ) =>
        _subscribe.subscribe(sExchange, sSymbol, eFlags, oContext);

    public void subscribeAutoLiquidate(AccountInfo oAccount) =>
        _subscribeAutoLiquidate.subscribeAutoLiquidate(oAccount);

    public void subscribeBar(BarParams oParams) =>
        _subscribeBar.subscribeBar(oParams);

    public void subscribeBracket(AccountInfo oAccount) =>
        _subscribeBracket.subscribeBracket(oAccount);

    public void subscribeByUnderlying(
        string            sUnderlying,
        string            sExchange,
        string            sExpiration,
        SubscriptionFlags eFlags,
        object            oContext
    ) =>
        _subscribeByUnderlying.subscribeByUnderlying(sUnderlying, sExchange, sExpiration, eFlags, oContext);

    public void subscribeDbo(
        string sExchange,
        string sSymbol,
        double dPrice,
        object oContext
    ) =>
        _subscribeDbo.subscribeDbo(sExchange, sSymbol, dPrice, oContext);

    public void subscribeEasyToBorrow(object oContext) =>
        _subscribeEasyToBorrow.subscribeEasyToBorrow(oContext);

    public void subscribeOrder(AccountInfo oAccount) =>
        _subscribeOrder.subscribeOrder(oAccount);

    public void subscribePnl(AccountInfo oAccount) =>
        _subscribePnl.subscribePnl(oAccount);

    public void subscribeTradeRoute(
        string sFcmId,
        string sIbId,
        object oContext
    ) =>
        _subscribeTradeRoute.subscribeTradeRoute(sFcmId, sIbId, oContext);

    public void unsetEnvironmentVariable(
        string sKey,
        string sVariable
    ) =>
        _unsetEnvironmentVariable.unsetEnvironmentVariable(sKey, sVariable);

    public void unsubscribe(
        string sExchange,
        string sSymbol
    ) =>
        _unsubscribe.unsubscribe(sExchange, sSymbol);

    public void unsubscribeAutoLiquidate(AccountInfo oAccount) =>
        _unsubscribeAutoLiquidate.unsubscribeAutoLiquidate(oAccount);

    public void unsubscribeBar(BarParams oParams) =>
        _unsubscribeBar.unsubscribeBar(oParams);

    public void unsubscribeBracket(AccountInfo oAccount) =>
        _unsubscribeBracket.unsubscribeBracket(oAccount);

    public void unsubscribeByUnderlying(
        string sUnderlying,
        string sExchange,
        string sExpiration
    ) =>
        _unsubscribeByUnderlying.unsubscribeByUnderlying(sUnderlying, sExchange, sExpiration);

    public void unsubscribeDbo(
        string sExchange,
        string sSymbol,
        double dPrice,
        object oContext
    ) =>
        _unsubscribeDbo.unsubscribeDbo(sExchange, sSymbol, dPrice, oContext);

    public void unsubscribeEasyToBorrow() =>
        _unsubscribeEasyToBorrow.unsubscribeEasyToBorrow();

    public void unsubscribeOrder(AccountInfo oAccount) =>
        _unsubscribeOrder.unsubscribeOrder(oAccount);

    public void unsubscribePnl(AccountInfo oAccount) =>
        _unsubscribePnl.unsubscribePnl(oAccount);

    public void unsubscribeTradeRoute(
        string sFcmId,
        string sIbId
    ) =>
        _unsubscribeTradeRoute.unsubscribeTradeRoute(sFcmId, sIbId);

}

/// <summary>
/// Base class implementing the IREngineProvider interface.
/// </summary>
public abstract class REngineProvider : IREngineProvider {

    /// <summary>
    /// Constructor.
    /// </summary>
    protected REngineProvider(REngine rEngine) {
        REngine = rEngine;
    }

    /// <summary>
    /// Rithmic Engine.
    /// </summary>
    public REngine? REngine { get; set; }

}

public class CancelAllOrders(REngine engine)
    : REngineProvider(engine),
      ICancelAllOrders {
    public void cancelAllOrders(
        AccountInfo oAccount,
        string      sEntryType,
        string      sTradingAlgorithm
    ) {
        REngine.cancelAllOrders
        (
            oAccount,
            sEntryType,
            sTradingAlgorithm
        );
    }
}

public class CancelOrder(REngine engine)
    : REngineProvider(engine),
      ICancelOrder {

    public void cancelOrder(
        AccountInfo oAccount,
        string      sOrderNum,
        string      sEntryType,
        string      sTradingAlgorithm,
        string      sUserMsg,
        object      oContext
    ) {
        REngine?.cancelOrder
        (
            oAccount,
            sOrderNum,
            sEntryType,
            sTradingAlgorithm,
            sUserMsg,
            oContext
        );
    }
}

public class CancelOrderList(REngine engine)
    : REngineProvider(engine),
      ICancelOrderList {

    public void cancelOrderList(
        ReadOnlyCollection<AccountInfo> oAccountList,
        ReadOnlyCollection<string>      oOrderNumList,
        ReadOnlyCollection<string>      oEntryTypeList,
        ReadOnlyCollection<string>      oTradingAlgorithmList,
        ReadOnlyCollection<string>      oUserMsgList,
        ReadOnlyCollection<object>      oContextList
    ) {
        REngine?.cancelOrderList
        (
            oAccountList,
            oOrderNumList,
            oEntryTypeList,
            oTradingAlgorithmList,
            oUserMsgList,
            oContextList
        );
    }
}

public class CancelQuoteList(REngine engine)
    : REngineProvider(engine),
      ICancelQuoteList {

    public void cancelQuoteList(ReadOnlyCollection<QuoteCancelParams> oList) {
        REngine?.cancelQuoteList(oList);
    }
}

public class ChangePassword(REngine engine)
    : REngineProvider(engine),
      IChangePassword {

    public void changePassword(
        string sOldPassword,
        string sNewPassword
    ) {
        REngine?.changePassword
        (
            sOldPassword,
            sNewPassword
        );
    }
}

public class CreateUserDefinedSpread(REngine engine)
    : REngineProvider(engine),
      ICreateUserDefinedSpread {

    public void createUserDefinedSpread(
        AccountInfo                oAccount,
        string                     sExchange,
        string                     sTradeRoute,
        string                     sEntryType,
        string                     sStrategyType,
        ReadOnlyCollection<string> oLegSymbols,
        ReadOnlyCollection<int>    oLegRatios,
        object                     oContext
    ) {
        REngine?.createUserDefinedSpread
        (
            oAccount,
            sExchange,
            sTradeRoute,
            sEntryType,
            sStrategyType,
            oLegSymbols,
            oLegRatios,
            oContext
        );
    }
}

public class ExitPosition(REngine engine)
    : REngineProvider(engine),
      IExitPosition {

    public void exitPosition(
        AccountInfo oAccount,
        string      sExchange,
        string      sSymbol,
        string      sEntryType,
        string      sTradingAlgorithm,
        object      oContext
    ) {
        REngine?.exitPosition
        (
            oAccount,
            sExchange,
            sSymbol,
            sEntryType,
            sTradingAlgorithm,
            oContext
        );
    }
}

public class GetAccounts(REngine engine)
    : REngineProvider(engine),
      IGetAccounts {

    public void getAccounts(string sStatus) {
        REngine?.getAccounts(sStatus);
    }
}

public class GetAuxRefData(REngine engine)
    : REngineProvider(engine),
      IGetAuxRefData {

    public void getAuxRefData(
        string sExchange,
        string sSymbol,
        object oContext
    ) {
        REngine?.getAuxRefData
        (
            sExchange,
            sSymbol,
            oContext
        );
    }
}

public class GetEasyToBorrowList(REngine engine)
    : REngineProvider(engine),
      IGetEasyToBorrowList {

    public void getEasyToBorrowList(object oContext) {
        REngine?.getEasyToBorrowList(oContext);
    }
}

public class GetEnvironment(REngine engine)
    : REngineProvider(engine),
      IGetEnvironment {

    public void getEnvironment(
        string sKey,
        object oContext
    ) {
        REngine?.getEnvironment
        (
            sKey,
            oContext
        );
    }
}

public class GetEquityOptionStrategyList(REngine engine)
    : REngineProvider(engine),
      IGetEquityOptionStrategyList {

    public void getEquityOptionStrategyList(
        string sExchange,
        string sUnderlying,
        string sStrategyType,
        string sExpiration,
        object oContext
    ) {
        REngine?.getEquityOptionStrategyList
        (
            sExchange,
            sUnderlying,
            sStrategyType,
            sExpiration,
            oContext
        );
    }
}

public class GetInstrumentByUnderlying(REngine engine)
    : REngineProvider(engine),
      IGetInstrumentByUnderlying {

    public void getInstrumentByUnderlying(
        string sUnderlying,
        string sExchange,
        string sExpiration,
        object oContext
    ) {
        REngine?.getInstrumentByUnderlying
        (
            sUnderlying,
            sExchange,
            sExpiration,
            oContext
        );
    }
}

public class GetOptionList(REngine engine)
    : REngineProvider(engine),
      IGetOptionList {

    public void getOptionList(
        string sExchange,
        string sProduct,
        string sExpirationCCYYMM,
        object oContext
    ) {
        REngine?.getOptionList
        (
            sExchange,
            sProduct,
            sExpirationCCYYMM,
            oContext
        );
    }
}

public class GetOrderContext(REngine engine)
    : REngineProvider(engine),
      IGetOrderContext {

    public object? getOrderContext(string sOrderNum) {
        return REngine?.getOrderContext(sOrderNum);
    }
}

public class GetPendingInputSize(REngine engine)
    : REngineProvider(engine),
      IGetPendingInputSize {

    public long? getPendingInputSize(ConnectionId eConnId) {
        return REngine?.getPendingInputSize(eConnId);
    }
}

public class GetPriceIncrInfo(REngine engine)
    : REngineProvider(engine),
      IGetPriceIncrInfo {

    public void getPriceIncrInfo(
        string sExchange,
        string sSymbol,
        object oContext
    ) {
        REngine?.getPriceIncrInfo
        (
            sExchange,
            sSymbol,
            oContext
        );
    }
}

public class GetProductRmsInfo(REngine engine)
    : REngineProvider(engine),
      IGetProductRmsInfo {

    public void getProductRmsInfo(
        AccountInfo oAccount,
        object      oContext
    ) {
        REngine?.getProductRmsInfo
        (
            oAccount,
            oContext
        );
    }
}

public class GetRefData(REngine engine)
    : REngineProvider(engine),
      IGetRefData {

    public void getRefData(
        string sExchange,
        string sSymbol,
        object oContext
    ) {
        REngine?.getRefData
        (
            sExchange,
            sSymbol,
            oContext
        );
    }
}

public class GetStrategyInfo(REngine engine)
    : REngineProvider(engine),
      IGetStrategyInfo {

    public void getStrategyInfo(
        string sExchange,
        string sSymbol,
        object oContext
    ) {
        REngine?.getStrategyInfo
        (
            sExchange,
            sSymbol,
            oContext
        );
    }
}

public class GetStrategyList(REngine engine)
    : REngineProvider(engine),
      IGetStrategyList {

    public void getStrategyList(
        string sExchange,
        string sProduct,
        string sStrategyType,
        string sExpirationCCYYMM,
        object oContext
    ) {
        REngine?.getStrategyList
        (
            sExchange,
            sProduct,
            sStrategyType,
            sExpirationCCYYMM,
            oContext
        );
    }
}

public class GetUserProfile(REngine engine)
    : REngineProvider(engine),
      IGetUserProfile {

    public void getUserProfile(
        ConnectionId eConnId,
        object       oContext
    ) {
        REngine?.getUserProfile
        (
            eConnId,
            oContext
        );
    }
}

public class GetVolumeAtPrice(REngine engine)
    : REngineProvider(engine),
      IGetVolumeAtPrice {

    public void getVolumeAtPrice(
        string sExchange,
        string sSymbol,
        object oContext
    ) {
        REngine?.getVolumeAtPrice
        (
            sExchange,
            sSymbol,
            oContext
        );
    }
}

public class IsThereAnAggregator(REngine engine)
    : REngineProvider(engine),
      IIsThereAnAggregator {

    public void isThereAnAggregator() {
        REngine?.isThereAnAggregator();
    }
}

public class LinkOrders(REngine engine)
    : REngineProvider(engine),
      ILinkOrders {

    public void linkOrders(
        ReadOnlyCollection<AccountInfo> oAccountList,
        ReadOnlyCollection<string>      oOrderNumList
    ) {
        REngine?.linkOrders
        (
            oAccountList,
            oOrderNumList
        );
    }
}

public class ListAgreements(REngine engine)
    : REngineProvider(engine),
      IListAgreements {

    public void listAgreements(
        bool   bAccepted,
        object oContext
    ) {
        REngine?.listAgreements
        (
            bAccepted,
            oContext
        );
    }
}

public class ListAssignedUsers(REngine engine)
    : REngineProvider(engine),
      IListAssignedUsers {

    public void listAssignedUsers(
        AccountInfo oAccount,
        object      oContext
    ) {
        REngine?.listAssignedUsers
        (
            oAccount,
            oContext
        );
    }
}

public class ListBinaryContracts(REngine engine)
    : REngineProvider(engine),
      IListBinaryContracts {

    public void listBinaryContracts(
        string sExchange,
        string sProductCode,
        object oContext
    ) {
        REngine?.listBinaryContracts
        (
            sExchange,
            sProductCode,
            oContext
        );
    }
}

public class ListEnvironments(REngine engine)
    : REngineProvider(engine),
      IListEnvironments {

    public void listEnvironments(object oContext) {
        REngine?.listEnvironments(oContext);
    }
}

public class ListExchanges(REngine engine)
    : REngineProvider(engine),
      IListExchanges {

    public void listExchanges(object oContext) {
        REngine?.listExchanges(oContext);
    }
}

public class ListIbs(REngine engine)
    : REngineProvider(engine),
      IListIbs {

    public void listIbs(object oContext) {
        REngine?.listIbs(oContext);
    }
}

public class ListOrderHistoryDates(REngine engine)
    : REngineProvider(engine),
      IListOrderHistoryDates {

    public void listOrderHistoryDates(object oContext) {
        REngine?.listOrderHistoryDates(oContext);
    }
}

public class ListTradeRoutes(REngine engine)
    : REngineProvider(engine),
      IListTradeRoutes {

    public void listTradeRoutes(object oContext) {
        REngine?.listTradeRoutes(oContext);
    }
}

public class Login(REngine engine)
    : REngineProvider(engine),
      ILogin {

    public void login(
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
    ) {
        REngine?.login
        (
            oCallbacksFacade,
            sMdEnvKey,
            sMdUser,
            sMdPassword,
            sMdCnnctPt,
            sTsEnvKey,
            sTsUser,
            sTsPassword,
            sTsCnnctPt,
            sPnlCnnctPt,
            sIhEnvKey,
            sIhUser,
            sIhPassword,
            sIhCnnctPt
        );
    }
}

public class LoginRepository(REngine engine)
    : REngineProvider(engine),
      ILoginRepository {

    public void loginRepository(
        RCallbacksFacade oCallbacksFacade,
        string           sEnvKey,
        string           sUser,
        string           sPassword,
        string           sCnnctPt
    ) {
        REngine?.loginRepository
        (
            oCallbacksFacade,
            sEnvKey,
            sUser,
            sPassword,
            sCnnctPt
        );
    }
}

public class Logout(REngine engine)
    : REngineProvider(engine),
      ILogout {

    public void logout() {
        REngine?.logout();
    }
}

public class LogoutRepository(REngine engine)
    : REngineProvider(engine),
      ILogoutRepository {

    public void logoutRepository() {
        REngine?.logoutRepository();
    }
}

public class ModifyBracketTier(REngine engine)
    : REngineProvider(engine),
      IModifyBracketTier {

    public void modifyBracketTier(
        AccountInfo oAccount,
        string      sOrderNum,
        bool        bTarget,
        int         iOldTickOffset,
        int         iNewTickOffset,
        object      oContext
    ) {
        REngine?.modifyBracketTier
        (
            oAccount,
            sOrderNum,
            bTarget,
            iOldTickOffset,
            iNewTickOffset,
            oContext
        );
    }
}

public class ModifyOrder(REngine engine)
    : REngineProvider(engine),
      IModifyOrder {

    public void modifyOrder(ModifyLimitOrderParams oParamsIn) {
        REngine?.modifyOrder(oParamsIn);
    }

    public void modifyOrder(ModifyOrderParams oParams) {
        REngine?.modifyOrder(oParams);
    }

    public void modifyOrder(ModifyStopLimitOrderParams oParamsIn) {
        REngine?.modifyOrder(oParamsIn);
    }

    public void modifyOrder(ModifyStopMarketOrderParams oParamsIn) {
        REngine?.modifyOrder(oParamsIn);
    }

    public void modifyOrderList(ReadOnlyCollection<ModifyOrderParams> oList) {
        REngine?.modifyOrderList(oList);
    }
}

public class ModifyOrderRefData(REngine engine)
    : REngineProvider(engine),
      IModifyOrderRefData {

    public void modifyOrderRefData(
        AccountInfo oAccount,
        string      sOrderNum,
        string      sUserMsg,
        string      sUserTag
    ) {
        REngine?.modifyOrderRefData
        (
            oAccount,
            sOrderNum,
            sUserMsg,
            sUserTag
        );
    }
}

public class RebuildBook(REngine engine)
    : REngineProvider(engine),
      IRebuildBook {

    public void rebuildBook(
        string sExchange,
        string sSymbol,
        object oContext
    ) {
        REngine?.rebuildBook
        (
            sExchange,
            sSymbol,
            oContext
        );
    }
}

public class RebuildDboBook(REngine engine)
    : REngineProvider(engine),
      IRebuildDboBook {

    public void rebuildDboBook(
        string sExchange,
        string sSymbol,
        double dPrice,
        object oContext
    ) {
        REngine?.rebuildDboBook
        (
            sExchange,
            sSymbol,
            dPrice,
            oContext
        );
    }
}

public class ReplayAllOrders(REngine engine)
    : REngineProvider(engine),
      IReplayAllOrders {

    public void replayAllOrders(
        AccountInfo oAccount,
        int         iStartSsboe,
        int         iEndSsboe,
        object      oContext
    ) {
        REngine?.replayAllOrders
        (
            oAccount,
            iStartSsboe,
            iEndSsboe,
            oContext
        );
    }
}

public class ReplayBars(REngine engine)
    : REngineProvider(engine),
      IReplayBars {

    public void replayBars(ReplayBarParams oParams) {
        REngine?.replayBars(oParams);
    }
}

public class ReplayBrackets(REngine engine)
    : REngineProvider(engine),
      IReplayBrackets {

    public void replayBrackets(
        AccountInfo oAccount,
        object      oContext
    ) {
        REngine?.replayBrackets
        (
            oAccount,
            oContext
        );
    }
}

public class ReplayExecutions(REngine engine)
    : REngineProvider(engine),
      IReplayExecutions {

    public void replayExecutions(
        AccountInfo oAccount,
        int         iStartSsboe,
        int         iEndSsboe,
        object      oContext
    ) {
        REngine?.replayExecutions
        (
            oAccount,
            iStartSsboe,
            iEndSsboe,
            oContext
        );
    }
}

public class ReplayHistoricalOrders(REngine engine)
    : REngineProvider(engine),
      IReplayHistoricalOrders {

    public void replayHistoricalOrders(
        AccountInfo oAccount,
        string      sDate,
        object      oContext
    ) {
        REngine?.replayHistoricalOrders
        (
            oAccount,
            sDate,
            oContext
        );
    }
}

public class ReplayOpenOrders(REngine engine)
    : REngineProvider(engine),
      IReplayOpenOrders {

    public void replayOpenOrders(
        AccountInfo oAccount,
        object      oContext
    ) {
        REngine?.replayOpenOrders
        (
            oAccount,
            oContext
        );
    }
}

public class ReplayPnl(REngine engine)
    : REngineProvider(engine),
      IReplayPnl {

    public void replayPnl(
        AccountInfo oAccount,
        object      oContext
    ) {
        REngine?.replayPnl
        (
            oAccount,
            oContext
        );
    }
}

public class ReplayQuotes(REngine engine)
    : REngineProvider(engine),
      IReplayQuotes {

    public void replayQuotes(
        AccountInfo oAccount,
        object      oContext
    ) {
        REngine?.replayQuotes
        (
            oAccount,
            oContext
        );
    }
}

public class ReplaySingleHistoricalOrder(REngine engine)
    : REngineProvider(engine),
      IReplaySingleHistoricalOrder {

    public void replaySingleHistoricalOrder(
        AccountInfo oAccount,
        string      sOrderNum,
        string      sDate,
        object      oContext
    ) {
        REngine?.replaySingleHistoricalOrder
        (
            oAccount,
            sOrderNum,
            sDate,
            oContext
        );
    }
}

public class ReplaySingleOrder(REngine engine)
    : REngineProvider(engine),
      IReplaySingleOrder {

    public void replaySingleOrder(
        AccountInfo oAccount,
        string      sOrderNum,
        object      oContext
    ) {
        REngine?.replaySingleOrder
        (
            oAccount,
            sOrderNum,
            oContext
        );
    }
}

public class ReplayTrades(REngine engine)
    : REngineProvider(engine),
      IReplayTrades {

    public void replayTrades(
        string sExchange,
        string sSymbol,
        int    iStartSsboe,
        int    iEndSsboe,
        object oContext
    ) {
        REngine?.replayTrades
        (
            sExchange,
            sSymbol,
            iStartSsboe,
            iEndSsboe,
            oContext
        );
    }
}

public class SearchInstrument(REngine engine)
    : REngineProvider(engine),
      ISearchInstrument {

    public void searchInstrument(
        string                         sExchange,
        ReadOnlyCollection<SearchTerm> oTerms,
        object                         oContext
    ) {
        REngine?.searchInstrument
        (
            sExchange,
            oTerms,
            oContext
        );
    }
}

public class SendBracketOrder(REngine engine)
    : REngineProvider(engine),
      ISendBracketOrder {

    public void sendBracketOrder(
        OrderParams   oEntry,
        BracketParams oBracketParams
    ) {
        REngine?.sendBracketOrder
        (
            oEntry,
            oBracketParams
        );
    }

    public void sendBracketOrder(
        OrderParams        oEntry,
        string             sBracketType,
        string             sOrderOperationType,
        bool               bTrailingStop,
        int                iTrailByPriceId,
        IList<BracketTier> oTargets,
        IList<BracketTier> oStops
    ) {
        REngine?.sendBracketOrder
        (
            oEntry,
            sBracketType,
            sOrderOperationType,
            bTrailingStop,
            iTrailByPriceId,
            oTargets,
            oStops
        );
    }
}

public class SendOcaList(REngine engine)
    : REngineProvider(engine),
      ISendOcaList {

    public void sendOcaList(
        string                          sOcaType,
        ReadOnlyCollection<OrderParams> oList
    ) {
        REngine?.sendOcaList
        (
            sOcaType,
            oList
        );
    }
}

public class SendOrder(REngine engine)
    : REngineProvider(engine),
      ISendOrder {

    public void sendOrder(LimitOrderParams oParamsIn) {
        REngine?.sendOrder(oParamsIn);
    }

    public void sendOrder(MarketOrderParams oParamsIn) {
        REngine?.sendOrder(oParamsIn);
    }

    public void sendOrder(StopLimitOrderParams oParamsIn) {
        REngine?.sendOrder(oParamsIn);
    }

    public void sendOrder(StopMarketOrderParams oParamsIn) {
        REngine?.sendOrder(oParamsIn);
    }

    public void sendOrderList(ReadOnlyCollection<OrderParams> oList) {
        REngine?.sendOrderList(oList);
    }
}

public class SetEnvironmentVariable(REngine engine)
    : REngineProvider(engine),
      ISetEnvironmentVariable {

    public void setEnvironmentVariable(
        string sKey,
        string sVariable,
        string sValue
    ) {
        REngine?.setEnvironmentVariable
        (
            sKey,
            sVariable,
            sValue
        );
    }
}

public class SetOrderContext(REngine engine)
    : REngineProvider(engine),
      ISetOrderContext {

    public void setOrderContext(
        string sOrderNum,
        object oContext
    ) {
        REngine?.setOrderContext
        (
            sOrderNum,
            oContext
        );
    }
}

public class Shutdown(REngine engine)
    : REngineProvider(engine),
      IShutdown {

    public void shutdown() {
        REngine?.shutdown();
    }
}

public class SubmitQuoteList(REngine engine)
    : REngineProvider(engine),
      ISubmitQuoteList {

    public void submitQuoteList(ReadOnlyCollection<QuoteParams> oList) {
        REngine?.submitQuoteList(oList);
    }
}

public class Subscribe(REngine engine)
    : REngineProvider(engine),
      ISubscribe {

    public void subscribe(
        string            sExchange,
        string            sSymbol,
        SubscriptionFlags eFlags,
        object            oContext
    ) {
        REngine?.subscribe
        (
            sExchange,
            sSymbol,
            eFlags,
            oContext
        );
    }
}

public class SubscribeAutoLiquidate(REngine engine)
    : REngineProvider(engine),
      ISubscribeAutoLiquidate {

    public void subscribeAutoLiquidate(AccountInfo oAccount) {
        REngine?.subscribeAutoLiquidate(oAccount);
    }
}

public class SubscribeBar(REngine engine)
    : REngineProvider(engine),
      ISubscribeBar {

    public void subscribeBar(BarParams oParams) {
        REngine?.subscribeBar(oParams);
    }
}

public class SubscribeBracket(REngine engine)
    : REngineProvider(engine),
      ISubscribeBracket {

    public void subscribeBracket(AccountInfo oAccount) {
        REngine?.subscribeBracket(oAccount);
    }
}

public class SubscribeByUnderlying(REngine engine)
    : REngineProvider(engine),
      ISubscribeByUnderlying {

    public void subscribeByUnderlying(
        string            sUnderlying,
        string            sExchange,
        string            sExpiration,
        SubscriptionFlags eFlags,
        object            oContext
    ) {
        REngine?.subscribeByUnderlying
        (
            sUnderlying,
            sExchange,
            sExpiration,
            eFlags,
            oContext
        );
    }
}

public class SubscribeDbo(REngine engine)
    : REngineProvider(engine),
      ISubscribeDbo {

    public void subscribeDbo(
        string sExchange,
        string sSymbol,
        double dPrice,
        object oContext
    ) {
        REngine?.subscribeDbo
        (
            sExchange,
            sSymbol,
            dPrice,
            oContext
        );
    }
}

public class SubscribeEasyToBorrow(REngine engine)
    : REngineProvider(engine),
      ISubscribeEasyToBorrow {

    public void subscribeEasyToBorrow(object oContext) {
        REngine?.subscribeEasyToBorrow(oContext);
    }
}

public class SubscribeOrder(REngine engine)
    : REngineProvider(engine),
      ISubscribeOrder {

    public void subscribeOrder(AccountInfo oAccount) {
        REngine?.subscribeOrder(oAccount);
    }
}

public class SubscribePnl(REngine engine)
    : REngineProvider(engine),
      ISubscribePnl {

    public void subscribePnl(AccountInfo oAccount) {
        REngine?.subscribePnl(oAccount);
    }
}

public class SubscribeTradeRoute(REngine engine)
    : REngineProvider(engine),
      ISubscribeTradeRoute {

    public void subscribeTradeRoute(
        string sFcmId,
        string sIbId,
        object oContext
    ) {
        REngine?.subscribeTradeRoute
        (
            sFcmId,
            sIbId,
            oContext
        );
    }
}

public class UnsetEnvironmentVariable(REngine engine)
    : REngineProvider(engine),
      IUnsetEnvironmentVariable {

    public void unsetEnvironmentVariable(
        string sKey,
        string sVariable
    ) {
        REngine?.unsetEnvironmentVariable
        (
            sKey,
            sVariable
        );
    }
}

public class Unsubscribe(REngine engine)
    : REngineProvider(engine),
      IUnsubscribe {

    public void unsubscribe(
        string sExchange,
        string sSymbol
    ) {
        REngine?.unsubscribe
        (
            sExchange,
            sSymbol
        );
    }
}

public class UnsubscribeAutoLiquidate(REngine engine)
    : REngineProvider(engine),
      IUnsubscribeAutoLiquidate {

    public void unsubscribeAutoLiquidate(AccountInfo oAccount) {
        REngine?.unsubscribeAutoLiquidate(oAccount);
    }
}

public class UnsubscribeBar(REngine engine)
    : REngineProvider(engine),
      IUnsubscribeBar {

    public void unsubscribeBar(BarParams oParams) {
        REngine?.unsubscribeBar(oParams);
    }
}

public class UnsubscribeBracket(REngine engine)
    : REngineProvider(engine),
      IUnsubscribeBracket {

    public void unsubscribeBracket(AccountInfo oAccount) {
        REngine?.unsubscribeBracket(oAccount);
    }
}

public class UnsubscribeByUnderlying(REngine engine)
    : REngineProvider(engine),
      IUnsubscribeByUnderlying {

    public void unsubscribeByUnderlying(
        string sUnderlying,
        string sExchange,
        string sExpiration
    ) {
        REngine?.unsubscribeByUnderlying
        (
            sUnderlying,
            sExchange,
            sExpiration
        );
    }
}

public class UnsubscribeDbo(REngine engine)
    : REngineProvider(engine),
      IUnsubscribeDbo {

    public void unsubscribeDbo(
        string sExchange,
        string sSymbol,
        double dPrice,
        object oContext
    ) {
        REngine?.unsubscribeDbo
        (
            sExchange,
            sSymbol,
            dPrice,
            oContext
        );
    }
}

public class UnsubscribeEasyToBorrow(REngine engine)
    : REngineProvider(engine),
      IUnsubscribeEasyToBorrow {

    public void unsubscribeEasyToBorrow() {
        REngine?.unsubscribeEasyToBorrow();
    }
}

public class UnsubscribeOrder(REngine engine)
    : REngineProvider(engine),
      IUnsubscribeOrder {

    public void unsubscribeOrder(AccountInfo oAccount) {
        REngine?.unsubscribeOrder(oAccount);
    }
}

public class UnsubscribePnl(REngine engine)
    : REngineProvider(engine),
      IUnsubscribePnl {

    public void unsubscribePnl(AccountInfo oAccount) {
        REngine?.unsubscribePnl(oAccount);
    }
}

public class UnsubscribeTradeRoute(REngine engine)
    : REngineProvider(engine),
      IUnsubscribeTradeRoute {

    public void unsubscribeTradeRoute(
        string sFcmId,
        string sIbId
    ) {
        REngine?.unsubscribeTradeRoute
        (
            sFcmId,
            sIbId
        );
    }
}