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
using System.Linq;
using Tutorial3.DAO;
using Tutorial3.Models;

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
            List<Flight> SortedList = _dbServices.ReturnFlightsPassenger(id).OrderBy(o=>o.OrderDate).ToList();
            
            return Ok();
        }
        
        [HttpPut("actions/{id}/endtime")]
        public IActionResult UpdateDate(DateTime date, int id)
        {
            if ( id < 0 )
            {
                return BadRequest("Parameters didn`t pass checking");
            }
            
            return Ok("Updated");
        }
    }
    
}