using System;

namespace ProRental.Domain.Entities;

public partial class Category
{
    private bool _isActiive;

    // Existing method
    public int GetCategoryId() => _categoryid;

    // Added to handle Category Name
    public string GetName() => _name;

    public void SetName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Category name cannot be empty.");
        }
        _name = value;
    }

    // Added to handle Category Status (Logic based on project requirements)
    // Since _isActiive was mentioned in your diagram, we map it here
    public bool GetIsActive() => _isActiive;

    public void SetIsActive(bool value)
    {
        _isActiive = value;
    }
}