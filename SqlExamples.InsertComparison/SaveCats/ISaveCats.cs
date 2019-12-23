using System.Collections.Generic;

namespace SqlExamples.InsertComparison
{
    public interface ISaveCats
    {
        void SaveCats(IEnumerable<Cat> cats);
    }
}