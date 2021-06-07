using System;

namespace WargamesGUI.Models
{
    public class BookLoan2
    {
        public int Loan_Id { get; set; }
        public int fk_BookCopy_Id { get; set; }
        public int fk_LibraryCard_Id { get; set; }
        public int fk_BookLoanStatus_Id { get; set; }
        public DateTime ReturnDate { get; set; }
        public DateTime ReturnedDate { get; set; }

        // ??
        public LibraryCard2 LibraryCard { get; set; }
        public Book2 Book { get; set; }
        public BookCopy BookCopy { get; set; }
        public BookLoanStatus BookLoanStatus { get; set; }

    }
}
