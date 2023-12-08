using PlanetWars.Models.Weapons.Contracts;
using PlanetWars.Utilities.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlanetWars.Models.Weapons
{
    public abstract class Weapon : IWeapon
    {
        private int destructionLevel;
        private double price;
        public Weapon(int destructionLevel, double price)
        {
            this.DestructionLevel = destructionLevel;
            Price = price;
        }

        public double Price
        {
            get => price;
            private set => price = value;
        }
        public int DestructionLevel
        {
            get => destructionLevel;
            private set
            {
                if (value < 1)
                {
                    throw new ArgumentException(ExceptionMessages.TooLowDestructionLevel);
                }
                else if (value > 10)
                {
                    throw new ArgumentException(ExceptionMessages.TooHighDestructionLevel);
                }
                destructionLevel = value;
            }
        }
        public override string ToString()
         {
             return $"{this.GetType().Name}";
         }
    }
}
