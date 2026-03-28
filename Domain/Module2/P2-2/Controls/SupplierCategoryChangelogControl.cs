using System;
using System.Collections.Generic;
using ProRental.Data.Module2.Interfaces;
using ProRental.Domain.Enums;
using ProRental.Domain.Module2.P2_2.Entities;
using ProRental.Domain.Module2.P2_2.Factories;

namespace ProRental.Domain.Module2.P2_2.Controls;

public class SupplierCategoryChangeLogControl
{
    private readonly ICategoryChangeLogMapper _logMapper;
    private readonly SupplierRegistryFactory _factory;

    public SupplierCategoryChangeLogControl(ICategoryChangeLogMapper logMapper, SupplierRegistryFactory factory)
    {
        _logMapper = logMapper;
        _factory = factory;
    }

    public SupplierCategoryChangeLog createLog(int supplierID, SupplierCategory previousCategory, SupplierCategory newCategory, string reason)
    {
        var entity = _factory.createSupplierRegistryEntity("SupplierCategoryChangeLog") as SupplierCategoryChangeLog;
        if (entity is null)
            throw new InvalidOperationException("Factory did not create SupplierCategoryChangeLog.");

        entity.SupplierID = supplierID;
        entity.PreviousCategory = previousCategory;
        entity.NewCategory = newCategory;
        entity.ChangedReason = reason;
        entity.ChangedAt = DateTime.UtcNow;

        _logMapper.insertCategoryChangeLog(entity);
        return entity;
    }

    public List<SupplierCategoryChangeLog> getLogsBySupplier(int supplierID)
    {
        return _logMapper.findLogsBySupplier(supplierID);
    }

    public bool updateLogReason(int logID, string newReason)
    {
        var log = _logMapper.findCategoryChangeLogById(logID);
        if (log is null)
            return false;

        log.updateReason(newReason);
        log.ChangedAt = DateTime.UtcNow;
        _logMapper.updateCategoryChangeLog(log);
        return true;
    }

    public bool deleteLog(int logID)
    {
        return _logMapper.deleteCategoryChangeLog(logID);
    }
}