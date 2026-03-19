using ProRental.Domain.Enums;

namespace ProRental.Domain.Entities;

public partial class Staffaccesslog
{
    private AccessEventType _eventType;
    private AccessEventType EventType { get => _eventType; set => _eventType = value; }
    public void UpdateEventType(AccessEventType newValue) => _eventType = newValue;
}