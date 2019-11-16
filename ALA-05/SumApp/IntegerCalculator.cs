using System.Threading;
using System.Threading.Tasks;

namespace SumApp
{
    public static class IntegerCalculator
    {
        public static async Task<double> GetSumAsync(int integer, CancellationToken cancellationToken)
        {
            double sum = 0;
            for (var number = 0; number <= integer; number++)
            {
                sum += number;

                await Task.Delay(100, cancellationToken);
            }

            return sum;
        }
    }
}
