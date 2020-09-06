using System.Collections.Generic;

namespace CheapestPriceTests
{
    public class DiscountService
    {
        public decimal CalculatorPrices(List<int> list)
        {
            return list.Count * 8.0m;
        }
    }
}