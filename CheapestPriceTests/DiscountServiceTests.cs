﻿using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace CheapestPriceTests
{
    [TestFixture]
    public class DiscountServiceTests
    {
        private readonly List<Discount> _discounts = new List<Discount>
        {
            new Discount { SetCount = 2, DiscountPercentage = 5 },
            new Discount { SetCount = 3, DiscountPercentage = 10 },
            new Discount { SetCount = 4, DiscountPercentage = 20 },
            new Discount { SetCount = 5, DiscountPercentage = 25 }
        };

        [Test]
        public void FinalGoal()
        {
            var discountService = new DiscountService();

            decimal calculatorPrices = discountService.CalculateCheapestPrice(_discounts, new List<int> { 1, 1, 2, 2, 3, 3, 4, 5 });

            Assert.AreEqual(new decimal(51.20), calculatorPrices);
        }

        [Test]
        public void GivenOneItemReturns8()
        {
            var discountService = new DiscountService();

            decimal calculatorPrices = discountService.CalculateCheapestPrice(_discounts, new List<int> { 1 });

            Assert.AreEqual(new decimal(8.0), calculatorPrices);
        }

        [Test]
        public void GivenOneSetOfItemsAndOneDuplicateReturnsOneSetPriceAndOneNormal()
        {
            var discountService = new DiscountService();

            decimal calculatorPrices = discountService.CalculateCheapestPrice(_discounts, new List<int> { 1, 2, 2 });

            Assert.AreEqual(new decimal(23.2), calculatorPrices);
        }

        [TestCase(new[]{1,2}, "15.2")]
        [TestCase(new[]{1, 2, 3, 4}, "25.6")]
        [TestCase(new[]{1, 2, 3, 4, 5}, "30")]
        public void GivenItemsReturnsExpectedPrice(int[] productList, decimal expectedPrice)
        {
            var discountService = new DiscountService();

            decimal calculatorPrices = discountService.CalculateCheapestPrice(_discounts, productList.ToList());

            Assert.AreEqual(expectedPrice, calculatorPrices);
        }

        [TestCase(new[] { 1, 2 }, "15.2")]
        [TestCase(new[] { 1, 2, 3, 4 }, "25.6")]
        [TestCase(new[] { 1, 2, 3, 4, 5 }, "30")]
        [TestCase(new[] { 1, 1, 1, 1, 1 }, "40")]
        [TestCase(new[] { 1, 1, 2, 2, 3, 3, 4, 5 }, "51.20")]
        public void GivenItemsReturnsExpectedPriceRecursively(int[] productList, decimal expectedPrice)
        {
            var discountService = new DiscountService();

            decimal calculatorPrices = discountService.CalculateCheapestPriceWithRecursion(_discounts, productList.ToList());

            Assert.AreEqual(expectedPrice, calculatorPrices);
        }

        [TestCase(new[] { 1, 2, 3, 3, 4, 4, 5, 5 }, "51.20")]
        public void MakeSureDiscountsCanBeAppliedWhenDuplicatesAreInTheHigherNumbers(int[] productList, decimal expectedPrice)
        {
            var discountService = new DiscountService();

            decimal calculatorPrices = discountService.CalculateCheapestPriceWithRecursion(_discounts, productList.ToList());

            Assert.AreEqual(expectedPrice, calculatorPrices);
        }
    }
}