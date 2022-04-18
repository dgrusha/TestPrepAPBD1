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
    [Route("api/")]
    public class FireController : ControllerBase
    {
        private DBServices db = new DBServices();
        
        [HttpGet("fire-trucks/1/{id}")]
        public IActionResult GetFireTruck([FromRoute]int id)
        {
            bool res = db.CheckIfIdExist(id);
            if ( id < 0 && res == true)
            {
                return BadRequest("Parameters didn`t pass checking");
            }
            
            Truck truck = db.ReturnTheTruck(id);
            return Ok(truck);
        }
        
        [HttpPut("actions/1/endtime")]
        public IActionResult UpdateDate(DateTime date, int id)
        {
            bool res = db.CheckDateConstraints(date, id);
            if ( id < 0 || res == true)
            {
                return BadRequest("Parameters didn`t pass checking");
            }
            
            db.UpdateDate(date, id);
            return Ok("Updated");
        }
    }
    
}