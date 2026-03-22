namespace ProRental.Domain.Entities;
using ProRental.Domain.Enums;

public partial class Analytic
{
    // ── Backing field only — NO private property wrapper ─────────────────────
    // EF Core scans private properties and tries to map them as DB columns.
    // Using the backing field directly avoids EF picking up 'type' as a shadow property.
    private AnalyticsType _analyticsType;

    public void UpdateType(AnalyticsType newValue) => _analyticsType = newValue;
    public string GetAnalyticsType() => _analyticsType.ToString();

    // ── Public getters via scaffold backing fields ────────────────────────────
    public int GetID()                 => _analyticsid;
    public DateTime? GetStartDate()    => _startdate;
    public DateTime? GetEndDate()      => _enddate;
    public int? GetLoanAmt()           => _loanamt;
    public int? GetReturnAmt()         => _returnamt;
    public int? GetRefPrimaryID()      => _refprimaryid;
    public string? GetRefPrimaryName() => _refprimaryname;
    public decimal? GetRefValue()      => _refvalue;

    // ── Public setters ────────────────────────────────────────────────────────
    public void SetStartDate(DateTime? value)    => _startdate = value;
    public void SetEndDate(DateTime? value)      => _enddate = value;
    public void SetLoanAmt(int? value)           => _loanamt = value;
    public void SetReturnAmt(int? value)         => _returnamt = value;
    public void SetRefPrimaryID(int? value)      => _refprimaryid = value;
    public void SetRefPrimaryName(string? value) => _refprimaryname = value;
    public void SetRefValue(decimal? value)      => _refvalue = value;
}