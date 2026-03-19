namespace ProRental.Domain.Entities;
using ProRental.Domain.Enums;
public partial class Supplier
{
    private SupplierCategory _category;
    private SupplierCategory category { get => _category; set => _category = value; }
    public void UpdateCategory(SupplierCategory newValue) => _category = newValue;

    private VettingDecision _decision;
    private VettingDecision decision { get => _decision; set => _decision = value; }
    public void UpdateDecision(VettingDecision newValue) => _decision = newValue;
}