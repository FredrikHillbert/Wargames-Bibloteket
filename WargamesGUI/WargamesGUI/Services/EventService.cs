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
    class EventService : DbHandler
    {
        public async Task<List<Event>> GetEventsFromDb()
        {
            var eventList = new List<Event>();

            using (SqlConnection con = new SqlConnection(theConString))
            {
                con.Open();
                using (var commad = new SqlCommand(queryForEventListPage, con))
                {
                    using (var reader = commad.ExecuteReader())
                    {
                        while (reader.Read())
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

                return eventList;
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
                    string sql =
                        $"INSERT INTO {theEventTableName}" +
                        $"(fk_category_Id, Title, Description, DateOfEvent) " +
                        $"VALUES('3', '{Title}', '{Description}', '{DateOfEvent}')";

                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }

                return success;
            }

            catch (Exception)
            {
                success = false;
                return success;
            }
        }

        /// <summary>
        /// Tar bort ett event från table tblEvent. 
        /// Måste skickas med: Eventets unika ID.
        /// </summary>
        /// <param name=""></param>
        /// <returns>
        /// Retunerar en bool som är true om det gick att ta bort eventet eller false 
        /// ifall det inte gick att ta bort eventet.'
        /// </returns>
        public async Task<bool> RemoveEvent(int id)
        {
            bool success = true;

            try
            {
                using (SqlConnection con = new SqlConnection(theConString))
                {
                    string sql =
                        $"DELETE FROM {theEventTableName} WHERE Id = {id}";

                    // Här ska det finnas en till SQL-sträng som lägger till det borttagna objektet i en "ObjectsRemoved"-table.
                    // Här ska det finnas en till SQL-sträng som tar bort objektet i alla tables där den är kopplad.

                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }

                return success;
            }

            catch (Exception)
            {
                success = false;
                return success;
            }
        }
        public bool UpdateEvent(int id, string Title, string Description, DateTime DateOfEvent)
        {
            bool success = true;

            try
            {
                using (SqlConnection con = new SqlConnection(theConString))
                {
                    string sql =
                        $"UPDATE {theEventTableName} " +
                        $"SET Title = {Title}, Description = {Description}, DateOfEvent = {DateOfEvent}" +
                        $"WHERE Id = {id}";

                    // Här ska det finnas en till SQL-sträng som uppdaterar objektet i alla tables där den finns.

                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.ExecuteNonQuery();
                    }

                }

                return success;
            }

            catch (Exception)
            {
                success = false;
                return success;
            }
        }
    }
}