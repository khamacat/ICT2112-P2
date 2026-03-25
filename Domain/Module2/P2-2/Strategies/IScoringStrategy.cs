namespace ProRental.Domain.Module2.P2_2.Strategies;

public interface IScoringStrategy
{
    double Calculate(double reliability, double turnoverRate);
}