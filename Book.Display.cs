namespace LibraryManagementSystem
{
    public partial class Book
    {
        public override void DisplayInfo()
        {
            Console.WriteLine($"[Book] {Title} by {Author} ({Year}) - {PageCount} pages");
        }

        public void PrintBookDetails()
        {
            Console.WriteLine($"  ID:     {Id}");
            Console.WriteLine($"  Title:  {Title}");
            Console.WriteLine($"  Author: {Author}");
            Console.WriteLine($"  Year:   {Year}");
            Console.WriteLine($"  Pages:  {PageCount}");
        }
    }
}
