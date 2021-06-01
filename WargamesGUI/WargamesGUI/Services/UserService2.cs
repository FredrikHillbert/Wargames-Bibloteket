﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WargamesGUI.Models;
using System.Linq;

namespace WargamesGUI.Services
{
    public class UserService2 : DbService
    {

        public async Task<List<User2>> ReadAllUsersFromDbAsync()
        {
            var userService = await dbService.GetUsersFromDb();
            var privilegeLevel = await dbService.GetPrivilegeFromDb();
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
                TypeOfUser = privilegeLevel.Select(p => p).Where(p => p.PrivilegeLevel == x.fk_PrivilegeLevel).ElementAtOrDefault(0),
                SSN = x.SSN,
            }).ToList();

            return listOfUsers ?? null;
        }

        public async Task<List<User2>> ReadOnlyVisitorFromDbAsync() 
        {
            var listOfVisitors = await ReadAllUsersFromDbAsync();
            return listOfVisitors.Where(x => x.fk_PrivilegeLevel == 3).ToList();
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

        //Retunerar en int som motsvarar status id:et och en sträng som motsvarar level-typen alltså "Admin", "Bibliotekarie", "Besökare"
        public async Task<(int, string)> GetStatusForLibraryCardFromDbAsync(int libraryCard_Id)
        {
            var libraryCards = await dbService.GetLibraryCardsFromDb();
            var getSpecificCard = libraryCards.Select(x => x).Where(x => x.LibraryCard_Id == libraryCard_Id).ElementAtOrDefault(0);
            return (getSpecificCard.fk_Status_Id, getSpecificCard.CardStatus.Status_Level);
        }
    }
}
