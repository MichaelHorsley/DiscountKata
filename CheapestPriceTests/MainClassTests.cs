using System.Collections.Generic;
using NUnit.Framework;

namespace CheapestPriceTests
{
    [TestFixture]
    public class MainClassTests
    {
        [Test]
        public void FinalGoal()
        {
            var discountService = new DiscountService();

            decimal calculatorPrices = discountService.CalculatorPrices(new List<int> { 1, 1, 2, 2, 3, 3, 4, 5 });

            Assert.AreEqual(new decimal(51.20), calculatorPrices);
        }
    }
}