using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using WargamesGUI.Models;
using System.Threading.Tasks;

namespace WargamesGUI.Services
{
    class BookService : DbHandler
    {
        
        public async Task<List<Book>> GetBooksFromDb()
        {
            var bookList = new List<Book>();

            using (SqlConnection con = new SqlConnection(theConString))
            {
                con.Open();
                using (var command = new SqlCommand(queryForBookListPage, con))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var book = new Book();

                            book.fk_Item_Id = 1;
                            book.Title = reader["Title"].ToString();
                            book.ISBN = reader["ISBN"].ToString();
                            book.Publisher = reader["Publisher"].ToString();
                            book.Description = reader["Description"].ToString();
                            book.Price = Convert.ToInt32(reader["Price"]);
                            book.Placement = reader["Placement"].ToString();
                            bookList.Add(book);
                        }
                    }
                }

                return bookList;
            }
        }

        /// <summary>
        /// Adderar en ny bok till table tblBook. 
        /// Måste skickas med: Title, ISBN, Publisher, Description, Price, Placement till boken. 
        /// Om det är eBook eller inte - då behövs inte parametern Placement.
        /// </summary>
        /// <param name=""></param>
        /// <returns>
        /// Retunerar en bool som är true om det gick att lägga till boken eller false 
        /// ifall det inte gick att lägga till boken.'
        /// </returns>
        public async Task<bool> AddNewBook(string Title, string ISBN, string Publisher,
                                           string Description, int Price, string Placement, bool eBook)
        {

            bool success = true;

            try
            {
                using (SqlConnection con = new SqlConnection(theConString))
                {
                    if (eBook == false)
                    {
                        string sql =
                        $"INSERT INTO {theBookTableName}" +
                        $"(fk_category_Id, Title, ISBN, Publisher, Description, Price, Placement) " +
                        $"VALUES('1', '{Title}', '{ISBN}', '{Publisher}', '{Description}', '{Price}', '{Placement}')";

                        con.Open();
                        using (SqlCommand cmd = new SqlCommand(sql, con))
                        {
                            cmd.ExecuteNonQuery();
                        }
                    }
                    else if (eBook == true)
                    {
                        string sql =
                        $"INSERT INTO {theBookTableName}" +
                        $"(fk_category_Id, Title, ISBN, Publisher, Description, Price) " +
                        $"VALUES(2', '{Title}', '{ISBN}', '{Publisher}', '{Description}', '{Price}')";

                        con.Open();
                        using (SqlCommand cmd = new SqlCommand(sql, con))
                        {
                            cmd.ExecuteNonQuery();
                        }
                    }
                }

                return success;
            }

            catch (Exception)
            {
                success = false;
                return success;
            }
        }

        /// <summary>
        /// Tar bort en bok från table tblBook. 
        /// Måste skickas med: Bokens unika ID.
        /// </summary>
        /// <param name=""></param>
        /// <returns>
        /// Retunerar en bool som är true om det gick att ta bort boken eller false 
        /// ifall det inte gick att ta bort boken.'
        /// </returns>
        public async Task<bool> RemoveBook(int id)
        {
            bool success = true;

            try
            {
                using (SqlConnection con = new SqlConnection(theConString))
                {
                    string sql = 
                        $"DELETE FROM {theBookTableName} WHERE Id = {id}";

                    // Här ska det finnas en till SQL-sträng som lägger till det borttagna objektet i en "ObjectsRemoved"-table.
                    // Här ska det finnas en till SQL-sträng som tar bort objektet i alla tables där den är kopplad.

                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }

                return success;
            }

            catch (Exception)
            {
                success = false;
                return success;
            }
        }

        /// <summary>
        ///  
        /// 
        /// </summary>
        /// <param name=""></param>
        /// <returns>
        /// 
        /// 
        /// </returns>
        public async Task<bool> UpdateBook(int id, string Title, string ISBN, string Publisher, 
                                           string Description, int Price, string Placement)
        {
            bool success = true;

            try
            {
                using (SqlConnection con = new SqlConnection(theConString))
                {
                    string sql =
                        $"UPDATE {theBookTableName} " +
                        $"SET Title = {Title}, ISBN = {ISBN}," +
                        $"Publisher = {Publisher}, Description = {Description}" +
                        $"Price = {Price}, Placement = {Placement}" +
                        $"WHERE Id = {id}";

                    // Här ska det finnas en till SQL-sträng som uppdaterar objektet i alla tables där den finns.

                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.ExecuteNonQuery();
                    }

                }

                return success;
            }

            catch (Exception)
            {
                success = false;
                return success;
            }
        }
    }    
}
