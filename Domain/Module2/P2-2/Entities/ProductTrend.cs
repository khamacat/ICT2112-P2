namespace ProRental.Domain.Entities;
using ProRental.Domain.Enums;
public partial class Analytic  // NOT a separate class. Variant of Analytic
{
    // Supplier-specific helpers — reads from RefPrimaryID/Name + RefValue
    // which the Analytics table already has
    public string? GetProductName()  => Refprimaryname;
    public int? GetProductID()       => Refprimaryid;
    public float? GetTurnoverRate()  => (float?)Refvalue;
}