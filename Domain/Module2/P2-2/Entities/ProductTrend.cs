namespace ProRental.Domain.Entities;
using ProRental.Domain.Enums;
public partial class ProductTrend
{
    private AnalyticsType _type;
    private AnalyticsType type { get => _type; set => _type = value; }
    public void UpdateType(AnalyticsType newValue) => _type = newValue;
}