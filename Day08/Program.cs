// See https://aka.ms/new-console-template for more information
using Day08;

DayClass day = new DayClass();

var watch = new System.Diagnostics.Stopwatch();
watch.Start();

day.Part1_2();

watch.Stop();
Console.WriteLine("Execution Time: {0} ms", watch.ElapsedMilliseconds);

Console.Write("Press Enter to continue...");
Console.ReadLine();

internal class DayClass
{
    public List<int> _values = new();
    public Node _root;
    public DayClass()
    {
        LoadData();
    }

    public void Part1_2()
    {
        Node node = new Node(NextValue, NextValue);
        _root = BuildTree(node);

        int metaSum = TraverseForMetasum(_root);

        Console.WriteLine("Part1: {0}", metaSum);
        Console.WriteLine("Part2: {0}", _root.Value);
    }

    private Node BuildTree(Node node)
    {
        for (int i = 0; i < node.NChildren; i++)
        {
            Node newNode = BuildTree(new Node(NextValue, NextValue));
            node.Children.Add(newNode);
        }
        for (int i = 0; i < node.NMetadata; i++)
        {
            node.Metadata.Add(NextValue);
        }
        if (node.NChildren == 0)
        {
            node.Value = node.Metadata.Sum();
        }
        else
        {
            for (int i = 0; i < node.NMetadata; i++)
            {
                int index = node.Metadata[i] - 1;
                if (index < node.NChildren)
                {
                    node.Value += node.Children[index].Value;
                }
            }
        }
        return node;
    }

    private int TraverseForMetasum(Node node)
    {
        int sum = node.Metadata.Sum();
        foreach (Node child in node.Children)
        {
            sum += TraverseForMetasum(child);            
        }
        return sum;
    }


    int _valueIndex = 0;
    public int NextValue
    {
        get
        {
            if (_valueIndex < _values.Count)
            {
                return _values[_valueIndex++];
            }
            else
            { 
                return -1;
            }
        }
    }

    private void LoadData()
    {
        string inputFile = AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\input.txt";

        if (File.Exists(inputFile))
        {
            string? line;
            StreamReader file = new StreamReader(inputFile);
            line = file.ReadLine();
            string[] parts = line.Split(' ');
            foreach (string part in parts)
            {
                _values.Add(int.Parse(part));
            }
            file.Close();
        }
    }

}
