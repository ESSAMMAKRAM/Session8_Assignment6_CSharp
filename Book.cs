namespace LibraryManagementSystem
{
    public partial class Book : LibraryItem, IComparable<Book>, ICloneable
    {
        public string Author { get; set; }
        public int PageCount { get; set; }

        public Book(int id, string title, int year, string author, int pageCount)
            : base(id, title, year)
        {
            Author = author;
            PageCount = pageCount;
        }

        // Part 2: sort by Year, then by Title
        public int CompareTo(Book? other)
        {
            if (other is null) return 1;

            int result = Year.CompareTo(other.Year);
            if (result != 0) return result;

            return string.Compare(Title, other.Title, StringComparison.Ordinal);
        }

        // Part 3: returns a NEW Book object
        public object Clone()
        {
            return new Book(Id, Title, Year, Author, PageCount);
        }

        // Part 7: operators (> and < must be overloaded together)
        public static bool operator >(Book left, Book right)
        {
            ArgumentNullException.ThrowIfNull(left);
            ArgumentNullException.ThrowIfNull(right);
            return left.PageCount > right.PageCount;
        }

        public static bool operator <(Book left, Book right)
        {
            ArgumentNullException.ThrowIfNull(left);
            ArgumentNullException.ThrowIfNull(right);
            return left.PageCount < right.PageCount;
        }

        // Combined book: Id = 0, Title = "A + B", Year = later year,
        // Author = same author or "A & B", PageCount = sum
        public static Book operator +(Book left, Book right)
        {
            ArgumentNullException.ThrowIfNull(left);
            ArgumentNullException.ThrowIfNull(right);

            string author = left.Author == right.Author
                ? left.Author
                : $"{left.Author} & {right.Author}";

            return new Book(
                0,
                $"{left.Title} + {right.Title}",
                Math.Max(left.Year, right.Year),
                author,
                left.PageCount + right.PageCount);
        }
    }
}
