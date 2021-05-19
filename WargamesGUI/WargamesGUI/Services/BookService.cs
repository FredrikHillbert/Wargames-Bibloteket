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
        public string exceptionMessage;
        public async Task<List<Book>> GetBooksFromDb()
        {
            var bookList = new List<Book>();

            using (SqlConnection con = new SqlConnection(theConString))
            {
                con.Open();
                using (var command = new SqlCommand(queryForBooks, con))
                {
                    using (var reader =  command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var book = new Book();

                            book.Id = Convert.ToInt32(reader["Id"]);
                            book.fk_Item_Id = Convert.ToInt32(reader["fk_Item_Id"]);
                            book.Title = reader["Title"].ToString();
                            book.ISBN = reader["ISBN"].ToString();
                            book.Publisher = reader["Publisher"].ToString();
                            book.Description = reader["Description"].ToString();
                            //book.Price = Convert.ToInt32(reader["Price"]);
                            book.Placement = reader["Placement"].ToString();
                            book.Author = reader["Author"].ToString();
                            book.InStock = Convert.ToInt32(reader["InStock"]);

                            bookList.Add(book);
                        }
                    }
                }
                return await Task.FromResult(bookList);
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
        public async Task<bool> AddNewBook(int item_id, string title, string ISBN, string publisher, string author,
                                           string description, int price, string placement)
        {
            bool success = true;

            try
            {

                using (SqlConnection con = new SqlConnection(theConString))
                {
                    await con.OpenAsync();

                    SqlCommand insertcmd = new SqlCommand("sp_AddBook", con);
                    insertcmd.CommandType = CommandType.StoredProcedure;
                    insertcmd.Parameters.Add("@fk_Item_Id", SqlDbType.Int).Value = item_id;
                    insertcmd.Parameters.Add("@Title", SqlDbType.VarChar).Value = title;
                    insertcmd.Parameters.Add("@ISBN", SqlDbType.VarChar).Value = ISBN;
                    insertcmd.Parameters.Add("@Publisher", SqlDbType.VarChar).Value = publisher;
                    insertcmd.Parameters.Add("@Author", SqlDbType.VarChar).Value = author;
                    insertcmd.Parameters.Add("@Description", SqlDbType.VarChar).Value = description;
                    insertcmd.Parameters.Add("@Price", SqlDbType.Int).Value = price;
                    insertcmd.Parameters.Add("@Placement", SqlDbType.VarChar).Value = placement;
                    await insertcmd.ExecuteNonQueryAsync();
                    return await Task.FromResult(success);
                }
            }

            catch
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
                    con.Open();
                    //await con.OpenAsync();

                    SqlCommand insertcmd = new SqlCommand("sp_RemoveBook", con);
                    insertcmd.CommandType = CommandType.StoredProcedure;

                    insertcmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;
                    insertcmd.Parameters.Add("@Reason", SqlDbType.VarChar).Value = reason;

                    await insertcmd.ExecuteNonQueryAsync();
                    return await Task.FromResult(success);
                }

                // Här ska det finnas en till SQL-sträng som tar bort objektet i alla tables där den är kopplad.
            }

            catch (Exception)
            {
                success = false;
                return await Task.FromResult(success);
            }

        }

        public async Task<bool> UpdateBook(int id, string title, string author, string publisher, string description,
                                           int price, string ISBN, string placement, int inStock)
        {
            bool success = true;

            try
            {
                using (SqlConnection con = new SqlConnection(theConString))
                {
                    await con.OpenAsync();

                    SqlCommand insertcmd = new SqlCommand("sp_UpdateBook", con);
                    insertcmd.CommandType = CommandType.StoredProcedure;

                    insertcmd.Parameters.Add("@Id", SqlDbType.VarChar).Value = id;
                    insertcmd.Parameters.Add("@Title", SqlDbType.VarChar).Value = title;
                    insertcmd.Parameters.Add("@Author", SqlDbType.VarChar).Value = author;
                    insertcmd.Parameters.Add("@Publisher", SqlDbType.VarChar).Value = publisher;
                    insertcmd.Parameters.Add("@Description", SqlDbType.VarChar).Value = description;
                    insertcmd.Parameters.Add("@Price", SqlDbType.Int).Value = price;
                    insertcmd.Parameters.Add("@ISBN", SqlDbType.VarChar).Value = ISBN;
                    insertcmd.Parameters.Add("@Placement", SqlDbType.VarChar).Value = placement;
                    insertcmd.Parameters.Add("@InStock", SqlDbType.Int).Value = inStock;

                    // Här ska det finnas en till SQL-sträng som uppdaterar objektet i alla tables där den finns.

                    await insertcmd.ExecuteNonQueryAsync();
                    return await Task.FromResult(success);
                }
            }

            catch
            {
                success = false;
                return await Task.FromResult(success);
            }
        }

        public async Task<List<Book>> Searching(string text)
        {
            List<Book> searchedValues = new List<Book>();

            using (SqlConnection con = new SqlConnection(theConString))
            {
                await con.OpenAsync();
                using (SqlCommand cmd = new SqlCommand("Search_Value", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@text", SqlDbType.VarChar).Value = text;

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {

                        while (await reader.ReadAsync())
                        {
                            var values = new Book();

                            values.ISBN = reader["ISBN"].ToString();
                            values.Title = reader["Title"].ToString();
                            values.Publisher = reader["Publisher"].ToString();
                            values.fk_Item_Id = Convert.ToInt32(reader["fk_Item_Id"]);
                            values.Placement = reader["Placement"].ToString();
                            values.Author = reader["Author"].ToString();
                            values.InStock = Convert.ToInt32(reader["InStock"]);

                            if (values.InStock == 0)
                            {
                                values.Status = "Unavailable";
                            }
                            else
                            {
                                values.Status = "Available";
                            }
                            
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
        public async Task<List<DeweySub>> GetDeweySubData()
        {
            var deweyList = new List<DeweySub>();

            var query = $"SELECT DeweySub_Id, SubCategoryName, fk_DeweyMain_Id FROM tblDeweySub";

            using (SqlConnection con = new SqlConnection(theConString))
            {
                con.Open();
                using (var command = new SqlCommand(query, con))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var dewey = new DeweySub();

                            dewey.SubCategoryName = reader["SubCategoryName"].ToString();
                            dewey.fk_DeweyMain_Id = Convert.ToInt32(reader["fk_DeweyMain_Id"]);
                            dewey.DeweySub_Id = Convert.ToInt32(reader["DeweySub_Id"]);
                                                        

                            deweyList.Add(dewey);
                        }
                    }
                }
                return await Task.FromResult(deweyList);
            }
        }

        public async Task<List<DeweyMain>> GetDeweyMainData()
        {
            var deweyList = new List<DeweyMain>();

            var query = $"SELECT DeweyMain_Id, MainCategoryName FROM tblDeweyMain";

            using (SqlConnection con = new SqlConnection(theConString))
            {
                con.Open();
                using (var command = new SqlCommand(query, con))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var dewey = new DeweyMain();

                            dewey.MainCategoryName = reader["MainCategoryName"].ToString();
                            dewey.DeweyMain_Id = Convert.ToInt32(reader["DeweyMain_Id"]);


                            deweyList.Add(dewey);
                        }
                    }
                }
                return await Task.FromResult(deweyList);
            }
        }
    }
}