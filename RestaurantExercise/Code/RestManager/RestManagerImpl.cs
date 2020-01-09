using RestaurantExercise.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RestaurantExercise.Code.RestManager
{
    public class RestManagerImpl : IRestManager
    {
        private readonly ConcurrentQueue<Message> messages;
        private readonly Dictionary<Table, List<ClientsGroups>> tables;
        private readonly List<ClientsGroups> clients;

        public RestManagerImpl()
        {
            this.messages = new ConcurrentQueue<Message>();
            this.tables = new Dictionary<Table, List<ClientsGroups>>();

            // заполним столы
            var tempTables = TableFactory.Create();
            foreach (var tempTable in tempTables)
            {
                this.tables.Add(tempTable, new List<ClientsGroups>());
            }

            Thread thread = new Thread(new ThreadStart(this.GetTask));
            thread.Start();
        }

        private void GetTask()
        {
            while(true)
            {
                if(this.messages.TryDequeue(out Message message))
                {
                    switch (message.Type)
                    {
                        case ActionType.Arrive:
                            this.Arrive(message.ClientsGroups);
                            break;
                        case ActionType.Leave:
                            this.Leave(message.ClientsGroups);
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    Thread.Sleep(50);
                }
            }
        }

        /// <summary>
        /// Посмотреть столик клиента
        /// </summary>
        /// <param name="clientsGroups"></param>
        /// <returns></returns>
        public Table Lookup(ClientsGroups clientsGroups)
        {
            return this.tables.FirstOrDefault(x => x.Value.Any(a => a.Guid == clientsGroups.Guid)).Key;
        }

        /// <summary>
        /// Прибытие клиента
        /// </summary>
        /// <param name="clientsGroups"></param>
        public void OnArrive(ClientsGroups clientsGroups)
        {
            this.messages.Enqueue(new Message(ActionType.Arrive, clientsGroups));
        }

        /// <summary>
        /// Убытие клиента
        /// </summary>
        /// <param name="clientsGroups"></param>
        public void OnLeave(ClientsGroups clientsGroups)
        {
            this.messages.Enqueue(new Message(ActionType.Leave, clientsGroups));
        }

        /// <summary>
        /// Убытие клиента
        /// </summary>
        private void Leave(ClientsGroups clientsGroups)
        {
            var table = this.tables
                .FirstOrDefault(x => x.Value.Any(a => a.Guid == clientsGroups.Guid))
                .Key;

            if(table != null)
            {
                this.tables[table].RemoveAll(x => x.Guid == clientsGroups.Guid);
            }

            var freeSize = table.Size - this.tables[table].Sum(x => x.Size);
            var tempFreeSize = freeSize;

            // будем искать людей в очереди, кто пришел раньше
            // начнем с тех, кого меньше
            for (int i = 1; i <= freeSize; i++)
            {
                var nextClients = this.clients.Where(x => x.Size == i).ToList();

                foreach (var nextClient in nextClients)
                {
                    // уберем из очереди
                    this.clients.Remove(nextClient);
                    // добавим к столу
                    this.tables[table].Add(nextClient);

                    // если свободного места не осталось, завершим перебор
                    if (freeSize < 1)
                    {
                        break;
                    }

                    freeSize -= i;
                }
            }

            this.Bored(freeSize);
        }

        /// <summary>
        /// Прибытие клиента
        /// </summary>
        /// <param name="clientsGroups"></param>
        private void Arrive(ClientsGroups clientsGroups)
        {
            // сначала пытаемся найти свободный стол нужного размера
            var freeTable = this.tables
                .Where(x => !x.Value.Any() && x.Key.Size == clientsGroups.Size)
                .FirstOrDefault()
                .Key;

            // .. ищем размера побольше
            if(freeTable == null)
            {
                freeTable = this.tables
                    .Where(x => !x.Value.Any() && x.Key.Size > clientsGroups.Size)
                    .FirstOrDefault()
                    .Key;
            }

            // .. ищем занятый
            if (freeTable == null)
            {
                freeTable = this.tables
                    .Where(x => (x.Key.Size-x.Value.Sum(s => s.Size)) >= clientsGroups.Size)
                    .FirstOrDefault()
                    .Key;
            }

            var freeSize = freeTable.Size - this.tables[freeTable].Sum(x => x.Size);

            if(freeTable == null)
            {
                this.clients.Add(clientsGroups);
                return;
            }

            this.tables[freeTable].Add(clientsGroups);

            this.Bored(freeSize);
        }

        private void Bored(Int32 freeSize)
        {
            // найдем всех, кто мог сесть на это
            var boredClients = this.clients
                .Where(x => x.Size <= freeSize)
                .ToList();

            for (int i = 0; i < boredClients.Count; i++)
            {
                this.clients.RemoveAll(x => x.IsBored() && boredClients.Any(a => a.Guid == x.Guid));
            }
        }
    }


    public enum ActionType
    {
        Arrive = 0,
        Leave = 1
    }

    public class Message
    {
        public Message(ActionType type, ClientsGroups clientsGroups)
        {
            this.Type = type;
            this.ClientsGroups = clientsGroups;
        }

        public ActionType Type { set; get; }

        public ClientsGroups ClientsGroups { set; get; }
    }
}
