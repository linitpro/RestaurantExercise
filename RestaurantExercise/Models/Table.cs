using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantExercise.Models
{
    public class Table
    {
        public Table(Int32 size)
        {
            this.Size = size;
        }

        public Int32 Size { set; get; }

        /// <summary>
        /// Клиенты
        /// </summary>
        public ClientsGroups ClientsGroups { set; get; }

        /// <summary>
        /// Клиенты, находящиеся за столиком
        /// </summary>
        public BlockingCollection<ClientsGroups> Clients { set; get; }
    }
}
