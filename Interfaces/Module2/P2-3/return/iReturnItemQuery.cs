using ProRental.Domain.Entities;

namespace ProRental.Interfaces.Domain;

public interface iReturnItemQuery
{
    Returnitem? GetReturnItem(int returnItemId);
    List<Returnitem> GetReturnItemByRequestId(int returnRequestId);
}