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

public class Marble
{
    public Marble(int id)
    {
        ID = id;
        Next = this;
        Prev = this;
    }

    public int ID { get; private set; }
    public Marble Next { get; set; }
    public Marble Prev { get; set; }

    public static Marble Insert(Marble current, Marble marble)
    {
        // inserts the marble between marbles 1 & 2 away clockwise (Next)
        current = current.Next;
        marble.Prev = current;
        marble.Next = current.Next;
        current.Next.Prev = marble;
        current.Next = marble;

        return marble;
    }

    public static (int, Marble) Remove(Marble current)
    {
        int idRemoved;

        // 7 marbles counter-clockwise (Prev) is removed & ID is returned
        // marble that was immediately clockwise (Next) is new current & returned

        for (int i = 0; i < 7; i++)
        {
            current = current.Prev;
        }
        idRemoved = current.ID;
        current.Prev.Next = current.Next;
        current.Next.Prev = current.Prev;
        current = current.Next;

        return (idRemoved, current);
    }
}

internal class DayClass
{

    public DayClass()
    {
    }

    public void Part1()
    {
        long highScore = PlayGame(411, 71170);
        Console.WriteLine("Part1: {0}", highScore);
    }

    public void Part2()
    {
        long highScore = PlayGame(411, 7117000);
        Console.WriteLine("Part2: {0}", highScore);
    }

    private long PlayGame(int nPlayers, long lastMarble)
    {
        bool finished = false;
        long winningScore = 0;

        long[] players = new long[nPlayers];
        Marble root = new Marble(0);
        Marble current = root;
        int player = 0;
        int i = 0;
        while (!finished)
        {
            i++;
            if (i == lastMarble)
            {

                finished = true;
                winningScore = players.Max();
                break;
            }
            player++;
            if (player % nPlayers == 0)
            {
                player = 0;
            }
            if (i % 23 == 0)
            {
                (int id, Marble curr) = Marble.Remove(current);
                current = curr;
                int value = id + i;
                players[player] += value;
            }
            else
            {
                Marble newMarble = new Marble(i);
                current = Marble.Insert(current, newMarble);
            }
        }

        return winningScore;
    }
}
