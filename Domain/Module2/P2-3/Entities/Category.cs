using System;

namespace ProRental.Domain.Entities;

// Ensure this namespace exactly matches the one in your base Entity file
public partial class Category
{
    internal void SetCategoryId(int id) => _categoryid = id; // Internal so only DB/Mappers can set ID
    public int GetCategoryId() => _categoryid;

    public string GetName() => _name;
    public void SetName(string value) => _name = value;
    public void UpdateName(string name) => _name = name;     // Explicit business action

    // This is the missing piece causing your error
    public string? GetDescription() => _description;
    
    public void SetDescription(string? value) 
    {
        _description = value;
    }
    public void UpdateDescription(string? description) => _description = description; // Explicit business action
}