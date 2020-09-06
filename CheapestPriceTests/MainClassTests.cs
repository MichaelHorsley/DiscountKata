﻿using System.Collections.Generic;
using NUnit.Framework;

namespace CheapestPriceTests
{
    [TestFixture]
    public class MainClassTests
    {
        private readonly List<Discount> _discounts = new List<Discount>
        {
            new Discount
            {
                SetCount = 2, DiscountPercentage = 5
            }
        };

        [Test]
        public void FinalGoal()
        {
            var discountService = new DiscountService();

            decimal calculatorPrices = discountService.CalculatorPrices(_discounts, new List<int> { 1, 1, 2, 2, 3, 3, 4, 5 });

            Assert.AreEqual(new decimal(51.20), calculatorPrices);
        }

        [Test]
        public void GivenOneItemReturns8()
        {
            var discountService = new DiscountService();

            decimal calculatorPrices = discountService.CalculatorPrices(_discounts, new List<int> { 1 });

            Assert.AreEqual(new decimal(8.0), calculatorPrices);
        }

        [Test]
        public void GivenOneAndTwoItemReturns15point2()
        {
            var discountService = new DiscountService();

            decimal calculatorPrices = discountService.CalculatorPrices(_discounts, new List<int> { 1, 2 });

            Assert.AreEqual(new decimal(15.2), calculatorPrices);
        }
    }
}