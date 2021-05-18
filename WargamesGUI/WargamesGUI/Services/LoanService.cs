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
        
        public async Task<List<Book>> GetLoanedBooksFromDb(int fk_LibraryCard)
        {
            var LoanedBooks = new List<Book>();

            string query = $"SELECT b.Title, b.Author, b.Publisher, b.Placement, b.InStock, bl.Loan_Id FROM tblBookLoan bl LEFT JOIN tblBook b ON b.Id = bl.fk_Book_Id WHERE {fk_LibraryCard} = bl.fk_LibraryCard_Id AND fk_BookLoanStatus_Id< 5";
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
    }
}
