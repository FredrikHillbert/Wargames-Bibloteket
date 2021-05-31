using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using WargamesGUI.Models;
using System.Configuration;
using System.Data;

namespace WargamesGUI.Services
{
    public enum TableName
    {
        tblUser,
        tblPrivilegeLevel,
        tblBook,
        tblEvent,
        tblRemovedItem,
        tblDeweyMain,
        tblDeweySub,
        tblBookLoan,
        tblBookLoanStatus,
        tblBookCopy,
        tblAvailability,
        tblConditionStatus,
        tblLibraryCard,
        tblLibraryCardStatus,
        tblItem,
    }
    public class DbService : IDbService
    {
        public string theConString;
        public string theConStringTest;

        public DbService()
        {
            theConString = ConfigurationManager.ConnectionStrings[1].ConnectionString;
            theConStringTest = ConfigurationManager.ConnectionStrings[2].ConnectionString;
        }
        //===============||
        //               ||
        //     BOOK      ||
        //               ||
        //===============||
        public async Task<List<Book2>> GetBooksFromDb()
        {

            var bookList = new List<Book2>();

            using (SqlConnection con = new SqlConnection(theConString))
            {
                await con.OpenAsync();
                using (var command = new SqlCommand($"SELECT * FROM {TableName.tblBook}", con))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var book = new Book2();

                            book.Id = Convert.ToInt32(reader["Id"]);
                            book.fk_Item_Id = Convert.ToInt32(reader["fk_Item_Id"]);
                            book.Title = reader["Title"].ToString();
                            book.ISBN = reader["ISBN"].ToString();
                            book.Publisher = reader["Publisher"].ToString();
                            book.Description = reader["Description"].ToString();
                            book.Placement = Convert.ToInt32(reader["Placement"]);
                            book.Author = reader["Author"].ToString();
                            //book.Price = Convert.ToInt32(reader["Price"]);

                            bookList.Add(book);
                        }
                    }
                }
                return await Task.FromResult(bookList);
            }
        }
        // !!
        // Behöver uppdatera mer än såhär i "UpdateBookInDb" - de nya objekten/listan som ligger till "Book2" ska också kunna ändras.
        // !!
        public async Task<bool> UpdateBookInDb(Book2 book)
        {
            bool success = true;

            try
            {
                using (SqlConnection con = new SqlConnection(theConString))
                {

                    await con.OpenAsync();

                    SqlCommand insertcmd = new SqlCommand("sp_UpdateBook", con);
                    insertcmd.CommandType = CommandType.StoredProcedure;

                    insertcmd.Parameters.Add("@Id", SqlDbType.Int).Value = book.Id;
                    insertcmd.Parameters.Add("@fk_Item_Id", SqlDbType.Int).Value = book.fk_Item_Id;
                    insertcmd.Parameters.Add("@Title", SqlDbType.VarChar).Value = book.Title ?? null;
                    insertcmd.Parameters.Add("@Author", SqlDbType.VarChar).Value = book.Author ?? null;
                    insertcmd.Parameters.Add("@Publisher", SqlDbType.VarChar).Value = book.Publisher ?? null;
                    insertcmd.Parameters.Add("@Description", SqlDbType.VarChar).Value = book.Description ?? null;
                    //insertcmd.Parameters.Add("@Price", SqlDbType.Int).Value = book.Price ?? null;
                    insertcmd.Parameters.Add("@ISBN", SqlDbType.VarChar).Value = book.ISBN ?? null;
                    insertcmd.Parameters.Add("@Placement", SqlDbType.VarChar).Value = book.Placement;
                    //insertcmd.Parameters.Add("@InStock", SqlDbType.Int).Value = book.InStock ?? null

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

        // !!
        // Behöver lägga till mer än såhär i "AddBookToDb" - de nya objekten/listan som ligger till "Book2" ska också läggas till.
        // !!
        public async Task<bool> AddBookToDb(Book2 book)
        {
            bool success = true;
            try
            {

                using (SqlConnection con = new SqlConnection(theConString))
                {
                    await con.OpenAsync();

                    SqlCommand insertcmd = new SqlCommand("sp_AddBook", con);
                    insertcmd.CommandType = CommandType.StoredProcedure;
                    insertcmd.Parameters.Add("@fk_Item_Id", SqlDbType.Int).Value = book.fk_Item_Id;
                    insertcmd.Parameters.Add("@Title", SqlDbType.VarChar).Value = book.Title;
                    insertcmd.Parameters.Add("@ISBN", SqlDbType.VarChar).Value = book.ISBN;
                    insertcmd.Parameters.Add("@Publisher", SqlDbType.VarChar).Value = book.Publisher;
                    insertcmd.Parameters.Add("@Author", SqlDbType.VarChar).Value = book.Author;
                    insertcmd.Parameters.Add("@Description", SqlDbType.VarChar).Value = book.Description;
                    insertcmd.Parameters.Add("@Price", SqlDbType.Int).Value = book.Price;
                    insertcmd.Parameters.Add("@Placement", SqlDbType.VarChar).Value = book.Placement;
                    //insertcmd.Parameters.Add("@category", SqlDbType.VarChar).Value = book.Category;
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
        public async Task<List<Item>> GetItemTypeFromDb()
        {
            var items = new List<Item>();

            using (SqlConnection con = new SqlConnection(theConString))
            {
                await con.OpenAsync();

                using (var command = new SqlCommand($"SELECT * FROM {TableName.tblItem}", con))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var item = new Item();

                            item.Item_Id = Convert.ToInt32(reader["Item_Id"]);
                            item.TypeOfItem = reader["TypeOfItem"].ToString();

                            items.Add(item);
                        }
                    }
                }
                return await Task.FromResult(items);
            }
        }

        //===============||
        //               ||
        //  BOOK-COPIES  ||
        //               ||
        //===============||
        public async Task<List<BookCopy>> GetBookCopiesFromDb()
        {
            var bookCopies = new List<BookCopy>();

            using (SqlConnection con = new SqlConnection(theConString))
            {
                await con.OpenAsync();

                using (var command = new SqlCommand($"SELECT * FROM {TableName.tblBookCopy}", con))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var bookCopy = new BookCopy();

                            bookCopy.Copy_Id = Convert.ToInt32(reader["Copy_Id"]);
                            bookCopy.fk_Book_Id = Convert.ToInt32(reader["fk_Book_Id"]);
                            bookCopy.fk_Condition_Id = Convert.ToInt32(reader["fk_Condition_Id"]);
                            bookCopy.fk_Availability = Convert.ToInt32(reader["fk_Availability"]);

                            bookCopies.Add(bookCopy);
                        }
                    }
                }
                return await Task.FromResult(bookCopies);
            }
        }
        public async Task<List<BookCondition>> GetBookCopyConditionsFromDb()
        {
            var bookConditions = new List<BookCondition>();

            using (SqlConnection con = new SqlConnection(theConString))
            {
                await con.OpenAsync();

                using (var command = new SqlCommand($"SELECT * FROM {TableName.tblConditionStatus}", con))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var bookCondition = new BookCondition();
                            bookCondition.Condition_Id = Convert.ToInt32(reader["Condition_Id"]);
                            bookCondition.ConditionType = reader["ConditionType"].ToString();

                            bookConditions.Add(bookCondition);
                        }
                    }
                }
                return await Task.FromResult(bookConditions);
            }
        }
        public async Task<List<BookAvailability>> GetBookCopyAvailabilityFromDb()
        {
            var bookAvailability = new List<BookAvailability>();

            using (SqlConnection con = new SqlConnection(theConString))
            {
                await con.OpenAsync();

                using (var command = new SqlCommand($"SELECT * FROM {TableName.tblAvailability}", con))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var bookAvailable = new BookAvailability();
                            bookAvailable.Id = Convert.ToInt32(reader["Id"]);
                            bookAvailable.Status = reader["Status"].ToString();

                            bookAvailability.Add(bookAvailable);
                        }
                    }
                }
                return await Task.FromResult(bookAvailability);
            }
        }
        public async Task<bool> RemoveBookCopyFromDb(int id, string reason)
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

        //===============||
        //               ||
        //  BOOK-LOANS   ||
        //               ||
        //===============||
        public async Task<List<BookLoan2>> GetBookLoansFromDb()
        {
            var bookLoans = new List<BookLoan2>();

            using (SqlConnection con = new SqlConnection(theConString))
            {
                await con.OpenAsync();

                using (var command = new SqlCommand($"SELECT * FROM {TableName.tblBookLoan}", con))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var bookLoan = new BookLoan2();

                            bookLoan.Loan_Id = Convert.ToInt32(reader["Loan_Id"]);
                            bookLoan.fk_BookCopy_Id = Convert.ToInt32(reader["fk_BookCopy_Id"]);
                            bookLoan.fk_BookLoanStatus_Id = Convert.ToInt32(reader["fk_BookLoanStatus_Id"]);
                            bookLoan.fk_LibraryCard_Id = Convert.ToInt32(reader["fk_LibraryCard_Id"]);
                            bookLoan.ReturnDate = Convert.ToDateTime(reader["ReturnDate"]);
                            bookLoan.ReturnedDate = Convert.ToDateTime(reader["ReturnDate"]);

                            //bookLoan.Checked_In = Convert.ToInt32(reader["Checked_In"]);

                            bookLoans.Add(bookLoan);
                        }
                    }
                }
                return await Task.FromResult(bookLoans);
            }
        }
        public async Task<List<BookLoanStatus>> GetBookLoanStatusFromDb()
        {
            var loanStatuses = new List<BookLoanStatus>();

            using (SqlConnection con = new SqlConnection(theConString))
            {
                await con.OpenAsync();

                using (var command = new SqlCommand($"SELECT * FROM {TableName.tblBookLoanStatus}", con))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var loanStatus = new BookLoanStatus();

                            loanStatus.BookLoanStatus_Id = Convert.ToInt32(reader["BookLoanStatus_Id"]);
                            loanStatus.BookLoan_Status = reader["BookLoan_Status"].ToString();

                            loanStatuses.Add(loanStatus);
                        }
                    }
                }
                return await Task.FromResult(loanStatuses);
            }
        }

        //===============||
        //               ||
        //     DEWEY     ||
        //               ||
        //===============||
        public async Task<List<DeweyMain>> GetDeweyMainFromDb()
        {
            var deweyMain = new List<DeweyMain>();

            using (SqlConnection con = new SqlConnection(theConString))
            {
                await con.OpenAsync();

                using (var command = new SqlCommand($"SELECT * FROM {TableName.tblDeweyMain}", con))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var dewey = new DeweyMain();

                            dewey.MainCategoryName = reader["MainCategoryName"].ToString();
                            dewey.DeweyMain_Id = Convert.ToInt32(reader["DeweyMain_Id"]);

                            deweyMain.Add(dewey);
                        }
                    }
                }
                return await Task.FromResult(deweyMain);
            }
        }
        public async Task<List<DeweySub>> GetDeweySubFromDb()
        {
            var deweySub = new List<DeweySub>();

            using (SqlConnection con = new SqlConnection(theConString))
            {
                await con.OpenAsync();

                using (var command = new SqlCommand($"SELECT * FROM {TableName.tblDeweySub}", con))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var dewey = new DeweySub();

                            dewey.SubCategoryName = reader["SubCategoryName"].ToString();
                            dewey.fk_DeweyMain_Id = Convert.ToInt32(reader["fk_DeweyMain_Id"]);
                            dewey.DeweySub_Id = Convert.ToInt32(reader["DeweySub_Id"]);

                            deweySub.Add(dewey);
                        }
                    }
                }
                return await Task.FromResult(deweySub);
            }
        }

        //===============||
        //               ||
        //     USERS     ||
        //               ||
        //===============||
        public async Task<List<User>> GetUsersFromDb()
        {
            var users = new List<User>();

            using (SqlConnection con = new SqlConnection(theConString))
            {
                await con.OpenAsync();

                using (var command = new SqlCommand($"SELECT * FROM {TableName.tblUser}", con))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var user = new User();

                            user.User_ID = Convert.ToInt32(reader["User_ID"]);
                            user.fk_PrivilegeLevel = Convert.ToInt32(reader["fk_PrivilegeLevel"]);
                            switch (user.fk_PrivilegeLevel)
                            {
                                case 1:
                                    user.privilegeName = "Admin";
                                    break;
                                case 2:
                                    user.privilegeName = "Bibliotekarie";
                                    break;
                                case 3:
                                    user.privilegeName = "Besökare";
                                    break;
                            }
                            user.First_Name = reader["First_Name"].ToString();
                            user.Last_Name = reader["Last_Name"].ToString();
                            user.Username = reader["Username"].ToString();
                            user.Address = reader["Address"].ToString();
                            user.Email = reader["E-mail"].ToString();
                            user.PhoneNumber = reader["PhoneNumber"].ToString();
                            if (int.TryParse(reader["fk_LibraryCard"].ToString(), out int cardnumber))
                            {
                                user.Cardnumber = cardnumber;
                            }

                            users.Add(user);
                        }
                    }
                }
                return await Task.FromResult(users);
            }
        }

        //===============||
        //               ||
        //     EVENTS    ||
        //               ||
        //===============||
        public async Task<List<Event>> GetEventsFromDb()
        {
            throw new NotImplementedException();
        }

        //===============||
        //               ||
        // LIBRARY-CARDS ||
        //               ||
        //===============||
        public async Task<List<LibraryCard2>> GetLibraryCardsFromDb()
        {
            var libraryCards = new List<LibraryCard2>();

            using (SqlConnection con = new SqlConnection(theConString))
            {
                await con.OpenAsync();

                using (var command = new SqlCommand($"SELECT * FROM {TableName.tblLibraryCard}", con))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var libraryCard = new LibraryCard2();

                            libraryCard.LibraryCard_Id = Convert.ToInt32(reader["LibraryCard_Id"]);
                            libraryCard.CardNumber = reader["CardNumber"].ToString();
                            libraryCard.fk_Status_Id = Convert.ToInt32(reader["fk_Status_Id"]);

                            libraryCards.Add(libraryCard);
                        }
                    }
                }
                return await Task.FromResult(libraryCards);
            }
        }
        public async Task<List<LibraryCardStatus>> GetLibraryCardStatusFromDb()
        {
            var cardStatuses = new List<LibraryCardStatus>();

            using (SqlConnection con = new SqlConnection(theConString))
            {
                await con.OpenAsync();

                using (var command = new SqlCommand($"SELECT * FROM {TableName.tblLibraryCardStatus}", con))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var cardStatus = new LibraryCardStatus();

                            cardStatus.Status_Id = Convert.ToInt32(reader["Status_Id"]);
                            cardStatus.Status_Level = reader["Status_Level"].ToString();

                            cardStatuses.Add(cardStatus);
                        }
                    }
                }
                return await Task.FromResult(cardStatuses);
            }
        }

        //===============||
        //               ||
        //    REPORTS    ||
        //               ||
        //===============||
        public async Task<List<RemovedItem>> GetRemovedItemsFromDb()
        {
            var removedItems = new List<RemovedItem>();

            using (SqlConnection con = new SqlConnection(theConString))
            {
                await con.OpenAsync();

                using (var command = new SqlCommand($"SELECT * FROM {TableName.tblRemovedItem}", con))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var removedItem = new RemovedItem();

                            removedItem.Id = Convert.ToInt32(reader["Id"]);
                            removedItem.Title = reader["Title"].ToString();
                            removedItem.Reason = reader["Reason"].ToString();
                            removedItem.Condition = reader["Condition"].ToString();
                            removedItem.Date = Convert.ToDateTime(reader["Date"]);

                            removedItems.Add(removedItem);
                        }
                    }
                }
                return await Task.FromResult(removedItems);
            }
        }
    }
    }

