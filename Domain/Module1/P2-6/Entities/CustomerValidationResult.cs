namespace ProRental.Domain.Entities;

/// <summary>
/// Returned by ICustomerValidationService.ValidateCustomer().
/// Carries whether the customer ID is valid and, if not, a human-readable reason.
/// </summary>
public class CustomerValidationResult
{
    public bool IsValid { get; private set; }
    public string? ValidationMessage { get; private set; }
    public int CustomerId { get; private set; }

    private CustomerValidationResult() { }

    public static CustomerValidationResult Valid(int customerId)
    {
        return new CustomerValidationResult
        {
            IsValid = true,
            CustomerId = customerId
        };
    }

    public static CustomerValidationResult Invalid(int customerId, string message)
    {
        return new CustomerValidationResult
        {
            IsValid = false,
            CustomerId = customerId,
            ValidationMessage = message
        };
    }
}