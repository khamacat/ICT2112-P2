using System;
using System.Collections.Generic;

namespace ProRental.Domain.Entities;

public partial class Analytic
{
    private int _analyticsid;
    private int Analyticsid { get => _analyticsid; set => _analyticsid = value; }

    private DateTime? _startdate;
    private DateTime? Startdate { get => _startdate; set => _startdate = value; }

    private DateTime? _enddate;
    private DateTime? Enddate { get => _enddate; set => _enddate = value; }

    private int? _loanamt;
    private int? Loanamt { get => _loanamt; set => _loanamt = value; }

    private int? _returnamt;
    private int? Returnamt { get => _returnamt; set => _returnamt = value; }

    private int? _refprimaryid;
    private int? Refprimaryid { get => _refprimaryid; set => _refprimaryid = value; }

    private string? _refprimaryname;
    private string? Refprimaryname { get => _refprimaryname; set => _refprimaryname = value; }

    private decimal? _refvalue;
    private decimal? Refvalue { get => _refvalue; set => _refvalue = value; }

    public virtual ICollection<Reportexport> Reportexports { get; private set; } = new List<Reportexport>();

    public virtual ICollection<Transactionlog> Transactionlogs { get; private set; } = new List<Transactionlog>();
}
