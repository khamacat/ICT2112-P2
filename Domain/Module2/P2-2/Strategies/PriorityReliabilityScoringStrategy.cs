namespace ProRental.Domain.Module2.P2_2.Strategies;

public class PriorityReliabilityScoringStrategy : IScoringStrategy
{
    private const double RELIABILITY_WEIGHT = 0.9;
    private const double TURNOVER_WEIGHT = 0.1;

    public double Calculate(double reliability, double turnoverRate)
    {
        return (RELIABILITY_WEIGHT * reliability) + 
               (TURNOVER_WEIGHT * (1 - turnoverRate));
    }
}