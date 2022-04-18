using System;
using System.Collections.Generic;

namespace Tutorial3.DAO
{
    public interface IDBServices
    {
        public bool CheckIfIdExist(int id);
        
        public Truck ReturnTheTruck(int id);
        
        public List<Action> ReturnActions(int id);

        public List<int> ReturnActionCodes(int id);

        public bool CheckDateConstraints(DateTime date, int id);

        public void UpdateDate(DateTime date, int id);
    }
}