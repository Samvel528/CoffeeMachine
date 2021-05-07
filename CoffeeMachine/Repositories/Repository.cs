using CoffeeMachine.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeMachine.Repositories
{
    public class Repository : IRepository
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

        public List<Coin> SelectCoins()
        {
            string commandText = @"Select * from Coin";
            List<Coin> coins = new List<Coin>();

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (var command = new SqlCommand(commandText, connection))
                {
                    using (var dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            coins.Add(new Coin()
                            {
                                Id = (int)dr["Id"],
                                Value = (int)dr["Value"]
                            });
                        }
                    }
                }
            }

            return coins;
        }

        public List<Coffee> SelectCoffees()
        {
            string commandText = @"Select * from Coffee; Select * from Sugar; Select * from Water;";

            List<Coffee> coffees = new List<Coffee>();
            List<Sugar> sugars = new List<Sugar>();
            List<Water> waters = new List<Water>();

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (var command = new SqlCommand(commandText, connection))
                {
                    using (var dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            coffees.Add(new Coffee()
                            {
                                Id = (int)dr["Id"],
                                Amount = (int)dr["Amount"],
                                Price = (int)dr["Price"],
                                SugarId = (int)dr["SugarId"],
                                WaterId = (int)dr["WaterId"]
                            });
                        }

                        dr.NextResult();

                        while (dr.Read())
                        {
                            sugars.Add(new Sugar()
                            {
                                Id = (int)dr["Id"],
                                Amount = (int)dr["Amount"]
                            });
                        }

                        dr.NextResult();

                        while (dr.Read())
                        {
                            waters.Add(new Water()
                            {
                                Id = (int)dr["Id"],
                                Amount = (int)dr["Amount"]
                            });
                        }
                    }
                }
            }

            for (int i = 0; i < coffees.Count; i++)
            {
                coffees[i].Sugar = sugars.First(sugar => sugar.Id == coffees[i].SugarId);
                coffees[i].Water = waters.First(water => water.Id == coffees[i].WaterId);
            }

            return coffees;
        }

        public Models.CoffeeMachine SelectCoffeeMachineTotals()
        {
            string commandText = @"Select * from CoffeeMachine";
            Models.CoffeeMachine coffeeMachine = new Models.CoffeeMachine();

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (var command = new SqlCommand(commandText, connection))
                {
                    using (var dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            coffeeMachine.Id = (int)dr["Id"];
                            coffeeMachine.TotalCoffee = (int)dr["TotalCoffee"];
                            coffeeMachine.TotalSugar = (int)dr["TotalSugar"];
                            coffeeMachine.TotalWater = (int)dr["TotalWater"];
                        }

                    }
                }
            }

            return coffeeMachine;
        }

        public void RecalculationTotals(Models.CoffeeMachine coffeeMachine, int id)
        {
            string commandText = @$"Update CoffeeMachine set TotalCoffee = @coffee, TotalSugar = @sugar, TotalWater = @water where Id = {id}";

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (var command = new SqlCommand(commandText, connection))
                {
                    command.Parameters.AddWithValue("@coffee", coffeeMachine.TotalCoffee);
                    command.Parameters.AddWithValue("@sugar", coffeeMachine.TotalSugar);
                    command.Parameters.AddWithValue("@water", coffeeMachine.TotalWater);

                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
