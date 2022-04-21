using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Tutorial3.Models;

namespace Tutorial3.DAO
{
    public class DBServices : IDBServices
    {
        private IConfiguration _configuration;

        public DBServices(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public bool CheckIfIdExist(int id)
        {
            
            int tmp_res = 0;
            using (var con = new SqlConnection(_configuration.GetConnectionString("ProductionDb")))
            using(var com = new SqlCommand())
            {
                com.Connection = con;
                com.CommandText = "Select Count(1) as counter from  FireTruck as ft Where ft.IdFiretruck = @id";
                com.Parameters.AddWithValue("id", id);
                con.Open();
                var dr = com.ExecuteReader();
                while (dr.Read())
                {
                    tmp_res = (int) dr["counter"];
                }

                return tmp_res >= 1;
            }
        }

        public Truck ReturnTheTruck(int id)
        {
            int tmp_res = 0;
            using (var con = new SqlConnection(_configuration.GetConnectionString("ProductionDb")))
            using(var com = new SqlCommand())
            {
                com.Connection = con;
                com.CommandText = "Select *  from  FireTruck as ft Where ft.IdFiretruck = @id";
                com.Parameters.AddWithValue("id", id);
                con.Open();
                var dr = com.ExecuteReader();
                Truck truck = null;
                while (dr.Read())
                {
                     truck = new Truck
                    {
                        IDTuck = (int) dr["IdFireTruck"],
                        OperationNumber = (string)dr["OperationNumber"],
                        SpecialEquipment = (bool)dr["SpecialEquipment"],
                        Actions = ReturnActions(id)
                    };
                }

                return truck;
            }
        }

        public List<Action> ReturnActions(int id)
        {
            List<int> ActionCodes = ReturnActionCodes(id);
            List<Action> res = new List<Action>();
            for (int i = 0; i < ActionCodes.Count; i++)
            {
                using (var con = new SqlConnection(_configuration.GetConnectionString("ProductionDb")))
                using(var com = new SqlCommand())
                {
                    //Finding number of firefighters
                    int FireFighetrsCounter = 0;
                    com.Connection = con;
                    com.CommandText = "Select Count(1) as counter  from  FireFighter_Action as fa Where fa.idAction = @id";
                    com.Parameters.AddWithValue("id", ActionCodes[i]);
                    con.Open();
                    var dr = com.ExecuteReader();
                    while (dr.Read())
                    {
                        FireFighetrsCounter = (int) dr["counter"];
                    }
                    dr.Close();
                    con.Close();
                    //Finding assigment date
                    DateTime AssDate = new DateTime();
                    com.CommandText = "Select AssigmentDate as ad  from  FireTruck_Action as fa Where fa.idAction = @id3 and fa.IdFireTruck = @id2";
                    com.Parameters.AddWithValue("id3", ActionCodes[i]);
                    com.Parameters.AddWithValue("id2", id);
                    con.Open();
                    dr = com.ExecuteReader();
                    while (dr.Read())
                    {
                        AssDate = (DateTime) dr["ad"];
                    }
                    dr.Close();
                    con.Close();
                    //Creating Action
                    Action action = new Action();
                    com.CommandText = "Select *  from  Action as a Where a.idAction = @id4";
                    com.Parameters.AddWithValue("id4", ActionCodes[i]);
                    con.Open();
                    dr = com.ExecuteReader();
                    while (dr.Read())
                    {
                        action = new Action
                        {
                            StartTime = dr["StartTime"] is DateTime ? (DateTime) dr["StartTime"] : default,
                            EndTime = dr["EndTime"] is DateTime ? (DateTime) dr["EndTime"] : default,
                            AssignedTime = AssDate,
                            NumOfFirefight = FireFighetrsCounter
                        };
                    }
                    res.Add(action);
                    dr.Close();
                    con.Close();
                }
            }

            return res;
        }

        public List<int> ReturnActionCodes(int id)
        {
            List<int> res = new List<int>();
            using (var con = new SqlConnection(_configuration.GetConnectionString("ProductionDb")))
            using(var com = new SqlCommand())
            {
                com.Connection = con;
                com.CommandText = "Select *  from  FireTruck_Action as fa Where fa.IdFiretruck = @id";
                com.Parameters.AddWithValue("id", id);
                con.Open();
                var dr = com.ExecuteReader();
                Truck truck = null;
                while (dr.Read())
                {
                    if((int)dr["idAction"]>0)res.Add((int)dr["idAction"]);
                }

                return res;
            }
        }

        public bool CheckDateConstraints(DateTime date, int id)
        {
            bool Res1 = false;
            bool Res2 = false;
            using (var con = new SqlConnection(_configuration.GetConnectionString("ProductionDb")))
            using(var com = new SqlCommand())
            {
                com.Connection = con;
                com.CommandText = "Select *  from  Action as a Where a.IdAction = @id";
                com.Parameters.AddWithValue("id", id);
                con.Open();
                var dr = com.ExecuteReader();
                while (dr.Read())
                {
                    if((DateTime)dr["StartTime"]<date)Res1=true;
                    DateTime EndTime = dr["EndTime"] is DateTime ? (DateTime) dr["EndTime"] : default;
                    if(EndTime==DateTime.MinValue)Res1=true;
                }

                return Res1&&Res2;
            }
        }

        public void UpdateDate(DateTime date, int id)
        {
            using (var con = new SqlConnection(_configuration.GetConnectionString("ProductionDb")))
            {
                con.Open();
                SqlTransaction tr = con.BeginTransaction();
                SqlCommand command = new SqlCommand("Update Action Set EndTime = @date Where IdAction = @id",con,tr);
                command.Parameters.AddWithValue("id", id);
                command.Parameters.AddWithValue("date", date);
                Console.WriteLine("UPDATED");
                command.ExecuteNonQuery();
                tr.Commit();
                tr.Dispose();
                con.Close();
            }
        }
        
        public bool CheckDateConstraints1()
        {
            bool Res1 = false;
            bool Res2 = false;
            using (var con = new SqlConnection(_configuration.GetConnectionString("ProductionDb")))
            using(var com = new SqlCommand())
            {
                com.Connection = con;
                com.CommandText = "Select *  from  Action ";
                con.Open();
                var dr = com.ExecuteReader();
                while (dr.Read())
                {
                    
                }
                Console.Write("RUN");
                return Res1&&Res2;
            }
        }

        
        //
        
        public IEnumerable<int> ReturnFlightCodes(int id)
        {
            List<int> Flights = new List<int>();
            using (var con = new SqlConnection(_configuration.GetConnectionString("ProductionDb")))
            using(var com = new SqlCommand())
            {
                com.Connection = con;
                com.CommandText = "Select *  from  Flight_Passenger as fp Where fp.IdFiretruck = @id";
                com.Parameters.AddWithValue("id", id);
                con.Open();
                var dr = com.ExecuteReader();
                while (dr.Read())
                {
                    if((int)dr["IdFlight"]>0)Flights.Add((int)dr["IdFlight"]);
                }

                return Flights;
            }
        }

        public Plane CreatePlane(int id, string name, int maxSeats)
        {
            Plane planeRes = new Plane
            {
                IDPlane = id,
                Name = name,
                MaxSeats = maxSeats
            };
            return planeRes;
        }

        public IEnumerable<Flight> ReturnFlightsPassenger(int id)
        {
            List<int> actionCodes = ReturnActionCodes(id);
            List<Flight> flights = new List<Flight>();
            for (int i = 0; i < actionCodes.Count; i++)
            {
                using (var con = new SqlConnection(_configuration.GetConnectionString("ProductionDb")))
                using(var com = new SqlCommand())
                {
                    com.Connection = con;
                    com.CommandText = @"Select c.City as city, p.idPlane as plane, p.Name as name, p.MaxSeats as seats
                                        From Flight as f, Plane as p, CityDict as c
                                        Where f.IdCityDict = c.IdCityDict and p.idPlane = f.idPlane and 
                                        f.Flights.IdFlight = @id";
                    com.Parameters.AddWithValue("id", id);
                    con.Open();
                    Plane plane = new Plane();
                    string city ="";
                    var dr = com.ExecuteReader();
                    while (dr.Read())
                    {
                        city = dr["city"].ToString();
                        int idPlane = (int)dr["plane"];
                        string name = dr["name"].ToString();
                        int maxSeats = (int)dr["seats"];
                        plane = CreatePlane(idPlane, name, maxSeats);
                    }

                    Flight flight = new Flight
                    {
                        Plane = plane,
                        CityName = city
                    };
                    flights.Add(flight);
                }
            }
            return flights;
        }

        public bool CheckPassengerExists(int id)
        {
            int passengerCounter = 0;
            using (var con = new SqlConnection(_configuration.GetConnectionString("ProductionDb")))
            using(var com = new SqlCommand())
            {
                com.Connection = con;
                com.CommandText = "Select Count(1) as counter from  Passenger as p Where p.idPassenger = @id";
                com.Parameters.AddWithValue("id", id);
                con.Open();
                var dr = com.ExecuteReader();
                while (dr.Read())
                {
                    passengerCounter = (int) dr["counter"];
                }

                return passengerCounter >= 1;
            }
        }
    }
    
    
    
}