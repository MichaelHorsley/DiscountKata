using System.Collections.Generic;
using System.Linq;

namespace CheapestPriceTests
{
    public class DiscountViaRecursionService
    {
        public static decimal CalculateCheapestPrice(List<Discount> discounts, List<int> productList)
        {
            var costCombinations = new List<decimal> { productList.Count * 8m };
            discounts = discounts.OrderByDescending(x => x.SetCount).ToList();
            productList = ShuffleProductListSoDuplicatedItemsArePutToTheFrontOfTheList(productList);

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

        private static void RemoveValidProductsFromList(List<int> productListToBeAltered, Discount discount)
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
                        return;
                    }
                }
            }
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

        private static List<int> ShuffleProductListSoDuplicatedItemsArePutToTheFrontOfTheList(List<int> productList)
        {
            var groupedUniqueSets = new List<List<int>>();
            var listToBeAltered = new List<int>(productList.OrderBy(x => x));

            while (listToBeAltered.Any())
            {
                var uniqueProductSet = new HashSet<int>();

                foreach (var productItem in listToBeAltered)
                {
                    uniqueProductSet.Add(productItem);
                }

                groupedUniqueSets.Add(uniqueProductSet.ToList());

                foreach (var itemToBeRemoved in uniqueProductSet)
                {
                    listToBeAltered.Remove(itemToBeRemoved);
                }
            }

            groupedUniqueSets.Reverse();

            return groupedUniqueSets.SelectMany(x => x).ToList();
        }
    }
}