using System.Collections.Generic;
using System.Linq;

namespace CheapestPriceTests
{
    public class DiscountService
    {
        public decimal CalculateCheapestPrice(List<Discount> discounts, List<int> list)
        {
            var discountCombinationPrices = new List<decimal>();

            var orderByDescending = discounts.OrderByDescending(x => x.SetCount).ToList();

            while (orderByDescending.Any())
            {
                discountCombinationPrices.Add(GetDiscountCombination(orderByDescending, list));

                orderByDescending.Remove(orderByDescending.First());
            }

            return discountCombinationPrices.Where(x => !x.Equals(0m)).Min();
        }

        private static decimal GetDiscountCombination(List<Discount> discounts, List<int> list)
        {
            var discountsOrderedByDescendingRequiredNumber = discounts.OrderByDescending(x => x.SetCount).ToList();
            
            var sumTotal = 0m;

            var productListToBeAltered = new List<int>(list);

            if (!RemoveValidProductsFromList(new List<int>(productListToBeAltered), discountsOrderedByDescendingRequiredNumber.First()))
            {
                return list.Count * 8m;
            }
            
            while (discountsOrderedByDescendingRequiredNumber.Any())
            {
                var discount = discountsOrderedByDescendingRequiredNumber.First();
                var validForDiscount = RemoveValidProductsFromList(productListToBeAltered, discount);

                if (validForDiscount)
                {
                    sumTotal += 8.0m * discount.SetCount * (100.0m - discount.DiscountPercentage) / 100.0m;
                }
                else
                {
                    discountsOrderedByDescendingRequiredNumber.Remove(discount);
                }
            }

            sumTotal += productListToBeAltered.Count * 8.0m;

            return sumTotal;
        }

        private static bool RemoveValidProductsFromList(List<int> productListToBeAltered, Discount discount)
        {
            var productHashSet = new HashSet<int>();
            var itemsAddedToDiscount = new List<int>();

            foreach (var product in productListToBeAltered)
            {
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