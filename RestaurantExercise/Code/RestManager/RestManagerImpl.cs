using RestaurantExercise.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantExercise.Code.RestManager
{
    /// <summary>
    /// Менеджер отвечающий за ресторан
    /// </summary>
    public class RestManagerImpl : IRestManager
    {
        /// <summary>
        /// Очередь клиентов
        /// </summary>
        private readonly ConcurrentQueue<ClientsGroups>[] clientsQueue;

        /// <summary>
        /// Очередь занятых столов
        /// </summary>
        private readonly ConcurrentDictionary<Table, ClientsGroups>[] tables;

        /// <summary>
        /// Очереди доступных столиков
        /// </summary>
        private readonly ConcurrentQueue<Table>[] availableTablesQueue;

        public RestManagerImpl()
        {
            // очереди столов с индексами их размера
            var tables1 = TableFactory.Create();

            this.tables = new ConcurrentDictionary<Table, ClientsGroups>[5];
            for (int i = 0; i < 5; i++)
            {
                this.tables[i] = new ConcurrentDictionary<Table, ClientsGroups>();
            }


            this.availableTablesQueue = new ConcurrentQueue<Table>[5];
            for (int i = 2; i <= 6; i++)
            {
                this.availableTablesQueue[i-2] = new ConcurrentQueue<Table>(tables1.Where(x => x.Size == i));
            }

            // очереди клиентов с индексами их размера
            this.clientsQueue = new ConcurrentQueue<ClientsGroups>[6];
            for (int i = 0; i < 6; i++)
            {
                this.clientsQueue[i] = new ConcurrentQueue<ClientsGroups>();
            }
        }

        /// <summary>
        /// По клиенту определить стол
        /// </summary>
        /// <param name="clientsGroups"></param>
        /// <returns></returns>
        public Table Lookup(ClientsGroups clientsGroups)
        {
            for (int i = clientsGroups.Size; i <= 6; i++)
            {
                var table = this.tables[i]
                    .FirstOrDefault(x => x.Value.Guid == clientsGroups.Guid).Key;

                if(table != null)
                {
                    return table;
                }
            }

            return null;
        }

        /// <summary>
        /// Прибытие
        /// </summary>
        /// <param name="clientsGroups"></param>
        public void OnArrive(ClientsGroups clientsGroups)
        {
            var boo = false;

            var tempSize = 2;
            if (clientsGroups.Size != 1)
            {
                tempSize = clientsGroups.Size;
            }

            for (int i = tempSize; i <= 6; i++)
            {
                boo = this.availableTablesQueue[tempSize-2].TryDequeue(out Table table);

                if(boo)
                {
                    table.ClientsGroups = clientsGroups;
                    this.tables[tempSize-2].TryAdd(table, clientsGroups);
                    break;
                }
                else
                {
                    continue;
                }
            }

            if(!boo)
            {
                this.clientsQueue[clientsGroups.Size].Enqueue(clientsGroups);
            }
        }

        public void OnLeave(ClientsGroups clientsGroups)
        {
            var tempSize = clientsGroups.Size;
            if(tempSize == 1)
            {
                tempSize = 2;
            }

            for (int i = tempSize; i <= 6; i++)
            {
                var table = this.tables[tempSize - 2]
                    .FirstOrDefault(x => x.Value.Guid == clientsGroups.Guid).Key;

                if(table != null)
                {
                    this.tables[tempSize - 2].TryRemove(table, out ClientsGroups client);
                    this.availableTablesQueue[tempSize - 2].Enqueue(table);
                    break;
                }

                continue;
            }
        }
    }
}
