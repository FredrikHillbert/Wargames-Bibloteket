using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using WargamesGUI.Models;
using System.Threading.Tasks;

namespace WargamesGUI.Services
{
    public class EventService : DbHandler
    {
        public async Task<List<Event>> GetEventsFromDb()
        {
            var eventList = new List<Event>();

            using (SqlConnection con = new SqlConnection(theConString))
            {
                con.Open();
                using (var commad = new SqlCommand(queryForEvents, con))
                {
                    using (var reader = await commad.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var eventObject = new Event();

                            eventObject.fk_Item_Id = 3;
                            eventObject.Title = reader["Title"].ToString();
                            eventObject.Description = reader["Description"].ToString();
                            eventObject.DateOfEvent = Convert.ToDateTime(reader["DateOfEvent"]);
                            eventList.Add(eventObject);
                        }
                    }
                }

                return await Task.FromResult(eventList);
            }
        }

        /// <summary>
        /// Adderar ett nytt event/seminarium till table tblEvent. 
        /// Måste skickas med: Title, Description, DateOfEvent till eventet.
        /// </summary>
        /// <param name=""></param>
        /// <returns>
        /// Retunerar en bool som är true om det gick att lägga till eventet eller false 
        /// ifall det inte gick att lägga till eventet.'
        /// </returns>
        public async Task<bool> AddNewEvent(string Title, string Description, DateTime DateOfEvent)
        {

            bool success = true;

            try
            {
                using (SqlConnection con = new SqlConnection(theConString))
                {
                    string query =
                        $"INSERT INTO {theEventTableName}" +
                        $"(fk_Item_Id, Title, Description, DateOfEvent) " +
                        $"VALUES('3', '{Title}', '{Description}', '{DateOfEvent}')";

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

        /// <summary>
        /// Tar bort ett event från table tblEvent. 
        /// Måste skickas med: Eventets unika ID
        /// </summary>
        /// <param name=""></param>
        /// <returns>
        /// Retunerar en bool som är true om det gick att ta bort eventet eller false 
        /// ifall det inte gick att ta bort eventet.'
        /// </returns>
        public async Task<bool> RemoveEvent(int id, string reason)
        {
            bool success = true;

            try
            {
                using (SqlConnection con = new SqlConnection(theConString))
                {
                    string query =
                        $"DELETE FROM {theEventTableName} WHERE Id = @id";

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
                }

                return await Task.FromResult(success);
            }

            catch (Exception)
            {
                success = false;
                return await Task.FromResult(success);
            }
        }
        public async Task<bool> UpdateEvent(int id, string Title, string Description, DateTime DateOfEvent)
        {
            bool success = true;

            try
            {
                using (SqlConnection con = new SqlConnection(theConString))
                {
                    string query =
                        $"UPDATE {theEventTableName} " +
                        $"SET Title = {Title}, Description = {Description}, DateOfEvent = {DateOfEvent}" +
                        $"WHERE Id = {id}";

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
    }
}