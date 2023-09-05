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

internal class Rect
{
    public Rect(int claim, int left, int top, int right, int bottom)
    {
        Claim = claim;
        Left = left;
        Top = top;
        Right = right;
        Bottom = bottom;
    }

    public int Claim { get; private set; }
    public int Left { get; private set; }
    public int Top { get; private set; }
    public int Right { get; private set; }
    public int Bottom { get; private set; }
}

internal class DayClass
{
    List<Rect> _rects = new();
    int[,] _grid;

    public DayClass()
    {
        LoadData();
        LoadGrid();
    }

    public void Part1()
    {
        int overlaps = 0;

        for (int row = Top; row <= Bottom; row++)
        {
            for (int col = Left; col <= Right; col++)
            {
                if (_grid[row, col] > 1)
                {
                    overlaps++;
                }
            }
        }
        

        Console.WriteLine("Part1: {0}", overlaps);
    }

    public void Part2()
    {
        bool found;
        int claim = 0;

        foreach (Rect rect in _rects)
        {
            found = true;
            for (int row = rect.Top; found && row <= rect.Bottom; row++)
            {
                for (int col = rect.Left; found && col <= rect.Right; col++)
                {
                    if (_grid[row,col] != 1)
                    {
                        found = false;
                    }
                }
            }
            if (found)
            {
                claim = rect.Claim;
                break;
            }
        }    


        Console.WriteLine("Part2: {0}", claim);
    }

    public void LoadGrid()
    {
        _grid = new int[Bottom + 1, Right + 1];

        foreach (Rect rect in _rects)
        {
            for (int row = rect.Top; row <= rect.Bottom; row++)
            {
                for (int col = rect.Left; col <= rect.Right; col++)
                {
                    _grid[row, col]++;
                }
            }
        }
    }
    
    public int Left { get; set; } = int.MaxValue;
    public int Top { get; set; } = int.MaxValue;
    public int Right { get; set; } = int.MinValue;
    public int Bottom { get; set; } = int.MinValue;


    private void LoadData()
    {
        string inputFile = AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\input.txt";

        if (File.Exists(inputFile))
        {
            string? line;
            StreamReader file = new StreamReader(inputFile);
            while ((line = file.ReadLine()) != null)
            {
                int claim;
                int left;
                int top;
                int right;
                int bottom;

                string[] parts = line.Split('#', ' ', '@', ',', ':', 'x');

                claim = int.Parse(parts[1]);
                left = int.Parse(parts[4]);
                top = int.Parse(parts[5]);
                right = int.Parse(parts[7]) + left - 1;
                bottom = int.Parse(parts[8]) + top - 1;

                _rects.Add(new Rect(claim, left, top, right, bottom));

                Left = Math.Min(Left, left);
                Right = Math.Max(Right, right);
                Top = Math.Min(Top, top);
                Bottom = Math.Max(Bottom, bottom);
            }

            file.Close();
        }
    }

}
