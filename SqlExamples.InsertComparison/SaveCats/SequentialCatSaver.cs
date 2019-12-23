using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace SqlExamples.InsertComparison
{
    public class SequentialCatSaver : ISaveCats
    {
        private readonly string _connectionString;

        public SequentialCatSaver(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void SaveCats(IEnumerable<Cat> cats)
        {
            int count = 0;
            foreach (var cat in cats)
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    connection.Execute("INSERT INTO Cat (Id, Color, FavoriteSound,Gender) VALUES (@Id, @Color, @FavoriteSound,@Gender)", cat);
                }

                string output = $"; Object Saved : {++count}   ";
                Console.Write(output);
                Console.SetCursorPosition(Console.CursorLeft - output.Length, Console.CursorTop);
            }
        }
    }
}