// See https://aka.ms/new-console-template for more information
DayClass day = new DayClass();

var watch = new System.Diagnostics.Stopwatch();
watch.Start();

day.Part1And2();

watch.Stop();
Console.WriteLine("Execution Time: {0} ms", watch.ElapsedMilliseconds);

Console.Write("Press Enter to continue...");
Console.ReadLine();

internal class DayClass
{
    Dictionary<(int x, int y), char> _filler = new();
    public DayClass()
    {
        LoadData();
        XMin = _filler.Keys.Min(x => x.x);
        XMax = _filler.Keys.Max(x => x.x);
        YMin = _filler.Keys.Min(y => y.y); // instructions say to ignore all before first clay row (this threw me for a while! I was 4 over for part one)
        YMax = _filler.Keys.Max(y => y.y);
    }

    public void Part1And2()
    {
        // fill up potential seepage paths & add left & right overflow paths
        XMin--;
        XMax++;
        for (int y = YMin; y <= YMax; y++)
        {
            for (int x = XMin; x <= XMax; x++)
            {
                if (_filler.ContainsKey((x, y)) == false)
                {
                    _filler[(x,y)] = '.';
                }
            }
        }

        FillItUp(500, YMin);

        int seep  = _filler.Values.Count(v => v == '|');
        int water = _filler.Values.Count(v => v == '~');


        Console.WriteLine("Part1: {0}", seep + water);
        Console.WriteLine("Part2: {0}", water);
    }

    public int XMin { get; set; }
    public int XMax { get; set; }
    public int YMin { get; set; }
    public int YMax { get; set; }


    void FillItUp(int x, int y)
    {
        if (_filler[(x, y)] == '.')
        {
            _filler[(x, y)] = '|';

            if (y < YMax)
            {
                FillItUp(x, y + 1);

                if (_filler[(x, y + 1)] == '#' || _filler[(x, y + 1)] == '~')
                {
                    if (x > XMin)
                    {
                        FillItUp(x - 1, y);
                    }
                    if (x < XMax)
                    {
                        FillItUp(x + 1, y);
                    }
                }

                if (RowIsPooled(x, y))
                {
                    // fill left
                    for (int xx = x; xx >= XMin && _filler[(xx,y)] == '|'; xx--)
                    {
                        _filler[(xx, y)] = '~';
                    }
                    // fill right
                    for (int xx = x; xx <= XMax && _filler[(xx, y)] == '|'; xx++)
                    {
                        _filler[(xx, y)] = '~';
                    }
                }
            }
        }
    }

    bool RowIsPooled(int x, int y)
    {
        bool pooled = true;

        // check left
        for (int xx = x; pooled && xx >= XMin && _filler[(xx, y)] != '#'; xx--)
        {
            if (_filler[(xx, y)] == '.' || _filler[(xx, y + 1)] == '|')
            {
                pooled = false;
            }
        }
        // check right
        for (int xx = x; pooled && xx <= XMax && _filler[(xx, y)] != '#'; xx++)
        {
            if (_filler[(xx, y)] == '.' || _filler[(xx, y + 1)] == '|')
            {
                pooled = false;
            }
        }

        return pooled;
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
                string[] parts = line.Split('=', ',', '.');
                int single = int.Parse(parts[1]);
                int from = int.Parse(parts[3]);
                int to = int.Parse(parts[5]);
                if (parts[0][0] == 'x')
                {
                    int x = single;
                    for (int y = from; y <= to; y++)
                    {
                        if (_filler.ContainsKey((x,y)) == false)
                        {
                            _filler[(x,y)] = '#';
                        }
                    }
                }
                else
                {
                    int y = single;
                    for (int x = from; x <= to; x++)
                    {
                        if (_filler.ContainsKey((x,y)) == false)
                        {
                            _filler[(x,y)] = '#';
                        }
                    }
                }
            }

            file.Close();
        }
    }

}
