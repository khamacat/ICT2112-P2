namespace ProRental.Domain.Entities;
using ProRental.Domain.Enums;
public partial class Reliabilityrating
{
    private RatingBand _rating;
    private RatingBand rating { get => _rating; set => _rating = value; }
    public void UpdateRating(RatingBand newValue) => _rating = newValue;
}