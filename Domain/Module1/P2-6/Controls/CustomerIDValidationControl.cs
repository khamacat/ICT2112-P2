using ProRental.Domain.Entities;
using ProRental.Interfaces.Domain;

namespace ProRental.Domain.Controls;

/// <summary>
/// <<Control>> CustomerIDValidationControl
/// Handles customer ID validation logic for guest/unregistered checkout flows.
/// Corresponds to the CustomerIDValidationControl class in the Module 1 P2-6 class diagram.
/// </summary>
public class CustomerIDValidationControl
{
    private readonly ICustomerValidationService _customerValidationService;

    public CustomerIDValidationControl(ICustomerValidationService customerValidationService)
    {
        _customerValidationService = customerValidationService;
    }

    /// <summary>
    /// Delegates to ICustomerValidationService to validate the customer ID.
    /// </summary>
    /// <param name="customerId">The customer ID entered by the user.</param>
    /// <returns>A CustomerValidationResult indicating valid/invalid with a message.</returns>
    public CustomerValidationResult ValidateCustomer(int customerId)
    {
        return _customerValidationService.ValidateCustomer(customerId);
    }

    /// <summary>
    /// Determines whether validation is required for the given customer type.
    /// Guests always require validation; registered customers bypass this step.
    /// </summary>
    /// <param name="customerType">A string representing the customer type (e.g. "guest", "registered").</param>
    /// <returns>True if validation must be performed before proceeding.</returns>
    public bool IsValidationRequired(string customerType)
    {
        // Only guest/unregistered flows require the customer ID entry step.
        return string.Equals(customerType, "guest", StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Interprets a CustomerValidationResult and returns the appropriate outcome string
    /// for use by the view layer ("proceed", "retry", or "blocked").
    /// </summary>
    /// <param name="result">The validation result from ValidateCustomer.</param>
    /// <returns>
    ///   "proceed"  — validation passed, allow the user to continue.
    ///   "retry"    — validation failed but the user may try again.
    ///   "blocked"  — validation failed in a non-recoverable way.
    /// </returns>
    public string HandleValidationOutcome(CustomerValidationResult result)
    {
        if (result.IsValid)
            return "proceed";

        // Distinguish retryable failures from hard blocks.
        // A null/empty message means an unexpected system error — block entirely.
        if (string.IsNullOrWhiteSpace(result.ValidationMessage))
            return "blocked";

        return "retry";
    }
}