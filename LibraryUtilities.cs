namespace LibraryManagementSystem
{
    public static class LibraryUtilities
    {
        public static decimal CalculateLateFee(int overdueDays)
        {
            if (overdueDays <= 0) return 0m;
            return overdueDays * LibraryConfiguration.Instance.LateFeePerDay;
        }

        public static string GenerateBookCode(int id, string title, int year)
        {
            string letters = new string(title.Where(char.IsLetter).ToArray()).ToUpper();
            string prefix = letters.Length >= 3 ? letters.Substring(0, 3) : letters.PadRight(3, 'X');
            return $"{prefix}-{year}-{id:D4}";
        }

        public static void PrintSeparator(char symbol = '-', int length = 50)
        {
            Console.WriteLine(new string(symbol, length));
        }
    }
}
