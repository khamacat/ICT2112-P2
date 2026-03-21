using Microsoft.EntityFrameworkCore;
using ProRental.Data.UnitOfWork;
using ProRental.Domain.Entities;
using ProRental.Domain.Enums;
using ProRental.Interfaces.Data;

namespace ProRental.Data;

/* * =========================================================================
 * ARCHITECTURE & AI ASSISTANT RULES FOR THIS FILE
 * =========================================================================
 * 1. STRICT ENCAPSULATION: All entity properties (Productid, Sku, etc.) are PRIVATE. 
 * Do NOT attempt to use standard LINQ (e.g., p.Productid == id). 
 * You MUST use EF.Property<T>(entity, "PropertyName") for all queries.
 * 2. COMPOSITE INCLUDES ONLY: You MAY use .Include(p => p.Productdetail) because 
 * Productdetail is a strict composite of Product. Do NOT include other aggregates (Alerts, Inventoryitems).
 * 3. UTC TIMESTAMPS: Always override the "Updatedat" to DateTime.UtcNow 
 * using _context.Entry() during updates to maintain the TIMESTAMPTZ standard.
 * =========================================================================
 */

public class ProductMapper : IProductMapper
{
    private readonly AppDbContext _context;

    public ProductMapper(AppDbContext context)
    {
        _context = context;
    }

    public Product? FindById(int productId)
    {
        // Using EF.Property to query against the private 'Productid'
        // .Include() ensures the composite Productdetail is fetched simultaneously
        return _context.Products
            .Include(p => p.Productdetail)
            .FirstOrDefault(p => EF.Property<int>(p, "Productid") == productId);
    }

    public ICollection<Product>? FindAll()
    {
        return _context.Products
            .Include(p => p.Productdetail)
            .ToList();
    }

    public ICollection<Product>? FindByCategoryId(int categoryId)
    {
        return _context.Products
            .Include(p => p.Productdetail)
            .Where(p => EF.Property<int>(p, "Categoryid") == categoryId)
            .ToList();
    }

    public ICollection<Product>? FindByStatus(ProductStatus status)
    {
        // Using EF.Property to query against the private 'Status' from your manual partial class
        return _context.Products
            .Include(p => p.Productdetail)
            .Where(p => EF.Property<ProductStatus>(p, "Status") == status)
            .ToList();
    }

    public void Insert(Product product)
    {
        // Because Productdetail is a navigation property, EF Core will automatically 
        // insert the Productdetail record if it is attached to this Product.
        _context.Products.Add(product);
        _context.SaveChanges();
    }

    public void Update(Product product)
    {
        // Automatically enforce the TIMESTAMPTZ standard for updates
        // We use Entry() to update the private Updatedat property safely
        _context.Entry(product).Property("Updatedat").CurrentValue = DateTime.UtcNow;

        _context.Products.Update(product);
        _context.SaveChanges();
    }

    public void Delete(Product product)
    {
        // Because of the strong composite relationship, if Cascade delete is set up,
        // this will also remove the Productdetail. If Restrict is set, it will throw an error
        // unless Productdetail is deleted first. Update(product) is generally preferred 
        // to change the Status to 'RETIRED' rather than hard deleting.
        _context.Products.Remove(product);
        _context.SaveChanges();
    }
}