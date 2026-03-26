namespace ProRental.Data.Module2.Gateways;

using ProRental.Domain.Entities;
using ProRental.Data.Module2.Interfaces;
using ProRental.Data.UnitOfWork;
using System.Linq;

public class ReliabilityRatingMapper : IReliabilityRatingMapper
{
    private readonly AppDbContext context;

    public ReliabilityRatingMapper(AppDbContext context)
    {
        this.context = context;
    }

    public Reliabilityrating Insert(Reliabilityrating rating)
    {
        context.Reliabilityratings.Add(rating);
        context.SaveChanges();
        return rating;
    }

    public bool Update(Reliabilityrating rating)
    {
        try
        {
            context.Reliabilityratings.Update(rating);
            context.SaveChanges();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public Reliabilityrating FindById(int ratingID)
    {
        return context.Reliabilityratings.Find(ratingID)!;
    }

    public Reliabilityrating FindBySupplierID(int supplierID)
    {
        return context.Reliabilityratings
            .AsEnumerable().Where(r => r.supplierid == supplierID)
            .OrderByDescending(r => r.calculatedat)
            .FirstOrDefault()!;
    }

    public bool Delete(int ratingID)
    {
        try
        {
            var rating = FindById(ratingID);
            if (rating == null) return false;

            context.Reliabilityratings.Remove(rating);
            context.SaveChanges();
            return true;
        }
        catch
        {
            return false;
        }
    }
}