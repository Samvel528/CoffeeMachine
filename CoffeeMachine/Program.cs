using CoffeeMachine.Functionalities;
using CoffeeMachine.Repositories;
using System;

namespace CoffeeMachine
{
    class Program
    {
        static void Main(string[] args)
        {
            IRepository repository = new Repository();
            var coins = repository.SelectCoins();
            var coffees = repository.SelectCoffees();
            var machineTotals = repository.SelectCoffeeMachineTotals();
            
            Functionality functionality = new Functionality();
            var totalMoney = functionality.InputCoin(coins);
            var selectedCoffee = functionality.ChooseCoffee(coffees, machineTotals, totalMoney);

            repository.RecalculationTotals(selectedCoffee.Item2, 1);
            Console.ReadKey();
        }
    }
}
