using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using INF272DB1StudentFiles2022.Models;

namespace INF272DB1StudentFiles2022.Models
{
    public class DataService
    {

        //it is optional to use the Stringbuilder but as was explained in the video, this makes your database more secure
        private SqlConnectionStringBuilder stringBuilder = new SqlConnectionStringBuilder(); 

        private SqlConnection currConnection;

        //The code below 'declares' the DataSevice class as a singleton
        private static DataService instance;

        public String getConnectionByHand()
        {
            String connectionString = "Data Source=MARCO_LAPTOP;Initial Catalog=touristDestinations;Integrated Security=True";
            return connectionString;
        }
        public static DataService getDataService()
        {
            if (instance == null)
            {
                instance = new DataService();
            }
            return instance;
        }

        //Complete the missing code below

        public string getConnectionString()
        {
            String conString = getConnectionByHand();
            currConnection = new SqlConnection(conString);
            currConnection.Open();
            return conString;
        }

        public bool openConnection()
        {
            bool status = true;
            try
            {
                String conString = getConnectionByHand();
                currConnection = new SqlConnection(conString);
                currConnection.Open();
            }
            catch (Exception exc)
            {
                status = false;
            }
            return status;
        }

        public bool closeConnection()
        {
            if (currConnection != null)
            {
                currConnection.Close();
            }
            return true;

        }

        public bool deleteDest(int id)
        {
            bool status = false;
            try
            {
                openConnection();
                SqlCommand command = new SqlCommand("delete from TouristSites  where id = " + id + ";", currConnection);
                command.ExecuteNonQuery();
                closeConnection();
                status = true;
            }
            catch (Exception e)
            {
                status = false;
            }
            finally
            {
                closeConnection();
            }
            return status;
        }

        public bool updateDest(DestinationModel someDest)
        {
            openConnection();
            String cmd = "update TouristSites set Name = '" + someDest.Name + "', Website = '" + someDest.Website + "' where id = " + someDest.ID + ";";
            SqlCommand command = new SqlCommand(cmd, currConnection);
            command.ExecuteNonQuery();
            closeConnection();
        }

        public void createDest(DestinationModel someDest)
        {
             openConnection();
            String cmd = "INSERT INTO " + someDest;
            SqlCommand command = new SqlCommand(cmd, currConnection);
            command.ExecuteNonQuery();
            closeConnection(); 
        }

        //code is provided for the read functionality below.
        //Name, Website and ID refers to class properties
        public List<DestinationModel> getDest()
        {

            List<DestinationModel> destinations = new List<DestinationModel>();
            try
            {
                openConnection();
                SqlCommand command = new SqlCommand("select * from TouristSites", currConnection);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        DestinationModel tmpDest = new DestinationModel();
                        tmpDest.Name = reader["Name"].ToString();
                        tmpDest.Website = reader["Website"].ToString();
                        tmpDest.ID = Convert.ToInt32(reader["id"]);

                        destinations.Add(tmpDest);
                    }
                }
                closeConnection();
            }
            catch
            {
            }
        
            return destinations;
        }

        //code is provided to get one Destination by id - ID refers here to a DestinationModel class property.
        public DestinationModel getDestById(int id)
        {
            DestinationModel dest = null;
            List<DestinationModel> dests = getDest();

            if (dests.Any(d => d.ID == id))
            {
                int index = dests.FindIndex(d => d.ID == id);
                dest = dests[index];
            }

            return dest;
        }
    }
}