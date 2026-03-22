namespace ProRental.Domain.Entities;
using ProRental.Domain.Enums;
public partial class Analytic  // NOT a separate class. Variant of Analytic
{
    // Supplier-specific helpers — reads from RefPrimaryID/Name + RefValue
    // which the Analytics table already has
    public string? GetSupplierName()     => Refprimaryname;
    public int? GetSupplierID()          => Refprimaryid;
    public float? GetSupReliability()    => (float?)Refvalue;
}