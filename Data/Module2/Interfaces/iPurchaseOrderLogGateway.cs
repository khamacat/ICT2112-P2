namespace ProRental.Data.Module2.Interfaces;

using ProRental.Domain.Entities;

/// <summary>
/// Gateway interface for the PurchaseOrderLog child table.
/// Each row's PK (PurchaseOrderLogId) must match an existing TransactionLogID.
/// </summary>
public interface IPurchaseOrderLogGateway
{
    Purchaseorderlog Insert(Purchaseorderlog log);
    List<Purchaseorderlog> GetAll();
    Purchaseorderlog? GetById(int purchaseOrderLogId);
    bool ExistsByPoId(int poId);
}