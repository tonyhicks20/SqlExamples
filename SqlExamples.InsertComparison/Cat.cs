using System;

namespace SqlExamples.BulkToSequentialInsertComparison
{
    public class Cat
    {
        public Cat(Guid id, string color, string favoriteSound, string gender)
        {
            Id = id;
            Color = color;
            FavoriteSound = favoriteSound;
            Gender = gender;
        }

        public Guid Id { get; }
        public string Color { get; }
        public string FavoriteSound { get; }
        public string Gender { get; }
    }
}