namespace ProRental.Domain.Entities;

public partial class Damagereport
{
    public int GetDamageReportId() => _damagereportid;
    public void SetDamageReportId(int id) => _damagereportid = id;

    public int GetReturnItemId() => _returnitemid;
    public void SetReturnItemId(int id) => _returnitemid = id;

    public string? GetDescription() => _description;
    public void SetDescription(string desc) => _description = desc;

    public string? GetSeverity() => _severity;
    public void SetSeverity(string severity) => _severity = severity;

    public decimal? GetRepairCost() => _repaircost;
    public void SetRepairCost(decimal? cost) => _repaircost = cost;

    public string? GetImages() => _images;
    public void SetImages(string images) => _images = images;

    public DateTime GetReportDate() => _reportdate;
    public void SetReportDate(DateTime date) => _reportdate = date;
}