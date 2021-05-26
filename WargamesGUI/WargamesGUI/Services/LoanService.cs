using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using WargamesGUI.Models;

namespace WargamesGUI.Services
{
    public class LoanService : DbHandler
    {
        public string exceptionMessage;

        public async Task<List<LibraryCard>> GetLibraryCardsFromDb()
        {
            var LibraryCards = new List<LibraryCard>();

            using (SqlConnection con = new SqlConnection(theConString))
            {
                await con.OpenAsync();
                using (var command = new SqlCommand(queryForLibraryCards, con))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var LibraryCard = new LibraryCard();

                            LibraryCard.LibraryCard_Id = Convert.ToInt32(reader["LibraryCard_Id"]);
                            LibraryCard.CardNumber = reader["CardNumber"].ToString();
                            LibraryCard.fk_Status_Id = Convert.ToInt32(reader["fk_Status_Id"]);
                            LibraryCard.Status_Level = reader["Status_Level"].ToString();

                            LibraryCards.Add(LibraryCard);
                        }
                    }
                }
                return await Task.FromResult(LibraryCards);
            }
        }

        public async Task<List<BookLoan>> GetBookLoans()
        {
            var BookLoans = new List<BookLoan>();

            string query = $"SELECT * FROM tblBookLoan";

            using (SqlConnection con = new SqlConnection(theConString))
            {
                await con.OpenAsync();
                using (var command = new SqlCommand(query, con))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var BookLoan = new BookLoan();

                            BookLoan.Loan_Id = Convert.ToInt32(reader["Loan_Id"]);
                            //BookLoan.fk_BookCopy_Id = Convert.ToInt32(reader["fk_BookCopy_Id"]);
                            BookLoan.fk_BookLoanStatus_Id = Convert.ToInt32(reader["fk_BookLoanStatus_Id"]);
                            BookLoan.fk_LibraryCard_Id = Convert.ToInt32(reader["fk_LibraryCard_Id"]);
                            BookLoan.ReturnDate = Convert.ToDateTime(reader["ReturnDate"]);
                            BookLoan.ReturnedDate = Convert.ToDateTime(reader["ReturnDate"]);

                            BookLoans.Add(BookLoan);
                        }
                        return await Task.FromResult(BookLoans);
                    }
                }
            }
        }
        public async Task<List<Book>> GetBorrowedBooksFromDb(int fk_LibraryCard)
        {
            var LoanedBooks = new List<Book>();

            string query = $"SELECT * FROM tblBookLoan bl LEFT JOIN tblBookCopy bc ON bc.Copy_Id = bl.fk_BookCopy_Id " +
                           $"LEFT JOIN tblBook b ON b.Id in(select bc.fk_Book_Id from tblBookCopy " +
                           $"WHERE bc.fk_Book_Id = b.Id) WHERE bl.fk_LibraryCard_Id = 18 " +
                           $"AND fk_BookLoanStatus_Id< 5; ";
            using (SqlConnection con = new SqlConnection(theConString))
            {
                con.Open();
                using (var command = new SqlCommand(query, con))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var LoanedBook = new Book();

                            LoanedBook.Title = reader["Title"].ToString();
                            LoanedBook.Author = reader["Author"].ToString();
                            LoanedBook.Publisher = reader["Publisher"].ToString();
                            LoanedBook.Placement = reader["Placement"].ToString();
                            LoanedBook.InStock = Convert.ToInt32(reader["InStock"]);
                            LoanedBook.Loan_Id = Convert.ToInt32(reader["Loan_Id"]);
                            LoanedBook.Category = reader["Category"].ToString();
                            LoanedBook.Description = reader["Description"].ToString();

                            LoanedBooks.Add(LoanedBook);
                        }
                    }
                }
                return await Task.FromResult(LoanedBooks);
            }
        }
        public async Task<List<Book>> GetBorrowedBooksFromDbLibrarian()
        {
            var BorrowedBooks = new List<Book>();
            string query = $"SELECT b.Title, b.Author, tu.Username, b.Placement, b.InStock, bl.ReturnDate,  bl.ReturnedDate, bl.fk_BookLoanStatus_Id, b.Available_copies, b.ISBN" +
                           $" FROM tblBookLoan bl" +
                           $" LEFT JOIN tblBookCopy tbc" +
                           $" ON tbc.Copy_Id = bl.fk_BookCopy_Id" +
                           $" LEFT JOIN tblBook b" +
                           $" ON b.Id in(select tbc.fk_Book_Id from tblBookCopy WHERE tbc.fk_Book_Id = b.Id)" +
                           $" LEFT JOIN tblUser tu" +
                           $" ON bl.fk_LibraryCard_Id = tu.fk_LibraryCard" +
                           $" WHERE Checked_In = 1" +
                           $" ORDER BY Title";

            using (SqlConnection con = new SqlConnection(theConString))
            {
                await con.OpenAsync();
                using (var command = new SqlCommand(query, con))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var BorrowedBo = new Book();
                            //var user = new User();

                            BorrowedBo.Username = reader["Username"].ToString();
                            //user.Cardnumber = Convert.ToInt32(reader["fk_LibraryCard_Id"]);
                            BorrowedBo.Title = reader["Title"].ToString();
                            BorrowedBo.Author = reader["Author"].ToString();
                            //BorrowedBo.Publisher = reader["Publisher"].ToString();
                            BorrowedBo.Placement = reader["Placement"].ToString();
                            BorrowedBo.InStock = Convert.ToInt32(reader["InStock"]);
                            //BorrowedBo.Loan_Id = Convert.ToInt32(reader["Loan_Id"]);
                            BorrowedBo.ISBN = reader["ISBN"].ToString();
                            BorrowedBo.Available_copies = Convert.ToInt32(reader["Available_copies"]);
                            switch (BorrowedBo.fk_BookLoanStatus_Id = Convert.ToInt32(reader["fk_BookLoanStatus_Id"]))
                            {
                                case 1:
                                    BorrowedBo.Status = "Utlånad";
                                    break;
                                case 2:
                                    BorrowedBo.Status = "Försenad";
                                    break;
                                case 3:
                                    BorrowedBo.Status = "Försvunnen";
                                    break;
                                case 4:
                                    BorrowedBo.Status = "Stulen";
                                    break;
                                

                            }

                            BorrowedBo.ReturnDate = Convert.ToDateTime(reader["ReturnDate"]);

                            bool a = DateTime.TryParse(reader["ReturnedDate"].ToString(), out DateTime dateTime);
                            if (a == true)
                            {
                                BorrowedBo.ReturnedDate = Convert.ToDateTime(reader["ReturnedDate"]);
                            }

                            BorrowedBooks.Add(BorrowedBo);
                        }
                    }
                }
                return await Task.FromResult(BorrowedBooks);
            }
        }

        public async Task<List<Book>> UpdateBorrowedBooksFromDbLibrarian(int loanID)
        {
            var BorrowedBooks = new List<Book>();

            string querySelect = $"UPDATE tblBookLoan" +
                                 $" SET fk_BookLoanStatus_Id = 1, ReturnedDate = GETDATE()" +
                                 $" WHERE Loan_Id = {loanID}" +
                                 $" SELECT b.Title, b.Author, b.Publisher, b.Placement, b.InStock, bl.ReturnDate, bl.ReturnedDate, bl.fk_BookLoanStatus_Id" +
                                 $" FROM tblBookLoan bl" +
                                 $" LEFT JOIN tblBook b" +
                                 $" ON b.Id = bl.fk_Book_Id";


            using (SqlConnection con = new SqlConnection(theConString))
            {
                await con.OpenAsync();
                using (var command = new SqlCommand(querySelect, con))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var BorrowedBo = new Book();

                            BorrowedBo.Title = reader["Title"].ToString();
                            BorrowedBo.Author = reader["Author"].ToString();
                            BorrowedBo.Publisher = reader["Publisher"].ToString();
                            BorrowedBo.Placement = reader["Placement"].ToString();
                            BorrowedBo.InStock = Convert.ToInt32(reader["InStock"]);
                            BorrowedBo.fk_BookLoanStatus_Id = Convert.ToInt32(reader["fk_BookLoanStatus_Id"]);
                            BorrowedBo.Status = "Handled";
                            BorrowedBo.ReturnDate = Convert.ToDateTime(reader["ReturnDate"]);


                            BorrowedBooks.Add(BorrowedBo);

                        }
                    }
                }
                return await Task.FromResult(BorrowedBooks);
            }
        }

        public async Task<int> LoanBook(int book_id, int fk_LibraryCard)
        {
            int returnValue = 0;

            try
            {
                using (SqlConnection con = new SqlConnection(theConString))
                {
                    await con.OpenAsync();

                    SqlCommand insertcmd = new SqlCommand("sp_LoanBook", con);
                    insertcmd.CommandType = CommandType.StoredProcedure;

                    insertcmd.Parameters.Add("@bookId", SqlDbType.Int).Value = book_id;
                    insertcmd.Parameters.Add("@fk_LibraryCard", SqlDbType.Int).Value = fk_LibraryCard;
                    insertcmd.Parameters.Add("@returnValue", SqlDbType.VarChar).Direction = ParameterDirection.ReturnValue;


                    await insertcmd.ExecuteNonQueryAsync();
                    returnValue = (int)insertcmd.Parameters["@returnValue"].Value;

                    return await Task.FromResult(returnValue);
                }
            }

            catch (Exception ex)
            {
                exceptionMessage = ex.Message;
                return await Task.FromResult(returnValue);
            }
        }

        public async Task<bool> ChangeCardStatus(int fk_Status_Id, int LibraryCard_Id)
        {
            bool success = true;

            try
            {
                using (SqlConnection con = new SqlConnection(theConString))
                {
                    await con.OpenAsync();

                    SqlCommand insertcmd = new SqlCommand("sp_ChangeCardStatus", con);
                    insertcmd.CommandType = CommandType.StoredProcedure;

                    insertcmd.Parameters.Add("@fk_Status_Id", SqlDbType.Int).Value = fk_Status_Id;
                    insertcmd.Parameters.Add("@LibraryCard_Id", SqlDbType.Int).Value = LibraryCard_Id;

                    await insertcmd.ExecuteNonQueryAsync();
                    return await Task.FromResult(success);
                }
            }

            catch (Exception)
            {
                success = false;
                return await Task.FromResult(success);
            }
        }
        public async Task<bool> ChangeBookLoanStatus(int Loan_Id)
        {
            bool success = true;

            try
            {
                using (SqlConnection con = new SqlConnection(theConString))
                {
                    await con.OpenAsync();

                    SqlCommand insertcmd = new SqlCommand("sp_ChangeBookLoanStatus", con);
                    insertcmd.CommandType = CommandType.StoredProcedure;

                    insertcmd.Parameters.Add("@Loan_Id", SqlDbType.Int).Value = Loan_Id;


                    await insertcmd.ExecuteNonQueryAsync();
                    return await Task.FromResult(success);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<bool> ManualAddLibraryCard(int user_id)
        {
            bool success = true;

            try
            {
                using (SqlConnection con = new SqlConnection(theConString))
                {
                    await con.OpenAsync();

                    SqlCommand insertcmd = new SqlCommand("sp_ManualAddLibraryCard", con);
                    insertcmd.CommandType = CommandType.StoredProcedure;

                    insertcmd.Parameters.Add("@User_Id", SqlDbType.Int).Value = user_id;

                    await insertcmd.ExecuteNonQueryAsync();

                    return await Task.FromResult(success);
                }
            }

            catch (Exception)
            {
                success = false;
                return await Task.FromResult(success);
            }
        }
        public async Task<List<Book>> GetHandledBooksFromLibrarianDb()
        {
            var HandledBooks = new List<Book>();
            string query = $"SELECT b.Title, b.Author, tu.Username, b.Placement, b.InStock, bl.ReturnDate,  bl.ReturnedDate, bl.fk_BookLoanStatus_Id, b.Available_copies, b.ISBN, tbc.fk_Condition_Id" +
                           $" FROM tblBookLoan bl" +
                           $" LEFT JOIN tblBookCopy tbc" +
                           $" ON tbc.Copy_Id = bl.fk_BookCopy_Id" +
                           $" LEFT JOIN tblBook b" +
                           $" ON b.Id in(select tbc.fk_Book_Id from tblBookCopy WHERE tbc.fk_Book_Id = b.Id)" +
                           $" LEFT JOIN tblUser tu" +
                           $" ON bl.fk_LibraryCard_Id = tu.fk_LibraryCard" +
                           $" WHERE Checked_In = 2" +
                           $" ORDER BY Title";

            using (SqlConnection con = new SqlConnection(theConString))
            {
                await con.OpenAsync();
                using (var command = new SqlCommand(query, con))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var HandledBo = new Book();   
                            HandledBo.Title = reader["Title"].ToString();
                            HandledBo.Author = reader["Author"].ToString();                            
                            HandledBo.Placement = reader["Placement"].ToString();
                            HandledBo.InStock = Convert.ToInt32(reader["InStock"]);                          
                            HandledBo.ISBN = reader["ISBN"].ToString();
                            
                            switch (HandledBo.BookCondition = Convert.ToInt32(reader["fk_Condition_Id"]))
                            {
                                case 1:
                                    HandledBo.BookConditionString = "Ny";
                                    break;
                                case 2:
                                    HandledBo.BookConditionString = "Som ny";
                                    break;
                                case 3:
                                    HandledBo.BookConditionString = "Väldigt bra";
                                    break;
                                case 4:
                                    HandledBo.BookConditionString = "Bra";
                                    break;
                                case 5:
                                    HandledBo.BookConditionString = "Acceptabel";
                                    break;
                                case 6:
                                    HandledBo.BookConditionString = "Sliten";
                                    break;
                                case 7:
                                    HandledBo.BookConditionString = "Förstörd";
                                    break;
                            }
                            switch (HandledBo.fk_BookLoanStatus_Id = Convert.ToInt32(reader["fk_BookLoanStatus_Id"]))
                            {
                                
                                case 2:
                                    HandledBo.Status = "Försenad";
                                    break;
                                case 3:
                                    HandledBo.Status = "Försvunnen";
                                    break;
                                case 4:
                                    HandledBo.Status = "Stulen";
                                    break;
                                case 5:
                                    HandledBo.Status = "Inskannad";
                                    break;


                            }                         

                            bool a = DateTime.TryParse(reader["ReturnedDate"].ToString(), out DateTime dateTime);
                            if (a == true)
                            {
                                HandledBo.ReturnedDate = Convert.ToDateTime(reader["ReturnedDate"]);
                            }

                            HandledBooks.Add(HandledBo);
                        }
                    }
                }
                return await Task.FromResult(HandledBooks);
            }
        }
    }
}
