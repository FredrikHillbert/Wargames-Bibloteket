using System;
using System.Collections.Generic;
using System.Text;

namespace WargamesGUI.Models
{
    public class BookLoan
    {
        public int Loan_Id { get; set; }
        public int fk_Book_Id { get; set; }
        public int fk_LibraryCard_Id { get; set; }
        public int fk_BookLoanStatus_Id { get; set; }
        public DateTime ReturnDate { get; set; }
        public DateTime ReturnedDate { get; set; }

    }
}
