using System.Collections.Generic;
using ProRental.Domain.Module2.P2_2.Entities;

namespace ProRental.Data.Module2.Interfaces;

public interface ISupplierMapper
{
    void insertSupplier(Supplier supplier);
    void updateSupplier(Supplier supplier);
    bool deleteSupplier(int supplierID);
    Supplier findSupplierById(int supplierID);
    List<Supplier> findAll();
}