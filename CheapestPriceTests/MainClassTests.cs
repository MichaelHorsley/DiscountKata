using System.Collections.Generic;
using System.Linq;
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

        [TestCase(new[]{1,2}, "15.2")]
        public void GivenItemsReturnsExpectedPrice(int[] productList, decimal expectedPrice)
        {
            var discountService = new DiscountService();

            decimal calculatorPrices = discountService.CalculatorPrices(_discounts, productList.ToList());

            Assert.AreEqual(expectedPrice, calculatorPrices);
        }
    }
}