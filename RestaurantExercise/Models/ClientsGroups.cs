﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantExercise.Models
{
    public class ClientsGroups
    {
        public ClientsGroups()
        {
            this.Guid = Guid.NewGuid();
        }

        public Byte Size { set; get; }

        public Guid Guid { set; get; }
    }
}