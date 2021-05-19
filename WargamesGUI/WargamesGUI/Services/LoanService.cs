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

        public async Task<List<Book>> GetBorrowedBooksFromDb(int fk_LibraryCard)
        {
            var LoanedBooks = new List<Book>();

            string query = $"SELECT b.Title, b.Author, b.Publisher, b.Placement, b.InStock, bl.Loan_Id FROM tblBookLoan bl LEFT JOIN tblBook b ON b.Id = bl.fk_Book_Id WHERE {fk_LibraryCard} = bl.fk_LibraryCard_Id AND fk_BookLoanStatus_Id < 5";
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
                            LoanedBook.Author = reader["Publisher"].ToString();
                            LoanedBook.Publisher = reader["Publisher"].ToString();
                            LoanedBook.Placement = reader["Publisher"].ToString();
                            LoanedBook.InStock = Convert.ToInt32(reader["InStock"]);
                            LoanedBook.Loan_Id = Convert.ToInt32(reader["Loan_Id"]);

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
            string query = $"SELECT b.Title, b.Author, tu.Username, b.Placement, b.InStock, bl.ReturnDate, bl.ReturnedDate, bl.fk_BookLoanStatus_Id, bl.Loan_Id" +
                           $" FROM tblBookLoan bl" +
                           $" LEFT JOIN tblBook b" +
                           $" ON b.Id = bl.fk_Book_Id" +
                           $" LEFT JOIN tblUser tu" +
                           $" ON bl.fk_LibraryCard_Id = tu.fk_LibraryCard";
            using (SqlConnection con = new SqlConnection(theConString))
            {
                con.Open();
                using (var command = new SqlCommand(query, con))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
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
                            BorrowedBo.Loan_Id = Convert.ToInt32(reader["Loan_Id"]);
                            switch (BorrowedBo.fk_BookLoanStatus_Id = Convert.ToInt32(reader["fk_BookLoanStatus_Id"]))
                            {
                                case 1:
                                    BorrowedBo.Status = "Active";
                                    break;
                                case 2:
                                    BorrowedBo.Status = "Delayed";
                                    break;
                                case 3:
                                    BorrowedBo.Status = "Lost";
                                    break;
                                case 4:
                                    BorrowedBo.Status = "Stolen";
                                    break;
                                case 5:
                                    BorrowedBo.Status = "Returned";
                                    break;
                                case 6:
                                    BorrowedBo.Status = "Handled";
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
                                 $" SET fk_BookLoanStatus_Id = 6, ReturnedDate = GETDATE()" +
                                 $" WHERE Loan_Id = {loanID}" +
                                 $" SELECT b.Title, b.Author, b.Publisher, b.Placement, b.InStock, bl.ReturnDate, bl.ReturnedDate, bl.fk_BookLoanStatus_Id" +
                                 $" FROM tblBookLoan bl" +
                                 $" LEFT JOIN tblBook b" +
                                 $" ON b.Id = bl.fk_Book_Id";
            using (SqlConnection con = new SqlConnection(theConString))
            {
                con.Open();
                using (var command = new SqlCommand(querySelect, con))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
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

                    insertcmd.Parameters.Add("@fk_Book_Id", SqlDbType.Int).Value = book_id;
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

        /// <summary>
        /// Ändrar Status på ett Library Card.
        /// Måste skickas med: Den nya ID:n för Status och ID för kortet det gäller.
        /// </summary>
        /// <param name=""></param>
        /// <returns>
        /// Retunerar en bool som är true om det gick att ändra kortets status eller false 
        /// ifall det inte gick att ändra.
        /// </returns>
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
    }
}
