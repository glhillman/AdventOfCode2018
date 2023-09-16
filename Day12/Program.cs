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

internal class Pot
{
    public Pot(int num, bool hasPlant)
    {
        Num = num;
        HasPlant = hasPlant;
        TimesWithPlant = hasPlant ? 1 : 0;
        Next = null;
        Prev = null;
    }

    public Pot InsertBefore(Pot newPot)
    {
        this.Prev = newPot;
        newPot.Next = this;
        return newPot;
    }

    public Pot InsertAfter(Pot newPot)
    {
        this.Next = newPot;
        newPot.Prev = this;
        return newPot;
    }

    public void SetPlant(bool hasPlant)
    {
        if (HasPlant == false && hasPlant == true)
        {
            TimesWithPlant++;
        }
        HasPlant = hasPlant;
    }

    public int Num { get; private set; }
    public bool HasPlant { get; set; }
    public int TimesWithPlant { get; set; }
    public Pot? Next { get; set; }
    public Pot? Prev { get; set; }

    public override string ToString()
    {
        return string.Format("Num: {0}, HasPlant: {1}, TimesWithPlant: {2}", Num, HasPlant, TimesWithPlant);
    }
}
internal class DayClass
{
    string _start;
    Dictionary<string, string> _patterns = new();
    public DayClass()
    {
        LoadData();
    }

    public void PartX()
    {
        Pot firstPot = new Pot(0, _start[0] == '#');
        Pot lastPot = firstPot;
        for (int num = 1; num < _start.Length; num++)
        {
            Pot pot = new Pot(num, _start[num] == '#');
            lastPot = lastPot.InsertAfter(pot);
        }

        DumpPots(firstPot);
    }

    public void DumpPots(Pot firstPot)
    {
        do
        {
            Console.Write("{0}", firstPot.HasPlant ? '#' : '.');
            firstPot = firstPot.Next;
        } while (firstPot != null);
    }

    public void Part1()
    {

        string input = _start;
        //Console.WriteLine(input);
        StringBuilder sb = new();
        int offset = 1;
        for (int i = 0; i < 20; i++)
        {
            input = "..." + input + "....";
            sb.Clear();
            for (int x = 0; x < input.Length-5; x++)
            {
                string sub = input.Substring(x, 5);
                if (_patterns.ContainsKey(sub))
                {
                    sb.Append(_patterns[sub]);
                }
                else
                {
                    sb.Append(".");
                }
            }
            input = sb.ToString();
            DumpLine(input, offset);
            offset++;
        }
        offset--;
        int sum = 0;

        for (int i = 0; i < input.Length; i++)
        {
            sum += input[i] == '#' ? i - offset : 0;
        }

        Console.WriteLine("Part1: {0}", sum);
    }

    private void DumpLine(string line, int offset)
    {
        Console.WriteLine(line);

        for (int i = 0; i < line.Length;i++)
        {
            Console.Write(i - offset == 0 ? '^' : ' ');
        }
        Console.WriteLine();
    }

    public void Part2()
    {

        long rslt = 0;

        Console.WriteLine("Part2: {0}", rslt);
    }

    private void LoadData()
    {
        string inputFile = AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\input.txt";

        if (File.Exists(inputFile))
        {
            string? line;
            StreamReader file = new StreamReader(inputFile);
            line = file.ReadLine();
            _start = line.Substring(15);
            file.ReadLine();
            while ((line = file.ReadLine()) != null)
            {
                string[] parts = line.Split(" => ");
                _patterns[parts[0]] = parts[1];
            }

            file.Close();
        }
    }

}
