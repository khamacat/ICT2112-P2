using ProRental.Domain.Entities;

namespace ProRental.Interfaces.Domain;

public interface iReturnOrderCRUD
{
    bool CreateReturnRequest(Returnrequest returnRequest);
}