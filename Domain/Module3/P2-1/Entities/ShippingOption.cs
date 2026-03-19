using ProRental.Domain.Enums;

namespace ProRental.Domain.Entities;

public partial class ShippingOption
{
    private PreferenceType? _preferenceType;
    private PreferenceType? PreferenceType { get => _preferenceType; set => _preferenceType = value; }
    public void UpdatePreferenceType(PreferenceType newValue) => _preferenceType = newValue;

    private TransportMode? _transportMode;
    private TransportMode? TransportMode { get => _transportMode; set => _transportMode = value; }
    public void UpdateTransportMode(TransportMode newValue) => _transportMode = newValue;
}
