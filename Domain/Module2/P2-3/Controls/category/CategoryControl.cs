using ProRental.Domain.Entities;
using ProRental.Interfaces.Domain;
using ProRental.Interfaces.Data;

namespace ProRental.Domain.Controls;

public class CategoryControl : ICategoryCRUD, ICategoryQuery
{
    private readonly ICategoryMapper _categoryMapper;
    private readonly IProductRead _productReader;

    public CategoryControl(ICategoryMapper categoryMapper, IProductRead productReader)
    {
        _categoryMapper = categoryMapper;
        _productReader = productReader;
    }

    // --- Query Implementation ---
    public List<Category> GetAllCategories() => 
        _categoryMapper.FindAll()?.ToList() ?? new List<Category>();
    
    public Category? GetCategoryById(int id) => _categoryMapper.FindById(id);

    // --- CRUD Implementation ---
    public bool CreateCategory(Category category)
    {
        if (ValidateCategory(category) && !CheckCategoryConflicts(category))
        {
            _categoryMapper.Insert(category);
            return true;
        }
        return false;
    }

    public bool UpdateCategory(Category category)
    {
        var existing = GetCategoryById(category.GetCategoryId());
        if (existing == null) return false;
        
        existing.SetName(category.GetName());
        existing.SetDescription(category.GetDescription());
        
        _categoryMapper.Update(existing);
        return true;
    }

    public bool DeleteCategory(int id, out string categoryName) 
{
    categoryName = string.Empty;
    
    // Fetch the category to retrieve its name before checking constraints
    var existing = GetCategoryById(id);
    if (existing == null) return false;

    categoryName = existing.GetName(); // Store the chosen category name

    // Check if any products are linked to this category ID
    var products = _productReader.FindAll();
    bool hasLinkedProducts = products != null && products.Any(p => p.GetCategoryId() == id);

    if (hasLinkedProducts)
    {
        // Return false to block deletion, but categoryName is now available to the Controller
        return false; 
    }

    // Proceed with deletion as no products are linked
    _categoryMapper.Delete(existing);
    return true;
}
    
    public bool ValidateCategory(Category c) => !string.IsNullOrEmpty(c.GetName());
    
    public bool CheckCategoryConflicts(Category c) 
    {
        var existing = _categoryMapper.FindAll();
        return existing != null && existing.Any(ex => 
            ex.GetName().Equals(c.GetName(), StringComparison.OrdinalIgnoreCase));
    }

    public bool UpdateCategoryStatus(int categoryId)
    {
        throw new NotImplementedException();
    }

    // UpdateCategoryStatus removed as requested
}