namespace LibraryManagementSystem
{
    public sealed class LibraryConfiguration
    {
        // Lazy<T> guarantees the factory runs exactly once, even with many threads
        private static readonly Lazy<LibraryConfiguration> _instance =
            new Lazy<LibraryConfiguration>(
                () => new LibraryConfiguration(),
                LazyThreadSafetyMode.ExecutionAndPublication);

        public static LibraryConfiguration Instance => _instance.Value;

        public string LibraryName { get; set; }
        public int MaximumBorrowDays { get; set; }
        public decimal LateFeePerDay { get; set; }

        // Private: nobody outside can call "new LibraryConfiguration()"
        private LibraryConfiguration()
        {
            LibraryName = "Central City Library";
            MaximumBorrowDays = 14;
            LateFeePerDay = 2.5m;
        }
    }
}
