using ProRental.Domain.Entities;

namespace ProRental.Interfaces.Domain;

public interface ICategoryCRUD
{
    bool CreateCategory(Category category);
    bool UpdateCategory(Category category);
    bool DeleteCategory(int categoryId);
    bool ValidateCategory(Category category);
    bool CheckCategoryConflicts(Category category);
    bool UpdateCategoryStatus(int categoryId);
}
