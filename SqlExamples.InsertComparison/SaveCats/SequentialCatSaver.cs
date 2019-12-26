using Dapper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Security.Cryptography;

namespace SqlExamples.BulkToSequentialInsertComparison
{
    
    [Display(Name = "Sequential", Order = 0)]
    public class SequentialCatSaver : ISaveCats
    {
        private readonly string _connectionString;

        public SequentialCatSaver(string connectionString)
        {
            _connectionString = connectionString;
        }

        public virtual void SaveCats(IEnumerable<Cat> cats)
        {
            int count = 0;
            foreach (var cat in cats)
            {
                SaveCat(cat);
                string output = $"; Object Saved : {++count}   ";
                Console.Write(output);
                Console.SetCursorPosition(Console.CursorLeft - output.Length, Console.CursorTop);
            }
        }
        
        protected void SaveCat(Cat cat)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                connection.Execute("INSERT INTO Cat (Id, Color, FavoriteSound,Gender) VALUES (@Id, @Color, @FavoriteSound,@Gender)", cat);
            }
        }
    }
}