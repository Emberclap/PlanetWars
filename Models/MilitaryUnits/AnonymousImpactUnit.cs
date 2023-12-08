using System;
using System.Collections.Generic;
using System.Text;

namespace PlanetWars.Models.MilitaryUnits
{
    public class AnonymousImpactUnit : MilitaryUnit
    {
        private const double cost = 30d;
        public AnonymousImpactUnit() 
            : base(cost)
        {
        }
    }
}
