namespace AntuDevOps.PointOfSale.Domain.Common;

public static class RandomExtensions
{
    public static bool Chance(this Random random, double chance)
    {
        if (chance < 0.0 || chance > 1.0)
            throw new ArgumentOutOfRangeException(nameof(chance), "Must be between 0 and 1");

        return random.NextDouble() < chance;
    }
}
