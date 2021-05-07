﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeMachine.Models
{
    public class Coffee
    {
        public int Id { get; set; }

        public int Amount { get; set; }

        public int Price { get; set; }

        public int SugarId { get; set; }

        public Sugar Sugar { get; set; }

        public int WaterId { get; set; }

        public Water Water { get; set; }
    }
}
