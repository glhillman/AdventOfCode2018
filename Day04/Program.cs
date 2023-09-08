// See https://aka.ms/new-console-template for more information
using System;
using System.Diagnostics;

DayClass day = new DayClass();

var watch = new System.Diagnostics.Stopwatch();
watch.Start();

day.Part1_2();

watch.Stop();
Console.WriteLine("Execution Time: {0} ms", watch.ElapsedMilliseconds);

Console.Write("Press Enter to continue...");
Console.ReadLine();

public enum RecType
{
    GuardStart,
    Asleep,
    Awake
};

internal class GuardSleepClass
{
    public GuardSleepClass(int guard)
    {
        Guard = guard;
        MinutesAsleep = new int[60];
        SumAsleep = 0;
    }

    public void MarkAsleep(int minuteStart, int minuteEnd)
    {
        for (int i = minuteStart; i < minuteEnd; i++)
        {
            MinutesAsleep[i]++;
            SumAsleep++;
        }
    }

    public int MaxAsleep
    {
        get { return MinutesAsleep.Max(); }
    }

    public int Guard { get; set; }

    public int[] MinutesAsleep;
    public int SumAsleep { get; set; }
    public int MaxMinute
    {
        get
        {
            int maxAsleep = MaxAsleep;
            int i = 0;
            while (MinutesAsleep[i] != maxAsleep)
            {
                i++;
            }
            return i;
        }
    }
}
internal class RecClass
{
    public RecClass(string year, string month, string day, string hour, string minute, int guard, RecType rectype)
    {
        Year = int.Parse(year);
        Month = int.Parse(month);
        Day = int.Parse(day);
        Hour = int.Parse(hour);
        Minute = int.Parse(minute);
        Guard = guard;
        Rectype = rectype;
        SortKey = string.Format("{0}-{1}-{2}-{3}", month, day, hour, minute);
    }

    public int Year { get; set; }
    public int Month { get; set; }
    public int Day { get; set; }
    public int Hour { get; set; }
    public int Minute { get; set; }
    public RecType Rectype { get; set; }
    public int Guard { get; set; }
    public string SortKey { get; set; }
    public string Visual {  get; set; }
    public override string ToString()
    {
        return string.Format("{0}-{1:D2}-{2:D2} {3:D2}:{4:D2} {5} {6}", Year, Month, Day, Hour, Minute, Guard, Rectype);
    }
}
internal class DayClass
{
    List<RecClass> _recs = new();

    public DayClass()
    {
        LoadData();
        _recs.Sort((x,y) => x.SortKey.CompareTo(y.SortKey));
        ResolveGuards();
    }

    public void Part1_2()
    {
        List<GuardSleepClass> sleepyGuards = new();
        GuardSleepClass sleepyGuard;
        var guardGroup = _recs.GroupBy(x => x.Guard);
        foreach ( var guard in guardGroup)
        {
            sleepyGuard = new GuardSleepClass(guard.Key); 
            int startSleep = -1;
            int stopSleep = -1;

            foreach (RecClass rec in guard)
            {
                switch (rec.Rectype)
                {
                    case RecType.GuardStart:
                        break;
                    case RecType.Asleep:
                        startSleep = rec.Minute;
                        break;
                    case RecType.Awake:
                        stopSleep = rec.Minute;
                        sleepyGuard.MarkAsleep(startSleep, stopSleep);
                        break;
                }
            }

            sleepyGuards.Add(sleepyGuard);

        }

        int max = sleepyGuards.Max(x => x.SumAsleep);
        sleepyGuard = sleepyGuards.First(x => x.SumAsleep == max);
        int rslt1 = sleepyGuard.Guard * sleepyGuard.MaxMinute;

        max = sleepyGuards.Max(x => x.MaxAsleep);
        sleepyGuard = sleepyGuards.First(x => x.MaxAsleep == max);
        int rslt2 = sleepyGuard.Guard * sleepyGuard.MaxMinute;

        Console.WriteLine("Part1: {0}", rslt1);
        Console.WriteLine("Part2: {0}", rslt2);
    }

    private void ResolveGuards()
    {
        int guard = -1;

        foreach (var rec in _recs)
        {
            if (rec.Rectype == RecType.GuardStart)
            {
                guard = rec.Guard;
            }
            else
            {
                rec.Guard = guard;
            }
        }
    }

    private void LoadData()
    {
        string inputFile = AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\input.txt";

        if (File.Exists(inputFile))
        {
            int guard = -1;
            RecType recType = RecType.GuardStart;

            string? line;
            StreamReader file = new StreamReader(inputFile);
            while ((line = file.ReadLine()) != null)
            {
                string[] parts = line.Split('[', ']', ' ', '#','-',':');
                if (parts[7] == "Guard")
                {
                    guard = int.Parse(parts[9]);
                    recType = RecType.GuardStart;
                }
                else
                {
                    guard = -1;
                    recType = parts[7] == "falls" ? RecType.Asleep : RecType.Awake;
                }
                _recs.Add(new RecClass(parts[1], parts[2], parts[3], parts[4], parts[5], guard, recType));
            }

            file.Close();
        }
    }

}
