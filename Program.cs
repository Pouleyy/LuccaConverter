using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
    }
}