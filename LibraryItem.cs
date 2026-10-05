namespace LibraryManagementSystem
{
    public abstract class LibraryItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }

        protected LibraryItem(int id, string title, int year)
        {
            Id = id;
            Title = title;
            Year = year;
        }

        // Abstract: no body, every derived class must override it
        public abstract void DisplayInfo();

        // Concrete: shared by all derived classes
        public void PrintBasicInfo()
        {
            Console.WriteLine($"ID: {Id} | Title: {Title} | Year: {Year}");
        }
    }
}
