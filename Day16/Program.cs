// See https://aka.ms/new-console-template for more information
using System.Security.Cryptography.X509Certificates;
using System.Text;
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

public class MatchingInstructions
{
    public MatchingInstructions(int[] before, int[] after, int[] instruction)
    {
        Before = new int[before.Length];
        before.CopyTo(Before, 0);
        After = new int[after.Length];
        after.CopyTo(After, 0);
        Instruction = new int[instruction.Length];
        instruction.CopyTo(Instruction, 0);
        MatchingOpCodes = new();
    }

    public int[] Before { get; set; }
    public int[] After { get; set; }
    public int[] Instruction { get; set; }
    public List<OpCodes> MatchingOpCodes { get; set; }

    public override string ToString()
    {
        string before = DumpReg(Before, true);
        string inst = DumpReg(Instruction, false);
        string after = DumpReg(After, true);
        StringBuilder sb = new();
        foreach (OpCodes opCode in MatchingOpCodes)
        {
            sb.Append(opCode.ToString());
            sb.Append(" ");
        }

        return string.Format("Before: {0}, Inst: {1}, After: {2}, Match: {3}", before, inst, after, sb.ToString());
    }

    public string DumpReg(int[] reg, bool showBrackets)
    {
        StringBuilder sb = new StringBuilder();
        if (showBrackets)
        {
            sb.Append('[');
            for (int i = 0; i < reg.Length; i++)
            {
                sb.Append(reg[i]);
                if (i < reg.Length - 1)
                {
                    sb.Append(", ");
                }
            }
            sb.Append(']');
        }
        else
        {
            for (int i = 0; i < reg.Length; i++)
            {
                sb.Append(reg[i]);
                sb.Append(' ');
            }
        }
        return sb.ToString();
    }
}
internal class DayClass
{
    List<MatchingInstructions> _instructions = new();
    public DayClass()
    {
        LoadData();
    }

    public void Part1()
    {
        int[] reg = new int[4];
        foreach (MatchingInstructions instr in _instructions)
        {
            foreach (OpCodes opCode in Enum.GetValues(typeof(OpCodes)))
            {
                instr.Before.CopyTo(reg, 0);
                Execute(opCode, instr.Instruction, reg);
                if (reg[0] == instr.After[0] && reg[1] == instr.After[1] && reg[2] == instr.After[2] && reg[3] == instr.After[3])
                {
                    instr.MatchingOpCodes.Add(opCode);
                }
            }
        }

        int threeOrMore = _instructions.Count(i => i.MatchingOpCodes.Count >= 3);

        Console.WriteLine("Part1: {0}", threeOrMore);
    }

    public void Part2()
    {
        OpCodes?[] codeMap = new OpCodes?[16];
        for (int i = 0; i < codeMap.Length; i++)
        {
            codeMap[i] = null;
        }
        while (codeMap.Contains(null))
        {
            var _singles = _instructions.Where(i => i.MatchingOpCodes.Count == 1);
            foreach (MatchingInstructions inst in _singles)
            {
                OpCodes opCode = inst.MatchingOpCodes[0];
                if (codeMap[inst.Instruction[0]] == null)
                {
                    codeMap[inst.Instruction[0]] = opCode;
                    foreach (MatchingInstructions match in _instructions)
                    {
                        match.MatchingOpCodes.Remove(opCode);
                    }
                }
            }
        }
        List<int[]> codes = new();

        LoadInput2(codes);

        int[] reg = new int[4];

        foreach (int[] code in codes)
        {
            Execute(codeMap[code[0]].Value, code, reg);
        }

        Console.WriteLine("Part2: {0}", reg[0]);
    }

    private void Execute(OpCodes op, int[] inst, int[] reg)
    {
        int a = inst[1];
        int b = inst[2];
        int c = inst[3];

        switch (op)
        {
            case OpCodes.addr: reg[c] = reg[a] + reg[b]; break;
            case OpCodes.addi: reg[c] = reg[a] + b; break;
            case OpCodes.mulr: reg[c] = reg[a] * reg[b]; break;
            case OpCodes.muli: reg[c] = reg[a] * b; break;
            case OpCodes.banr: reg[c] = reg[a] & reg[b]; break;
            case OpCodes.bani: reg[c] = reg[a] & b; break;
            case OpCodes.borr: reg[c] = reg[a] | reg[b]; break;
            case OpCodes.bori: reg[c] = reg[a] | b; break;
            case OpCodes.setr: reg[c] = reg[a]; break;
            case OpCodes.seti: reg[c] = a; break;
            case OpCodes.gtir: reg[c] = a > reg[b] ? 1 : 0; break;
            case OpCodes.gtri: reg[c] = reg[a] > b ? 1 : 0; break;
            case OpCodes.gtrr: reg[c] = reg[a] > reg[b] ? 1 : 0; break;
            case OpCodes.eqir: reg[c] = a == reg[b] ? 1 : 0; break;
            case OpCodes.eqri: reg[c] = reg[a] == b ? 1 : 0; break;
            case OpCodes.eqrr: reg[c] = reg[a] == reg[b] ? 1 : 0; break;
        }
    }
    private void LoadData()
    {
        string inputFile = AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\input1.txt";

        if (File.Exists(inputFile))
        {
            string? line;
            StreamReader file = new StreamReader(inputFile);
            while ((line = file.ReadLine()) != null)
            {
                if (line != null)
                {
                    string[] parts = line.Split(':', '[', ',', ']', ' ');
                    int[] before = new int[] { int.Parse(parts[3]), int.Parse(parts[5]), int.Parse(parts[7]), int.Parse(parts[9]) };
                    parts = file.ReadLine().Split(' ');
                    int[] instruction = new int[] { int.Parse(parts[0]), int.Parse(parts[1]), int.Parse(parts[2]), int.Parse(parts[3]) };
                    parts = file.ReadLine().Split(':', '[', ',', ']', ' ');
                    int[] after = new int[] { int.Parse(parts[4]), int.Parse(parts[6]), int.Parse(parts[8]), int.Parse(parts[10]) };
                    _instructions.Add(new MatchingInstructions(before, after, instruction));
                    line = file.ReadLine();
                }
            }

            file.Close();
        }
    }

    private void LoadInput2(List<int[]> codes)
    {
        string inputFile = AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\input2.txt";

        if (File.Exists(inputFile))
        {
            string? line;
            StreamReader file = new StreamReader(inputFile);
            while ((line = file.ReadLine()) != null)
            {
                if (line != null)
                {
                    string[] parts = line.Split(' ');
                    int[] code = new int[4];
                    code[0] = int.Parse(parts[0]);
                    code[1] = int.Parse(parts[1]);
                    code[2] = int.Parse(parts[2]);
                    code[3] = int.Parse(parts[3]);
                    codes.Add(code);
                }
            }

            file.Close();
        }
    }
}
