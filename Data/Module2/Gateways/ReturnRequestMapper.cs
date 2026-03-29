using Microsoft.EntityFrameworkCore;
using ProRental.Data.UnitOfWork;
using ProRental.Domain.Entities;
using ProRental.Domain.Enums;
using ProRental.Interfaces.Data;

namespace ProRental.Data;

public class ReturnRequestMapper : IReturnRequestMapper
{
    private readonly AppDbContext _context;

    public ReturnRequestMapper(AppDbContext context)
    {
        _context = context;
    }

    public Returnrequest? FindById(int requestId)
    {
        // RULE: Use EF.Property to access the private 'Returnrequestid'.
        return _context.Returnrequests
            .FirstOrDefault(r => EF.Property<int>(r, "Returnrequestid") == requestId);
    }

    public Returnrequest? FindByOrderId(int orderId)
    {
        // RULE: Use EF.Property to access the private 'Orderid'.
        return _context.Returnrequests
            .FirstOrDefault(r => EF.Property<int>(r, "Orderid") == orderId);
    }

    public ICollection<Returnrequest>? FindAll()
    {
        return _context.Returnrequests.ToList();
    }

    public ICollection<Returnrequest>? FindByCustomerId(int customerId)
    {
        // RULE: Use EF.Property to query against the private 'Customerid'.
        return _context.Returnrequests
            .Where(r => EF.Property<int>(r, "Customerid") == customerId)
            .ToList();
    }

    public ICollection<Returnrequest>? FindByStatus(ReturnRequestStatus status)
    {
        // RULE: Use EF.Property to query against the private 'Status' enum from the manual partial class.
        return _context.Returnrequests
            .Where(r => EF.Property<ReturnRequestStatus>(r, "Status") == status)
            .ToList();
    }

    public void Insert(Returnrequest request)
    {
        _context.Returnrequests.Add(request);
        _context.SaveChanges();
    }

    public void Update(Returnrequest request)
    {
        var existing = _context.Returnrequests
            .FirstOrDefault(r => EF.Property<int>(r, "Returnrequestid") == request.GetReturnRequestId());

        if (existing == null) return;

        _context.Entry(existing).CurrentValues.SetValues(request);
        _context.SaveChanges();
    }

    public void Delete(Returnrequest request)
    {
        var existing = _context.Returnrequests
            .FirstOrDefault(r => EF.Property<int>(r, "Returnrequestid") == request.GetReturnRequestId());
            
        if (existing != null)
        {
            _context.Returnrequests.Remove(existing);
            _context.SaveChanges();
        }
    }
}