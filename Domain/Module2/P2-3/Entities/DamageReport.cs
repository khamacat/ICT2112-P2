namespace ProRental.Domain.Entities;

public partial class Damagereport
{
    // PUBLIC GETTERS   
    public int GetDamageReportId() => _damagereportid;

    public int GetReturnItemId() => _returnitemid;
    public void UpdateReturnItemId(int id) => _returnitemid = id;

    public string? GetDescription() => _description;
    public void UpdateDescription(string desc) => _description = desc;

    public string? GetSeverity() => _severity;
    public void UpdateSeverity(string severity) => _severity = severity;

    public decimal? GetRepairCost() => _repaircost;
    public void UpdateRepairCost(decimal? cost) => _repaircost = cost;

    public string? GetImages() => _images;
    public void UpdateImages(string images) => _images = images;

    public DateTime GetReportDate() => _reportdate;
    public void UpdateReportDate(DateTime date) => _reportdate = date;

    // PRIVATE SETTERS 
    private void SetDamageReportId(int id) => _damagereportid = id;

    private void SetReturnItemId(int id) => _returnitemid = id;

    private void SetDescription(string desc) => _description = desc;

    private void SetSeverity(string severity) => _severity = severity;

    private void SetRepairCost(decimal? cost) => _repaircost = cost;

    private void SetImages(string images) => _images = images;

    private void SetReportDate(DateTime date) => _reportdate = date;
}