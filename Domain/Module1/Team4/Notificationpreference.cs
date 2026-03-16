using ProRental.Domain.Enums;

namespace ProRental.Domain.Entities;

public partial class Notificationpreference
{
    public NotificationFrequency? NotificationFrequency { get; set; }
    public TransportMode? TransportMode { get; set; }
}
