using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using WargamesGUI.Models;
using System.Configuration;
using System.Data;
using WargamesGUI.Services;

namespace WargamesGUI.DAL
{
    public class DbService : IDbService
    {
        public string theConString;
        public string theConStringTest;
        
        public DbService()
        {
            theConString = ConfigurationManager.ConnectionStrings[1].ConnectionString;
            //theConStringTest = ConfigurationManager.ConnectionStrings[2].ConnectionString;
        }
        public string SelectAllFromQuery(TableName tableName)
        {
            return $"SELECT * FROM {tableName}";
        }
        //========================================================================================||
        //                                                                                        ||
        //  BOOKS                                                                                 ||
        //                                                                                        ||
        //========================================================================================||
        public async Task<List<Book2>> GetBooksFromDb()
        {
            var bookList = new List<Book2>();

            using (SqlConnection con = new SqlConnection(theConString))
            {
                await con.OpenAsync();
                using (var command = new SqlCommand(SelectAllFromQuery(TableName.tblBook), con))
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
                            book.Available_copies = Convert.ToInt32(reader["Available_copies"]);
                            book.InStock = Convert.ToInt32(reader["InStock"]);
                            book.Price = Convert.ToInt32(reader["Price"]);

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
        public async Task<bool> ProcedureUpdateBookInDb(Book2 book)
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
                    insertcmd.Parameters.Add("@fk_Item_Id", SqlDbType.Int).Value = book.BookType.Item_Id;
                    insertcmd.Parameters.Add("@Title", SqlDbType.VarChar).Value = book.Title ?? null;
                    insertcmd.Parameters.Add("@Author", SqlDbType.VarChar).Value = book.Author ?? null;
                    insertcmd.Parameters.Add("@Publisher", SqlDbType.VarChar).Value = book.Publisher ?? null;
                    insertcmd.Parameters.Add("@Description", SqlDbType.VarChar).Value = book.Description ?? null;
                    insertcmd.Parameters.Add("@ISBN", SqlDbType.VarChar).Value = book.ISBN ?? null;
                    insertcmd.Parameters.Add("@Placement", SqlDbType.VarChar).Value = book.DeweySub.DeweySub_Id;

                    await insertcmd.ExecuteNonQueryAsync();
                }

                return await Task.FromResult(success);
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
        public async Task<bool> ProcedureAddBookToDb(Book2 book)
        {
            bool success = true;
            try
            {
                using (SqlConnection con = new SqlConnection(theConString))
                {
                    await con.OpenAsync();

                    SqlCommand insertcmd = new SqlCommand("sp_AddBook", con);
                    insertcmd.CommandType = CommandType.StoredProcedure;
                    insertcmd.Parameters.Add("@fk_Item_Id", SqlDbType.Int).Value = book.BookType.Item_Id;
                    insertcmd.Parameters.Add("@Title", SqlDbType.VarChar).Value = book.Title;
                    insertcmd.Parameters.Add("@ISBN", SqlDbType.VarChar).Value = book.ISBN;
                    insertcmd.Parameters.Add("@Publisher", SqlDbType.VarChar).Value = book.Publisher;
                    insertcmd.Parameters.Add("@Author", SqlDbType.VarChar).Value = book.Author;
                    insertcmd.Parameters.Add("@Description", SqlDbType.VarChar).Value = book.Description;
                    insertcmd.Parameters.Add("@Price", SqlDbType.Int).Value = book.Price;
                    insertcmd.Parameters.Add("@Placement", SqlDbType.VarChar).Value = book.DeweySub.DeweySub_Id;
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

        public async Task<bool> ProcedureDeleteBookFromDb(Book2 removeBook) 
        {
            bool success = false;
            try
            {
                using (SqlConnection con = new SqlConnection(theConString)) 
                {
                    await con.OpenAsync();

                    SqlCommand sp = new SqlCommand("sp_RemoveBookObject", con);
                    sp.CommandType = CommandType.StoredProcedure;
                    sp.Parameters.Add("@BookId", SqlDbType.Int).Value = removeBook.Id;
                    await sp.ExecuteNonQueryAsync();

                }
                return success = true;
            }
            catch (Exception e)
            {
                return success;
            }   
        }
        public async Task<List<Item>> GetItemTypeFromDb()
        {
            var items = new List<Item>();

            using (SqlConnection con = new SqlConnection(theConString))
            {
                await con.OpenAsync();

                using (var command = new SqlCommand(SelectAllFromQuery(TableName.tblItem), con))
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

        //========================================================================================||
        //                                                                                        ||
        //  BOOK-COPIES                                                                           ||
        //                                                                                        ||
        //========================================================================================||
        public async Task<List<BookCopy>> GetBookCopiesFromDb()
        {
            var bookCopies = new List<BookCopy>();

            using (SqlConnection con = new SqlConnection(theConString))
            {
                await con.OpenAsync();

                using (var command = new SqlCommand(SelectAllFromQuery(TableName.tblBookCopy), con))
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

                using (var command = new SqlCommand(SelectAllFromQuery(TableName.tblConditionStatus), con))
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

                using (var command = new SqlCommand(SelectAllFromQuery(TableName.tblAvailability), con))
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
        public async Task<bool> ProcedureRemoveBookCopyFromDb(int id, string reason)
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
        public async Task<bool> ProcedureUpdateBookCopyConditionInDb(int id, int new_id)
        {
            bool success = true;

            try
            {
                using (SqlConnection con = new SqlConnection(theConString))
                {

                    await con.OpenAsync();

                    SqlCommand insertcmd = new SqlCommand("sp_UpdateBookCopy", con);
                    insertcmd.CommandType = CommandType.StoredProcedure;

                    insertcmd.Parameters.Add("@Copy_Id", SqlDbType.Int).Value = id;
                    insertcmd.Parameters.Add("@newCondition_Id", SqlDbType.Int).Value = new_id;

                    await insertcmd.ExecuteNonQueryAsync();
                }

                return await Task.FromResult(success);
            }

            catch (Exception ex)
            {
                string a = ex.Message;
                success = false;
                return await Task.FromResult(success);
            }
        }

        //========================================================================================||
        //                                                                                        ||
        //  BOOK-LOANS                                                                            ||
        //                                                                                        ||
        //========================================================================================||
        public async Task<List<BookLoan2>> GetBookLoansFromDb()
        {
            var bookLoans = new List<BookLoan2>();

            using (SqlConnection con = new SqlConnection(theConString))
            {
                await con.OpenAsync();

                using (var command = new SqlCommand(SelectAllFromQuery(TableName.tblBookLoan), con))
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
        public async Task<bool> UpdateBookLoanInDb(int id)
        {
            var bookList = new List<Book2>();
            try
            {
                using (SqlConnection con = new SqlConnection(theConString))
                {
                    await con.OpenAsync();
                    using (var command = new SqlCommand($"UPDATE {TableName.tblBookLoan} SET {tblBookLoan.fk_BookLoanStatus_Id} = 5 WHERE {tblBookLoan.Loan_Id} = {id}", con))
                    {
                        await command.ExecuteNonQueryAsync();
                        return await Task.FromResult(true);
                    }
                }
            }
            catch (Exception)
            {
                return await Task.FromResult(false);
            }
        }
        public async Task<bool> UpdateBookLoanInDb(int id, int status)
        {
            var bookList = new List<Book2>();
            try
            {
                using (SqlConnection con = new SqlConnection(theConString))
                {
                    await con.OpenAsync();
                    using (var command = new SqlCommand($"UPDATE {TableName.tblBookLoan} SET {tblBookLoan.fk_BookLoanStatus_Id} = 5 WHERE {tblBookLoan.Loan_Id} = {id}", con))
                    {
                        await command.ExecuteNonQueryAsync();
                        return await Task.FromResult(true);
                    }
                }
            }
            catch (Exception)
            {
                return await Task.FromResult(false);
            }
        }
        public async Task<List<BookLoanStatus>> GetBookLoanStatusFromDb()
        {
            var loanStatuses = new List<BookLoanStatus>();

            using (SqlConnection con = new SqlConnection(theConString))
            {
                await con.OpenAsync();

                using (var command = new SqlCommand(SelectAllFromQuery(TableName.tblBookLoanStatus), con))
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
        public async Task<int> ProcedureLoanBook(int book_id, int fk_LibraryCard)
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

            catch (Exception)
            {
                return await Task.FromResult(returnValue);
            }
        }
        public async Task<bool> ProcedureBookLoanReturn(int loanID)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(theConString))
                {
                    await con.OpenAsync();
                    SqlCommand insertcmd = new SqlCommand("sp_ReturnBookLoan", con);
                    insertcmd.CommandType = CommandType.StoredProcedure;
                    insertcmd.Parameters.Add("@loanID", SqlDbType.Int).Value = loanID;
                    insertcmd.Parameters.Add("@returnValue", SqlDbType.VarChar).Direction = ParameterDirection.ReturnValue;
                    await insertcmd.ExecuteNonQueryAsync();
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<bool> ProcedureRegisterReturnedBookCopy(BookLoan2 bookLoan)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(theConString))
                {
                    await con.OpenAsync();

                    SqlCommand cmd = new SqlCommand("sp_ReturnBookToLibrary_2", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@copyId", SqlDbType.Int).Value = bookLoan.fk_BookCopy_Id;
                    cmd.Parameters.Add("@loanId", SqlDbType.Int).Value = bookLoan.Loan_Id;
                    cmd.Parameters.Add("@bookId", SqlDbType.Int).Value = bookLoan.BookCopy.fk_Book_Id;
                    cmd.Parameters.Add("@bookTitle", SqlDbType.VarChar).Value = bookLoan.BookCopy.Book.Title;

                    cmd.Parameters.Add("@returnValue", SqlDbType.VarChar).Direction = ParameterDirection.ReturnValue;
                    await cmd.ExecuteNonQueryAsync();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<bool> ProcedureRegisterDestroyedBookCopy(int copy_Id, int loan_Id, string reason)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(theConString))
                {
                    await con.OpenAsync();
                    SqlCommand cmd = new SqlCommand("sp_ReturnBookToLibraryDestroyed", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@copyId", SqlDbType.Int).Value = copy_Id;
                    cmd.Parameters.Add("@loanId", SqlDbType.Int).Value = loan_Id;
                    cmd.Parameters.Add("@reason", SqlDbType.VarChar).Value = reason;
                    // cmd.Parameters.Add("@returnValue", SqlDbType.VarChar).Direction = ParameterDirection.ReturnValue;
                    await cmd.ExecuteNonQueryAsync();
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        //========================================================================================||
        //                                                                                        ||
        //  DEWEY                                                                                 ||
        //                                                                                        ||
        //========================================================================================||
        public async Task<List<DeweyMain>> GetDeweyMainFromDb()
        {
            var deweyMain = new List<DeweyMain>();

            using (SqlConnection con = new SqlConnection(theConString))
            {
                await con.OpenAsync();

                using (var command = new SqlCommand(SelectAllFromQuery(TableName.tblDeweyMain), con))
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

                using (var command = new SqlCommand(SelectAllFromQuery(TableName.tblDeweySub), con))
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

        //========================================================================================||
        //                                                                                        ||
        //  USERS                                                                                 ||
        //                                                                                        ||
        //========================================================================================||
        public async Task<List<User2>> GetUsersFromDb()
        {
            var users = new List<User2>();

            using (SqlConnection con = new SqlConnection(theConString))
            {
                await con.OpenAsync();

                using (var command = new SqlCommand($"SELECT * FROM {TableName.tblUser}", con))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var user = new User2();

                            user.User_ID = Convert.ToInt32(reader["User_ID"]);
                            user.fk_PrivilegeLevel = Convert.ToInt32(reader["fk_PrivilegeLevel"]);
                            user.First_Name = reader["First_Name"].ToString();
                            user.Last_Name = reader["Last_Name"].ToString();
                            user.Username = reader["Username"].ToString();
                            user.Address = reader["Address"].ToString();
                            user.Email = reader["E-mail"].ToString();
                            user.PhoneNumber = reader["PhoneNumber"].ToString();
                            user.Password = reader["Password"].ToString();
                            if (user.fk_PrivilegeLevel == 3) { user.fk_LibraryCard = Convert.ToInt32(reader["fk_LibraryCard"]); }
                            
                            
                            users.Add(user);
                        }
                    }
                }
                    return await Task.FromResult(users);
            }
        }
        /// <summary>
        /// Tar bort en specifik användare med hjälp av userID
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<bool> RemoveUserFromDb(int userId)
        {
            bool isWorking;
            try
            {
                using (SqlConnection con = new SqlConnection(theConString))
                {
                    con.Open();
                    SqlCommand deletecmd = new SqlCommand("sp_DeleteUser", con);
                    deletecmd.CommandType = CommandType.StoredProcedure;
                    deletecmd.CommandText = "sp_DeleteUser";
                    deletecmd.Parameters.Add("@Id", SqlDbType.Int).Value = userId;
                    deletecmd.ExecuteNonQuery();
                    isWorking = true;
                    return await Task.FromResult(isWorking);
                }
            }
            catch (Exception)
            {

                isWorking = false;
                return await Task.FromResult(isWorking);
            }
        }
        /// <summary>
        /// Adderar en ny användare
        /// </summary>
        /// <param name="newUser"></param>
        /// <returns></returns>
        public async Task<bool> AddNewUser(User2 newUser)
        {
            bool success = true;
            try
            {
                using (SqlConnection con = new SqlConnection(theConString))
                {
                    await con.OpenAsync();
                    SqlCommand insertcmd = new SqlCommand("sp_AddNewUser", con);
                    insertcmd.CommandType = CommandType.StoredProcedure;
                    insertcmd.Parameters.Add("@fk_PL", SqlDbType.Int).Value = newUser.fk_PrivilegeLevel;
                    insertcmd.Parameters.Add("@firstName", SqlDbType.VarChar).Value = newUser.First_Name;
                    insertcmd.Parameters.Add("@lastName", SqlDbType.VarChar).Value = newUser.Last_Name;
                    insertcmd.Parameters.Add("@sSN", SqlDbType.VarChar).Value = newUser.SSN;
                    insertcmd.Parameters.Add("@address", SqlDbType.VarChar).Value = newUser.Address;
                    insertcmd.Parameters.Add("@email", SqlDbType.VarChar).Value = newUser.Email;
                    insertcmd.Parameters.Add("@phoneNumber", SqlDbType.VarChar).Value = newUser.PhoneNumber;
                    insertcmd.Parameters.Add("@username", SqlDbType.VarChar).Value = newUser.Username;
                    insertcmd.Parameters.Add("@password", SqlDbType.VarChar).Value = newUser.Password;
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
        /// <summary>
        /// Hämtar alla olika privilege levels som finns i databasen så att det sedan kan kopplas till en specifik användare med linq. 
        /// </summary>
        /// <returns></returns>
        public async Task<List<Privilege>> GetPrivilegeFromDb()
        {
            var privilegeList = new List<Privilege>();
            using (SqlConnection con = new SqlConnection(theConString))
            {
                await con.OpenAsync();

                using (var command = new SqlCommand($"SELECT * FROM {TableName.tblPrivilegeLevel}", con))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var privilege = new Privilege();
                            privilege.PrivilegeLevel = Convert.ToInt32(reader["PrivilegeLevel_Id"]);
                            privilege.TypeOfUser = reader["TypeOfPrivilegeLevel"].ToString();
                            privilegeList.Add(privilege);
                        }
                    }
                }
                return await Task.FromResult(privilegeList);
            }
        }
        /// <summary>
        /// Kollar ifall en användare finns genom att man har skrivit in korrekt användarnamn och lösenord
        /// </summary>
        /// <param name="loginUser"></param>
        /// <returns>retunerar en INT som motsvarar vilken privilege level personen tillhör</returns>
        public async Task<int> SignIn(User2 loginUser, string password)
        {
           
            SqlConnection Connection = new SqlConnection(theConString);
            Connection.Open();
            var login = $"SELECT fk_PrivilegeLevel FROM {TableName.tblUser} WHERE Username = '{loginUser.Username}' AND Password = HASHBYTES('SHA1','{password}')";

            // string query2 = $"SELECT fk_PrivilegeLevel, Username, fk_LibraryCard, Password, CardNumber FROM tblUser LEFT JOIN tblLibraryCard ON fk_LibraryCard = LibraryCard_Id WHERE Username = '{username}' AND Password = HASHBYTES('SHA1','{password}')";

            using (SqlCommand command = new SqlCommand(login, Connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        loginUser.fk_PrivilegeLevel = Convert.ToInt32(reader["fk_PrivilegeLevel"]);
                    }
                }
            }
            return await Task.FromResult(loginUser.fk_PrivilegeLevel);
        }

        //========================================================================================||
        //                                                                                        ||
        //  EVENTS                                                                                ||
        //                                                                                        ||
        //========================================================================================||
        public async Task<List<Event>> GetEventsFromDb()
        {
            throw new NotImplementedException();
        }

        //========================================================================================||
        //                                                                                        ||
        //  LIBRARY-CARDS                                                                         ||
        //                                                                                        ||
        //========================================================================================||
        public async Task<List<LibraryCard2>> GetLibraryCardsFromDb()
        {
            var libraryCards = new List<LibraryCard2>();

            using (SqlConnection con = new SqlConnection(theConString))
            {
                await con.OpenAsync();

                using (var command = new SqlCommand(SelectAllFromQuery(TableName.tblLibraryCard), con))
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

                using (var command = new SqlCommand(SelectAllFromQuery(TableName.tblLibraryCardStatus), con))
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
        public async Task<bool> ProcedureAddLibraryCard(int user_id)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(theConString))
                {
                    await con.OpenAsync();

                    SqlCommand insertcmd = new SqlCommand("sp_ManualAddLibraryCard", con);
                    insertcmd.CommandType = CommandType.StoredProcedure;

                    insertcmd.Parameters.Add("@User_Id", SqlDbType.Int).Value = user_id;
                    await insertcmd.ExecuteNonQueryAsync();

                    return await Task.FromResult(true);
                }
            }

            catch (Exception)
            {
                return await Task.FromResult(false);
            }
        }
        public async Task<bool> ProcedureChangeLibraryCardStatus(int libraryCard_id, int libraryCardStatus_Id)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(theConString))
                {
                    await con.OpenAsync();

                    SqlCommand insertcmd = new SqlCommand("sp_ChangeCardStatus", con);
                    insertcmd.CommandType = CommandType.StoredProcedure;

                    insertcmd.Parameters.Add("@fk_Status_Id", SqlDbType.Int).Value = libraryCardStatus_Id;
                    insertcmd.Parameters.Add("@LibraryCard_Id", SqlDbType.Int).Value = libraryCard_id;

                    await insertcmd.ExecuteNonQueryAsync();
                    return await Task.FromResult(true);
                }
            }

            catch (Exception)
            {
                return await Task.FromResult(false);
            }
        }

        //========================================================================================||
        //                                                                                        ||
        //  REPORTS                                                                               ||
        //                                                                                        ||
        //========================================================================================||
        public async Task<List<RemovedItem>> GetRemovedItemsFromDb()
        {
            var removedItems = new List<RemovedItem>();

            using (SqlConnection con = new SqlConnection(theConString))
            {
                await con.OpenAsync();

                using (var command = new SqlCommand(SelectAllFromQuery(TableName.tblRemovedItem), con))
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
        public async Task<List<HandledBookLoan>> GetArchivedBookLoansFromDb()
        {
            var handledLoans = new List<HandledBookLoan>();

            using (SqlConnection con = new SqlConnection(theConString))
            {
                using (var command = new SqlCommand(SelectAllFromQuery(TableName.tblHandledBookLoan), con))
                {
                    await con.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var handledLoan = new HandledBookLoan();

                            handledLoan.handled_Loan_Id = Convert.ToInt32(reader["handled_Loan_Id"]);
                            handledLoan.handled_BookCopy_Id = Convert.ToInt32(reader["handled_BookCopy_Id"]);
                            handledLoan.ReturnedDate = Convert.ToDateTime(reader["ReturnedDate"]);

                            handledLoans.Add(handledLoan);
                        }
                    }
                }

                return await Task.FromResult(handledLoans);
            }
        }
    }
}

