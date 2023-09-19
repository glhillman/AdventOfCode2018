// See https://aka.ms/new-console-template for more information
using System.Diagnostics.CodeAnalysis;
using System.Net.Mail;

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

    public DayClass()
    {
    }

    public void Part1()
    {
        int target = 633601 + 10;
        List<int> scores = new() { 3, 7 };
        int elf1 = 0;
        int elf2 = 1;
        //DumpScores(scores, elf1, elf2);

        while (scores.Count < target)
        {
            int sum = scores[elf1] + scores[elf2];
            if (sum >= 10)
            {
                scores.Add(sum / 10);
            }
            scores.Add(sum % 10);
            elf1 = (elf1 + scores[elf1] + 1) % scores.Count;
            elf2 = (elf2 + scores[elf2] + 1) % scores.Count;
            //DumpScores(scores, elf1, elf2);
        }

        long sumScore = 0;
        for (int i = target - 10; i < target; i++)
        {
            sumScore *= 10;
            sumScore += scores[i];
        }

        Console.WriteLine("Part1: {0}", sumScore);
    }


    public void Part2()
    {
        int targetNum = 633601;
        List<int> scores = new() { 3, 7 };
        int elf1 = 0;
        int elf2 = 1;
        Stack<int> stack = new();
        List<int> target = new();
        bool isFound = false;
        int temp;

        while (targetNum > 0)
        {
            stack.Push(targetNum % 10);
            targetNum /= 10;
        }
        while (stack.Count > 0)
        {
            target.Add(stack.Pop());
        }
        while (!isFound)
        { 
            int sum = scores[elf1] + scores[elf2];
            if (sum >= 10)
            {
                temp = sum / 10;
                scores.Add(sum / 10);
                isFound = Filter(temp, scores, target);
            }
            if (!isFound)
            {
                temp = sum % 10;
                scores.Add(temp);
                isFound = Filter(temp, scores, target);
                elf1 = (elf1 + scores[elf1] + 1) % scores.Count;
                elf2 = (elf2 + scores[elf2] + 1) % scores.Count;
            }
        }

        int prevSize = scores.Count - target.Count;

        Console.WriteLine("Part2: {0}", prevSize);
    }

    private bool Filter(int value, List<int> scores, List<int> target)
    {
        bool isFound = true;

        if (value == target[target.Count - 1] && scores.Count > target.Count)
        {
            int targetCount = target.Count;
            // last char in target matches. 
            int scoresIndex = scores.Count - targetCount;
            for (int i = 0; i < targetCount; i++)
            {
                if (scores[scoresIndex] != target[i])
                {
                    isFound = false;
                    break;
                }
                scoresIndex++;
            }
        }
        else
        {
            isFound = false;
        }

        return isFound;
    }
    private void DumpScores(List<int> scores, int elf1, int elf2)
    {
        for (int i = 0; i < scores.Count; i++)
        {
            if (i == elf1)
            {
                Console.Write("({0})", scores[i]);
            }
            else if (i == elf2)
            {
                Console.Write("[{0}]", scores[i]);
            }
            else
            {
                Console.Write(" {0} ", scores[i]);
            }
        }
        Console.WriteLine();
    }
}
