using ProRental.Domain.Entities;

namespace ProRental.Interfaces.Domain;

public interface ICustomerValidationService
{
    /// <summary>
    /// Validates whether the given customerId exists and is in good standing.
    /// </summary>
    CustomerValidationResult ValidateCustomer(int customerId);
}