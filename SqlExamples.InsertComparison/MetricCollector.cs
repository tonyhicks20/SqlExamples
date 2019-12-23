namespace SqlExamples.InsertComparison
{
    public class MetricCollector

    {

        public MetricCollector(params string[] keys)
        {
            Keys = keys;
        }

        private long count = 0;
        private long sum = 0;

        public double Average { get => sum / count; }
        public long Max { get; private set; }
        public long Min { get; private set; } = 9999999;

        public string[] Keys { get; }

        public void SetValue(long value)
        {
            ++count;
            sum += value;
            if (value < Min)
                Min = value;
            if (value > Max)
                Max = value;
        }                                 
    }
}