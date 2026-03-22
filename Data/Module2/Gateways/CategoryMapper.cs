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
 * 4. DISCONNECTED UPDATES: Always use CurrentValues.SetValues() to update entities 
 * to avoid tracking conflicts without writing manual property-by-property mapping.
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
        // RULE: Use EF.Property for audit timestamps if they are private
        _context.Categories.Add(category);
        _context.SaveChanges();
    }

    public void Update(Category category)
    {
        // RULE: Access private ID via EF.Property for the lookup
        var existing = _context.Categories
            .FirstOrDefault(c => EF.Property<int>(c, "Categoryid") == category.GetCategoryId());

        if (existing == null) return;

        // RULE: Disconnected update using SetValues
        _context.Entry(existing).CurrentValues.SetValues(category);
        
        // RULE: Manually override the UTC timestamp for the shadow property
        _context.Entry(existing).Property("Updateddate").CurrentValue = DateTime.UtcNow;

        _context.SaveChanges();
    }

    public void Delete(Category category)
    {
        // RULE: Find the tracked entity first before removing
        var existing = _context.Categories
            .FirstOrDefault(c => EF.Property<int>(c, "Categoryid") == category.GetCategoryId());
            
        if (existing != null)
        {
            _context.Categories.Remove(existing);
            _context.SaveChanges();
        }
    }
}