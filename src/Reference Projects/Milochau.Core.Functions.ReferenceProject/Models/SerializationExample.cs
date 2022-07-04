namespace Milochau.Core.Functions.ReferenceProject.Models
{
    public class SerializationExample
    {
        public string String { get; set; } = "Name";
        public string? NullString { get; set; }
        public string EmptyString { get; set; } = "";
        public SerializationExampleType Type { get; set; } = SerializationExampleType.Default;
        public SerializationExampleType? NullableType { get; set; } = null;
        public SerializationExampleType? NullableTypeWithValue { get; set; } = SerializationExampleType.Default;
        public double Number { get; set; } = 0;
        public int Integer { get; set; } = 0;
        public double? NullableNumber { get; set; } = null;
        public double? NullableNumberWithValue { get; set; } = 0;
    }

    public enum SerializationExampleType
    {
        Default = 0,
        Other = 1
    }
}
