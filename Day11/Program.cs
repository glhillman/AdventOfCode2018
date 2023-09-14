// See https://aka.ms/new-console-template for more information
using System.Numerics;

DayClass day = new DayClass();

var watch = new System.Diagnostics.Stopwatch();
watch.Start();

day.Part1();
day.Part2();

watch.Stop();
Console.WriteLine("Execution Time: {0} ms", watch.ElapsedMilliseconds);

Console.Write("Press Enter to continue...");
Console.ReadLine();

internal class DayClass
{
    int[,] _rawPower = new int[301, 301];
    int _serial = 3999;

    public DayClass()
    {
        for (int y = 1; y <= 300; y++)
        {
            for (int x = 1; x <= 300; x++)
            {
                int rackID = x + 10;
                int power = ((rackID * y) + _serial) * rackID;
                // extract 100's
                if (power >= 100)
                {
                    power = power / 100 % 10;
                }
                else
                {
                    power = 0;
                }

                _rawPower[x, y] = power - 5;
            }
        }
    }

    public void Part1()
    {
        (int x, int y, int max) = FindMax(3);

        Console.WriteLine("Part1: {0},{1}", x, y);
    }

    public void Part2()
    {
        int maxX = 0;
        int maxY = 0;
        int maxValue = int.MinValue;
        int maxSize = 0;

        for (int size = 1; size <= 300; size++)
        {
            (int x, int y, int max) = FindMax(size);
            if (max > maxValue)
            {
                maxValue = max;
                maxX = x;
                maxY = y;
                maxSize = size;
            }
        }

        Console.WriteLine("Part2: {0},{1},{2}", maxX, maxY, maxSize);
    }

    private (int x, int y, int max) FindMax(int size)
    {
        int[,] xSums = new int[301, 301];
        int[,] gridSums = new int[301, 301];

        for (int y = 1; y <= 300; y++)
        {
            int drop = _rawPower[1, y];
            int sum = drop;
            for (int x = 2; x <= size; x++)
            {
                sum += _rawPower[x, y];
            }
            xSums[1, y] = sum;

            for (int x = 2; x <= 301 - size; x++)
            {
                sum -= drop;
                drop = _rawPower[x, y];
                sum += _rawPower[x + size - 1, y];
                xSums[x, y] = sum;
            }
        }

        for (int x = 1; x <= 300; x++)
        {
            int drop = xSums[x, 1];
            int sum = drop;
            for (int y = 2; y <= size; y++)
            {
                sum += xSums[x, y];
            }
            gridSums[x, 1] = sum;

            for (int y = 2; y <= 301 - size; y++)
            {
                sum -= drop;
                drop = xSums[x, y];
                sum += xSums[x, y + size - 1];
                gridSums[x, y] = sum;
            }
        }

        int max = int.MinValue;
        int maxX = 0;
        int maxY = 0;

        for (int x = 1; x <= 301 - size; x++)
        {
            for (int y = 1; y <= 301 - size; y++)
            {
                int value = gridSums[x, y];
                if (value > max)
                {
                    max = value;
                    maxX = x;
                    maxY = y;
                }
            }
        }

        return (maxX, maxY, max);
    }
}
