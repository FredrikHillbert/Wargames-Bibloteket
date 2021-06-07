using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using WargamesGUI.Models;
using WargamesGUI.DAL;


namespace WargamesGUI.Services
{
    public class BookService2
    {
        private DbService dbService = new DbService();
        private LoanService2 loanService = new LoanService2();

        // Books
        public async Task<List<Book2>> GetAllBooks()
        {
            var deweySub = await dbService.GetDeweySubFromDb();
            var deweyMain = await dbService.GetDeweyMainFromDb();
            var books = await dbService.GetBooksFromDb();
            var items = await dbService.GetItemTypeFromDb();
            var bookCopies = await GetAllBookCopies();

            var listOfBooks = books.Select(x => new Book2
            {
                Id = x.Id,
                fk_Item_Id = x.fk_Item_Id,
                Title = x.Title,
                ISBN = x.ISBN,
                Publisher = x.Publisher,
                Description = x.Description,
                Placement = x.Placement,
                Author = x.Author,
                BookType = items.Select(i => i).Where(i => i.Item_Id == x.fk_Item_Id).ToList().ElementAtOrDefault(0),
                DeweySub = deweySub.Select(y => y)
                .Where(y => y.DeweySub_Id == x.Placement)
                .ElementAtOrDefault(0),
                DeweyMain = deweyMain.Select(y => y)
                .Where(y => y.DeweyMain_Id == deweySub.Where(s => s.DeweySub_Id == x.Placement)
                .Select(s => s.fk_DeweyMain_Id)
                .ToList()
                 .ElementAtOrDefault(0)).ElementAtOrDefault(0),
                BookCopies = bookCopies.Select(c => c).Where(c => c.fk_Book_Id == x.Id).ToList(),

            }).ToList();

            return listOfBooks ?? null;
        }
        public async Task<bool> RemoveBookCopy(BookCopy bookCopy, string reason)
        {
            if (bookCopy != null && string.IsNullOrEmpty(reason) && string.IsNullOrWhiteSpace(reason))
            {
                return await dbService.ProcedureRemoveBookCopyFromDb(bookCopy.Copy_Id, reason);
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> RemoveBookObject(Book2 removeBook) 
        {

            return await dbService.ProcedureDeleteBookFromDb(removeBook);

        }
        public async Task<bool> RemoveBookCopy(int bookCopy_Id, string reason)
        {
            if (bookCopy_Id != 0 && string.IsNullOrEmpty(reason) && string.IsNullOrWhiteSpace(reason))
            {
                return await dbService.ProcedureRemoveBookCopyFromDb(bookCopy_Id, reason);
            }
            else
            {
                return false;
            }
        }
        public async Task<(bool, string)> UpdateBook(Book2 book)
        {
            bool success = await dbService.ProcedureUpdateBookInDb(book);
            if (success) return (success, "Success, returned true.");
            else return (success, $"Error {nameof(this.UpdateBook)}, - returned false.");
        }
        public async Task<bool> AddNewBook(Book2 book)
        {
            return await dbService.ProcedureAddBookToDb(book);
        }
        //BookCopy
        public async Task<List<BookCopy>> GetAllBookCopies()
        {
            var bookCopies = await dbService.GetBookCopiesFromDb();
            var bookConditions = await dbService.GetBookCopyConditionsFromDb();
            var bookAvailability = await dbService.GetBookCopyAvailabilityFromDb();
            var books = await dbService.GetBooksFromDb();

            var result = bookCopies.Select(x => new BookCopy
            {
                Copy_Id = x.Copy_Id,
                fk_Book_Id = x.fk_Book_Id,
                fk_Availability = x.fk_Availability,
                fk_Condition_Id = x.fk_Condition_Id,
                Book = books.Select(y => y).Where(y => y.Id == x.fk_Book_Id).ElementAtOrDefault(0),
                BookCondition = bookConditions.Select(y => y)
                .Where(y => y.Condition_Id == x.fk_Condition_Id)
                .ElementAtOrDefault(0),
                BookAvailability = bookAvailability.Select(y => y)
                .Where(y => y.Id == x.fk_Availability)
                .ElementAtOrDefault(0),

            }).ToList();

            return result ?? null;
        }
        public async Task<(bool, string)> ChangeBookCopyCondition(BookCopy bookCopy)
        {
            bool success = await dbService.ProcedureUpdateBookCopyConditionInDb(bookCopy.Copy_Id, bookCopy.BookCondition.Condition_Id);
            if (success) return (success, "Success, returned true.");
            else return (success, $"Error {nameof(this.ChangeBookCopyCondition)}, - returned false.");
        }
        public async Task<(bool, string)> ChangeBookCopyCondition(int bookCopy_Id, int new_Id)
        {
            bool success = await dbService.ProcedureUpdateBookCopyConditionInDb(bookCopy_Id, new_Id);
            if (success) return (success, "Success, returned true.");
            else return (success, $"Error {nameof(this.ChangeBookCopyCondition)}, - returned false.");
        }
        public async Task<List<BookCopy>> GetAvailableBookCopies()
        {
            var bookCopies = await dbService.GetBookCopiesFromDb();
            return bookCopies.Where(x => x.fk_Availability == 1).ToList() ?? null;
            
        }
        public async Task<List<BookCopy>> GetAvailableBookCopies(Book2 book)
        {
            var bookCopies = await GetAllBookCopies();
            if (bookCopies.Where(x => x.fk_Book_Id == book.Id).Any(x => x.BookAvailability.Id == 1 && x.BookAvailability.Status == "Tillgänglig"))
            {
                return bookCopies.Where(x => x.fk_Book_Id == book.Id).Where(x => x.fk_Availability == 1).ToList();
            }
            else
            {
                return null;
            }
        }
        public async Task<List<BookCopy>> GetUnavailableBookCopies()
        {
            var bookCopies = await GetAllBookCopies();
            if (bookCopies.Any(x => x.fk_Availability == 2 && x.BookAvailability.Status == "Otillgänglig"))
            {
                return bookCopies.Where(x => x.fk_Availability == 2).ToList();
            }
            else
            {
                return null;
            }
        }
        public async Task<List<BookCopy>> GetUnavailableBookCopies(Book book)
        {
            var bookCopies = await GetAllBookCopies();
            if (bookCopies.Where(x => x.fk_Book_Id == book.Id).Any(x => x.fk_Availability == 2 && x.BookAvailability.Status == "Otillgänglig"))
            {
                return bookCopies.Where(x => x.fk_Book_Id == book.Id).Where(x => x.fk_Availability == 2).ToList();
            }
            else
            {
                return null;
            }
        }
        // Sätt så att det bara går att välja vissa saker i GUI:t - dvs de bookCondition som finns.
        public async Task<List<BookCopy>> FilterBookCopiesByCondition(int bookCondition)
        {
            var bookCopies = await dbService.GetBookCopiesFromDb();
            if (bookCopies.Any(x => x.fk_Condition_Id == bookCondition))
            {
                return bookCopies.Where(x => x.fk_Condition_Id == bookCondition).ToList();
            }
            else
            {
                return null;
            }
        }

        // Null check ints?
        public async Task<List<Book2>> SearchBook(string text)
        {
            var books = await GetAllBooks();
            return books.Where(x => x.Author != null && x.Author.ToUpper().Contains(text.ToUpper()) ||
                                      x.Publisher != null && x.Publisher.ToUpper().Contains(text.ToUpper()) ||
                                      x.ISBN != null && x.ISBN.ToUpper().Contains(text.ToUpper()) ||
                                      x.Title != null && x.Title.ToUpper().Contains(text.ToUpper()) ||
                                      x.Description != null && x.Description.ToUpper().Contains(text.ToUpper()) ||
                                      x.DeweyMain != null && x.DeweyMain.MainCategoryName.ToString().ToUpper().Contains(text.ToUpper()) ||
                                      x.DeweySub != null && x.DeweySub.SubCategoryName.ToString().ToUpper().Contains(text.ToUpper())
                                      //x.Category != null && x.Category.ToUpper().Contains(text.ToUpper()) ||
                                      ).Select(x => x)
                                      .ToList();
            }
        public async Task<List<DeweyMain>> GetDeweyMain()
        {
            return await dbService.GetDeweyMainFromDb();
        }
        public async Task<List<DeweySub>> GetDeweySub()
        {
            return await dbService.GetDeweySubFromDb();
        }

    }
}