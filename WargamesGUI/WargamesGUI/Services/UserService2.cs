using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using WargamesGUI.DAL;
using WargamesGUI.Models;

namespace WargamesGUI.Services
{
    public class UserService2 : DbService
    {
        public DbService dbService = new DbService();
        public LoanService2 loanService = new LoanService2();
        public async Task<List<User2>> ReadAllUsersFromDbAsync()
        {
            var userService = await GetUsersFromDb();
            var privilegeLevel = await GetPrivilegeFromDb();
            var libraryCards = await loanService.GetAllLibraryCards();
            var listOfUsers = userService.Select(x => new User2
            {
                First_Name = x.First_Name,
                Last_Name = x.Last_Name,
                Address = x.Address,
                Email = x.Email,
                PhoneNumber = x.PhoneNumber,
                User_ID = x.User_ID,
                Username = x.Username,
                Password = x.Password,
                SSN = x.SSN,
                TypeOfUser = privilegeLevel.Select(y => y).Where(y => y.PrivilegeLevel == x.fk_PrivilegeLevel).FirstOrDefault(),
                LibraryCard = libraryCards.Select(y => y).Where(y => y.LibraryCard_Id == x.fk_LibraryCard).FirstOrDefault(),
            }).ToList();

            return listOfUsers ?? null;
        }

        public async Task<List<User2>> ReadOnlyVisitorFromDbAsync() 
        {
            var listOfVisitors = await ReadAllUsersFromDbAsync();
            return listOfVisitors.Where(x => x.TypeOfUser.PrivilegeLevel == 3).ToList();
        }

        public async Task<int> SignInUser(User2 loginUser, string password) 
        {
            var canLoginUser = await dbService.SignIn(loginUser, password);
            return canLoginUser;
        
        
        }
        public async Task<bool> AddNewUserAsync(User2 NewUser)
        {
            var canAddNewUser = await dbService.AddNewUser(NewUser);
            return canAddNewUser;
        }
        public async Task<bool> RemoveUserFromDbAsync(int userId)
        {
            var canRemoveUser = await dbService.RemoveUserFromDb(userId);
            return canRemoveUser;
        }

       // Retunerar en int som motsvarar status id:et och en sträng som motsvarar level-typen alltså "Admin", "Bibliotekarie", "Besökare"
        public async Task<(int, string)> GetStatusForLibraryCardFromDbAsync(int libraryCard_Id)
        {
            var libraryCards = await loanService.GetAllLibraryCards();
            var getSpecificCard = libraryCards.Select(x => x).Where(x => x.LibraryCard_Id == libraryCard_Id).ElementAtOrDefault(0);
            return (getSpecificCard.fk_Status_Id, getSpecificCard.CardStatus.Status_Level);
        }
    }
}
