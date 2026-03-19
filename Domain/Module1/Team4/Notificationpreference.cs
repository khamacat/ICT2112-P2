using ProRental.Domain.Enums;

namespace ProRental.Domain.Entities;

public partial class Notificationpreference
{
    private NotificationFrequency? _notificationfrequency;
    private NotificationFrequency? Notificationfrequency { get => _notificationfrequency; set => _notificationfrequency = value; }
    public void UpdateNotificationfrequency(NotificationFrequency newValue) => _notificationfrequency = newValue;

    private NotificationGranularity? _notificationGranularity;
    private NotificationGranularity? NotificationGranularity { get => _notificationGranularity; set => _notificationGranularity = value; }
    public void UpdateNotificationGranularity(NotificationGranularity newValue) => _notificationGranularity = newValue;
}
