// See https://aka.ms/new-console-template for more information
using Day07;
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
    Dictionary<char, List<char>> _steps;
    public DayClass()
    {
    }

    public void Part1()
    {
        LoadData();

        List<char> available = new List<char>();

        while (_steps.Count > 0)
        {
            // find avaialble steps (they have no prerequesites)
            var availableSteps = _steps.Where(x => x.Value.Count == 0).ToList();
            var topStep = availableSteps.Min(x => x.Key);
            available.Add(topStep);
            _steps.Remove(topStep);
            foreach (var step in _steps)
            {
                if (step.Value.Contains(topStep))
                {
                    step.Value.Remove(topStep);
                }
            }
        }

        StringBuilder sb = new();
        foreach (char c in available)
        {
            sb.Append(c);
        }

        Console.WriteLine("Part1: {0}", sb.ToString());
    }

    public void Part2()
    {
        LoadData();

        int nWorkers = 5;
        int offset = 60;
        List<Worker> workers = new List<Worker>();
        int totalSeconds = 0;

        for (int i = 0; i < nWorkers; i++)
        {
            workers.Add(new Worker(i, offset));
        }

        while (_steps.Count > 0 || workers.Where(w => w.StepID != null).Count() > 0)
        {
            // find available workers (idle)
            var availWorkers = workers.Where(w => w.StepID == null).ToList();
            if (availWorkers.Count > 0)
            {
                var availableSteps = _steps.Where(x => x.Value.Count == 0).ToList();
                if (availableSteps.Count > 0)
                {
                    availableSteps.Sort((x, y) => x.Key.CompareTo(y.Key));
                    int i = 0;
                    foreach (var worker in availWorkers)
                    {
                        if (i < availableSteps.Count)
                        {
                            worker.StepID = availableSteps[i].Key;
                            _steps.Remove(availableSteps[i].Key);
                            i++;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
            totalSeconds++;
            foreach (var worker in workers)
            {
                char? completedStep = worker.Tick();
                if (completedStep != null)
                {
                    foreach (var step in _steps)
                    {
                        if (step.Value.Contains(completedStep.Value))
                        {
                            step.Value.Remove(completedStep.Value);
                        }
                    }

                }
            }
        }

        Console.WriteLine("Part2: {0}", totalSeconds);
    }

    private void LoadData()
    {
        _steps = new();

        string inputFile = AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\input.txt";

        if (File.Exists(inputFile))
        {
            string? line;
            StreamReader file = new StreamReader(inputFile);
            while ((line = file.ReadLine()) != null)
            {
                string[] parts = line.Split(' ');
                char id = parts[7][0];
                char prereq = parts[1][0];
                if (_steps.ContainsKey(prereq) == false)
                {
                    _steps[prereq] = new List<char>();
                }
                if (_steps.ContainsKey(id))
                {
                    _steps[id].Add(prereq);
                }
                else
                {
                    _steps[id] = new List<char>();
                    _steps[id].Add(prereq);
                }
            }

            file.Close();
        }
    }

}
