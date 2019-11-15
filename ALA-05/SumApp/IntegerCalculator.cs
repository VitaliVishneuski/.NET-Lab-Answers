using System.Threading;
using System.Threading.Tasks;

namespace SumApp
{
    public static class IntegerCalculator
    {
        public static async Task<double> GetSumAsync(int integer, CancellationToken cancellationToken)
        {
            return await Task.Run(() => GetSum(integer, cancellationToken), cancellationToken);
        }

        private static double GetSum(int integer, CancellationToken cancellationToken)
        {
            double sum = 0;
            for (var number = 0; number <= integer; number++)
            {
                cancellationToken.ThrowIfCancellationRequested();

                sum += number;
                Thread.Sleep(100);
            }

            return sum;
        }
    }
}