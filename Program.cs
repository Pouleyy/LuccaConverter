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
                var graph = CreateGraph ();
        static MyGraph<string> CreateGraph () {
            var edges = new List<Tuple<string, string>> ();
            foreach (var item in infoConversion) {
                edges.Add (Tuple.Create (item.Item1, item.Item2));
            }
            return new MyGraph<string> (listCurrency, edges);
        }
    }
}