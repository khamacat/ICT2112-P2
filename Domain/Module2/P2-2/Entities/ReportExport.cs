namespace ProRental.Domain.Entities;
using ProRental.Domain.Enums;

public partial class Reportexport
{
    // ── Backing fields only — no private property wrappers ───────────────────
    private VisualType _visualType;
    private FileFormat _fileFormat;

    public void UpdateType(VisualType newValue)   => _visualType = newValue;
    public void UpdateFormat(FileFormat newValue) => _fileFormat = newValue;

    // ── Public getters ────────────────────────────────────────────────────────
    public int GetReportID()          => _reportid;
    public int? GetRefAnalyticsID()   => _refanalyticsid;
    public string? GetTitle()         => _title;
    public string? GetFileURL()       => _url;
    public VisualType GetVisualType() => _visualType;
    public FileFormat GetFileFormat() => _fileFormat;

    // ── Public setters ────────────────────────────────────────────────────────
    public void SetRefAnalyticsID(int? value) => _refanalyticsid = value;
    public void SetTitle(string? value)       => _title = value;
    public void SetUrl(string? value)         => _url = value;
}