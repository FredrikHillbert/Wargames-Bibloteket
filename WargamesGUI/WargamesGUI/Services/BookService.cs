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
                    using (var reader = command.ExecuteReader())
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
                            book.Price = (reader["Price"]).ToString();
                            book.Placement = reader["Placement"].ToString();
                            book.Author = reader["Author"].ToString();
                            book.Available_copies = Convert.ToInt32(reader["Available_copies"]);
                            book.Category = reader["Category"].ToString();

                            if (book.fk_Item_Id == 1)
                            {
                                book.TypeOfBook = "Bok";
                            }
                            else if (book.fk_Item_Id == 2)
                            {
                                book.TypeOfBook = "E-bok";
                            }

                            bookList.Add(book);
                        }
                    }
                }
                return await Task.FromResult(bookList);
            }
        }

        public async Task<List<BookCopy>> GetBookCopiesFromDb()
        {
            var bookCopies = new List<BookCopy>();

            using (SqlConnection con = new SqlConnection(theConString))
            {
                await con.OpenAsync();
                using (var command = new SqlCommand(queryForBookCopiesExtra, con))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var book = new BookCopy();

                            book.Copy_Id = Convert.ToInt32(reader["Copy_Id"]);
                            book.fk_Book_Id = Convert.ToInt32(reader["fk_Book_Id"]);
                            book.fk_Condition_Id = Convert.ToInt32(reader["fk_Condition_Id"]);
                            book.fk_Availability = Convert.ToInt32(reader["fk_Availability"]);

                            book.Condition = reader["ConditionType"].ToString();
                            book.Availability = reader["Status"].ToString();

                            bookCopies.Add(book);
                        }
                    }
                }
                return await Task.FromResult(bookCopies);
            }
        }

        public async Task<bool> AddNewBook(int item_id, string title, string ISBN, string publisher, string author,
                                           string description, int price, string placement, string category)
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
                    insertcmd.Parameters.Add("@category", SqlDbType.VarChar).Value = category;
                    await insertcmd.ExecuteNonQueryAsync();

                    return success;
                }
            }
            catch
            {
                success = false;
                return success;
            }


        }

        public async Task<bool> RemoveBookCopy(int id, string reason)
        {
            bool success = true;

            using (SqlConnection con = new SqlConnection(theConString))
            {
                await con.OpenAsync();

                SqlCommand insertcmd = new SqlCommand("sp_RemoveBookCopy", con);
                insertcmd.CommandType = CommandType.StoredProcedure;

                insertcmd.Parameters.Add("@Copy_Id", SqlDbType.Int).Value = id;
                insertcmd.Parameters.Add("@Reason", SqlDbType.VarChar).Value = reason;
                insertcmd.Parameters.Add("@returnValue", SqlDbType.Int).Direction = ParameterDirection.Output;

                await insertcmd.ExecuteNonQueryAsync();

                var retval = (int)insertcmd.Parameters["@returnValue"].Value;
                if (retval == 0)
                {
                    return success;
                }
                else
                {
                    success = false;
                    return success;
                }

            }

        }
        public async Task<bool> UpdateBookCopy(int Copy_Id, int fk_Condition_Id)
        {
            bool success = true;
            try
            {
                using (SqlConnection con = new SqlConnection(theConString))
                {
                    await con.OpenAsync();

                    SqlCommand insertcmd = new SqlCommand("sp_UpdateBookCopy", con);
                    insertcmd.CommandType = CommandType.StoredProcedure;

                    insertcmd.Parameters.Add("@Copy_Id", SqlDbType.Int).Value = Copy_Id;
                    insertcmd.Parameters.Add("@newCondition_Id", SqlDbType.VarChar).Value = fk_Condition_Id;

                    await insertcmd.ExecuteNonQueryAsync();
                    return success;

                }
            }
            catch (Exception ex )
            {
                var messege = ex.Message;
                success = false;
                return success;
            }
            

        }
        public async Task<bool> UpdateBook(int id, int item_id, string title, string author, string publisher, string description,
                                           string ISBN, string placement, string category)
        {
            bool success = true;

            try
            {
                using (SqlConnection con = new SqlConnection(theConString))
                {
                    await con.OpenAsync();

                    SqlCommand insertcmd = new SqlCommand("sp_UpdateBook", con);
                    insertcmd.CommandType = CommandType.StoredProcedure;

                    insertcmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;
                    insertcmd.Parameters.Add("@fk_Item_Id", SqlDbType.Int).Value = item_id;
                    insertcmd.Parameters.Add("@Title", SqlDbType.VarChar).Value = title;
                    insertcmd.Parameters.Add("@Author", SqlDbType.VarChar).Value = author;
                    insertcmd.Parameters.Add("@Publisher", SqlDbType.VarChar).Value = publisher;
                    insertcmd.Parameters.Add("@Description", SqlDbType.VarChar).Value = description;
                    //insertcmd.Parameters.Add("@Price", SqlDbType.Int).Value = price;
                    insertcmd.Parameters.Add("@ISBN", SqlDbType.VarChar).Value = ISBN;
                    insertcmd.Parameters.Add("@Placement", SqlDbType.VarChar).Value = placement;
                    insertcmd.Parameters.Add("@category", SqlDbType.VarChar).Value = category;
                    //insertcmd.Parameters.Add("@InStock", SqlDbType.Int).Value = inStock;

                    // Här ska det finnas en till SQL-sträng som uppdaterar objektet i alla tables där den finns.

                    await insertcmd.ExecuteNonQueryAsync();
                    return await Task.FromResult(success);
                }
            }

            catch (Exception ex)
            {
                string a = ex.Message;
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
                            values.Available_copies = Convert.ToInt32(reader["Available_copies"]);
                            values.Category = reader["Category"].ToString();
                            values.Description = reader["Description"].ToString();

                            if (values.Available_copies == 0)
                            {
                                values.Status = "Inte tillgänglig";
                            }
                            else
                            {
                                values.Status = "Tillgänglig";
                            }

                            if (values.fk_Item_Id == 1)
                            {
                                values.TypeOfBook = "Bok";
                            }
                            else if (values.fk_Item_Id == 2)
                            {
                                values.TypeOfBook = "E-bok";
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

            using (SqlConnection con = new SqlConnection(theConString))
            {
                await con.OpenAsync();
                using (var command = new SqlCommand(queryForDeweySub, con))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
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

            using (SqlConnection con = new SqlConnection(theConString))
            {
                await con.OpenAsync();
                using (var command = new SqlCommand(queryForDeweyMain, con))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
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
        public async Task<List<RemovedItem>> GetRemovedBooksFromDB()
        {
            var removedbooksfromDB = new List<RemovedItem>();

            using (SqlConnection con = new SqlConnection(theConString))
            {
                await con.OpenAsync();
                using (var command = new SqlCommand(queryForDeletedBooks, con))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var removedbooks = new RemovedItem();

                            removedbooks.Condition = reader["Condition"].ToString();
                            removedbooks.Title = reader["Title"].ToString();
                            removedbooks.Reason = reader["Reason"].ToString();

                            removedbooksfromDB.Add(removedbooks);
                        }
                    }
                }
                return await Task.FromResult(removedbooksfromDB);
            }
        }
    }
}