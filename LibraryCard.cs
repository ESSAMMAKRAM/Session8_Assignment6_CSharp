namespace LibraryManagementSystem
{
    public sealed class LibraryCard
    {
        public string CardNumber { get; }
        public string MemberName { get; }
        public DateTime ExpirationDate { get; }

        public LibraryCard(string cardNumber, string memberName, DateTime expirationDate)
        {
            CardNumber = cardNumber;
            MemberName = memberName;
            ExpirationDate = expirationDate;
        }

        public void DisplayCard()
        {
            Console.WriteLine($"Card #{CardNumber} | Member: {MemberName} | Expires: {ExpirationDate:yyyy-MM-dd}");
        }
    }

    // INVALID (error CS0509: cannot derive from sealed type 'LibraryCard'):
    // public class PremiumLibraryCard : LibraryCard { }
}
