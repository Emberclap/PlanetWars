using PlanetWars.Models.MilitaryUnits;
using PlanetWars.Models.MilitaryUnits.Contracts;
using PlanetWars.Models.Planets.Contracts;
using PlanetWars.Models.Weapons;
using PlanetWars.Models.Weapons.Contracts;
using PlanetWars.Repositories;
using PlanetWars.Repositories.Contracts;
using PlanetWars.Utilities.Messages;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Net;
using System.Text;
using System.Transactions;

namespace PlanetWars.Models.Planets
{
    public class Planet : IPlanet
    {
        private string name;
        private double budget;
        private double militaryPower;
        private IRepository<IMilitaryUnit> unitRepository;
        private IRepository<IWeapon> weaponsRepository;

        public Planet(string name, double budget)
        {
            Name = name;
            Budget = budget;
            unitRepository = new UnitRepository();
            weaponsRepository = new WeaponRepository();
        }

        public string Name 
        { 
            get => name;
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException(ExceptionMessages.InvalidPlanetName);
                }
                name = value;
            }
        }

        public double Budget 
        {
            get => budget;
            private set
            {
                if (value < 0)
                {
                    throw new ArgumentException(ExceptionMessages.InvalidBudgetAmount);
                }
                budget = value;
            }
        }

        public double MilitaryPower => MilitaryPowerSum();
        public IReadOnlyCollection<IMilitaryUnit> Army => unitRepository.Models;

        public IReadOnlyCollection<IWeapon> Weapons => weaponsRepository.Models;

        public double MilitaryPowerSum()
        {
            double weponsDestruction = Army.Sum(u => u.EnduranceLevel);
            double unitsEndurance = Weapons.Sum(w => w.DestructionLevel);
            double totalAmount = weponsDestruction + unitsEndurance;
            if (Army.Any(x=>x.GetType().Name == nameof(AnonymousImpactUnit)))
            {
                totalAmount += totalAmount * 0.3;
            }
            if (Weapons.Any(w=>w.GetType().Name == nameof(NuclearWeapon)))
            {
                totalAmount += totalAmount * 0.45;
            }

            return Math.Round(totalAmount, 3);
        }
        public void AddUnit(IMilitaryUnit unit) => unitRepository.AddItem(unit);
        

        public void AddWeapon(IWeapon weapon) => weaponsRepository.AddItem(weapon);


        public string PlanetInfo()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"Planet: {this.Name}");
            sb.AppendLine($"--Budget: {this.Budget} billion QUID");
            sb.Append($"--Forces: ");
            if(!Army.Any())
            {
                sb.AppendLine("No units");
            }
            else
            {
               
                sb.AppendLine(string.Join(", ", Army));
            }
            sb.Append($"--Combat equipment: ");
            if(!Weapons.Any())
            {
                sb.AppendLine("No weapons");
            }
            else
            {
                sb.AppendLine(string.Join(", ", this.Weapons));
            }
            sb.AppendLine($"--Military Power: {this.MilitaryPower}");
            return sb.ToString().TrimEnd();
        }

        public void Profit(double amount)
        {
            this.budget += amount;
        }

        public void Spend(double amount)
        {
            double current = this.budget;
            if((current -= amount) < 0)
            {
                throw new InvalidOperationException(ExceptionMessages.UnsufficientBudget);
            }
            else
            {
                this.budget -= amount;
            }
        }

        public void TrainArmy()
        {
            foreach (var unit in Army)
            {
                unit.IncreaseEndurance();
            }
        }
    }
}
