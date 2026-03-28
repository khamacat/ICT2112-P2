using System.Collections.Generic;
using ProRental.Domain.Module2.P2_2.Entities;

namespace ProRental.Data.Module2.Interfaces;

public interface ICategoryChangeLogMapper
{
    void insertCategoryChangeLog(SupplierCategoryChangeLog log);
    void updateCategoryChangeLog(SupplierCategoryChangeLog log);
    bool deleteCategoryChangeLog(int logID);
    SupplierCategoryChangeLog findCategoryChangeLogById(int logID);
    List<SupplierCategoryChangeLog> findLogsBySupplier(int supplierID);
}