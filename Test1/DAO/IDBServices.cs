using System;
using System.Collections.Generic;
using Tutorial3.Models;

namespace Tutorial3.DAO
{
    public interface IDBServices
    {
        public IEnumerable<int> ReturnFlightCodes(int id);

        public Plane CreatePlane(int id, string name, int maxSeats);

        public IEnumerable<Flight> ReturnFlightsPassenger(int id);

        public bool CheckPassengerExists(int id);

    }
}