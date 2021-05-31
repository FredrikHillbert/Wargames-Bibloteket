using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WargamesGUI.Models;


namespace WargamesGUI.Services
{
    public interface IDbService
    {
        Task<List<Book2>> GetBooksFromDb();
        Task<List<BookCopy>> GetBookCopiesFromDb();
        Task<List<BookLoan2>> GetBookLoansFromDb();
        Task<List<DeweyMain>> GetDeweyMainFromDb();
        Task<List<DeweySub>> GetDeweySubFromDb();
        Task<List<Event>> GetEventsFromDb();
        Task<List<LibraryCard2>> GetLibraryCardsFromDb();
        Task<List<RemovedItem>> GetRemovedItemsFromDb();
        Task<List<User>> GetUsersFromDb();
        Task<List<BookCondition>> GetBookCopyConditionsFromDb();
        Task<List<BookAvailability>> GetBookCopyAvailabilityFromDb();
        Task<List<LibraryCardStatus>> GetLibraryCardStatusFromDb();
    }
}
