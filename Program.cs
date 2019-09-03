using System;
using System.IO;

namespace LuccaConverter {
    class Program {
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
                while ((line = reader.ReadLine ()) != null) {
                    var lineContent = line.Split (";");
                }
            }
        }
    }
}