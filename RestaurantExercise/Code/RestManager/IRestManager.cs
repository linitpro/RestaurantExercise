using RestaurantExercise.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantExercise.Code.RestManager
{
    public interface IRestManager
    {
        void OnArrive(ClientsGroups clientsGroups);
        void OnLeave(ClientsGroups clientsGroups);
        Table Lookup(ClientsGroups clientsGroups);
    }
}
