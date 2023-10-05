// See https://aka.ms/new-console-template for more information
using System.Text.RegularExpressions;

DayClass day = new DayClass();

var watch = new System.Diagnostics.Stopwatch();
watch.Start();

day.Part1();
day.Part2();

watch.Stop();
Console.WriteLine("Execution Time: {0} ms", watch.ElapsedMilliseconds);

Console.Write("Press Enter to continue...");
Console.ReadLine();

public enum OpCodes
{
    addr,
    addi,
    mulr,
    muli,
    banr,
    bani,
    borr,
    bori,
    setr,
    seti,
    gtir,
    gtri,
    gtrr,
    eqir,
    eqri,
    eqrr
};

public class Instruction
{
    public Instruction(string opcode, int a, int b, int c)
    {
        OpCode = (OpCodes)Enum.Parse(typeof(OpCodes), opcode);
        A = a;
        B = b;
        C = c;
    }

    public OpCodes OpCode { get; private set; }
    public int A { get; private set; }
    public int B { get; private set; }
    public int C { get; private set; }
    public override string ToString()
    {
        return string.Format("{0} {1} {2} {3}", OpCode, A, B, C);
    }
}

    internal class DayClass
{
    List<Instruction> _instructions = new();
    public DayClass()
    {
        LoadData();
        IP = 0;
        Regs = new int[6];
    }

    public void Part1()
    {
        while (IP < _instructions.Count)
        {
            Step();
            IP++;
        }

        Console.WriteLine("Part1: {0}", Regs[0]);
    }

    public void Part2()
    {
        IP = 0;
        Regs[0] = 1;

        for (int i = 1; i < Regs.Length; i++)
        {
            Regs[i] = 0;
        }

        // code is summing the factors of a very large number
        // run 1000 cyles, find the largest number in the registers and sum it's factors
        for (int i = 0; i < 1000; i++)
        {
            Step();
            IP++;
        }

        int maxReg = Regs.Max();
        
        // now factor & sum 
        int sum = 0;
        for (int i = 1; i <= maxReg + 1; i++)
        {
            if (maxReg % i == 0)
            {
                sum += i;
            }
        }

        Console.WriteLine("Part2: {0}", sum);
    }

    public int IPReg { get; set; } = 0;
    public int IP { get; set; }
    public int[] Regs { get; set; }
    private void Step()
    {
        Regs[IPReg] = IP;
        int a = _instructions[IP].A;
        int b = _instructions[IP].B;
        int c = _instructions[IP].C;

        switch (_instructions[IP].OpCode)
        {
            case OpCodes.addr: Regs[c] = Regs[a] + Regs[b]; break;
            case OpCodes.addi: Regs[c] = Regs[a] + b; break;
            case OpCodes.mulr: Regs[c] = Regs[a] * Regs[b]; break;
            case OpCodes.muli: Regs[c] = Regs[a] * b; break;
            case OpCodes.banr: Regs[c] = Regs[a] & Regs[b]; break;
            case OpCodes.bani: Regs[c] = Regs[a] & b; break;
            case OpCodes.borr: Regs[c] = Regs[a] | Regs[b]; break;
            case OpCodes.bori: Regs[c] = Regs[a] | b; break;
            case OpCodes.setr: Regs[c] = Regs[a]; break;
            case OpCodes.seti: Regs[c] = a; break;
            case OpCodes.gtir: Regs[c] = a > Regs[b] ? 1 : 0; break;
            case OpCodes.gtri: Regs[c] = Regs[a] > b ? 1 : 0; break;
            case OpCodes.gtrr: Regs[c] = Regs[a] > Regs[b] ? 1 : 0; break;
            case OpCodes.eqir: Regs[c] = a == Regs[b] ? 1 : 0; break;
            case OpCodes.eqri: Regs[c] = Regs[a] == b ? 1 : 0; break;
            case OpCodes.eqrr: Regs[c] = Regs[a] == Regs[b] ? 1 : 0; break;
        }

        IP = Regs[IPReg];
    }

    private void LoadData()
    {
        string inputFile = AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\input.txt";

        if (File.Exists(inputFile))
        {
            string pattern = @"(\S+) (\d+) (\d+) (\d+)";
            string? line;
            StreamReader file = new StreamReader(inputFile);
            line = file.ReadLine();
            IPReg = int.Parse(line.Substring(4));
            while ((line = file.ReadLine()) != null)
            {
                Match match = Regex.Match(line, pattern);
                _instructions.Add(new Instruction(match.Groups[1].Value,
                                                  int.Parse(match.Groups[2].Value),
                                                  int.Parse(match.Groups[3].Value),
                                                  int.Parse(match.Groups[4].Value)));
                    
            }

            file.Close();
        }
    }

}

