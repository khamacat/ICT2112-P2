using ProRental.Domain.Enums;

namespace ProRental.Domain.Entities;

public partial class Clearancebatch
{
    private ClearanceBatchStatus _status;
    private ClearanceBatchStatus Status { get => _status; set => _status = value; }

    // ── Public Accessors ───────────────────────────────────────────────────────
    // The scaffolded entity has all properties private. These public methods
    // expose the backing fields for use in control and presentation layers.

    public int GetClearanceBatchId() => _clearancebatchid;
    public void SetClearanceBatchId(int value) => _clearancebatchid = value;

    public string GetBatchName() => _batchname;
    public void SetBatchName(string value) => _batchname = value;

    public DateTime GetCreatedDate() => _createddate;
    public void SetCreatedDate(DateTime value) => _createddate = value;

    public DateTime? GetClearanceDate() => _clearancedate;
    public void SetClearanceDate(DateTime? value) => _clearancedate = value;

    public ClearanceBatchStatus GetStatus() => _status;
    public void UpdateStatus(ClearanceBatchStatus newValue) => _status = newValue;
}