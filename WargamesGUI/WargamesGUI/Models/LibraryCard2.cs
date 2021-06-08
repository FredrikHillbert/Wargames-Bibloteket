namespace WargamesGUI.Models
{
    public class LibraryCard2
    {
        public int LibraryCard_Id { get; set; }
        public string CardNumber { get; set; }
        public int fk_Status_Id { get; set; }
        public LibraryCardStatus CardStatus { get; set; }
        public User2 User { get; set; }
    }
}
