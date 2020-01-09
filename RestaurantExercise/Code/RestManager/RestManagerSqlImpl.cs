using RestaurantExercise.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantExercise.Code.RestManager
{
    public class RestManagerSqlImpl : IRestManager
    {
        private readonly String connectionString;

        public RestManagerSqlImpl()
        {
            this.connectionString = "";
        }

        public Table Lookup(ClientsGroups clientsGroups)
        {
            throw new NotImplementedException();
        }

        public void OnArrive(ClientsGroups clientsGroups)
        {
            throw new NotImplementedException();
        }

        public void OnLeave(ClientsGroups clientsGroups)
        {
            throw new NotImplementedException();
        }
    }
}
