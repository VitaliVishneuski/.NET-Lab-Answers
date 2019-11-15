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

                _ = OutputSumAsync(integer, cancellationToken);

                savedCancellationTokenSource = cancellationTokenSource;
            } while (true);
        }

        private static  ConsoleKeyInfo _key;
        private static readonly StringBuilder Builder = new StringBuilder();

        private static int ReadInteger()
        {
            do
            {
                SafePrint(0, 0, 50, "Введите целое число: ");

                Builder.Clear();
                SafeClear(0, 1, 50);
                do
                {
                    _key = Console.ReadKey();
                    Builder.Append(_key.KeyChar);
                } while (_key.Key != ConsoleKey.Enter);

                if (int.TryParse(Builder.ToString(), out int integer))
                    return integer;

                SafePrint(0, 2, 50, "Введено не целое число!!!");
            } while (true);
        }

        private static void CancelOperation(CancellationTokenSource cancellationTokenSource)
        {
            if (cancellationTokenSource != null)
            {
                cancellationTokenSource.Cancel();
                cancellationTokenSource.Dispose();
            }
        }

        private static async Task OutputSumAsync(int integer, CancellationToken cancellationToken)
        {
            try
            {
                SafePrint(0, 4, 50, $"({integer}) Операция начата!!!");
                var sum = await IntegerCalculator.GetSumAsync(integer, cancellationToken);
                SafePrint(0, 4, 100, $"Сумма целых чисел от 0 до {integer} = {sum}");
            }
            catch (OperationCanceledException)
            {
                SafePrint(0, 5, 50, $"({integer}) Операция прервана!!!");
            }
        }

        private static readonly object LockObject = new object();

        private static void SafePrint(int left, int top, int length, string message = null)
        {
            lock (LockObject)
            {
                SafeClear(left, top, length);
                Console.SetCursorPosition(left, top);
                Console.WriteLine(message);
                SetInputPosition();
            }
        }

        private static void SafeClear(int left, int top, int length)
        {
            var buffer = Enumerable.Repeat(' ', length).ToArray();
            lock (LockObject)
            {
                Console.SetCursorPosition(left, top);
                Console.WriteLine(buffer);
                SetInputPosition();
            }
        }

        private static void SetInputPosition()
        {
            Console.SetCursorPosition(Builder.Length, 1);
        }
    }
}