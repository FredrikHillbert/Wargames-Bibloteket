using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using WargamesGUI.Models;
using System.Linq;

namespace WargamesGUI.Services
{
    public class LoanService2
    {
        private DbService dbService = new DbService();
        private BookService2 bookService = new BookService2();

        //Bookloan
        public async Task<List<BookLoan2>> GetAllBookLoans()
        {
            var bookLoans = await dbService.GetBookLoansFromDb();
            var loanStatuses = await dbService.GetBookLoanStatusFromDb();
            var libraryCards = await GetAllLibraryCards();
            var bookCopies = await bookService.GetAllBookCopies();

            var listOfBookLoans = bookLoans.Select(x => new BookLoan2
            {
                Loan_Id = x.Loan_Id,
                fk_BookCopy_Id = x.fk_BookCopy_Id,
                fk_LibraryCard_Id = x.fk_LibraryCard_Id,
                fk_BookLoanStatus_Id = x.fk_BookLoanStatus_Id,
                ReturnDate = x.ReturnDate,
                ReturnedDate = x.ReturnedDate,
                LibraryCard = libraryCards.Select(y => y).Where(y => y.LibraryCard_Id == x.fk_LibraryCard_Id).ElementAtOrDefault(0),
                Book = bookCopies.Where(y => y.Copy_Id == x.fk_BookCopy_Id)
                .Select(y => y.Book)
                .ElementAtOrDefault(0),
                BookCopy = bookCopies.Select(y => y).Where(y => y.Copy_Id == x.fk_BookCopy_Id).ElementAtOrDefault(0),
                BookLoanStatus = loanStatuses.Select(y => y)
                .Where(y => y.BookLoanStatus_Id == x.fk_BookLoanStatus_Id)
                .ElementAtOrDefault(0),

                
            }).ToList();
            

            return listOfBookLoans ?? null;
        }
        public async Task<List<BookLoan>> GetAllBookLoans(User user)
        {
            return null;
        }
        public async Task<List<BookLoan>> GetAllBookLoans(LibraryCard libraryCard)
        {
            return null;
        }
        public async Task<bool> ChangeBookLoanStatus()
        {
            return true;
        }
        public async Task<bool> LoanBook()
        {
            return true;
        }

        //Librarycard
        public async Task<List<LibraryCard2>> GetAllLibraryCards()
        {
            var libraryCards = await dbService.GetLibraryCardsFromDb();
            var libraryCardStatuses = await dbService.GetLibraryCardStatusFromDb();

            var listOfLibraryCards = libraryCards.Select(x => new LibraryCard2
            {
                LibraryCard_Id = x.LibraryCard_Id,
                CardNumber = x.CardNumber,
                fk_Status_Id = x.fk_Status_Id,
                CardStatus = libraryCardStatuses.Select(y => y).Where(y => y.Status_Id == x.fk_Status_Id).ElementAtOrDefault(0),

            }).ToList();


            return listOfLibraryCards ?? null;
        }
        public async Task<bool> AddNewLibraryCard()
        {
            return true;
        }
        public async Task<bool> ChangeLibraryCardStatus()
        {
            return true;
        }
    }
}
