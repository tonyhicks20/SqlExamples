using System.Collections.Generic;

namespace SqlExamples.BulkToSequentialInsertComparison
{
    public interface ISaveCats
    {
        void SaveCats(IEnumerable<Cat> cats);
    }
}