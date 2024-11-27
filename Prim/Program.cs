using System.Diagnostics;

public class Graph
{
    public int Vertices { get; }
    public List<(int, int, int)> Edges { get; } 

    public Graph(int vertices)
    {
        Vertices = vertices;
        Edges = new List<(int, int, int)>();
    }

    public void AddEdge(int u, int v, int weight)
    {
        Edges.Add((u, v, weight));
    }

    public List<(int, int, int)> GetEdges() => Edges;
}

public class PrimAlgorithm
{
    public static (int totalWeight, List<(int, int, int)> mstEdges) Run(Graph graph)
    {
        int totalWeight = 0;
        bool[] inMST = new bool[graph.Vertices + 1];
        PriorityQueue<(int, int), int> pq = new PriorityQueue<(int, int), int>();
        pq.Enqueue((1, 0), 0);  

        List<(int, int, int)> mstEdges = new List<(int, int, int)>(); 

        while (pq.Count > 0)
        {
            var (u, weight) = pq.Dequeue();

            if (inMST[u]) continue;

            inMST[u] = true;
            totalWeight += weight;

           
            foreach (var (start, end, w) in graph.GetEdges())
            {
                if (start == u && !inMST[end])
                {
                    pq.Enqueue((end, w), w);
                    mstEdges.Add((start, end, w)); 
                }
                else if (end == u && !inMST[start])
                {
                    pq.Enqueue((start, w), w);
                    mstEdges.Add((end, start, w)); 
                }
            }
        }

        return (totalWeight, mstEdges);
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("Por favor, forneça o caminho do arquivo como argumento.");
            return;
        }

        string filePath = args[0];

        if (!File.Exists(filePath))
        {
            Console.WriteLine("Arquivo não encontrado.");
            return;
        }



        
        var graph = ReadGraphFromFile(filePath);

       
        Stopwatch stopwatch = Stopwatch.StartNew();

        var (mstWeight, mstEdges) = PrimAlgorithm.Run(graph);

       
        stopwatch.Stop();
        foreach (var edge in mstEdges)
        {
            Console.WriteLine($"Vértice {edge.Item1} - Vértice {edge.Item2} com peso {edge.Item3}");
        }
        Console.WriteLine("\n\n\n");
        Console.WriteLine("=================   Resumo   =====================");
       
        Console.WriteLine($"Tempo de execução: {stopwatch.ElapsedMilliseconds} ms");

     
        Console.WriteLine($"Número de vértices: {graph.Vertices}");
        Console.WriteLine($"Número de arestas: {graph.Edges.Count}");

      
        Console.WriteLine($"Peso da Árvore Geradora Mínima (MST): {mstWeight}");

     
        Console.WriteLine("\nArestas da Árvore Geradora Mínima:");
        
    }

    public static Graph ReadGraphFromFile(string filePath)
    {
        var lines = File.ReadAllLines(filePath);
        int vertices = 0, edges = 0;

        var graph = new Graph(0);
        foreach (var line in lines)
        {
            if (line.StartsWith("p sp"))
            {
                var parts = line.Split(' ');
                vertices = int.Parse(parts[2]);
                edges = int.Parse(parts[3]);
                graph = new Graph(vertices);
            }
            else if (line.StartsWith("a"))
            {
                var parts = line.Split(' ');
                int u = int.Parse(parts[1]);
                int v = int.Parse(parts[2]);
                int weight = int.Parse(parts[3]);
                graph.AddEdge(u, v, weight);
            }
        }

        return graph;
    }
}
