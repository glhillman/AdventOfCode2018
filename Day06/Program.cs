// See https://aka.ms/new-console-template for more information
DayClass day = new DayClass();

var watch = new System.Diagnostics.Stopwatch();
watch.Start();

day.Part1();
day.Part2();

watch.Stop();
Console.WriteLine("Execution Time: {0} ms", watch.ElapsedMilliseconds);

Console.Write("Press Enter to continue...");
Console.ReadLine();

internal class Point
{
    public Point (int x, int y)
    {
        X = x;
        Y = y;
    }

    public int X { get; set; }
    public int Y { get; set; }

    public override string ToString()
    {
        return string.Format("{0},{1}", X, Y);
    }
}

internal class Location
{
    public Location(int iD, int mDistance)
    {
        ID = iD;
        MDistance = mDistance;
    }
    
    public int ID { get; set; }
    public int MDistance { get; set; } // -1 == infinity

    public override string ToString()
    {
        return string.Format("ID: {0}, MDistance: {1}", ID, MDistance);
    }
}
internal class DayClass
{
    List<Point> _points = new();
    
    public DayClass()
    {
        LoadData();
        MinX = _points.Min(p => p.X);
        MaxX = _points.Max(p => p.X) + 1;
        MinY = _points.Min(p => p.Y);
        MaxY = _points.Max(p => p.Y);
    }

    public void Part1()
    {
        Location[,] map = new Location[MaxX+1, MaxY+1];
        Point p;

        for (int i = 0; i < _points.Count; i++)
        {
            p = _points[i];
            map[p.X, p.Y] = new Location(i, 0);
        }

        // Load map
        // first, flood with first point
        p = _points[0];
        for (int x = 0; x <= MaxX; x++)
        {
            for (int y = 0; y <= MaxY; y++)
            {
                map[x,y] = new Location(0, Manhattan(p.X, p.Y, x, y));
            }
        }

        for (int i = 1; i < _points.Count; i++)
        {
            p = _points[i];
            for (int x = 0; x <= MaxX; x++)
            {
                for (int y = 0;y <= MaxY; y++)
                {
                    int mDistance = Manhattan(p.X, p.Y, x, y);
                    if (mDistance < map[x,y].MDistance)
                    {
                        map[x, y].ID = i;
                        map[x,y].MDistance = mDistance;
                    }
                    else if (mDistance == map[x,y].MDistance)
                    {
                        map[x, y].ID = -1;
                    }
                }
            }
        }

        int[] sums = new int[_points.Count];
        for (int x = 0; x<=MaxX; x++)
        {
            for (int y = 0; y<=MaxY; y++)
            {
                int index = map[x, y].ID;
                if (index >= 0)
                {
                    sums[map[x, y].ID]++;
                }
            }
        }
        // eliminate the infinite edges
        for (int x = 0; x <= MaxX; x++)
        {
            int index = map[x, 0].ID;
            if (index >= 0)
            {
                sums[map[x, 0].ID] = -1;
            }
            index = map[x, MaxY].ID;
            if (index >= 0)
            {
                sums[index] = -1;
            }
        }
        for (int y = 0; y  <= MaxY; y++)
        {
            int index = map[0, y].ID;
            if (index >= 0)
            {
                sums[index] = -1;
            }
            index = map[MaxX, y].ID;
            if (index >= 0)
            {
                sums[index] = -1;
            }
        }

        int maxInternal = sums.Max();

        //DumpMap(map);

        Console.WriteLine("Part1: {0}", maxInternal);
    }

    public void Part2()
    {
        int sum = 0;
        bool found = false;

        for (int y = 0; !found && y <= MaxY; y++)
        {
            for (int x = 0; !found && x <= MaxX; x++)
            {
                int dist = TryManhattan(x, y);
                if (dist < 10000)
                {
                    sum++;
                }
            }
        }

        Console.WriteLine("Part2: {0}", sum);
    }

    private int TryManhattan(int x, int y)
    {
        int sum = 0;
        foreach (Point p in _points)
        {
            sum += Manhattan(x, y, p.X, p.Y);
        }

        return sum;
    }

    private int Manhattan(int x1, int y1, int x2, int y2)
    {
        return Math.Abs(x1 - x2) + Math.Abs(y1 - y2);
    }

    private int MinX { get; set; } = int.MaxValue;
    private int MinY { get; set; } = int.MaxValue;
    private int MaxX { get; set; } = int.MinValue;
    private int MaxY { get; set; } = int.MinValue;

    private void DumpMap(Location[,] map)
    {
        for (int y = 0; y <= MaxY; y++)
        {
            for (int x = 0; x <= MaxX; x++)
            {
                if (map[x, y].ID != 9)
                {
                    Console.Write(map[x, y].ID);
                }
                else
                {
                    Console.Write('.');
                }
            }
            Console.WriteLine();
        }
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
                string[] parts = line.Split(", ");
                _points.Add(new Point(int.Parse(parts[0]), int.Parse(parts[1])));
            }

            file.Close();
        }
    }

}
