using System;
using System.IO;
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

        [Test]
        public void FullConstructor_Test()
        {
            var calculator = new ConsoleCalculator(5, 1, 2, 3, CalculatorOperators.Substract);
            Assert.AreEqual(5, calculator.Memory);
            Assert.AreEqual(1, calculator.Result);
            Assert.AreEqual(2, calculator.Operand1);
            Assert.AreEqual(3, calculator.Operand2);
            Assert.AreEqual(CalculatorOperators.Substract, calculator.Operator);
        }
    }

    [TestFixture]
    public class Run_Tests
    {
        [Test, Timeout(200)]
        public void RunEnds_Tests()
        {
            var input = new StringReader("Exit");
            Console.SetIn(input);
            var calculator = new ConsoleCalculator();
            Assert.That(calculator.Run, Throws.Nothing);
        }
    }

    [TestFixture]
    public class PrintMessages_Tests
    {
        [Test]
        public void PrintErrorMessage_Test()
        {
            var output = new StringWriter();
            Console.SetOut(output);
            ConsoleCalculator.PrintErrorMessage(ConsoleMessages.DivisionByZero);
            Assert.That(output.ToString().Equals(ConsoleMessages.DivisionByZero + "\n"));
        }

        [Test]
        public void PrintInfoMessage_Test()
        {
            var output = new StringWriter();
            Console.SetOut(output);
            ConsoleCalculator.PrintInfoMessage(ConsoleMessages.DivisionByZero);
            Assert.That(output.ToString().Equals(ConsoleMessages.DivisionByZero + "\n"));
        }
    }

    [TestFixture]
    public class ParceString_Tests
    {
        [Test]
        public void NoElementCommand_Test()
        {
            var calculator = new ConsoleCalculator();
            var actualResult = calculator.ParseString("");
            Assert.AreEqual(ResultStatus.InvalidInput, actualResult);
        }

        [TestCase("Help", ResultStatus.OK)]
        [TestCase("Exit", ResultStatus.OK)]
        [TestCase("M+", ResultStatus.OK)]
        [TestCase("M-", ResultStatus.OK)]
        [TestCase("MC", ResultStatus.OK)]
        public void OneElementCorrectCommand_Test(string input, ResultStatus expectedResult)
        {
            var calculator = new ConsoleCalculator();
            var resultStatus = calculator.ParseString(input);
            var expOperation = CalculatorOperators.FromString(input);
            Assert.AreEqual(expectedResult, resultStatus);
            Assert.AreEqual(expOperation, calculator.Operator);
        }

        [TestCase("MC1", ResultStatus.InvalidInput)]
        public void OneElementIncorrectCommand_Test(string input, ResultStatus expectedResult)
        {
            var calculator = new ConsoleCalculator();
            var resultStatus = calculator.ParseString(input);
            var expOperation = CalculatorOperators.Add;
            Assert.AreEqual(expectedResult, resultStatus);
            Assert.AreEqual(expOperation, calculator.Operator);
        }

        [TestCase("* 5", 5, 10, ResultStatus.OK, "*", 5)]
        [TestCase("- MR", 5, 10, ResultStatus.OK, "-", 5)]
        [TestCase("/ 0", 5, 10, ResultStatus.OK, "/", 0)]
        [TestCase("- -5", 5, 10, ResultStatus.OK, "-", -5)]
        public void TwoElementCorrectCommand_Test(string input, double memoryField, double resultField, ResultStatus expectedResult, string expectedOperation, double expectedOp2)
        {
            var calculator = new ConsoleCalculator(memoryField, resultField, 0, 0, CalculatorOperators.Add);
            var resultStatus = calculator.ParseString(input);
            var expOperation = CalculatorOperators.FromString(expectedOperation);
            Assert.AreEqual(expectedResult, resultStatus);
            Assert.AreEqual(expOperation, calculator.Operator);
            Assert.AreEqual(resultField, calculator.Operand1);
            Assert.AreEqual(expectedOp2, calculator.Operand2);
        }

        [TestCase("-5", ResultStatus.InvalidInput)]
        [TestCase("5 -", ResultStatus.InvalidInput)]
        [TestCase("- -", ResultStatus.InvalidInput)]
        [TestCase("! 0", ResultStatus.InvalidInput)]
        [TestCase("- !", ResultStatus.InvalidInput)]
        [TestCase("- M+", ResultStatus.InvalidInput)]
        [TestCase("M- 5", ResultStatus.InvalidInput)]
        [TestCase("- help", ResultStatus.InvalidInput)]
        [TestCase("help exit", ResultStatus.InvalidInput)]
        public void TwoElementIncorrectCommand_Test(string input, ResultStatus expectedResult)
        {
            var calculator = new ConsoleCalculator();
            var resultStatus = calculator.ParseString(input);
            var expOperation = CalculatorOperators.Add;
            Assert.AreEqual(expectedResult, resultStatus);
            Assert.AreEqual(expOperation, calculator.Operator);
        }

        [TestCase("1 * 5", 5, 10, ResultStatus.OK, "*", 1, 5)]
        [TestCase("2 - MR", 5, 10, ResultStatus.OK, "-", 2, 5)]
        [TestCase("12 / 0", 5, 10, ResultStatus.OK, "/", 12, 0)]
        [TestCase("6 - -5", 5, 10, ResultStatus.OK, "-", 6, -5)]
        [TestCase("MR * MR", 5, 10, ResultStatus.OK, "*", 5, 5)]
        public void ThreeElementCorrectCommand_Test(string input, double memoryField, double resultField, ResultStatus expectedResult, string expectedOperation, double expectedOp1, double expectedOp2)
        {
            var calculator = new ConsoleCalculator(memoryField, resultField, 0, 0, CalculatorOperators.Add);
            var resultStatus = calculator.ParseString(input);
            var expOperation = CalculatorOperators.FromString(expectedOperation);
            Assert.AreEqual(expectedResult, resultStatus);
            Assert.AreEqual(expOperation, calculator.Operator);
            Assert.AreEqual(expOperation, calculator.Operator);
            Assert.AreEqual(expectedOp1, calculator.Operand1);
            Assert.AreEqual(expectedOp2, calculator.Operand2);
        }

        [TestCase("5-5", ResultStatus.InvalidInput)]
        [TestCase("5- 5", ResultStatus.InvalidInput)]
        [TestCase("5 -5", ResultStatus.InvalidInput)]
        [TestCase("5 - s", ResultStatus.InvalidInput)]
        [TestCase("5 s -", ResultStatus.InvalidInput)]
        [TestCase("! * 0", ResultStatus.InvalidInput)]
        [TestCase("! - !", ResultStatus.InvalidInput)]
        [TestCase("! ! !", ResultStatus.InvalidInput)]
        [TestCase("- 5 -", ResultStatus.InvalidInput)]
        [TestCase("- 5 5", ResultStatus.InvalidInput)]
        [TestCase("5 5 -", ResultStatus.InvalidInput)]
        [TestCase("5 5 5", ResultStatus.InvalidInput)]
        [TestCase("5 MR 5", ResultStatus.InvalidInput)]
        public void ThreeElementIncorrectCommand_Test(string input, ResultStatus expectedResult)
        {
            var calculator = new ConsoleCalculator();
            var resultStatus = calculator.ParseString(input);
            var expOperation = CalculatorOperators.Add;
            Assert.AreEqual(expectedResult, resultStatus);
            Assert.AreEqual(expOperation, calculator.Operator);
        }

        [TestCase("5 + 2 / 4", ResultStatus.OK)]
        [TestCase("+ 2 / 4", ResultStatus.InvalidInput)]
        public void MoreThanThreeElementCommand_Test(string str, ResultStatus expectedResult)
        {
            var calculator = new ConsoleCalculator();
            var resultStatus = calculator.ParseString(str);
            Assert.AreEqual(expectedResult, resultStatus);
        }
    }

    [TestFixture]
    public class SplitString_Tests
    {
        [Test]
        public void SplitStringEmptyString_Test()
        {
            var actualResult = ConsoleCalculator.SplitString("").Length;
            Assert.AreEqual(0, actualResult);
        }

        [Test]
        public void SplitStringLongString_Test()
        {
            var actualResult = ConsoleCalculator.SplitString("gggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggg")[0].Length;
            Assert.AreEqual(80, actualResult);
        }

        [Test]
        public void SplitStringTwoElements_Test()
        {
            var actualResult = ConsoleCalculator.SplitString("5 +");
            var expectedResult = new string[] { "5", "+" };
            Assert.AreEqual(2, actualResult.Length);
            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        public void SplitStringManyElements_Test()
        {
            var actualResult = ConsoleCalculator.SplitString("5 + 2 + 6 + 7 + 8 + 9");
            var expectedResult = new string[] { "5", "+", "2" };
            Assert.AreEqual(3, actualResult.Length);
            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        public void SplitStringManySpacers_Test()
        {
            var actualResult = ConsoleCalculator.SplitString("   5   +    5     ").Length;
            Assert.AreEqual(3, actualResult);
        }
    }
}