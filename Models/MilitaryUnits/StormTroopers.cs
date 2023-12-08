﻿using System;
using System.Collections.Generic;
using System.Text;

namespace PlanetWars.Models.MilitaryUnits
{

    public class StormTroopers : MilitaryUnit
    {
        private const double cost = 2.5d;
        public StormTroopers() 
            : base(cost)
        {
        }
    }
}
