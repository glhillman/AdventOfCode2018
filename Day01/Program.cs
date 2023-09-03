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

internal class DayClass
{
    List<int> _allInts = new();

    public DayClass()
    {
        LoadData();
    }

    public void Part1()
    {

        long rslt = 0;
        foreach (int i in _allInts)
        {
            rslt += i;
        }

        Console.WriteLine("Part1: {0}", rslt);
    }

    public void Part2()
    {
        HashSet<int> freqs = new();
        
        int rslt = 0;
        int index = 0;
        int intCount = _allInts.Count;

        while (true)
        {
            rslt += _allInts[index % intCount];
            if (freqs.Contains(rslt))
            {
                break;
            }
            else
            {
                index++;
                freqs.Add(rslt);
            }
        }

        Console.WriteLine("Part2: {0}", rslt);
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
                _allInts.Add(int.Parse(line));
            }

            file.Close();
        }
    }

}
