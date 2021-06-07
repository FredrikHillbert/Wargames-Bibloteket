using System;
namespace WargamesGUI.Models
{
    public class HandledBookLoan
    {
        public int handled_Loan_Id { get; set; }
        public int handled_BookCopy_Id { get; set; }
        public DateTime ReturnedDate { get; set; }
        public BookCopy BookCopy { get; set; }

    }
}
