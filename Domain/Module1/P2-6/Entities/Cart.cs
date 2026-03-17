using ProRental.Domain.Enums;
namespace ProRental.Domain.Entities;
public partial class Cart
{
    public CartStatus? Status { get; set; }
}