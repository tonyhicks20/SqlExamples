using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SqlExamples.BulkToSequentialInsertComparison
{
    public static class DataRandomizer
    {
        private static string[]
          _Colors = new[] { "Grey", "Brown", "Tortoise Shell", "Marble", "Black" },
          _Sounds = new[] { "Electric can opener", "Birds chirping", "Catnip bag opening", "Hamster cage opening" },
          _Gender = new[] { "Female", "Male" };

        private static Random _Rand = new Random();

        public static IEnumerable<Cat> CreateRandomCats(int count)
        {
            string getRandomElement(string[] values) => values[_Rand.Next(0, values.Length)];

            return Enumerable.Range(0, count).Select(x => new Cat
            (
                Guid.NewGuid(),
                getRandomElement(_Colors),
                getRandomElement(_Sounds),
                getRandomElement(_Gender)
            )).ToList();
        }
    }
}
