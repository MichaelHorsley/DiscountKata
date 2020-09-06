using System.Collections.Generic;

namespace CheapestPriceTests
{
    public class DiscountService
    {
        public decimal CalculatorPrices(List<Discount> discounts,List<int> list)
        {
            return list.Count * 8.0m;
        }
    }

    public class Discount
    {
        public int SetCount { get; set; }
        public int DiscountPercentage { get; set; }
    }
}