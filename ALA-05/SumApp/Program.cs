using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SumApp
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.Unicode;
            Console.InputEncoding = Encoding.Unicode;

            CancellationTokenSource savedCancellationTokenSource = null;

            do
            {
                int integer = ReadInteger();
                CancelOperation(savedCancellationTokenSource);

                var cancellationTokenSource = new CancellationTokenSource();
                var cancellationToken = cancellationTokenSource.Token;

                _ = WriteSumAsync(integer, IntegerCalculator.GetSumAsync(integer, cancellationToken));

                savedCancellationTokenSource = cancellationTokenSource;
            } while (true);
        }

        private static void CancelOperation(CancellationTokenSource cancellationTokenSource)
        {
            if (cancellationTokenSource == null)
                return;

            cancellationTokenSource.Cancel();
            cancellationTokenSource.Dispose();
        }

        private static ConsoleKeyInfo _key;
        private static readonly StringBuilder Builder = new StringBuilder();

        private static int ReadInteger()
        {
            do
            {
                WriteSafe(0, 0, 50, "Введите целое число: ");

                Builder.Clear();
                ClearSafe(0, 1, 50);
                do
                {
                    _key = Console.ReadKey();
                    Builder.Append(_key.KeyChar);
                } while (_key.Key != ConsoleKey.Enter);

                if (int.TryParse(Builder.ToString(), out var integer))
                    return integer;

                WriteSafe(0, 2, 50, "Введено не целое число!!!");
            } while (true);
        }

        private static async Task WriteSumAsync(int integer, Task<double> getSum)
        {
            try
            {
                WriteSafe(0, 4, 50, $"({integer}) Операция начата!!!");
                WriteSafe(0, 4, 100, $"Сумма целых чисел от 0 до {integer} = {await getSum}");
            }
            catch (OperationCanceledException)
            {
                WriteSafe(0, 5, 50, $"({integer}) Операция прервана!!!");
            }
        }

        private static readonly object LockObject = new object();

        private static void WriteSafe(int left, int top, int length, string message = null)
        {
            lock (LockObject)
            {
                ClearSafe(left, top, length);
                Console.SetCursorPosition(left, top);
                Console.WriteLine(message);
                SetInputPositionSafe();
            }
        }

        private static void ClearSafe(int left, int top, int length)
        {
            var buffer = Enumerable.Repeat(' ', length).ToArray();
            lock (LockObject)
            {
                Console.SetCursorPosition(left, top);
                Console.WriteLine(buffer);
                SetInputPositionSafe();
            }
        }

        private static void SetInputPositionSafe()
        {
            lock (LockObject)
            {
                Console.SetCursorPosition(Builder.Length, 1);
            }
        }
    }
}
