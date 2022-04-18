using System;

namespace Tutorial3.DAO
{
    public interface IDBServices
    { 
        public bool CheckExistence1(int idProduct, int idWarehouse);
        public int CheckExistence2(int idProduct,int amount, DateTime createdAt);
        
        public int ExistOrderInProdWarehouse(int OrderId);

        public void UpdateDate(int OrderId);

        public void InsertTestData(int idProduct,int idWarehouse,int OrderId,int amount, DateTime createdAt);
        
        public decimal TakePrice(int ProductId);

        public int ReturnPK(int idProduct, int idWarehouse, int OrderId, int amount);

        public void UseStoredProcedure(int idProduct, int idWarehouse, int amount, DateTime createdAt);
    }
}