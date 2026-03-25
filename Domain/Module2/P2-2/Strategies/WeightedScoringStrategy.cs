namespace ProRental.Domain.Module2.P2_2.Strategies;

public class WeightedScoringStrategy : IScoringStrategy
{
    private const double RELIABILITY_WEIGHT = 0.7;
    private const double TURNOVER_WEIGHT = 0.3;

    public double Calculate(double reliability, double turnoverRate)
    {
        return (RELIABILITY_WEIGHT * reliability) + 
               (TURNOVER_WEIGHT * (1 - turnoverRate));
    }
}