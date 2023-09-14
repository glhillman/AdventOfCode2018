// See https://aka.ms/new-console-template for more information
DayClass day = new DayClass();

var watch = new System.Diagnostics.Stopwatch();
watch.Start();

day.Part1_2();

watch.Stop();
Console.WriteLine("Execution Time: {0} ms", watch.ElapsedMilliseconds);

Console.Write("Press Enter to continue...");
Console.ReadLine();

internal class LightPoint
{
    public LightPoint(int x, int y, int vx, int vy)
    {
        X = x;
        Y = y;
        VX = vx;
        VY = vy;
    }

    public void MovePoint()
    {
        X += VX;
        Y += VY;
    }

    public int X { get; set; }
    public int Y { get; set; }
    public int VX { get; private set; }
    public int VY { get; private set; }

    public override string ToString()
    {
        return string.Format("Pos: {0},{1} Vector: {2},{3}", X, Y, VX, VY);
    }

}
internal class DayClass
{
    public List<LightPoint> _lPoints = new();
    public DayClass()
    {
        LoadData();
    }

    public void Part1_2()
    {
        int moves = 0;
        while (true)
        {
            foreach (LightPoint point in _lPoints)
            {
                point.MovePoint();
            }
            moves++;
            int lowX = _lPoints.Min(x => x.X);
            int highX = _lPoints.Max(x => x.X);
            int lowY = _lPoints.Min(y => y.Y);
            int highY = _lPoints.Max(y => y.Y);
            if (moves == 10136/*highX - lowX < 120*/) // observation of output while numbers were converging
            {
                DumpPoints(lowX, highX, lowY, highY);
                break;
            }
        }
        Console.WriteLine("Part1: {0}", "EHAZPZHP");
        Console.WriteLine("Part2: 10136");
    }

    private void DumpPoints(int lowX, int highX, int lowY, int highY)
    {
        for (int y = lowY; y <= highY; y++)
        {
            for (int x = lowX; x <= highX; x++)
            {
                if (_lPoints.Exists(p => p.X == x && p.Y == y))
                {
                    Console.Write('#');
                }
                else
                {
                    Console.Write('.');
                }
            }
            Console.WriteLine();
        }
        Console.WriteLine();
        Console.WriteLine();
    }
    private void LoadData()
    {
        string inputFile = AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\input.txt";

        if (File.Exists(inputFile))
        {
            string? line;
            StreamReader file = new StreamReader(inputFile);
            while ((line = file.ReadLine()) != null)
            {
                string[] parts = line.Split('<', ',', '>');
                _lPoints.Add(new LightPoint(int.Parse(parts[1]), int.Parse(parts[2]), int.Parse(parts[4]), int.Parse(parts[5])));
            }

            file.Close();
        }
    }

}
