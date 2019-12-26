using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;

namespace SqlExamples.BulkToSequentialInsertComparison
{
    [Display(Name = "Bulk", Order = 2)]
    public class BulkCopyCatSaver : ISaveCats
    {
        private readonly string _connectionString;

        public BulkCopyCatSaver(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void SaveCats(IEnumerable<Cat> cats)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                {
                    bulkCopy.DestinationTableName = "dbo.Cat";
                    DataTable dt = GetDataTable(cats);
                    bulkCopy.WriteToServer(dt);
                }
            }
        }

        private DataTable GetDataTable(IEnumerable<Cat> cats)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(nameof(Cat.Id), typeof(Guid));
            dt.Columns.Add(nameof(Cat.Color));
            dt.Columns.Add(nameof(Cat.FavoriteSound));
            dt.Columns.Add(nameof(Cat.Gender));

            foreach (var cat in cats)
            {
                DataRow dr = dt.NewRow();
                dr[nameof(Cat.Id)] = cat.Id;
                dr[nameof(Cat.Color)] = cat.Color;
                dr[nameof(Cat.FavoriteSound)] = cat.FavoriteSound;
                dr[nameof(Cat.Gender)] = cat.Gender;
                dt.Rows.Add(dr);
            }

            return dt;
        }
    }
}