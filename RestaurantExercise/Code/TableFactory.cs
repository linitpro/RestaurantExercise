using RestaurantExercise.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantExercise.Code
{
    /// <summary>
    /// Фабрика по производству столов
    /// </summary>
    public class TableFactory
    {
        /// <summary>
        /// Создает коллекцию столов
        /// </summary>
        /// <returns></returns>
        public static List<Table> Create()
        {
            var random = new Random();
            var tables = new List<Table>();

            // сделаем минимальное необходимое кол-во столов по заданию
            for (Int32 i = 1; i < 7; i++)
            {
                tables.Add(new Table(i));
            }

            for (int i = 5; i < Defaults.MAXCOUNTOFTABLES; i++)
            {
                tables.Add(new Table(random.Next(1, 6)));   
            }

            return tables;
        }
    }
}
