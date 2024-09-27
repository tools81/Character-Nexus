namespace Utility
{
    public class Prerequisite<T>
    {
        public string Type { get; set; }
        public string? Name { get; set; }
        public T Value { get; set; }        
        public Prerequisite<T>[]? LogicalAnd { get; set; } = null;
        public Prerequisite<T>[]? LogicalOr { get; set; } = null;
        public Prerequisite<T>[]? LogicalXor { get; set; } = null;
    }
}