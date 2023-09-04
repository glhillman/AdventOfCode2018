// See https://aka.ms/new-console-template for more information
using System.Reflection.PortableExecutable;

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
    List<string> _wordList = new();

    public DayClass()
    {
        LoadData();
    }

    public void Part1()
    {
        int doubles = 0;
        int triples = 0;

        foreach (string word in _wordList)
        {
            int doubleCounter = 1;
            int tripleCounter = 1;

            var groups = word.GroupBy(x => x);
            foreach (var group in groups)
            {
                if (group.Count() == 2)
                {
                    doubles += doubleCounter;
                    doubleCounter = 0;
                }
                else if (group.Count() == 3)
                {
                    triples += tripleCounter;
                    tripleCounter = 0;
                }
            }
        }


        long rslt = doubles * triples;

        Console.WriteLine("Part1: {0}", rslt);
    }

    public void Part2()
    {
        string answer = string.Empty;

        for (int i = 0; i < _wordList.Count-1; i++)
        {
            for (int j = i+1; j < _wordList.Count; j++)
            {
                int offset = DiffBy(_wordList[i], _wordList[j]);
                if (offset >= 0 )
                {
                    answer = _wordList[i].Remove(offset, 1);
                    i = _wordList.Count();
                    break;
                }
            }
        }        

        Console.WriteLine("Part2: {0}", answer);
    }

    private int DiffBy(string word1, string word2)
    {
        int diffCount = 0;
        int offset = -1;

        for (int i = 0; i <word1.Length; i++)
        {
            if (word1[i] != word2[i])
            {
                diffCount++;
                offset = i;
            }
        }

        return diffCount == 1 ? offset : -1;
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
                _wordList.Add(line);
            }

            file.Close();
        }
    }

}
