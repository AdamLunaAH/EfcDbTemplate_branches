using System;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Seido.Utilities.SeedGenerator;

namespace Models
{
	public class Car  : ISeed<Car>
	{
        [Key]
        public Guid CarId {get; set;}

        public string RegNumber {get; set;}

        public string Make { get; set; }

        public string Model { get; set; }

        //Nav props
        public Owner Owner { get; set; } = null;

        public Garage Garage { get; set; } = null;


        public bool Seeded { get; set; } = false;

        public Car Seed(SeedGenerator seeder)
        {
            string regchar = seeder.FromString("ABC, EFT, HJY, HGT, GTR");
            int regnr = seeder.Next(111,999);
            string carmake = seeder.FromString("BMW, Fiat, VOLVO, VW, Ford");
            string carmodel = seeder.FromString("Polo, 500, V70, M3, Fiesta");



            return new Car
            {
                CarId = Guid.NewGuid(),
                RegNumber = $"{regchar} {regnr}",
                Make = $"{carmake}",
                Model = $"{carmodel}",
                Seeded = true
            };
        }
    }
}