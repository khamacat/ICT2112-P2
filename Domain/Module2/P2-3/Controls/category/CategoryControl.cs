using ProRental.Domain.Entities;
using ProRental.Interfaces.Domain;
using ProRental.Interfaces.Data;

namespace ProRental.Domain.Controls;

public class CategoryControl : ICategoryCRUD, ICategoryQuery
{
    private readonly ICategoryMapper _categoryMapper;

    public CategoryControl(ICategoryMapper categoryMapper)
    {
        _categoryMapper = categoryMapper;
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

    public bool DeleteCategory(int id) 
    {
        var existing = GetCategoryById(id);
        if (existing != null)
        {
            _categoryMapper.Delete(existing);
            return true;
        }
        return false;
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