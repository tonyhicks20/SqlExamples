using Dapper;
using System;
using System.Linq;
using System.Collections.Concurrent;
using System.Data.SqlClient;
using System.Diagnostics;
using SqlExamples.Common;

namespace SqlExamples.BulkToSequentialInsertComparison
{
    internal class Program
    {
        private static string _connectionString = "Server=localhost; Initial Catalog=AllTheCats; Integrated Security=SSPI;";
        private static ConcurrentDictionary<string, MetricCollector> dataCollectors = new ConcurrentDictionary<string, MetricCollector>();

        private static void Main(string[] args)
        {            
            ClearTable();
            int iterations = 10;
            SaveTheCats(new BulkCopyCatSaver(_connectionString), iterations);
            SaveTheCats(new SequentialCatSaver(_connectionString), iterations);

            LogCollectors();
            Console.ReadLine();
        }

        private static void LogCollectors()
        {
            

            string getObjectSavingMethod(string typeName) => typeName.StartsWith("Bulk") ? "Bulk" : "Sequential";

            string resultTable = dataCollectors.Values
                  .OrderByDescending(x => getObjectSavingMethod(x.Keys[0]))
                  .ThenBy(x => x.Keys[1])
                  .ToStringTable(new[] { "Object Saving Method", "Object Count", "Min (ms)", "Max (ms)", "Avg (ms)" },
                      x => getObjectSavingMethod(x.Keys[0]),
                      x => x.Keys[1],
                      x => x.Min,
                      x => x.Max,
                      x => x.Average
                      );

            Console.WriteLine("\n\n" + resultTable);
        }

        private static void SaveTheCats(ISaveCats catSaver, int iterations)
        {
            void benchmark(int count)
            {
                for (int i = 0; i < iterations; i++)
                {
                    ClearTable();

                    Console.Write($"\r Benchmark number : {i+1}, Count: {count}, Saving Type : {catSaver.GetType().Name}");
                    var cats = DataRandomizer.CreateRandomCats(count);
                    var st = Stopwatch.StartNew();
                    catSaver.SaveCats(cats);
                    st.Stop();
                    var collector = dataCollectors.GetOrAdd(catSaver.GetType().Name + $" with {count} objects", x => new MetricCollector(catSaver.GetType().Name, count.ToString()));
                    collector.SetValue(st.ElapsedMilliseconds);
                }

                LogCollectors();
            }

            benchmark(1000);

            benchmark(10000);

            benchmark(100000);

        }
        

        private static void ClearTable()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                connection.Execute("TRUNCATE TABLE dbo.Cat");
            }
        }

       
    }
}