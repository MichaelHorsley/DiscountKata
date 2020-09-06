using System.Collections.Generic;
using System.Linq;

namespace CheapestPriceTests
{
    public class DiscountService
    {
        public decimal CalculatorPrices(List<Discount> discounts, List<int> list)
        {
            var sumTotal = 0m;

            foreach (var discount in discounts.OrderByDescending(x => x.SetCount))
            {
                if (list.Count == discount.SetCount)
                {
                    sumTotal += 8.0m * list.Count * (100.0m - discount.DiscountPercentage) / 100.0m;
                    
                    for (int i = 0; i < discount.SetCount; i++)
                    {
                        list.RemoveAt(0);
                    }
                }
            }

            sumTotal += list.Count * 8.0m;

            return sumTotal;
        }
    }

    public class Discount
    {
        public int SetCount { get; set; }
        public int DiscountPercentage { get; set; }
    }
}