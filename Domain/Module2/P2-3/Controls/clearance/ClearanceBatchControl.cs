using ProRental.Domain.Entities;
using ProRental.Domain.Enums;
using ProRental.Interfaces.Data;
using ProRental.Interfaces.Domain;

namespace ProRental.Domain.Controls;

public class ClearanceBatchControl : iClearanceBatchQuery, iClearanceBatchControl
{
    private readonly IClearanceBatchMapper _batchMapper;
    private readonly IClearanceItemMapper _itemMapper;
    private readonly IClearanceLogEnricher _logEnricher;

    public ClearanceBatchControl(
        IClearanceBatchMapper batchMapper,
        IClearanceItemMapper itemMapper,
        IClearanceLogEnricher logEnricher)
    {
        _batchMapper = batchMapper;
        _itemMapper = itemMapper;
        _logEnricher = logEnricher;
    }

    // ── Batch CRUD ─────────────────────────────────────────────────────────────

    public bool CreateClearanceBatch(Clearancebatch clearanceBatch)
    {
        try
        {
            if (!ValidateClearanceBatch(clearanceBatch))
                return false;

            if (CheckBatchConflicts(clearanceBatch))
                return false;

            // Default status to SCHEDULED on creation
            clearanceBatch.UpdateStatus(ClearanceBatchStatus.SCHEDULED);
            clearanceBatch.SetCreatedDate(DateTime.UtcNow);

            _batchMapper.Insert(clearanceBatch);
            _logEnricher.LogClearanceProcess(clearanceBatch.GetClearanceBatchId());
            return true;
        }
        catch
        {
            return false;
        }
    }

    public Clearancebatch GetClearanceBatchById(int clearanceBatchId)
    {
        return _batchMapper.FindById(clearanceBatchId)!;
    }

    public List<Clearancebatch> GetAllBatches()
    {
        var result = _batchMapper.FindAll();
        return result?.ToList() ?? new List<Clearancebatch>();
    }

    public List<Clearancebatch> GetActiveBatches()
    {
        var result = _batchMapper.FindByStatus(ClearanceBatchStatus.ACTIVE);
        return result?.ToList() ?? new List<Clearancebatch>();
    }

    public bool UpdateClearanceBatch(Clearancebatch clearanceBatch)
    {
        try
        {
            if (!ValidateClearanceBatch(clearanceBatch))
                return false;

            _batchMapper.Update(clearanceBatch);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public bool DeleteClearanceBatch(int clearanceBatchId)
    {
        try
        {
            var batch = _batchMapper.FindById(clearanceBatchId);
            if (batch == null)
                return false;

            // Only allow deletion of SCHEDULED batches (not ACTIVE or CLOSED)
            if (batch.GetStatus() == ClearanceBatchStatus.ACTIVE)
                return false;

            // Remove all clearance items in this batch first
            var items = _itemMapper.FindByBatchId(clearanceBatchId);
            if (items != null)
            {
                foreach (var item in items)
                {
                    _itemMapper.Delete(item);
                }
            }

            _batchMapper.Delete(batch);
            return true;
        }
        catch
        {
            return false;
        }
    }

    // ── Batch Lifecycle ────────────────────────────────────────────────────────
    // Status transitions: SCHEDULED → ACTIVE → CLOSED

    public bool ScheduleBatch(int clearanceBatchId)
    {
        try
        {
            var batch = _batchMapper.FindById(clearanceBatchId);
            if (batch == null)
                return false;

            // Can only schedule a batch that has no status yet or is being reset
            batch.UpdateStatus(ClearanceBatchStatus.SCHEDULED);
            _batchMapper.Update(batch);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public bool ActivateBatch(int clearanceBatchId)
    {
        try
        {
            var batch = _batchMapper.FindById(clearanceBatchId);
            if (batch == null)
                return false;

            // Can only activate a SCHEDULED batch
            if (batch.GetStatus() != ClearanceBatchStatus.SCHEDULED)
                return false;

            batch.UpdateStatus(ClearanceBatchStatus.ACTIVE);
            _batchMapper.Update(batch);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public bool CloseBatch(int clearanceBatchId)
    {
        try
        {
            var batch = _batchMapper.FindById(clearanceBatchId);
            if (batch == null)
                return false;

            // Can only close an ACTIVE batch
            if (batch.GetStatus() != ClearanceBatchStatus.ACTIVE)
                return false;

            batch.UpdateStatus(ClearanceBatchStatus.CLOSED);
            _batchMapper.Update(batch);
            _logEnricher.LogClearanceProcess(clearanceBatchId);
            return true;
        }
        catch
        {
            return false;
        }
    }

    // ── Batch Validation ──────────────────────────────────────────────────────

    private bool ValidateClearanceBatch(Clearancebatch clearanceBatch)
    {
        // Batch name is required
        if (string.IsNullOrWhiteSpace(clearanceBatch.GetBatchName()))
            return false;

        // Clearance date must be set and in the future (for new batches)
        var clearanceDate = clearanceBatch.GetClearanceDate();
        if (clearanceDate == null)
            return false;

        return true;
    }

    private bool CheckBatchConflicts(Clearancebatch clearanceBatch)
    {
        // Check if a batch with the same name already exists
        var allBatches = _batchMapper.FindAll();
        if (allBatches == null)
            return false;

        foreach (var existing in allBatches)
        {
            // Skip self-comparison for updates
            if (existing.GetClearanceBatchId() == clearanceBatch.GetClearanceBatchId()
                && clearanceBatch.GetClearanceBatchId() != 0)
                continue;

            // Check for duplicate batch name
            if (existing.GetBatchName() == clearanceBatch.GetBatchName())
                return true; // Conflict found

            // Check for date overlap with active/scheduled batches
            if (existing.GetStatus() != ClearanceBatchStatus.CLOSED
                && existing.GetClearanceDate().HasValue
                && clearanceBatch.GetClearanceDate().HasValue
                && existing.GetClearanceDate() == clearanceBatch.GetClearanceDate())
                return true; // Date conflict
        }

        return false; // No conflicts
    }
}
