namespace ProRental.Domain.Entities;
using ProRental.Domain.Enums;
public partial class Suppliercategorychangelog
{
    private SupplierCategory _category;
    private SupplierCategory category { get => _category; set => _category = value; }
    public void UpdateCategory(SupplierCategory newValue) => _category = newValue;
}