using CoffeeMachine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeMachine.Repositories
{
    public interface IRepository
    {
        List<Coin> SelectCoins();

        List<Coffee> SelectCoffees();

        Models.CoffeeMachine SelectCoffeeMachineTotals();

        void RecalculationTotals(Models.CoffeeMachine coffeeMachine, int id);
    }

    public interface ICoffeeMachineService
    {
        Coin[] GetCoinTypes();

        Coffee[] GetCoffeeTypes();
    }
}
