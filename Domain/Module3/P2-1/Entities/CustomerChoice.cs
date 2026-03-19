using ProRental.Domain.Enums;

namespace ProRental.Domain.Entities;

public partial class CustomerChoice
{
    private PreferenceType? _preferenceType;
    private PreferenceType? PreferenceType { get => _preferenceType; set => _preferenceType = value; }
    public void UpdatePreferenceType(PreferenceType newValue) => _preferenceType = newValue;
}
