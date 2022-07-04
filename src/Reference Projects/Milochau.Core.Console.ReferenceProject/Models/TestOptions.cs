namespace Milochau.Core.Console.ReferenceProject.Models
{
    public class TestOptions
    {
        public string? PublicValueOnlyShared { get; set; }
        public string? PublicValueOnlyApplication { get; set; }
        public string? PublicValueOnlySharedAndApplication { get; set; }

        public string? SecretValue { get; set; }
    }
}
