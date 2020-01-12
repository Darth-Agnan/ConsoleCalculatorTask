using NUnit.Framework;

namespace ConsoleCalculator
{
    [TestFixture]
    public class CalculatorOperators_Tests
    {
        [Test]
        public void CalculatorOperatorsList_Test()
        {
            int id = 0;
            foreach (var i in CalculatorOperators.List())
            {
                Assert.AreEqual(id, i.Id);
                id++;
            }
        }

        [Test]
        public void CalculatorOperatorsFromString()
        {
            foreach (var i in CalculatorOperators.List())
            {
                Assert.AreEqual(i, CalculatorOperators.FromString(i.Symbols));
            }
        }

        [TestCase("+", true)]
        [TestCase("-", true)]
        [TestCase("*", true)]
        [TestCase("/", true)]
        [TestCase("^", true)]
        [TestCase("M+", true)]
        [TestCase("M-", true)]
        [TestCase("MR", true)]
        [TestCase("MC", true)]
        [TestCase("Help", true)]
        [TestCase("Exit", true)]
        [TestCase("++", false)]
        [TestCase("testStr", false)]
        [TestCase("0", false)]
        public void CalculatorOperatorsContains_Test(string symbols, bool contains)
        {
            Assert.AreEqual(contains, CalculatorOperators.Contains(symbols));
        }

        [TestCase("+", false)]
        [TestCase("-", false)]
        [TestCase("*", false)]
        [TestCase("/", false)]
        [TestCase("^", false)]
        [TestCase("M+", false)]
        [TestCase("M-", false)]
        [TestCase("MR", true)]
        [TestCase("MC", false)]
        [TestCase("Help", false)]
        [TestCase("Exit", false)]
        public void CalculatorOperatorsIsMR_Test(string symbols, bool isMR)
        {
            Assert.AreEqual(isMR, CalculatorOperators.IsMR(symbols));
        }

        [TestCase("+", false)]
        [TestCase("-", false)]
        [TestCase("*", false)]
        [TestCase("/", false)]
        [TestCase("^", false)]
        [TestCase("M+", true)]
        [TestCase("M-", true)]
        [TestCase("MR", true)]
        [TestCase("MC", true)]
        [TestCase("Help", true)]
        [TestCase("Exit", true)]
        public void CalculatorOperatorsIsOneElementCommand_Test(string symbols, bool isMR)
        {
            Assert.AreEqual(isMR, CalculatorOperators.IsOneElementCommand(symbols));
        }

        [Test]
        public void CalculatorOperatorsGetHashCode_Test()
        {
            var id = 0;
            foreach (var i in CalculatorOperators.List())
            {
                var a = id.GetHashCode();
                var b = i.GetHashCode();
                Assert.AreEqual(id.GetHashCode(), i.GetHashCode());
                id++;
            }
        }
    }

    [TestFixture]
    public class CalculatorOperatorsWithModification_Tests
    {

        [Test]
        public void CalculatorOperatorsConstructor_Test()
        {
            var id = 20;
            var name = "Test Operator";
            var str = "test";
            var oneElement = true;
            var testOperator = new CalculatorOperators(id, name, str, oneElement);
            Assert.AreEqual(id, testOperator.Id);
            Assert.AreEqual(name, testOperator.Name);
            Assert.AreEqual(str, testOperator.Symbols);
            Assert.AreEqual(oneElement, testOperator.OneElementCommand);
        }

        [Test]
        public void CalculatorOperatorsToString_Test()
        {
            var id = 20;
            var name = "Test Operator";
            var str = "test";
            var oneElement = true;
            var testOperator = new CalculatorOperators(id, name, str, oneElement);
            Assert.AreEqual(name, testOperator.ToString());
        }

        [Test]
        public void CalculatorOperatorsEquals_Test()
        {
            var id = 20;
            var name = "Test Operator";
            var str = "test";
            var oneElement = true;
            var testOperator = new CalculatorOperators(id, name, str, oneElement);
            Assert.AreEqual(false, testOperator.Equals(CalculatorOperators.Help));
            Assert.AreEqual(true, testOperator.Equals(testOperator));
        }
    }
}
