using LibraryManagementSystem;

// 1. ABSTRACT CLASS
Console.WriteLine("=== 1. Abstract class & polymorphism ===");

// LibraryItem bad = new LibraryItem(...);  // ERROR: abstract

LibraryItem item1 = new Book(1, "Clean Code", 2008, "Robert C. Martin", 464);
LibraryItem item2 = new Magazine(2, "National Geographic", 2024, 5);

item1.PrintBasicInfo();
item1.DisplayInfo();
item2.PrintBasicInfo();
item2.DisplayInfo();

Console.WriteLine("\nPolymorphism with a list of LibraryItem:");
List<LibraryItem> items = new() { item1, item2 };
foreach (LibraryItem item in items)
{
    item.DisplayInfo();
}

// 2. IComparable<Book>
LibraryUtilities.PrintSeparator('=');
Console.WriteLine("=== 2. IComparable<Book> - Sort by Year, then Title ===");

List<Book> books = new()
{
    new Book(1, "Clean Code", 2008, "Robert C. Martin", 464),
    new Book(2, "The Pragmatic Programmer", 1999, "Andrew Hunt & David Thomas", 352),
    new Book(3, "Design Patterns", 1994, "Erich Gamma et al.", 395),
    new Book(4, "C# in Depth", 2008, "Jon Skeet", 528)
};

Console.WriteLine("Before sorting:");
books.ForEach(b => b.DisplayInfo());

books.Sort();

Console.WriteLine("\nAfter sorting (the two 2008 books are ordered by Title):");
books.ForEach(b => b.DisplayInfo());

// 3. ICloneable
LibraryUtilities.PrintSeparator('=');
Console.WriteLine("=== 3. ICloneable ===");

Book originalBook = new Book(10, "Clean Code", 2008, "Robert C. Martin", 464);
Book clonedBook = (Book)originalBook.Clone();
clonedBook.Title = clonedBook.Title + " - Copy";

Console.WriteLine("Original:");
Console.WriteLine($"Title: {originalBook.Title}");
Console.WriteLine("Clone:");
Console.WriteLine($"Title: {clonedBook.Title}");
Console.WriteLine($"Same object? {ReferenceEquals(originalBook, clonedBook)}");

Book sameReference = originalBook;
sameReference.Title = "Changed through the second reference";
Console.WriteLine($"After 'Book sameReference = originalBook' and changing its title, original is now: {originalBook.Title}");
originalBook.Title = "Clean Code";

// 4. STATIC CLASS
LibraryUtilities.PrintSeparator('=');
Console.WriteLine("=== 4. Static class ===");

// LibraryUtilities u = new LibraryUtilities();  // ERROR: static

Console.WriteLine($"Late fee for 5 overdue days: {LibraryUtilities.CalculateLateFee(5)}");
Console.WriteLine($"Late fee for 0 overdue days: {LibraryUtilities.CalculateLateFee(0)}");
Console.WriteLine($"Book code: {LibraryUtilities.GenerateBookCode(originalBook.Id, originalBook.Title, originalBook.Year)}");
LibraryUtilities.PrintSeparator('*', 30);

// 5. SEALED CLASS
LibraryUtilities.PrintSeparator('=');
Console.WriteLine("=== 5. Sealed class ===");

LibraryCard card = new LibraryCard("LC-1001", "Essam Makram", DateTime.Today.AddYears(1));
card.DisplayCard();
Console.WriteLine("LibraryCard is sealed, so 'class PremiumLibraryCard : LibraryCard' is a compile error (CS0509).");
Console.WriteLine("Reason: 'sealed' tells the compiler that no class may inherit from LibraryCard.");

// 6. PARTIAL CLASS
LibraryUtilities.PrintSeparator('=');
Console.WriteLine("=== 6. Partial class ===");

Console.WriteLine("Book is split across: Book.cs (properties, constructor, IComparable, ICloneable, operators)");
Console.WriteLine("                  and: Book.Display.cs (DisplayInfo, PrintBookDetails)");
Console.WriteLine($"Runtime type: {typeof(Book).FullName} (a single type, not two)");
Book partialDemo = books[0];
partialDemo.PrintBookDetails();
Console.WriteLine($"CompareTo (Book.cs) works on the same object: {partialDemo.CompareTo(books[1])}");

// 7. OPERATOR OVERLOADING
LibraryUtilities.PrintSeparator('=');
Console.WriteLine("=== 7. Operator overloading ===");

Book book1 = new Book(21, "C# in Depth", 2008, "Jon Skeet", 528);
Book book2 = new Book(22, "Design Patterns", 1994, "Erich Gamma et al.", 395);

Console.WriteLine($"Book 1: {book1.PageCount} pages | Book 2: {book2.PageCount} pages");

if (book1 > book2)
{
    Console.WriteLine("book1 > book2  => Book 1 has more pages.");
}
if (book1 < book2)
{
    Console.WriteLine("book1 < book2  => Book 2 has more pages.");
}

Book combined = book1 + book2;
Console.WriteLine("book1 + book2  =>");
combined.PrintBookDetails();
Console.WriteLine($"Pages check: {book1.PageCount} + {book2.PageCount} = {combined.PageCount}");

// 8. SINGLETON
LibraryUtilities.PrintSeparator('=');
Console.WriteLine("=== 8. Singleton ===");

// var bad = new LibraryConfiguration();  // ERROR: private constructor

var config1 = LibraryConfiguration.Instance;
var config2 = LibraryConfiguration.Instance;

Console.WriteLine($"Library name: {config1.LibraryName}");
Console.WriteLine($"ReferenceEquals(config1, config2): {ReferenceEquals(config1, config2)}");

config1.MaximumBorrowDays = 21;
Console.WriteLine($"config2.MaximumBorrowDays after changing config1: {config2.MaximumBorrowDays}");

var instances = new System.Collections.Concurrent.ConcurrentBag<LibraryConfiguration>();
Parallel.For(0, 100, _ => instances.Add(LibraryConfiguration.Instance));
bool allSame = instances.All(c => ReferenceEquals(c, config1));
Console.WriteLine($"100 parallel requests all returned the same instance: {allSame}");

LibraryUtilities.PrintSeparator('=');
Console.WriteLine("Done.");
