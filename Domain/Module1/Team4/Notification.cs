using ProRental.Domain.Enums;

namespace ProRental.Domain.Entities;

public partial class Notification
{
    public NotificationType? NotificationType { get; set; }
}
