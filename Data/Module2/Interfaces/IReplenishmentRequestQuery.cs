using ProRental.Domain.Entities;

namespace ProRental.Data.Interfaces
{
    public interface IReplenishmentRequestQuery
    {
        Replenishmentrequest? GetRequest(int reqId);
        List<Lineitem> GetLineItems(int reqId);
    }
}