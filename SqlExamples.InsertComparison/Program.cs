using Dapper;
using System;
using System.Linq;
using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Security.Cryptography;
using SqlExamples.Common;

namespace SqlExamples.BulkToSequentialInsertComparison
{
    internal class Program
    {
        private static readonly string _connectionString = "";//Set me!

        private static readonly ConcurrentDictionary<string, MetricCollector> _dataCollectors =
            new ConcurrentDictionary<string, MetricCollector>();

        private static void Main(string[] args)
        {
            int iterations = 10;
            

            SaveTheCats(new BulkCopyCatSaver(_connectionString), iterations);
            SaveTheCats(new ConcurrentCatSaver(_connectionString), iterations);
            SaveTheCats(new SequentialCatSaver(_connectionString), iterations);

            Console.ReadLine();
        }

        private static void LogCollectors()
        {
            string resultTable = _dataCollectors.Values
                .OrderBy(x => x.DisplayOrder)
                .ThenBy(x => x.AdditionalProperties["ObjectCount"])
                .ToStringTable(new[] {"Object Saving Method", "Object Count", "Min (ms)", "Max (ms)", "Avg (ms)"},
                    x => x.DisplayName,
                    x => x.AdditionalProperties["ObjectCount"],
                    x => x.Min,
                    x => x.Max,
                    x => x.Average
                );

            Console.WriteLine("\n\n" + resultTable);
        }

        private static void SaveTheCats(ISaveCats catSaver, int simulationRuns)
        {
            
            void benchmark(int count)
            {
                if(string.IsNullOrWhiteSpace(_connectionString))
                    throw new NotSupportedException("Connection string must be set!");
                
                
                for (int i = 0; i < simulationRuns; i++)
                {
                    ClearTable();

                    Console.Write(
                        $"\r Simulation number : {i + 1}, Count: {count}, Saving Type : {catSaver.GetType().Name}");
                    
                    var cats = DataRandomizer.CreateRandomCats(count);
                    var st = Stopwatch.StartNew();
                    catSaver.SaveCats(cats);
                    st.Stop();
                    
                    GetCollector(count, catSaver).SetValue(st.ElapsedMilliseconds);
                }

                LogCollectors();
            }

            benchmark(1000);

            benchmark(10000);

            benchmark(100000);
        }

        private static MetricCollector GetCollector(int count,object obj)
        {
            var id = string.Join("~", obj.GetType().Name, count);
            
            return _dataCollectors.GetOrAdd(id, x =>
            {
                var display = (DisplayAttribute) obj.GetType()
                    .GetCustomAttributes(typeof(DisplayAttribute), true)
                    .FirstOrDefault();

                var collector = new MetricCollector(id)
                {
                    DisplayName = display?.Name ?? obj.GetType().Name,
                    DisplayOrder = display?.Order ?? 99999
                };

                collector.AdditionalProperties["ObjectCount"] = count;

                return collector;
            });
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