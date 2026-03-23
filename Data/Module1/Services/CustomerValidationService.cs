using Microsoft.EntityFrameworkCore;
using ProRental.Data.UnitOfWork;
using ProRental.Domain.Entities;
using ProRental.Interfaces.Domain;

namespace ProRental.Data.Services;

/// <summary>
/// Concrete implementation of ICustomerValidationService.
/// Uses raw SQL to bypass Customer.Customerid which is a private property
/// that cannot be accessed directly (entity file cannot be modified).
/// </summary>
public class CustomerValidationService : ICustomerValidationService
{
    private readonly AppDbContext _db;

    public CustomerValidationService(AppDbContext db)
    {
        _db = db;
    }

    public CustomerValidationResult ValidateCustomer(int customerId)
    {
        var customers = _db.Customers
            .AsEnumerable()
            .Where(c => GetPrivateProperty<int>(c, "Customerid") == customerId)
            .ToList();

        if (customers.Count == 0)
            return CustomerValidationResult.Invalid(customerId, "Customer ID not found.");

        return CustomerValidationResult.Valid(customerId);
    }

    private static T? GetPrivateProperty<T>(object obj, string propertyName)
    {
        var prop = obj.GetType().GetProperty(
            propertyName,
            System.Reflection.BindingFlags.NonPublic |
            System.Reflection.BindingFlags.Instance);
        return prop == null ? default : (T?)prop.GetValue(obj);
    }
}