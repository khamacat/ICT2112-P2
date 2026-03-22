namespace ProRental.Interfaces;

/// <summary>
/// Common contract for all Analytics product types returned by AnalyticsFactory.
/// GetType() renamed to GetAnalyticsType() to avoid hiding object.GetType().
/// </summary>
public interface IAnalytics
{
    string GetAnalyticsType();   // was GetType() — renamed to avoid CS0108
    int GetID();
}