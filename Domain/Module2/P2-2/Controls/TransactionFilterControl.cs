namespace ProRental.Domain.Module2.P2_2.Controls;

using Microsoft.EntityFrameworkCore;
using ProRental.Domain.Entities;
using ProRental.Interfaces;
using ProRental.Data.Module2.Interfaces;
using ProRental.Domain.Module2.P2_2.Strategy;

public class TransactionFilterControl : ITransactionLoggingUI, ITransactionLogService
{
    private readonly ITransactionLogGateway _transactionLogGateway;
    private readonly IRentalOrderLogGateway _rentalOrderLogGateway;
    private readonly ILoanLogGateway _loanLogGateway;
    private readonly IReturnLogGateway _returnLogGateway;
    private readonly IClearanceLogGateway _clearanceLogGateway;
    private readonly IPurchaseOrderLogGateway _purchaseOrderLogGateway;
    private readonly TransactionLogControl _transactionLogControl;

    public TransactionFilterControl(
        ITransactionLogGateway transactionLogGateway,
        IRentalOrderLogGateway rentalOrderLogGateway,
        ILoanLogGateway loanLogGateway,
        IReturnLogGateway returnLogGateway,
        IClearanceLogGateway clearanceLogGateway,
        IPurchaseOrderLogGateway purchaseOrderLogGateway,
        TransactionLogControl transactionLogControl)
    {
        _transactionLogGateway = transactionLogGateway;
        _rentalOrderLogGateway = rentalOrderLogGateway;
        _loanLogGateway = loanLogGateway;
        _returnLogGateway = returnLogGateway;
        _clearanceLogGateway = clearanceLogGateway;
        _purchaseOrderLogGateway = purchaseOrderLogGateway;
        _transactionLogControl = transactionLogControl;
    }

    public List<Transactionlog> GetAllLogs()
    {
        _transactionLogControl.PullAndLogPurchaseOrders();
        return LoadAllLogsWithChildren();
    }

    public List<Transactionlog> GetFilteredLogs(string filterType, string filterValue)
    {
        _transactionLogControl.PullAndLogPurchaseOrders();

        IFilterStrategy strategy = CreateStrategy(filterType, filterValue);

        if (!strategy.validate())
            return LoadAllLogsWithChildren();

        var allLogs = LoadAllLogsWithChildren();
        return strategy.filter(allLogs);
    }

    public Transactionlog? GetLogDetails(int transactionLogId)
    {
        var log = _transactionLogGateway.GetById(transactionLogId);
        if (log == null) return null;
        LoadChildLog(log);
        return log;
    }

    private IFilterStrategy CreateStrategy(string filterType, string filterValue)
    {
        return filterType.ToLower() switch
        {
            "customer" => new FilterByCustomerId(filterValue),
            "supplier" => new FilterBySupplierId(filterValue),
            "order" => new FilterByOrderId(filterValue),
            "daterange" => new FilterByDateRange(filterValue),
            _ => new FilterByDateRange(DateTime.MinValue, DateTime.MaxValue)
        };
    }

    private List<Transactionlog> LoadAllLogsWithChildren()
    {
        var logs = _transactionLogGateway.GetAll();
        foreach (var log in logs)
            LoadChildLog(log);
        return logs;
    }

    private void LoadChildLog(Transactionlog log)
    {
        if (log.Rentalorderlog == null && log.Loanlog == null && log.Returnlog == null
            && log.Clearancelog == null && log.Purchaseorderlog == null)
        {
            _rentalOrderLogGateway.GetById(log.transaction_logid);
            _loanLogGateway.GetById(log.transaction_logid);
            _returnLogGateway.GetById(log.transaction_logid);
            _clearanceLogGateway.GetById(log.transaction_logid);
            _purchaseOrderLogGateway.GetById(log.transaction_logid);
        }
    }
}