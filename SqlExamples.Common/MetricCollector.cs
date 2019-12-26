using System.Collections.Generic;

namespace SqlExamples.Common
{
    public class MetricCollector

    {

        public MetricCollector(string id)
        {
            Id = id;
        }

        private long _sum = 0;

        public double Average => _sum / Count;
        public long Max { get; private set; }
        public long Min { get; private set; } = 9999999;
        public string Id { get; }

        public long Count { get; private set; }

        public int DisplayOrder { get; set; }
        
        public string DisplayName { get; set; }

        public Dictionary<string, object> AdditionalProperties { get; } = new Dictionary<string, object>(); 
        

        public void SetValue(long value)
        {
            ++Count;
            _sum += value;
            if (value < Min)
                Min = value;
            if (value > Max)
                Max = value;
        }                                 
    }
}