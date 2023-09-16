// See https://aka.ms/new-console-template for more information
using System.Runtime.CompilerServices;

DayClass day = new DayClass();

var watch = new System.Diagnostics.Stopwatch();
watch.Start();

day.Part1();
day.Part2();

watch.Stop();
Console.WriteLine("Execution Time: {0} ms", watch.ElapsedMilliseconds);

Console.Write("Press Enter to continue...");
Console.ReadLine();

internal enum Directions
{
    Up,
    Down,
    Left,
    Right,
    None
};

internal class Cart
{
    public Cart(int id, int x, int y, char direction)
    {
        ID = id;
        X = x;
        Y = y;
        Direction = direction;
        Moved = false;
    }

    public int ID {  get; private set; }
    public int X {  get; set; }
    public int Y { get; set; }
    public bool Moved { get; set; }

    public void Move(Dictionary<(int x, int y), char> track)
    {
        int tempX = X;
        int tempY = Y;

        switch (Direction)
        {
            case '>':
                X++;
                switch (track[(X,Y)])
                {
                    case '\\':
                        Direction = 'v';
                        break;
                    case '/':
                        Direction = '^';
                        break;
                    case '+':
                        switch (Crossroad())
                        {
                            case Directions.Left:
                                Direction = '^';
                                break;
                            case Directions.Right:
                                Direction = 'v';
                                break;
                            default:
                                break;
                        }
                        break;
                }
                break;
            case '<':
                X--;
                switch (track[(X,Y)])
                {
                    case '\\':
                        Direction = '^';
                        break;
                    case '/':
                        Direction = 'v';
                        break;
                    case '+':
                        switch (Crossroad())
                        {
                            case Directions.Left:
                                Direction = 'v';
                                break;
                            case Directions.Right:
                                Direction = '^';
                                break;
                            default:
                                break;
                        }
                        break;
                }
                break;
            case '^':
                Y--;
                switch (track[(X, Y)])
                {
                    case '\\':
                        Direction = '<';
                        break;
                    case '/':
                        Direction = '>';
                        break;
                    case '+':
                        switch (Crossroad())
                        {
                            case Directions.Left:
                                Direction = '<';
                                break;
                            case Directions.Right:
                                Direction = '>';
                                break;
                            default:
                                break;
                        }
                        break;
                }
                break;
            case 'v':
                Y++;
                switch (track[(X, Y)])
                {
                    case '\\':
                        Direction = '>';
                        break;
                    case '/':
                        Direction = '<';
                        break;
                    case '+':
                        switch (Crossroad())
                        {
                            case Directions.Left:
                                Direction = '>';
                                break;
                            case Directions.Right:
                                Direction = '<';
                                break;
                            default:
                                break;
                        }
                        break;
                }
                break;
            default:
                break;

        }
        Moved = true;
    }

    public char Direction { get; set; }
    private int TurnCount { get; set; }
    public Directions Crossroad()
    {
        Directions turnDirection;
        switch (TurnCount % 3)
        {
            case 0: turnDirection = Directions.Left; break;
            case 1: turnDirection = Directions.None; break; // go straight
            default: turnDirection = Directions.Right; break; // case 2: 
        }

        TurnCount++;

        return turnDirection;
    }

    public override string ToString()
    {
        return string.Format("ID: {0}, Pos: {1},{2}, Direction: {3}", ID, X, Y, Direction);
    }
}
internal class DayClass
{
    Dictionary<(int x, int y), char> _track;
    List<Cart> _carts;

    public DayClass()
    {
    }

    public void Part1()
    {
        LoadData();
        bool collision = false;
        List<Cart> carts = _carts.OrderBy(c => c.Y).ThenBy(c => c.X).ToList();
        int crashX = 0;
        int crashY = 0;

        while (collision == false)
        {
            foreach (Cart crt in carts)
            {
                crt.Move(_track);
                collision = carts.Count(c => c.X == crt.X && c.Y == crt.Y) > 1;
                if (collision)
                {
                    crashX = crt.X;
                    crashY = crt.Y;
                    break;
                }
            }
            carts = carts.OrderBy(c => c.Y).ThenBy(c => c.X).ToList();
        }

        Console.WriteLine("Part1: {0},{1}", crashX, crashY);
    }

    public void Part2()
    {
        LoadData();
        List<Cart> carts = _carts.OrderBy(c => c.Y).ThenBy(c => c.X).ToList();
        while (carts.Count > 1)
        {
            foreach (Cart c in carts)
            {
                c.Moved = false;
            }
            int count = carts.Count;
            for (int i = 0; i < count; i++)
            {
                Cart crt = carts[i];
                if (crt.Moved == false)
                {
                    crt.Move(_track);
                    if (carts.Count(c => c.X == crt.X && c.Y == crt.Y) > 1)
                    {
                        int index = 0;
                        int anchor = int.MaxValue;
                        while (index < carts.Count)
                        {
                            if (carts[index].X == crt.X && carts[index].Y == crt.Y)
                            {
                                anchor = Math.Min(anchor, index);
                                carts.RemoveAt(index);
                                count--;
                            }
                            else
                            {
                                index++;
                            }
                        }
                        i = anchor - 1;
                    }
                }
            }
            carts = carts.OrderBy(c => c.Y).ThenBy(c => c.X).ToList();
        }

        Console.WriteLine("Part2: {0},{1}", carts[0].X, carts[0].Y);
    }

    private void LoadData()
    {
        _carts = new();
        _track = new();
        int cartNum = 0;
        string inputFile = AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\input.txt";

        if (File.Exists(inputFile))
        {
            string? line;
            StreamReader file = new StreamReader(inputFile);
            int y = 0;
            while ((line = file.ReadLine()) != null)
            {
                for (int x = 0; x < line.Length; x++)
                {
                    char? c = line[x];
                    switch (c.Value)
                    {
                        case '>':
                        case '<':
                            _carts.Add(new Cart(cartNum++, x, y, c.Value));
                            c = '-';
                            break;
                        case '^':
                        case 'v':
                            _carts.Add(new Cart(cartNum++, x, y, c.Value));
                            c = '|';
                            break;
                        case ' ':
                            c = null;
                            break;
                        default:
                            break;

                    }
                    if (c != null)
                    {
                        _track[(x, y)] = c.Value;
                    }
                }
                y++;
            }

            file.Close();
        }
    }

}
