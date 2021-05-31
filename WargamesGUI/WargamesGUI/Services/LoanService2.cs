using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using WargamesGUI.Models;

namespace WargamesGUI.Services
{
    public class LoanService2
    {
        private DbService dbService = new DbService();

        //Bookloan
        public async Task<List<BookLoan>> GetAllBookLoans()
        {
            var bookLoans = await dbService.GetBookLoansFromDb();

            return bookLoans ?? null;
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
        public async Task<List<LibraryCard>> GetAllLibraryCards()
        {
            return null;
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
