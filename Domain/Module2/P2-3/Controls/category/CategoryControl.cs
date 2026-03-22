using ProRental.Domain.Entities;
using ProRental.Interfaces.Domain;

namespace ProRental.Domain.Module2.P2_3.Controls.category
{
    public class CategoryControl : ICategoryCRUD, ICategoryQuery
    {
        private static List<Category> _categoryList = new List<Category>();

        // Query Implementation
        public List<Category> GetAllCategories() => _categoryList;
        
        // Use GetCategoryId() instead of .CategoryId
        public Category GetCategoryById(int id) => 
            _categoryList.FirstOrDefault(c => c.GetCategoryId() == id);
        
        // Use GetIsActive() instead of .IsActive
        public List<Category> GetActiveCategories() => 
            _categoryList.Where(c => c.GetIsActive()).ToList();

        // CRUD Implementation
        public bool CreateCategory(Category category)
        {
            if (ValidateCategory(category) && !CheckCategoryConflicts(category))
            {
                _categoryList.Add(category);
                return true;
            }
            return false;
        }

        public bool UpdateCategory(Category category)
        {
            var existing = GetCategoryById(category.GetCategoryId());
            if (existing == null) return false;
            
            // Use SetName() and GetName()
            existing.SetName(category.GetName());
            return true;
        }

        public bool DeleteCategory(int id) => 
            _categoryList.RemoveAll(c => c.GetCategoryId() == id) > 0;
        
        public bool ValidateCategory(Category c) => 
            !string.IsNullOrEmpty(c.GetName());
        
        public bool CheckCategoryConflicts(Category c) => 
            _categoryList.Any(ex => ex.GetName().Equals(c.GetName(), StringComparison.OrdinalIgnoreCase));
        
        public bool UpdateCategoryStatus(int id)
        {
            var category = GetCategoryById(id);
            if (category != null)
            {
                // Use SetIsActive() and GetIsActive()
                category.SetIsActive(!category.GetIsActive());
                return true;
            }
            return false;
        }
    }
}