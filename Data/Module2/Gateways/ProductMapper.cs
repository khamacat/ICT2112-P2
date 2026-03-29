using Microsoft.EntityFrameworkCore;
using ProRental.Data.UnitOfWork;
using ProRental.Domain.Entities;
using ProRental.Domain.Enums;
using ProRental.Interfaces.Data;

namespace ProRental.Data;

public class ProductMapper : IProductMapper
{
    private readonly AppDbContext _context;

    public ProductMapper(AppDbContext context)
    {
        _context = context;
    }

    public Product? FindById(int productId)
    {
        return _context.Products
            .Include(p => p.Productdetail)
            .FirstOrDefault(p => EF.Property<int>(p, "Productid") == productId);
    }

    public ICollection<Product>? FindAll()
    {
        return _context.Products
            .Include(p => p.Productdetail)
            .ToList();
    }

    public ICollection<Product>? FindByCategoryId(int categoryId)
    {
        return _context.Products
            .Include(p => p.Productdetail)
            .Where(p => EF.Property<int>(p, "Categoryid") == categoryId)
            .ToList();
    }

    public ICollection<Product>? FindByStatus(ProductStatus status)
    {
        return _context.Products
            .Include(p => p.Productdetail)
            .Where(p => EF.Property<ProductStatus>(p, "Status") == status)
            .ToList();
    }

    public void Insert(Product product)
    {
        _context.Products.Add(product);
        _context.SaveChanges();
    }

    public void Update(Product product)
    {
        // 1. Fetch the existing entity to avoid Tracking Exceptions
        var existing = _context.Products
            .Include(p => p.Productdetail)
            .FirstOrDefault(p => EF.Property<int>(p, "Productid") == product.GetProductId());

        if (existing == null)
            return;

        // Update root Product values through public domain methods
        existing.AssignCategory(product.GetCategoryId());
        existing.UpdateSku(product.GetSku());
        existing.UpdateStatus(product.GetStatus());
        existing.UpdateThreshold(product.GetThreshold());

        // 2. Elegantly update all root scalar properties using EF Core native mapping
        _context.Entry(existing).CurrentValues.SetValues(product);

        // 3. Handle the composite Productdetail
        var incomingDetail = product.GetProductdetail();
        var existingDetail = existing.GetProductdetail();
        
        if (incomingDetail != null)
        {
            if (existingDetail == null)
            {
                incomingDetail.AssignProductId(existing.GetProductId());
                existing.AttachProductdetail(incomingDetail);
            }
            else
            {
                existingDetail.UpdateName(incomingDetail.GetName());
                existingDetail.UpdateDescription(incomingDetail.GetDescription());
                existingDetail.UpdateTotalQuantity(incomingDetail.GetTotalQuantity());
                existingDetail.UpdateWeight(incomingDetail.GetWeight());
                existingDetail.UpdateImage(incomingDetail.GetImage());
                existingDetail.UpdatePrice(incomingDetail.GetPrice());
                existingDetail.UpdateDepositRate(incomingDetail.GetDepositRate());
            }
        }

        // 4. Force the UTC timestamp override safely
        _context.Entry(existing).Property("Updatedat").CurrentValue = DateTime.UtcNow;

        _context.SaveChanges();
    }

    public void Delete(Product product)
    {
        // Fetch existing first to ensure we aren't deleting an untracked instance
        var existing = _context.Products
            .FirstOrDefault(p => EF.Property<int>(p, "Productid") == product.GetProductId());

        if (existing != null)
        {
            _context.Products.Remove(existing);
            _context.SaveChanges();
        }
    }
}