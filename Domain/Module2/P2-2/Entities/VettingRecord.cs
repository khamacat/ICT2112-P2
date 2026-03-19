namespace ProRental.Domain.Entities;
using ProRental.Domain.Enums;
public partial class Vettingrecord
{
    private VettingDecision _decision;
    private VettingDecision decision { get => _decision; set => _decision = value; }
    public void UpdateDecision(VettingDecision newValue) => _decision = newValue;
}