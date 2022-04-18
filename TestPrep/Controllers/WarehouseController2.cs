using System;
using System.Collections;
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
    [Route("api/warehouse2")]
    public class WarehouseController2 : ControllerBase
    {
        private DBServices db = new DBServices();
        
        [HttpPost("WarehousePost")]
        public IActionResult UpdateWarehouse(int idProduct, int idWarehouse, int amount, DateTime createdAt)
        {
            int OrderId = db.CheckExistence2(idProduct, amount, createdAt);
            db.UseStoredProcedure(idProduct, idWarehouse, amount, createdAt);
            int pk = db.ReturnPK(idProduct, idWarehouse, OrderId, amount);
            return Ok(pk);
        }
    }
    
}