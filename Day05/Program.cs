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
    string _data;
    Dictionary<char, char> _map;

    public DayClass()
    {
        LoadData();

        _map = new();
        char lower = 'a';
        char upper = 'A';

        while (lower <= 'z')
        {
            _map[lower] = upper;
            _map[upper] = lower;
            lower++;
            upper++;
        }
    }

    public void Part1()
    {
        long rslt = React(_data);

        Console.WriteLine("Part1: {0}", rslt);
    }

    public void Part2()
    {
        int len = int.MaxValue;

        foreach (char c in _map.Keys)
        {
            if (char.IsLower(c))
            {
                char cUpper = _map[c];
                StringBuilder sb = new StringBuilder(_data);
                sb.Replace(c.ToString(), "");
                sb.Replace(cUpper.ToString(), "");
                len = Math.Min(len, React(sb.ToString()));
            }
        }

        Console.WriteLine("Part2: {0}", len);
    }

    private int React(string input)
    {
        StringBuilder sb = new StringBuilder(input);

        int index = 0;
        while (index < sb.Length - 1)
        {
            if (sb[index + 1] == _map[sb[index]])
            {
                sb.Remove(index, 2);
                if (index > 0)
                {
                    index--;
                }
            }
            else
            {
                index++;
            }
        }

        return sb.Length;
    }
    private void LoadData()
    {
        string inputFile = AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\input.txt";

        if (File.Exists(inputFile))
        {
            StreamReader file = new StreamReader(inputFile);
            _data = file.ReadLine();
            file.Close();
        }
    }

}
