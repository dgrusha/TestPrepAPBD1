using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using Tutorial3.DAO;

namespace Tutorial3.Controllers
{
    [ApiController]
    [Route("api/passengers")]
    public class PassengerController : ControllerBase
    {
        private IDBServices _dbServices;
        
        public PassengerController(IDBServices dbServices)
        {
            _dbServices = dbServices;
        }


        [HttpGet("{id}/flights")]
        public IActionResult GetFlights([FromRoute]int id)
        {
            bool passengerExist = _dbServices.CheckPassengerExists(id);
            if ( id < 0 )
            {
                return BadRequest("Parameters didn`t pass checking");
            }
            if (passengerExist == false)
            {
                return NotFound("Such a passenger does not exist");
            }
            
            return Ok(_dbServices.ReturnFlightsPassenger(id));
        }
        
        [HttpPost]
        public IActionResult PutPassenger(int idP, int idF)
        {
            bool passengerExist = _dbServices.CheckPassengerExists(idP);
            if ( idP < 0  || idF < 0)
            {
                return BadRequest("Parameters didn`t pass checking");
            }
            if (passengerExist == false)
            {
                return NotFound("Such a passenger does not exist");
            }
            if (_dbServices.WasFlightFinished(idF) == true)
            {
                return BadRequest("Flight was finished");
            }

            if (_dbServices.SeatsPerFlight(idF) - _dbServices.PassengersPerFlight(idF) <= 0)
            {
                return BadRequest("No more seats");
            }

            return Ok("Updated");
        }
        
    }
    
}