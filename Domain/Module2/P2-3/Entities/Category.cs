using System;

namespace ProRental.Domain.Entities;

// Ensure this namespace exactly matches the one in your base Entity file
public partial class Category
{
    public int GetCategoryId() => _categoryid;

    public string GetName() => _name;
    public void SetName(string value) => _name = value;

    // This is the missing piece causing your error
    public string? GetDescription() => _description;
    
    public void SetDescription(string? value) 
    {
        _description = value;
    }
}