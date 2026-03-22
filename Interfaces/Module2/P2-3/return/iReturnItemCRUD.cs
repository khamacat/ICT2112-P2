using ProRental.Domain.Entities;

namespace ProRental.Interfaces.Domain;

public interface iReturnItemCRUD
{
    bool CreateReturnItem(Returnitem returnItem);
    bool UpdateReturnItem(Returnitem returnItem);
}