using System.Collections.Generic;
using System.Linq;

namespace CheapestPriceTests
{
    public class DiscountService
    {
        public static decimal CalculateCheapestPrice(List<Discount> discounts, List<int> list)
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

        public static decimal CalculateCheapestPriceWithRecursion(List<Discount> discounts, List<int> productList)
        {
            var costCombinations = new List<decimal>{ productList.Count * 8m };
            discounts = discounts.OrderByDescending(x => x.SetCount).ToList();

            while (discounts.Any())
            {
                var productListForCombination = new List<int>(productList);

                var discount = discounts.First();

                if (CheckValidForDiscount(productList, discount))
                {
                    RemoveValidProductsFromList(productListForCombination, discount);

                    var baseSum = 8.0m * discount.SetCount * (100.0m - discount.DiscountPercentage) / 100.0m;

                    costCombinations.AddRange(AddToBaseSumOrAddToPriceListDepthFirstTreeTraversal(baseSum, discounts, productListForCombination));
                }

                discounts.RemoveAt(0);
            }

            return costCombinations.Min();
        }

        private static List<decimal> AddToBaseSumOrAddToPriceListDepthFirstTreeTraversal(decimal baseSum, List<Discount> discounts, List<int> productListForCombination)
        {
            var combinationPrices = new List<decimal>();

            foreach (var discount in discounts)
            {
                var pathwayList = new List<int>(productListForCombination);

                if (CheckValidForDiscount(pathwayList, discount))
                {
                    RemoveValidProductsFromList(pathwayList, discount);
                    baseSum += 8.0m * discount.SetCount * (100.0m - discount.DiscountPercentage) / 100.0m;

                    combinationPrices.AddRange(AddToBaseSumOrAddToPriceListDepthFirstTreeTraversal(baseSum, discounts, pathwayList));
                }
                else
                {
                    combinationPrices.Add(baseSum + productListForCombination.Count * 8);
                }
            }

            return combinationPrices;
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

        private static bool CheckValidForDiscount(List<int> productList, Discount discount)
        {
            var productHashSet = new HashSet<int>();
            var itemsAddedToDiscount = new List<int>();
            var alterableProductList = new List<int>(productList);

            for (var index = alterableProductList.Count - 1; index >= 0; index--)
            {
                var product = alterableProductList[index];
                var added = productHashSet.Add(product);

                if (added)
                {
                    itemsAddedToDiscount.Add(product);

                    if (productHashSet.Count == discount.SetCount)
                    {
                        itemsAddedToDiscount.ForEach(x => alterableProductList.Remove(x));
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