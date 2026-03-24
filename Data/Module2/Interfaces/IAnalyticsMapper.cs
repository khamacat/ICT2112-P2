using ProRental.Domain.Entities;

namespace ProRental.Interfaces;

public interface IAnalyticsMapper
{
    Task<bool> InsertAsync(Analytic analytics);
    Task<Analytic?> FindByIDAsync(int id);
    Task<IEnumerable<Analytic>> FindAllAsync();
    Task<IEnumerable<Analytic>> FindByDateAsync(DateTime start, DateTime end);
    Task<IEnumerable<Analytic>> FindBySupplierAsync(int supplierID);
    Task<IEnumerable<Analytic>> FindByProductAsync(int productID);
    Task<IEnumerable<Analytic>> FindByNameAsync(string name);
    Task<bool> UpdateAsync(Analytic analytics);
    Task<bool> DeleteAsync(Analytic analytics);
}