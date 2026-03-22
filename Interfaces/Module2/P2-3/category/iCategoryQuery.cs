using System.Collections.Generic;
using ProRental.Domain.Entities;

namespace ProRental.Interfaces.Domain;
public interface ICategoryQuery
{
        Category GetCategoryById(int categoryId);
    List<Category> GetAllCategories();
    List<Category> GetActiveCategories();
}