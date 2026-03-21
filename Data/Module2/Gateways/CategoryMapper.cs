using Microsoft.EntityFrameworkCore;
using ProRental.Data.UnitOfWork;
using ProRental.Domain.Entities;
using ProRental.Interfaces.Data;

namespace ProRental.Data;

/* * =========================================================================
 * ARCHITECTURE & AI ASSISTANT RULES FOR THIS FILE
 * =========================================================================
 * 1. STRICT ENCAPSULATION: All entity properties (Categoryid, Name, etc.) are PRIVATE. 
 * Do NOT attempt to use standard LINQ (e.g., c.Categoryid == id). 
 * You MUST use EF.Property<T>(entity, "PropertyName") for all queries.
 * 2. NO CROSS-AGGREGATE INCLUDES: Do NOT use .Include(c => c.Products). 
 * If a developer needs Products, they must use the IProductMapper.
 * 3. UTC TIMESTAMPS: Always override the "Updateddate" to DateTime.UtcNow 
 * using _context.Entry() during updates to maintain the TIMESTAMPTZ standard.
 * =========================================================================
 */

public class CategoryMapper : ICategoryMapper
{
    private readonly AppDbContext _context;

    public CategoryMapper(AppDbContext context)
    {
        _context = context;
    }

    public Category? FindById(int categoryId)
    {
        // RULE: Use EF.Property to access the private 'Categoryid'.
        // RULE: Do NOT add .Include(c => c.Products). Respect aggregate boundaries.
        return _context.Categories
            .FirstOrDefault(c => EF.Property<int>(c, "Categoryid") == categoryId);
    }

    public ICollection<Category>? FindAll()
    {
        // RULE: Return only the Category aggregate. No .Include() allowed.
        return _context.Categories.ToList();
    }

    public void Insert(Category category)
    {
        _context.Categories.Add(category);
        _context.SaveChanges();
    }

    public void Update(Category category)
    {
        // RULE: Automatically enforce the TIMESTAMPTZ standard for updates.
        // We use Entry() to update the private Updateddate property safely without breaking encapsulation.
        _context.Entry(category).Property("Updateddate").CurrentValue = DateTime.UtcNow;

        _context.Categories.Update(category);
        _context.SaveChanges();
    }

    public void Delete(Category category)
    {
        // Note: If this category has associated Products, Entity Framework will throw a 
        // referential integrity constraint error due to the Restrict deletion behavior 
        // configured in the DbContext. The products must be reassigned or deleted first.
        _context.Categories.Remove(category);
        _context.SaveChanges();
    }
}