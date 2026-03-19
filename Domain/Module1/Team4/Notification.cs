using ProRental.Domain.Enums;

namespace ProRental.Domain.Entities;

public partial class Notification
{
    private NotificationType? _notificationType;
    private NotificationType? NotificationType { get => _notificationType; set => _notificationType = value; }
    public void UpdateNotificationType(NotificationType newValue) => _notificationType = newValue;
}