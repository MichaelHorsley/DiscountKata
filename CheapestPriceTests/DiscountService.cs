using System.Collections.Generic;
using System.Linq;

namespace CheapestPriceTests
{
    public class DiscountService
    {
        public static decimal CalculateCheapestPrice(List<Discount> discounts, List<int> productList)
        {
            var discountCombinationPrices = new List<decimal>();
            productList = ShuffleProductListSoDuplicatedItemsArePutToTheFrontOfTheList(productList);

            var orderByDescending = OrganiseDiscountsByLargestSetRequirementToSmallest(discounts);

            while (orderByDescending.Any())
            {
                discountCombinationPrices.Add(GetDiscountCombination(orderByDescending, productList));

                orderByDescending.Remove(orderByDescending.First());
            }

            return discountCombinationPrices.Where(x => !x.Equals(0m)).Min();
        }

        private static List<Discount> OrganiseDiscountsByLargestSetRequirementToSmallest(List<Discount> discounts)
        {
            return discounts.OrderByDescending(x => x.SetCount).ToList();
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