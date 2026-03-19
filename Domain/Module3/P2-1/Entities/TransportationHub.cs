using ProRental.Domain.Enums;

namespace ProRental.Domain.Entities;

public partial class TransportationHub
{
    private HubType? _hubType;
    private HubType? HubType { get => _hubType; set => _hubType = value; }
    public void UpdateHubType(HubType newValue) => _hubType = newValue;
}