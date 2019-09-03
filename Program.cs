using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LuccaConverter {
    class Program {
        public class MyGraph<T> {
            public MyGraph () { }
            public MyGraph (IEnumerable<T> vertices, IEnumerable<Tuple<T, T>> edges) {
                foreach (var vertex in vertices)
                    AddVertex (vertex);

                foreach (var edge in edges)
                    AddEdge (edge);
            }

            public Dictionary<T, HashSet<T>> AdjacencyList { get; } = new Dictionary<T, HashSet<T>> ();

            public void AddVertex (T vertex) {
                AdjacencyList[vertex] = new HashSet<T> ();
            }

            public void AddEdge (Tuple<T, T> edge) {
                if (AdjacencyList.ContainsKey (edge.Item1) && AdjacencyList.ContainsKey (edge.Item2)) {
                    AdjacencyList[edge.Item1].Add (edge.Item2);
                    AdjacencyList[edge.Item2].Add (edge.Item1);
                }
            }
        }
        private static string startingCurrency;
        private static string endingCurrency;
        private static double amountToChange;
        private static List < (string, string, double) > infoConversion = new List < (string, string, double) > (); //edges of the graph
        private static List<String> listCurrency = new List<string> (); //vertices of the graph

        static void Main (string[] args) {
            if (args[0] != null) {
                StreamFile (args[0]);
                var graph = CreateGraph ();
                var shortestPath = ShortestPath (graph, startingCurrency, endingCurrency);
                var result = ComputeChange (shortestPath);
                Console.WriteLine (result.ToString ());

            } else {
                Console.WriteLine ("Error, you need to specify a path to the file");
            }
        }

        static void StreamFile (string pathToFile) {
            var fs = new FileStream (pathToFile, FileMode.Open);
            using (StreamReader reader = new StreamReader (fs)) {
                string line = String.Empty;
                var count = 0;
                var listCurrencyWithDouble = new List<string> ();
                while ((line = reader.ReadLine ()) != null) {
                    var lineContent = line.Split (";");
                    if (count == 0) {
                        startingCurrency = lineContent[0];
                        endingCurrency = lineContent[2];
                        amountToChange = Convert.ToDouble (lineContent[1]);
                    }
                    if (count > 1) {
                        var currency1 = lineContent[0];
                        var currency2 = lineContent[1];
                        var rate = Convert.ToDouble (lineContent[2]);
                        listCurrencyWithDouble.Add (currency1);
                        listCurrencyWithDouble.Add (currency2);
                        infoConversion.Add ((currency1, currency2, rate));
                    }
                    count++;
                }
                listCurrency = new HashSet<String> (listCurrencyWithDouble).ToList ();
            }
        }

        static MyGraph<string> CreateGraph () {
            var edges = new List<Tuple<string, string>> ();
            foreach (var item in infoConversion) {
                edges.Add (Tuple.Create (item.Item1, item.Item2));
            }
            return new MyGraph<string> (listCurrency, edges);
        }

        static IEnumerable<T> ShortestPath<T> (MyGraph<T> graph, T start, T end) {
            var previous = new Dictionary<T, T> ();

            var queue = new Queue<T> ();
            queue.Enqueue (start);

            while (queue.Count > 0) {
                var vertex = queue.Dequeue ();
                foreach (var neighbor in graph.AdjacencyList[vertex]) {
                    if (previous.ContainsKey (neighbor))
                        continue;

                    previous[neighbor] = vertex;
                    queue.Enqueue (neighbor);
                }
            }

            var path = new List<T> { };

            var current = end;
            while (!current.Equals (start)) {
                path.Add (current);
                current = previous[current];
            };

            path.Add (start);
            path.Reverse ();

            return path;
        }

        static double ComputeChange (IEnumerable<string> shortestPath) {
            var listPath = shortestPath.ToList ();
            var amount = amountToChange;
            for (var i = 0; i < shortestPath.Count () - 1; i++) {
                var convertFrom = listPath[i];
                var convertTo = listPath[i + 1];
                var conversion = infoConversion.First (item => (item.Item1 == convertFrom && item.Item2 == convertTo) || (item.Item2 == convertFrom && item.Item1 == convertTo));
                if (conversion.Item1 == convertFrom) {
                    amount = Math.Round (amount * conversion.Item3, 4);
                } else {
                    var changeRate = Math.Round (1 / conversion.Item3, 4);
                    amount = Math.Round (amount * changeRate, 4);
                }
            }
            return Math.Round (amount, 0);
        }
    }
}