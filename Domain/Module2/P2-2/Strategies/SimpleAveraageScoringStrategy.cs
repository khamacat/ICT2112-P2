namespace ProRental.Domain.Module2.P2_2.Strategies;

public class SimpleAverageScoringStrategy : IScoringStrategy
{
    public double Calculate(double reliability, double turnoverRate)
    {
        return (reliability + (1 - turnoverRate)) / 2.0;
    }
}