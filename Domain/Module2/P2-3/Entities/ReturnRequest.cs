using ProRental.Domain.Enums;

namespace ProRental.Domain.Entities;

public partial class Returnrequest
{
	private ReturnRequestStatus _status;
    private ReturnRequestStatus Status { get => _status; set => _status = value; }
    public void UpdateStatus(ReturnRequestStatus newValue) => _status = newValue;
}