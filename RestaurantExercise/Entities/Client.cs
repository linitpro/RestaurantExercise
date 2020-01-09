using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantExercise.Entities
{
    public class Client
    {
        public Int64 Id { set; get; }

        public Int32 Size { set; get; }

        /// <summary>
        /// Связанная таблица
        /// </summary>
        public Table Table { set; get; }

        public Int64? TableId { set; get; }
    }
}
