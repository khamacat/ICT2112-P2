using System.Data;
using ProRental.Domain.Enums;
namespace ProRental.Domain.Entities;
public partial class Cart
{
    private CartStatus? _status;
    private CartStatus? Status { get => _status; set => _status = value; }

    public void UpdateStatus(CartStatus status) { _status = status; }
}