using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WargamesGUI.DAL;
using WargamesGUI.Models;

namespace WargamesGUI.Services
{
    public class LoanService2
    {
        private DbService dbService = new DbService();
        private BookService2 bookService;

        //Bookloan
        public async Task<List<BookLoan2>> GetAllBookLoans()
        {
            bookService = new BookService2();

            var bookLoans = await dbService.GetBookLoansFromDb();
            var loanStatuses = await dbService.GetBookLoanStatusFromDb();
            var libraryCards = await GetAllLibraryCards();
            var books = await bookService.GetAllBooks();
            var bookCopies = await bookService.GetAllBookCopies();

            var listOfBookLoans = bookLoans.Select(x => new BookLoan2
            {
                Loan_Id = x.Loan_Id,
                fk_BookCopy_Id = x.fk_BookCopy_Id,
                fk_LibraryCard_Id = x.fk_LibraryCard_Id,
                fk_BookLoanStatus_Id = x.fk_BookLoanStatus_Id,
                ReturnDate = x.ReturnDate,
                ReturnedDate = x.ReturnedDate,
                LibraryCard = libraryCards.Select(y => y)
                .Where(y => y.LibraryCard_Id == x.fk_LibraryCard_Id)
                .ElementAtOrDefault(0),
                Book = bookCopies.Where(y => y.Copy_Id == x.fk_BookCopy_Id)
                .Select(y => y.Book)
                .ElementAtOrDefault(0),
                BookCopy = bookCopies.Select(y => y)
                .Where(y => y.Copy_Id == x.fk_BookCopy_Id)
                .ElementAtOrDefault(0),
                BookLoanStatus = loanStatuses.Select(y => y)
                .Where(y => y.BookLoanStatus_Id == x.fk_BookLoanStatus_Id)
                .ElementAtOrDefault(0),
            }).ToList();            

            return listOfBookLoans ?? null;
        }
        public async Task<List<BookLoan2>> GetAllBookLoans(User2 user)
        {
            bookService = new BookService2();
            var bookLoans = await GetAllBookLoans();
            return bookLoans.Where(x => x.fk_LibraryCard_Id == user.LibraryCard.LibraryCard_Id).ToList() ?? null;                   
        }
        public async Task<List<BookLoan2>> GetAllBookLoans(LibraryCard2 libraryCard)
        {
            var bookLoans = await GetAllBookLoans();
            return bookLoans.Where(x => x.fk_LibraryCard_Id == libraryCard.LibraryCard_Id).ToList() ?? null;
        }
        public async Task<(bool, int)> LoanBook(Book2 book, User2 user)
        {
            var result = await dbService.ProcedureLoanBook(book.Id, user.LibraryCard.LibraryCard_Id);
            if (result == 0) return (true, 0);
            else return (false, result);
        }
        public async Task<int> LoanBook(Book2 book, LibraryCard2 libraryCard)
        {
            var result = await dbService.ProcedureLoanBook(book.Id, libraryCard.LibraryCard_Id);
            return result;
        }
        public async Task<int> LoanBook(Book2 book, int libraryCard_Id)
        {
            var result = await dbService.ProcedureLoanBook(book.Id, libraryCard_Id);
            return result;
        }
        public async Task<int> LoanBook(BookLoan bookLoan, LibraryCard2 libraryCard)
        {
            var result = await dbService.ProcedureLoanBook(bookLoan.fk_BookCopy_Id, libraryCard.LibraryCard_Id);
            return result;
        }
        public async Task<int> LoanBook(BookLoan bookLoan, User2 user)
        {
            var result = await dbService.ProcedureLoanBook(bookLoan.fk_BookCopy_Id, user.LibraryCard.LibraryCard_Id);
            return result;
        }
        public async Task<List<BookLoan2>> GetHandledBookLoans()
        {
            var bookLoans = await GetAllBookLoans();
            return bookLoans.Where(x => x.BookLoanStatus.BookLoanStatus_Id > 1 && x.BookCopy.fk_Availability == 2).Select(x => x).ToList() ?? null;
            
        }
        public async Task<List<HandledBookLoan>> GetAllArchivedBookLoans()
        {
            var handledBookLoans = await dbService.GetArchivedBookLoansFromDb();
            var bookCopies = await bookService.GetAllBookCopies();
            return handledBookLoans = handledBookLoans.Select(x => new HandledBookLoan
            {
                handled_Loan_Id = x.handled_Loan_Id,
                handled_BookCopy_Id = x.handled_BookCopy_Id,
                ReturnedDate = x.ReturnedDate,
                BookCopy = bookCopies.Select(y => y).Where(y => y.Copy_Id == x.handled_BookCopy_Id).ToList().ElementAtOrDefault(0),
            }).ToList();
        }
        public async Task<(bool, string)> ChangeBookLoanStatusUser(BookLoan2 bookLoan)
        {
            bool success = await dbService.UpdateBookLoanInDb(bookLoan.Loan_Id, bookLoan.fk_BookLoanStatus_Id);
            if (success) return (success, "Success, returned true.");
            else return (success, $"Error: {nameof(this.ChangeBookLoanStatusUser)} - returned false.");
        }
        public async Task<(bool, string)> ChangeBookLoanStatusLibrarian(int bookLoan_Id, int status)
        {
            bool success = await dbService.UpdateBookLoanInDb(bookLoan_Id, status);
            if (success) return (success, "Success, returned true.");
            else return (success, $"Error: {nameof(this.ChangeBookLoanStatusLibrarian)} - returned false.");
        }
        public async Task<(bool, string)> BookLoanReturned(BookLoan bookLoan)
        {
            bool success = await dbService.ProcedureBookLoanReturn(bookLoan.Loan_Id);
            if (success) return (success, "Success, returned true.");
            else return (success, $"Error: {nameof(this.BookLoanReturned)} - returned false.");
        }
        public async Task<(bool, string)> BookCopyReturnedCheck(BookCopy bookCopy)
        {
            bool success = await dbService.ProcedureRegisterReturnedBookCopy(bookCopy.Copy_Id);
            if (success) return (success, "Success, returned true.");
            else return (success, $"Error: {nameof(this.BookCopyReturnedCheck)} - returned false.");
        }
        // Fixa
        //public async Task<(bool, string)> Register()
        //{
        //    dbService.RegisterReturnedBook();
        //}
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
        public async Task<(bool, string)> AddLibraryCardToUser(User2 user)
        {
            bool success = await dbService.ProcedureAddLibraryCard(user.User_ID);
            if (success) return (success, "Success, returned true.");
            else return (success, $"Error: {nameof(this.AddLibraryCardToUser)} - returned false.");
        }
        public async Task<(bool, string)> ChangeLibraryCardStatus(LibraryCard2 libraryCard)
        {
            bool success = await dbService.ProcedureChangeLibraryCardStatus(libraryCard.LibraryCard_Id, libraryCard.fk_Status_Id);
            var getAllCards = await GetAllLibraryCards();
            var message = getAllCards.Where(x => x.LibraryCard_Id == libraryCard.LibraryCard_Id).Select(x => x.CardStatus.Status_Level).FirstOrDefault();
            if (success) return (success, message);
            else return (success, $"Error: {nameof(this.ChangeLibraryCardStatus)} - returned false.");
            
        }
    }
}
