// See https://aka.ms/new-console-template for more information
using System.Text;

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
    char[,,] _maps;
    public DayClass()
    {
    }

    public void Part1()
    {
        int result = LoopChanges(10);

        Console.WriteLine("Part1: {0}", result);
    }

    public void Part2()
    {
        int result = LoopChanges(1_000_000_000);

        Console.WriteLine("Part2: {0}", result);
    }

    private int LoopChanges(int loops)
    {
        LoadData();

        int result;
        int mapIndex = 0; // toggles between 0 & 1
        Dictionary<string, int> keys = new();
        bool dupFound = false;

        for (int i = 0; i < loops; i++)
        {
            string hashkey = ChangeState(mapIndex, (mapIndex + 1) % 2);

            if (!dupFound)
            {
                if (keys.ContainsKey(hashkey))
                {
                    // we've found a duplicate - find the interval & skip a massive amount of looping
                    int anchor = keys[hashkey];
                    int interval = i - anchor;
                    int skip = (loops - i) / interval;
                    i = skip * interval + i;
                    dupFound = true;
                }
                else
                {
                    keys[hashkey] = i;
                }
            }

            mapIndex = (mapIndex + 1) % 2;
        }

        int wooded = 0;
        int lyards = 0;

        for (int row = 0; row < Size; row++)
        {
            for (int col = 0; col < Size; col++)
            {
                char c = _maps[mapIndex, row, col];
                wooded += c == '|' ? 1 : 0;
                lyards += c == '#' ? 1 : 0;
            }
        }

        result = wooded * lyards;

        return result;
    }

    private string ChangeState(int src, int dst)
    {
        for (int row = 0; row < Size; row++)
        {
            for (int col = 0; col < Size; col++)
            {
                int openCount;
                int treeCount;
                int lyardCount;

                (openCount, treeCount, lyardCount) = CountAdjacent(src, row, col);
                switch (_maps[src, row, col])
                {
                    case '.':
                        _maps[dst, row, col] = treeCount >= 3 ? '|' : '.';
                        break;
                    case '|':
                        _maps[dst, row, col] = lyardCount >= 3 ? '#' : '|';
                        break;
                    case '#':
                        _maps[dst, row, col] = (lyardCount >= 1 && treeCount >= 1) ? '#' : '.'; 
                        break;
                }
            }
        }

        return MakeHashKey(dst);
    }

    private string MakeHashKey(int mapIndex)
    {
        // hash key is a string of the entire matrix
        StringBuilder sb = new StringBuilder();

        for (int row = 0; (row < Size); row++)
        {
            for (int col =0; col < Size; col++)
            {
                sb.Append(_maps[mapIndex, row, col]);
            }
        }

        return sb.ToString();
    }

    private void DumpMap(int mapIndex)
    {
        for (int row = 0; row < Size; row++)
        {
            for (int col = 0;  col < Size; col++)
            {
                Console.Write(_maps[mapIndex, row, col]);
            }
            Console.WriteLine();
        }
        Console.WriteLine();
    }

    public int Size { get; private set; }

    private (int, int, int) CountAdjacent(int map, int row, int col)
    {
        int openCount = 0;
        int treeCount = 0;
        int lyardCount = 0;

        foreach (int r in new int[] { -1, 0, 1 })
        {
             foreach (int c in new int[] { -1, 0, 1 })
             {
                int rtemp = row + r;
                int ctemp = col + c;

                if ((rtemp != row || ctemp != col) &&
                    rtemp >= 0 && rtemp < Size &&
                    ctemp >= 0 && ctemp < Size)
                {
                    char chr = _maps[map, rtemp, ctemp];
                    openCount += chr == '.' ? 1 : 0;
                    treeCount += chr == '|' ? 1 : 0;
                    lyardCount += chr == '#' ? 1 : 0;
                }
             }
        }

        return (openCount, treeCount, lyardCount);
    }

    private void LoadData()
    {
        string inputFile = AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\input.txt";

        if (File.Exists(inputFile))
        {
            string[] lines = File.ReadAllLines(inputFile);
            Size = lines.Length;
            _maps = new char[2, Size, Size];
            // _maps is an array(2) of char[,].
            for (int row = 0; row < Size;  row++)
            {
                for (int col  = 0; col < Size; col++)
                {
                    _maps[0, row, col] = lines[row][col];
                    _maps[1, row, col] = '?';
                }
            }
        }

    }

}
