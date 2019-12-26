using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SqlExamples.BulkToSequentialInsertComparison
{
    [Display(Name = "Concurrent", Order = 1)]
    public class ConcurrentCatSaver : SequentialCatSaver
    {

        public ConcurrentCatSaver(string connectionString) : base(connectionString)
        {
            
        }
        
        public override void SaveCats(IEnumerable<Cat> cats) => cats.AsParallel().ForAll(SaveCat);
        
    }
}