using ProRental.Domain.Enums;

namespace ProRental.Domain.Entities;

public partial class Staffaccesslog
{
    public AccessEventType EventType { get; private set; }
}