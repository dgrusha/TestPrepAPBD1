using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Tutorial3.DAO
{
    public class DBServices : IDBServices
    {
        
        public bool CheckExistence1(int idProduct, int idWarehouse)
        {
            bool res1 = false;
            bool res2 = false;
            using (var con = new SqlConnection("Data Source = db-mssql.pjwstk.edu.pl; Initial Catalog=2019SBD; Integrated Security = True"))
            using(var com = new SqlCommand())
            {
                com.Connection = con;
                com.CommandText = "Select Count(1) as counter from WareHouse as wr Where wr.IdWarehouse = @id ";
                com.Parameters.AddWithValue("id", idWarehouse);
                con.Open();
                var dr = com.ExecuteReader();
                Console.WriteLine("data");
                while (dr.Read())
                {
                    int count = (int) dr["counter"];
                    Console.WriteLine(count);
                    if (count >= 1) res1 = true;
                }
                dr.Close();
                com.CommandText = "Select Count(1) as counter from Product as pr Where pr.IdProduct = @id2 ";
                com.Parameters.AddWithValue("id2", idProduct);
                dr = com.ExecuteReader();
                while (dr.Read())
                {
                    int count = (int) dr["counter"];
                    if (count >= 1) res2 = true;
                }
            }
            return res1 && res2;
        }
        
        public int CheckExistence2(int idProduct,int amount, DateTime createdAt)
        {
            int res1 = -1;
            using (var con = new SqlConnection("Data Source = db-mssql.pjwstk.edu.pl; Initial Catalog=2019SBD; Integrated Security = True"))
            using(var com = new SqlCommand())
            {
                com.Connection = con;
                com.CommandText = "Select ord.IdOrder as orderId from  Order1 as ord Where ord.IdProduct = @id and ord.Amount = @amount and ord.CreatedAt < @Cdate";
                com.Parameters.AddWithValue("id", idProduct);
                com.Parameters.AddWithValue("amount", amount);
                com.Parameters.AddWithValue("Cdate", createdAt);
                con.Open();
                var dr = com.ExecuteReader();
                Console.WriteLine("data");
                while (dr.Read())
                {
                    res1 = (int) dr["orderId"];
                    Console.WriteLine(res1);
                }
                
            }
            return res1;
        }

        public int ExistOrderInProdWarehouse(int OrderId)
        {
            int res1 = 1;
            using (var con = new SqlConnection("Data Source = db-mssql.pjwstk.edu.pl; Initial Catalog=2019SBD; Integrated Security = True"))
            using(var com = new SqlCommand())
            {
                com.Connection = con;
                com.CommandText = "Select Count(1) as exist from  Product_Warehouse as wr Where wr.IdOrder = @id";
                com.Parameters.AddWithValue("id", OrderId);
                con.Open();
                var dr = com.ExecuteReader();
                while (dr.Read())
                {
                    res1 = (int) dr["exist"];
                    Console.WriteLine(res1);
                    if (res1 >= 1) res1 = -1;
                }
                
            }
            return res1;
        }

        public void UpdateDate(int OrderId)
        {
            using (var con = new SqlConnection("Data Source = db-mssql.pjwstk.edu.pl; Initial Catalog=2019SBD; Integrated Security = True"))
            {
                con.Open();
                SqlTransaction tr = con.BeginTransaction();
                SqlCommand command = new SqlCommand("Update Order1 Set FulfilledAt = GETDATE() Where IdOrder = @id",con,tr);
                command.Parameters.AddWithValue("id", OrderId);
                Console.WriteLine("UPDATED");
                command.ExecuteNonQuery();
                tr.Commit();
                tr.Dispose();
                con.Close();
            }
        }

        public void InsertTestData(int idProduct,int idWarehouse,int orderId,int amount, DateTime createdAt)
        {
            using (var con = new SqlConnection("Data Source = db-mssql.pjwstk.edu.pl; Initial Catalog=2019SBD; Integrated Security = True"))
            {
                decimal Price = TakePrice(idProduct);
                con.Open();
                using (SqlTransaction tr = con.BeginTransaction())
                {
                    SqlCommand command = new SqlCommand(@"INSERT INTO Product_Warehouse( idWarehouse, idProduct, idOrder, Amount, Price, CreatedAt)
                    VALUES(@idW, @idP, @idO, @Amount, @Price, GETDATE());", con,tr);
                    command.Parameters.AddWithValue("idW", idWarehouse);
                    command.Parameters.AddWithValue("idP", idProduct);
                    command.Parameters.AddWithValue("idO", orderId);
                    command.Parameters.AddWithValue("Amount", amount);
                    command.Parameters.AddWithValue("Price", Price*amount);
                    command.ExecuteNonQuery();
                    tr.Commit();
                }
            }
        }

        public decimal TakePrice(int ProductId)
        {
            decimal res1 = -1;
            using (var con = new SqlConnection("Data Source = db-mssql.pjwstk.edu.pl; Initial Catalog=2019SBD; Integrated Security = True"))
            using(var com = new SqlCommand())
            {
                com.Connection = con;
                com.CommandText = "Select Price as productPrice from  Product as pr Where pr.IdProduct = @id";
                com.Parameters.AddWithValue("id", ProductId);
                con.Open();
                var dr = com.ExecuteReader();
                while (dr.Read())
                {
                    res1 = (decimal) dr["productPrice"];
                    Console.WriteLine(res1);
                }
                
            }
            return res1;
        }

        public int ReturnPK(int idProduct, int idWarehouse, int OrderId, int amount)
        {
            int res1 = -1;
            using (var con = new SqlConnection("Data Source = db-mssql.pjwstk.edu.pl; Initial Catalog=2019SBD; Integrated Security = True"))
            using(var com = new SqlCommand())
            {
                com.Connection = con;
                com.CommandText = "Select IdProductWarehouse as resultPK from  Product_Warehouse  Where IdProduct = @idP and IdWarehouse = @idW and IdOrder = @idO and Amount = @Amount ";
                com.Parameters.AddWithValue("idW", idWarehouse);
                com.Parameters.AddWithValue("idP", idProduct);
                com.Parameters.AddWithValue("idO", OrderId);
                com.Parameters.AddWithValue("Amount", amount);
                con.Open();
                var dr = com.ExecuteReader();
                while (dr.Read())
                {
                    res1 = (int) dr["resultPK"];
                    Console.WriteLine(res1);
                }
                
            }
            return res1;
        }

        public void UseStoredProcedure(int idProduct, int idWarehouse, int amount, DateTime createdAt)
        {
            using (var conn = new SqlConnection("Data Source = db-mssql.pjwstk.edu.pl; Initial Catalog=2019SBD; Integrated Security = True"))
            using (var command = new SqlCommand("AddProductToWarehouse", conn) { 
                CommandType = CommandType.StoredProcedure }) {
                conn.Open();
                /*@IdProduct INT, @IdWarehouse INT, @Amount INT,  
                    @CreatedAt DATETIME*/
                command.Parameters.Add("@IdProduct", SqlDbType.Int).Value = idProduct;
                command.Parameters.Add("@IdWarehouse", SqlDbType.Int).Value = idWarehouse;
                command.Parameters.Add("@Amount", SqlDbType.Int).Value = amount;
                command.Parameters.Add("@CreatedAt", SqlDbType.DateTime).Value = createdAt;
                command.ExecuteNonQuery();
            }

        }
    }
}