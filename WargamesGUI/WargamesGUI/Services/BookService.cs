using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using WargamesGUI.Models;

namespace WargamesGUI.Services
{
    public class BookService : DbHandler
    {
        public async Task<List<Book>> GetBooksFromDb()
        {
            var bookList = new List<Book>();

            using (SqlConnection con = new SqlConnection(theConString))
            {
                using (var command = new SqlCommand(queryForBooks, con))
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
        public async Task<List<Book>> GetEbooksFromDb()
        {
            var eBookList = new List<Book>();

            using (SqlConnection con = new SqlConnection(theConString))
            {
                using (var command = new SqlCommand(queryForBooks, con))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var eBbook = new Book();

                            eBbook.fk_Item_Id = 2;
                            eBbook.Title = reader["Title"].ToString();
                            eBbook.ISBN = reader["ISBN"].ToString();
                            eBbook.Publisher = reader["Publisher"].ToString();
                            eBbook.Description = reader["Description"].ToString();
                            eBbook.Price = Convert.ToInt32(reader["Price"]);

                            eBookList.Add(eBbook);
                        }
                    }
                }
                return eBookList;
            }
        }

        /// <summary>
        /// Adderar en ny bok till table tblBook. 
        /// Måste skickas med: Title, ISBN, Publisher, Description, Price, Placement till boken.
        /// </summary>
        /// <param name=""></param>
        /// <returns>
        /// Retunerar en bool som är true om det gick att lägga till boken eller false 
        /// ifall det inte gick att lägga till boken.
        /// </returns>
        public async Task<bool> AddNewBook(string Title, string ISBN, string Publisher,
                                           string Description, int Price, string Placement)
        {
            bool success = true;

            try
            {
                using (SqlConnection con = new SqlConnection(theConString))
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

                return success;
            }

            catch (Exception)
            {
                success = false;
                return success;
            }
        }

        /// <summary>
        /// Adderar en ny E-bok till table tblBook. 
        /// Måste skickas med: Title, ISBN, Publisher, Description, Price till E-boken. 
        /// </summary>
        /// <param name=""></param>
        /// <returns>
        /// Retunerar en bool som är true om det gick att lägga till E-boken eller false 
        /// ifall det inte gick att lägga till boken.
        /// </returns>
        public async Task<bool> AddNewEbook(string Title, string ISBN, string Publisher,
                                           string Description, int Price)
        {
            bool success = true;

            try
            {
                using (SqlConnection con = new SqlConnection(theConString))
                {
                    string sql =
                        $"INSERT INTO {theBookTableName}" +
                        $"(fk_category_Id, Title, ISBN, Publisher, Description, Price, Placement) " +
                        $"VALUES('1', '{Title}', '{ISBN}', '{Publisher}', '{Description}', '{Price}')";

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
        /// Tar bort en bok från table tblBook. 
        /// Måste skickas med: Bokens unika ID.
        /// </summary>
        /// <param name=""></param>
        /// <returns>
        /// Retunerar en bool som är true om det gick att ta bort boken eller false 
        /// ifall det inte gick att ta bort boken.'
        /// </returns>
        public bool RemoveBook(int id)
        {
            bool success = true;

            try
            {
                using (SqlConnection con = new SqlConnection(theConString))
                {
                    string sql =
                        $"DELETE FROM {theBookTableName} WHERE Id = {id}";

                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    // Här ska det finnas en till SQL-sträng som lägger till det borttagna objektet i en "ObjectsRemoved"-table.
                    // Här ska det finnas en till SQL-sträng som tar bort objektet i alla tables där den är kopplad.
                }

                return success;
            }

            catch (Exception)
            {
                success = false;
                return success;
            }
        }

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

                    // Ändra för E-bok?
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
        public async Task<List<Book>> Searching(string text)
        {
            List<Book> searchedValues = new List<Book>();
            string query = $"SELECT * FROM tblBook WHERE CONCAT_WS('',Title, ISBN, Publisher, fk_Item_Id, Price, Placement) LIKE '%{text}%'";


            using (SqlConnection con = new SqlConnection(theConString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var values = new Book();

                            values.ISBN = reader["ISBN"].ToString();
                            values.Title = reader["Title"].ToString();
                            values.Publisher = reader["Publisher"].ToString();
                            values.fk_Item_Id = Convert.ToInt32(reader["fk_Item_Id"]);
                            values.Price = Convert.ToInt32(reader["Price"]);
                            values.Placement = reader["Placement"].ToString();

                            switch (values.fk_Item_Id)
                            {
                                case 1:
                                    values.Category = "Book";
                                    break;
                                case 2:
                                    values.Category = "Ebook";
                                    break;
                                case 3:
                                    values.Category = "Seminar";
                                    break;
                                default:
                                    break;
                            }


                            searchedValues.Add(values);
                        }

                    }
                }
                return await Task.FromResult(searchedValues);
            }
        }
    }
}