using NUnit.Framework;

namespace ConsoleCalculator
{
    [TestFixture]
    public class Constructor_Tests
    {
        [Test]
        public void EmptyConstructor_Test()
        {
            var calculator = new ConsoleCalculator();
            Assert.AreEqual(0, calculator.Memory);
            Assert.AreEqual(0, calculator.Result);
            Assert.AreEqual(0, calculator.Operand1);
            Assert.AreEqual(0, calculator.Operand2);
            Assert.AreEqual(CalculatorOperators.Add, calculator.Operator);
        }

        [Test]
        public void NonEmptyConstructor_Test()
        {
            var calculator = new ConsoleCalculator(2, 3, CalculatorOperators.Substract);
            Assert.AreEqual(0, calculator.Memory);
            Assert.AreEqual(0, calculator.Result);
            Assert.AreEqual(2, calculator.Operand1);
            Assert.AreEqual(3, calculator.Operand2);
            Assert.AreEqual(CalculatorOperators.Substract, calculator.Operator);
        }
    }

    [TestFixture]
    public class Run_Tests
    {
        //TODO подумать о целесообразности
    }

    [TestFixture]
    public class ParceString_Tests
    {
        //TODO после разработки парсера
    }

    [TestFixture]
    public class DoOperation_Tests
    {
        [TestCase(1, 2, CalculatorOperators.Add, 3)]
        [TestCase(1, 2, CalculatorOperators.Substract, -1)]
        [TestCase(4, 5, CalculatorOperators.Multiply, 20)]
        [TestCase(4, 5, CalculatorOperators.Divide, 0.8)]
        [TestCase(2, 10, CalculatorOperators.POW, 1024)]
        [TestCase(1024, 0.5, CalculatorOperators.POW, 32)]
        public void DoOperation_SimpleOperations_Test(double operand1, double operand2, CalculatorOperators operator1, double expected)
        {
            var calculator = new ConsoleCalculator(operand1, operand2, operator1);
            var actualResultStatus = calculator.DoOperation();
            Assert.AreEqual(expected, calculator.Result);
            Assert.AreEqual(ResultStatus.OK, actualResultStatus);
        }

        [TestCase(1, 2, CalculatorOperators.MPlus, 1, 3)]
        [TestCase(1, 2, CalculatorOperators.MMinus, 1, 1)]
        [TestCase(4, 5, CalculatorOperators.MR, 5, 5)]
        [TestCase(4, 5, CalculatorOperators.MC, 4, 0)]
        public void DoOperation_MemoryOperations_Test(double initialResult, double initialMemory, CalculatorOperators operator1, double expectedResult, double expectedMemory)
        {
            var calculator = new ConsoleCalculator(initialMemory, initialResult, 0, 0, operator1);
            var actualResultStatus = calculator.DoOperation();
            Assert.AreEqual(expectedResult, calculator.Result);
            Assert.AreEqual(expectedMemory, calculator.Memory);
            Assert.AreEqual(ResultStatus.OK, actualResultStatus);
        }

        [Test]
        public void DoOperation_Help_Test()
        {
            var calculator = new ConsoleCalculator(2, 3, CalculatorOperators.Help);
            var actualResultStatus = calculator.DoOperation();
            Assert.AreEqual(ResultStatus.OK, actualResultStatus);
        }

        [Test]
        public void DoOperation_Exit_Test()
        {
            var calculator = new ConsoleCalculator(2, 3, CalculatorOperators.Exit);
            var actualResultStatus = calculator.DoOperation();
            Assert.AreEqual(ResultStatus.Exit, actualResultStatus);
        }

        [Test]
        public void DoOperation_DivByZero_Test()
        {
            var calculator = new ConsoleCalculator(2, 0, CalculatorOperators.Divide);
            var actualResultStatus = calculator.DoOperation();
            Assert.AreEqual(ResultStatus.DivisionByZero, actualResultStatus);
        }
    }

    [TestFixture]
    public class DoOperation1_Tests
    {
        [Test]
        public void DoOperation_Test1()
        {
            var calculator = new ConsoleCalculator(2, 3, CalculatorOperators.Add);
            calculator.DoOperation();
            Assert.AreEqual(5, calculator.Result);
        }

        [Test]
        public void DoOperation_Test2()
        {
            var calculator = new ConsoleCalculator(2, 3, CalculatorOperators.Substract);
            calculator.DoOperation();
            Assert.AreEqual(-1, calculator.Result);
        }

        [Test]
        public void DoOperation_Test3()
        {
            var calculator = new ConsoleCalculator(2, 3, CalculatorOperators.Multiply);
            calculator.DoOperation();
            Assert.AreEqual(6, calculator.Result);
        }
    }
}
