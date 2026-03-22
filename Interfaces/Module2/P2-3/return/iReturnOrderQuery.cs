using ProRental.Domain.Entities;
using ProRental.Domain.Enums;

namespace ProRental.Interfaces.Domain;

public interface iReturnOrderQuery
{
    ICollection<Returnrequest> GetAllReturnRequests();
    Returnrequest? GetReturnRequestById(int returnRequestId);
    ReturnRequestStatus GetReturnStatus(int returnRequestId);
    Returnrequest? GetReturnRequestByOrderId(int orderId);
}