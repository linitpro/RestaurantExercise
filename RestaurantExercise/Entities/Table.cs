using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantExercise.Entities
{
    public class Table
    {
        /// <summary>
        /// Ид
        /// </summary>
        public Int64 Id { set; get; }

        /// <summary>
        /// Размер
        /// </summary>
        public Int32 Size { set; get; }

        /// <summary>
        /// Клиенты
        /// </summary>
        public List<Client> Clients { set; get; }
    }
}
