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


        //
        
        public List<int> ReturnFlightCodes(int id)
        {
            List<int> Flights = new List<int>();
            using (var con = new SqlConnection(_configuration.GetConnectionString("ProductionDb")))
            using(var com = new SqlCommand())
            {
                com.Connection = con;
                com.CommandText = "Select *  from  Flight_Passenger as fp Where fp.IdPassenger = @id";
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
            List<int> actionCodes = ReturnFlightCodes(id);
            List<Flight> flights = new List<Flight>();
            for (int i = 0; i < actionCodes.Count; i++)
            {
                using (var con = new SqlConnection(_configuration.GetConnectionString("ProductionDb")))
                using(var com = new SqlCommand())
                {
                    com.Connection = con;
                    com.CommandText = @"Select c.City as city, p.idPlane as plane, p.Name as name, p.MaxSeats as seats, f.FlightDate
                                        From Flight as f, Plane as p, CityDict as c
                                        Where f.IdCityDict = c.IdCityDict and p.idPlane = f.idPlane and 
                                        f.IdFlight = @id";
                    com.Parameters.AddWithValue("id", actionCodes[i]);
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

        public bool WasFlightFinished(int id)
        {
            bool wasFinished = false;
            using (var con = new SqlConnection(_configuration.GetConnectionString("ProductionDb")))
            using(var com = new SqlCommand())
            {
                com.Connection = con;
                com.CommandText = "Select FlightDate as dateF from  Flight as f Where f.idFlight = @id";
                com.Parameters.AddWithValue("id", id);
                con.Open();
                DateTime? dateTime = null;
                var dr = com.ExecuteReader();
                while (dr.Read())
                {
                    dateTime = (DateTime) dr["dateF"];
                }

                if (dateTime<DateTime.Now)
                {
                    wasFinished = true;
                }

                return wasFinished;
            }
        }

        public int SeatsPerFlight(int id)
        {
            int seatsPerFlight = 0;
            using (var con = new SqlConnection(_configuration.GetConnectionString("ProductionDb")))
            using(var com = new SqlCommand())
            {
                com.Connection = con;
                com.CommandText = "Select maxSeats as seats from  Plane as p, Flight as f Where f.idPlane = p.IdPlane and f.IdFlight = @id";
                com.Parameters.AddWithValue("id", id);
                con.Open();
                var dr = com.ExecuteReader();
                while (dr.Read())
                {
                    seatsPerFlight = (int) dr["seats"];
                }
                return seatsPerFlight;
            }
        }

        public int PassengersPerFlight(int id)
        {
            int passengersPerFlight = 0;
            using (var con = new SqlConnection(_configuration.GetConnectionString("ProductionDb")))
            using(var com = new SqlCommand())
            {
                com.Connection = con;
                com.CommandText = "Select Count(1) as passangers from  Flight_Passenger as fp, Flight as f Where f.idFlight = fp.idFlight and f.IdFlight = @id";
                com.Parameters.AddWithValue("id", id);
                con.Open();
                var dr = com.ExecuteReader();
                while (dr.Read())
                {
                    passengersPerFlight = (int) dr["passangers"];
                }
                return passengersPerFlight;
            }
        }

        public void InsertPassengers(int idF, int idP)
        {
            using (var con = new SqlConnection(_configuration.GetConnectionString("ProductionDb")))
            {
                con.Open();
                using (SqlTransaction tr = con.BeginTransaction())
                {
                    SqlCommand command = new SqlCommand(@"Insert into Flight_Passenger(IdFlight, IdPassenger)values(@idF, @idP)", con,tr);
                    command.Parameters.AddWithValue("idF", idF);
                    command.Parameters.AddWithValue("idP", idP);
                    command.ExecuteNonQuery();
                    tr.Commit();
                }
            }
        }
    }
    
    
    
}