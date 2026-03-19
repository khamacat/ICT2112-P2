using ProRental.Domain.Enums;

namespace ProRental.Domain.Entities;

public partial class RouteLeg
{
    private TransportMode? _transportMode;
    private TransportMode? TransportMode { get => _transportMode; set => _transportMode = value; }
    public void UpdateTransportMode(TransportMode newValue) => _transportMode = newValue;
}
