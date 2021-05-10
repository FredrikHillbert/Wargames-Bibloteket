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
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var book = new Book();

                            book.fk_Item_Id = 1;
                            book.Title = reader["Title"].ToString();
                            book.ISBN = reader["ISBN"].ToString();
                            book.Publisher = reader["Publisher"].ToString();
                            book.Description = reader["Description"].ToString();
                            book.Price = Convert.ToInt32(reader["Price"]);
                            book.Placement = reader["Placement"].ToString();
                            book.Author = reader["Author"].ToString();

                            bookList.Add(book);
                        }
                    }
                }
                return await Task.FromResult(bookList);
            }
        }
        public async Task<List<Book>> GetBooksFromDb1()
        {
            var bookList = new List<Book>();

            using (SqlConnection con = new SqlConnection(theConString))
            {
                con.Open();
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
                            book.Author = reader["Author"].ToString();

                            bookList.Add(book);
                        }
                    }
                }
                return await Task.FromResult(bookList);
            }
        }
        public async Task<List<Book>> GetEbooksFromDb()
        {
            var eBookList = new List<Book>();

            using (SqlConnection con = new SqlConnection(theConString))
            {
                using (var command = new SqlCommand(queryForBooks, con))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
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
                return await Task.FromResult(eBookList);
            }
        }

        /// <summary>
        /// Adderar en ny bok till table tblBook. 
        /// Måste skickas med: Title, ISBN, Publisher, Description, Price, Placement till boken.
        /// Item_id berättar om det är Book eller Ebook.
        /// </summary>
        /// <param name=""></param>
        /// <returns>
        /// Retunerar en bool som är true om det gick att lägga till boken eller false 
        /// ifall det inte gick att lägga till boken.
        /// </returns>
        public async Task<bool> AddNewBook(int Item_id, string Title, string ISBN, string Publisher, string Author,
                                           string Description, int Price, string Placement)
        {
            bool success = true;

            try
            {

                using (SqlConnection con = new SqlConnection(theConString))
                {
                    await con.OpenAsync();

                    SqlCommand insertcmd = new SqlCommand("sp_AddBook", con);
                    insertcmd.CommandType = CommandType.StoredProcedure;
                    insertcmd.CommandText = "sp_AddBook";
                    insertcmd.Parameters.Add("@Title", SqlDbType.VarChar).Value = Title;
                    insertcmd.Parameters.Add("@ISBN", SqlDbType.VarChar).Value = ISBN;
                    insertcmd.Parameters.Add("@Publisher", SqlDbType.VarChar).Value = Publisher;
                    insertcmd.Parameters.Add("@Author", SqlDbType.VarChar).Value = Author;
                    insertcmd.Parameters.Add("@Description", SqlDbType.VarChar).Value = Description;
                    insertcmd.Parameters.Add("@Placement", SqlDbType.VarChar).Value = Placement;

                    await insertcmd.ExecuteNonQueryAsync();
                    success = true;
                }

                return await Task.FromResult(success);

                // Book
                if (Item_id == 1)
                {
                    using (SqlConnection con = new SqlConnection(theConString))
                    {
                        string query =
                            $"INSERT INTO {theBookTableName}" +
                            $"(fk_Item_Id, Title, ISBN, Publisher, Author, Description, Price, Placement) " +
                            $"VALUES('1', '{Title}', '{ISBN}', '{Publisher}', '{Author}' '{Description}', '{Price}', '{Placement}')";

                        await con.OpenAsync();

                        using (SqlCommand cmd = new SqlCommand(query, con))
                        {
                            await cmd.ExecuteNonQueryAsync();
                        }
                    }
                }

                // E-book
                else if (Item_id == 2)
                {
                    using (SqlConnection con = new SqlConnection(theConString))
                    {
                        string query =
                            $"INSERT INTO {theBookTableName}" +
                            $"(fk_Item_Id, Title, ISBN, Publisher, Author, Description, Price, Placement) " +
                            $"VALUES('2', '{Title}', '{ISBN}', '{Publisher}', '{Author}' '{Description}', '{Price}', '{Placement}')";

                        await con.OpenAsync();

                        using (SqlCommand cmd = new SqlCommand(query, con))
                        {
                            await cmd.ExecuteNonQueryAsync();
                        }
                    }
                }

                return await Task.FromResult(success);
            }

            catch (Exception)
            {
                success = false;
                return await Task.FromResult(success);
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
        public async Task<bool> RemoveBook(int id, string reason)
        {
            bool success = true;

            try
            {
                using (SqlConnection con = new SqlConnection(theConString))
                {
                    string query =
                        $"DELETE FROM {theBookTableName} WHERE Id = @id";

                    await con.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("id", id);
                        await cmd.ExecuteNonQueryAsync();
                    }

                    query = $"UPDATE {theRemovedItemTableName} " +
                            $"SET Reason = @reason " +
                            $"WHERE Id = @id";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("reason", reason);
                        cmd.Parameters.AddWithValue("id", id);
                        await cmd.ExecuteNonQueryAsync();
                    }

                    // Här ska det finnas en till SQL-sträng som tar bort objektet i alla tables där den är kopplad.
                }

                return await Task.FromResult(success);
            }

            catch (Exception)
            {
                success = false;
                return await Task.FromResult(success);
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

                    string query =
                        $"UPDATE {theBookTableName} " +
                        $"SET Title = {Title}, ISBN = {ISBN}, " +
                        $"Publisher = {Publisher}, Description = {Description} " +
                        $"Price = {Price}, Placement = {Placement} " +
                        $"WHERE Id = {id}";

                    // Ändra för E-bok?
                    // Här ska det finnas en till SQL-sträng som uppdaterar objektet i alla tables där den finns.

                    await con.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        await cmd.ExecuteNonQueryAsync();
                    }
                }

                return await Task.FromResult(success);
            }

            catch (Exception)
            {
                success = false;
                return await Task.FromResult(success);
            }
        }
        public async Task<List<Book>> Searching(string text)
        {
            List<Book> searchedValues = new List<Book>();
            string query =  $"SELECT * FROM tblItem ti " +
                            $"LEFT JOIN tblBook tb ON tb.fk_Item_Id = ti.Item_Id " +
                            $"LEFT JOIN tblEvent te ON te.fk_Item_Id = ti.Item_Id " +
                            $"WHERE CONCAT_WS('', tb.Title, tb.ISBN, tb.Publisher, tb.fk_Item_Id, te.fk_Item_Id, tb.Price, tb.Placement, tb.Author, ti.TypeOfItem, te.Title, te.Description) " +
                            $"LIKE '%{text}%'";

            string query2 = $"SELECT* FROM tblBook WHERE CONCAT_WS('', Title, ISBN, Publisher, fk_Item_Id, Price, Placement, Author) LIKE '%{text}%'";



            using (SqlConnection con = new SqlConnection(theConString))
            {
                await con.OpenAsync();
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var values = new Book();

                            values.ISBN = reader["ISBN"].ToString();
                            values.Title = reader["Title"].ToString();
                            values.Publisher = reader["Publisher"].ToString();
                            values.fk_Item_Id = Convert.ToInt32(reader["fk_Item_Id"]);
                            values.Price = Convert.ToInt32(reader["Price"]);
                            values.Placement = reader["Placement"].ToString();
                            values.Author = reader["Author"].ToString();
                            
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