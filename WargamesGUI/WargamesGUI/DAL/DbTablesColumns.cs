using System;
using System.Collections.Generic;
using System.Text;

namespace WargamesGUI.Services
{
    public enum TableName
    {
        tblUser,
        tblPrivilegeLevel,
        tblBook,
        tblEvent,
        tblRemovedItem,
        tblDeweyMain,
        tblDeweySub,
        tblBookLoan,
        tblBookLoanStatus,
        tblBookCopy,
        tblAvailability,
        tblConditionStatus,
        tblLibraryCard,
        tblLibraryCardStatus,
        tblItem,
        tblHandledBookLoan,
    }
    public enum tblBookLoan
    {
        Loan_Id,
        fk_BookCopy_Id,
        fk_LibraryCard_Id,
        fk_BookLoanStatus_Id,
        ReturnDate,
        ReturnedDate,
        Checked_In,
    }
}
