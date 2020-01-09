using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RestaurantExercise.Code.RestManager;
using RestaurantExercise.Models;

namespace RestaurantExercise.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RestController : ControllerBase
    {
        private readonly ILogger<RestController> _logger;
        private readonly IRestManager restManager;

        public RestController(ILogger<RestController> logger, IRestManager restManager)
        {
            _logger = logger;
            this.restManager = restManager;
        }

        /// <summary>
        /// Прибытие клиента
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public void OnArrive([FromBody]ClientsGroups clientsGroups)
        {
            this.CheckClientGroup(clientsGroups);
            this.restManager.OnArrive(clientsGroups);
        }
        
        /// <summary>
        /// Убытие клиента (был обслужен или передумал)
        /// </summary>
        /// <param name="clientsGroups"></param>
        [HttpDelete]
        public void OnLeave([FromBody]ClientsGroups clientsGroups)
        {
            this.CheckClientGroup(clientsGroups);
            this.restManager.OnLeave(clientsGroups);
        }

        /// <summary>
        /// Посмотреть, какие клиенты сидят за столиком
        /// </summary>
        /// <param name="clientsGroups"></param>
        /// <returns></returns>
        [HttpGet]
        public Table Lookup([FromQuery]ClientsGroups clientsGroups)
        {
            this.CheckClientGroup(clientsGroups);
            return this.restManager.Lookup(clientsGroups);
        }

        /// <summary>
        /// Посмотреть все
        /// </summary>
        /// <param name="clientsGroups"></param>
        /// <returns></returns>
        [HttpGet("all")]
        public Table LookupAll([FromQuery]ClientsGroups clientsGroups)
        {
            this.CheckClientGroup(clientsGroups);
            return this.restManager.Lookup(clientsGroups);
        }

        /// <summary>
        /// Валидация 
        /// </summary>
        /// <param name="clientsGroups"></param>
        private void CheckClientGroup(ClientsGroups clientsGroups)
        {
            if (clientsGroups?.Size < 1 && clientsGroups?.Size > 6)
            {
                throw new Exception("Количество гостей от 1 до 6");
            }
        }
    }
}
