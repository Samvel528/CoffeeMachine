using CoffeeMachine.Exceptions;
using CoffeeMachine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeMachine.Functionalities
{
    public class Functionality
    {
        public int InputCoin(List<Coin> coins)
        {
            int tempCoinValue;
            bool isEntered = true;
            int totalAmount = 0;
            ConsoleKeyInfo cKI;
            Console.WriteLine("The machine accepts the following pennies:\n");
            for (int i = 0; i < coins.Count; i++)
            {
                Console.WriteLine($"{i + 1}.{coins[i].Value}");
            }

            while (isEntered)
            {
                Console.Write("\nPlease enter coin: ");
                tempCoinValue = int.Parse(Console.ReadLine());

                try
                {
                    if (!coins.Any(coin => coin.Value == tempCoinValue))
                        throw new ArgumentException("The machine does not accept such coins.");
                    else
                    {
                        Console.WriteLine("If you do not want to deposit any more money, click Enter else click other.");

                        cKI = Console.ReadKey(true);

                        Console.WriteLine($"Key pressed: {cKI.Key}\n");

                        if (cKI.Key == ConsoleKey.Enter)
                            isEntered = false;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    continue;
                }

                totalAmount += tempCoinValue;
            }

            return totalAmount;
        }

        public Tuple<Coffee, Models.CoffeeMachine> ChooseCoffee(List<Coffee> coffees, Models.CoffeeMachine coffeeMachineTotals, int totalMoney)
        {
            Console.WriteLine("Please choose coffee.");
            Console.WriteLine("Coffees list: -----------------------\n");

            for (int i = 0; i < coffees.Count; i++)
            {
                Console.WriteLine($"{i + 1}: CoffeeAmount: {coffees[i].Amount}, Price: {coffees[i].Price}, SugarAmount: {coffees[i].Sugar.Amount}, WaterAmount: {coffees[i].Water.Amount}");
            }


            bool exactNumber = true;
            int coffeeNumber = 0;
            while (exactNumber)
            {
                Console.Write("\nEnter 1-10 to choose coffee: ");
                coffeeNumber = int.Parse(Console.ReadLine());

                if (coffeeNumber > 0 && coffeeNumber < 11)
                {
                    Console.WriteLine($"Selected number {coffeeNumber} coffee.");
                    try
                    {
                        if (coffees[coffeeNumber - 1].Price > totalMoney)
                            throw new InsufficientFundsException("Your money is not enough.");

                        if (coffees[coffeeNumber - 1].Amount > coffeeMachineTotals.TotalCoffee ||
                            coffees[coffeeNumber - 1].Sugar.Amount > coffeeMachineTotals.TotalSugar ||
                            coffees[coffeeNumber - 1].Water.Amount > coffeeMachineTotals.TotalWater)
                            throw new InsufficientFundsException("Insufficient funds in coffee machine!!! Please enter other coffee.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        continue;
                    }

                    Recalculation(coffeeMachineTotals, coffees[coffeeNumber - 1].Amount, coffees[coffeeNumber - 1].Sugar.Amount, coffees[coffeeNumber - 1].Water.Amount);
                    var money = RefundableMoney(coffees[coffeeNumber - 1].Price, totalMoney);
                    Console.WriteLine($"{coffeeNumber}: CoffeeAmount: {coffees[coffeeNumber - 1].Amount}, SugarAmount: {coffees[coffeeNumber - 1].Sugar.Amount}, WaterAmount: {coffees[coffeeNumber - 1].Water.Amount}");
                    exactNumber = false;
                    Console.WriteLine("Coffee is ready!");
                    Console.WriteLine($"Refundable money is {money}");
                }
                else
                {
                    try
                    {
                        throw new ArgumentException("There is no coffee with this number, please indicate the correct number.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        continue;
                    }
                }
            }

            return Tuple.Create(coffees[coffeeNumber - 1], coffeeMachineTotals);
        }

        private int RefundableMoney(int price, int totalMoney)
        {
            return totalMoney - price;
        }

        private Models.CoffeeMachine Recalculation(Models.CoffeeMachine coffeeMachine, int tempCoffeeAmount, int tempSugarAmount, int tempWaterAmount)
        {
            coffeeMachine.TotalCoffee -= tempCoffeeAmount;
            coffeeMachine.TotalSugar -= tempSugarAmount;
            coffeeMachine.TotalWater -= tempWaterAmount;

            return coffeeMachine;
        }
    }
}
