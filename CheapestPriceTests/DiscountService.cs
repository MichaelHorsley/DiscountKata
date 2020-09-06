using System.Collections.Generic;
using System.Linq;

namespace CheapestPriceTests
{
    public class DiscountService
    {
        public decimal CalculatorPrices(List<Discount> discounts, List<int> list)
        {
            var sumTotal = 0m;

            var productListToBeAltered = new List<int>(list);

            foreach (var discount in discounts.OrderByDescending(x => x.SetCount))
            {
                var validForDiscount = ValidForDiscount(productListToBeAltered, discount);

                if (validForDiscount)
                {
                    sumTotal += 8.0m * discount.SetCount * (100.0m - discount.DiscountPercentage) / 100.0m;
                }
            }

            sumTotal += productListToBeAltered.Count * 8.0m;

            return sumTotal;
        }

        private static bool ValidForDiscount(List<int> productListToBeAltered, Discount discount)
        {
            var productHashSet = new HashSet<int>();
            var itemsAddedToDiscount = new List<int>();

            for (var index = productListToBeAltered.Count - 1; index >= 0; index--)
            {
                var product = productListToBeAltered[index];
                var added = productHashSet.Add(product);

                if (added)
                {
                    itemsAddedToDiscount.Add(product);

                    if (productHashSet.Count == discount.SetCount)
                    {
                        itemsAddedToDiscount.ForEach(x => productListToBeAltered.Remove(x));
                        return true;
                    }
                }
            }

            return false;
        }
    }

    public class Discount
    {
        public int SetCount { get; set; }
        public int DiscountPercentage { get; set; }
    }
}