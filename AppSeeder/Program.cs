using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System.Data.Common;

using Seido.Utilities.SeedGenerator;

using Configuration;
using Models;
using DbContext;
using System.Xml.Schema;

namespace AppConsole
{
    static class MyLinqExtensions
    {
        public static void Print<T>(this IEnumerable<T> collection)
        {
            collection.ToList().ForEach(item => Console.WriteLine(item));
        }
    }


    class Program
    {
        const int nrItemsSeed = 1000;

        // const int nrGarage = 10;

        static void Main(string[] args)
        {
            #region run below to test the model only

            Console.WriteLine($"\nSeeding the Model...");
            var modelList = SeedModel(nrItemsSeed);
            // var modelList2 = SeedModel(nrGarage);

            Console.WriteLine($"\nTesting Model...");
            WriteModel(modelList);
            // WriteModel(modelList2);
            #endregion


            #region  run below only when Database i created
            Console.WriteLine($"\nConnecting to database...");
            Console.WriteLine($"Database type: {AppConfig.DbSetActive.DbServer}");
            Console.WriteLine($"Connection used: {AppConfig.DbSetActive.DbConnection}");

            Console.WriteLine($"\nSeeding database...");
            try
            {
                SeedDataBase(modelList).Wait();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError: Database could not be seeded. Ensure the database is correctly created");
                Console.WriteLine($"\nError: {ex.Message}");
                Console.WriteLine($"\nError: {ex.InnerException.Message}");
                return;
            }

            Console.WriteLine("\nQuery database...");
            QueryDatabaseAsync().Wait();
            #endregion

        }


        #region Update to reflect you new Model
        private static void WriteModel(List<Car> modelList)
        {
            Console.WriteLine($"NrOfCars: {modelList.Count()}");
            Console.WriteLine($"First Car: {modelList.First().RegNumber} owned by {modelList.First().Owner.FirstName}");
            Console.WriteLine($"Last Car: {modelList.Last().RegNumber} owned by {modelList.First().Owner.FirstName}");
        }

        // private static void WriteModel2(List<Car> modelList2)
        // {
        //     Console.WriteLine($" : { modelList2.Count()}");
        // }

        private static List<Car> SeedModel(int nrItems)
        {
            var seeder = new SeedGenerator();

            //Seed Cars
            var garages = seeder.ItemsToList<Garage>(10);
            var cars = seeder.ItemsToList<Car>(nrItems);

            // var maxgarages = 0;



            foreach (var item in cars)
            {
                item.Owner = new Owner().Seed(seeder);

                // item.Garage = seeder.Bool ? seeder.FromList<Garage>(garages) : null;


                // if (maxgarages < 10)
                // {
                //     var _garages = new List<Garage>();
                //     for (int c = 0; c < seeder.Next(1, 5); c++)
                //     // {
                //     //     _garages.Add(Garage.Seed(seeder));
                //     // }
                //     {
                //         var garage = new Garage().Seed(seeder);
                //         // garage.Seed(seeder);
                //         _garages.Add(garage);

                //     }

                //     item.Garage = _garages.Count > 0 ? _garages : null;

                //     maxgarages++;

                // }


            }


            foreach (var item in garages)
            {
                item.Cars = seeder.UniqueIndexPickedFromList(seeder.Next(1, 4), cars);
                foreach (var car in item.Cars)
                {
                    car.Garage = item;
                }
            }


            // for (int i = 0; i < nrItems; i++)
                // {
                //     cars[i].Owner = new Owner().Seed(seeder);

                //     if (maxgarages < 10)
                //     {
                //         var _garages = new List<Garage>();
                //         for (int c = 0; c < seeder.Next(1, 5); c++)
                //         // {
                //         //     _garages.Add(Garage.Seed(seeder));
                //         // }
                //         {
                //             var garage = new Garage().Seed(seeder);
                //             // garage.Seed(seeder);
                //             _garages.Add(garage);

                //         }

                //         cars[i].Garage = _garages.Count > 0 ? _garages : null;

                //         maxgarages++;

                //     }



                // }
                // foreach (var item in cars)
                //     {
                //         item.Owner = new Owner().Seed(seeder);
                //         // item.Garage = new Garage().Seed(seeder);


                //         var _garages = new List<Garage>();
                //         for (int c = 0; c < seeder.Next(1, 5); c++)
                //         {
                //             _garages.Add(new Garage.Seed(seeder));
                //         }




                //     }

                // var garages = seeder.ItemsToList<Garage>(10);
                // foreach (var items in garages)
                // {

                // }

                return cars;
        }

        // private static List<Garage> SeedModel(int nrItems)
        // {

        // }

        private static async Task SeedDataBase(List<Car> _modelList)
        {
            using (var db = MainDbContext.DbContext())
            {
                #region move the seeded model into the database using EFC
                foreach (var item in _modelList)
                {
                    db.Cars.Add(item);
                }
                #endregion

                await db.SaveChangesAsync();
            }
        }

        private static async Task QueryDatabaseAsync()
        {
            Console.WriteLine("--------------");
            using (var db = MainDbContext.DbContext())
            {
                #region Reading the database using EFC
                var _modelList = await db.Cars
                    .Include(x => x.Owner)
                    // .Include(x => x.Garage)
                    .ToListAsync();
                #endregion

                WriteModel(_modelList);
            }
        }
        #endregion
    }
}
