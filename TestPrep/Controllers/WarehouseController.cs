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
    [Route("api/warehouse")]
    public class WarehouseController : ControllerBase
    {
        private DBServices db = new DBServices();
        
        [HttpPut("WarehousePut")]
        public IActionResult UpdateWarehouse(int idProduct, int idWarehouse, int amount, DateTime createdAt)
        {
            
            int OrderId = db.CheckExistence2(idProduct, amount, createdAt);
            bool check1 = db.CheckExistence1(idProduct, idWarehouse);
            if (idProduct == null || idWarehouse == null || amount < 1 || createdAt == null || check1 == false || OrderId == -1)
            {
                return BadRequest("Parameters didn`t pass checking");
            }
            else
            {
                if (db.ExistOrderInProdWarehouse(OrderId) == -1)
                {
                    return Conflict("Such an order already exists");
                }
                db.UpdateDate(OrderId);
                db.InsertTestData(idProduct, idWarehouse, OrderId, amount, createdAt);
            }

            int pk = db.ReturnPK(idProduct, idWarehouse, OrderId, amount);
            return Ok(pk);
        }
    }
    
}