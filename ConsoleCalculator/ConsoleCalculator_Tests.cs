using System;
using System.IO;
using System.Text;
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
        //TODO тесты на все ветви свича

        [Test, Timeout(200)]
        public void RunAllSwitchBranches_Tests()
        {
            var inputStrings = new StringBuilder();
            inputStrings.AppendLine("0 + 0");
            inputStrings.AppendLine("MC");
            inputStrings.AppendLine("help");
            inputStrings.AppendLine("asdasdas");
            inputStrings.AppendLine("0 / 0");
            inputStrings.AppendLine("exit");
            var outputStrings = new StringBuilder();
            outputStrings.AppendLine(ConsoleMessages.Greeting.Description);
            outputStrings.AppendLine("Результат: 0");
            outputStrings.AppendLine("Память: 0");
            outputStrings.AppendLine(ConsoleMessages.Help.Description);
            outputStrings.AppendLine(ConsoleMessages.InvalidInput.Description);
            outputStrings.AppendLine(ConsoleMessages.DivisionByZero.Description);
            var input = new StringReader(inputStrings.ToString());
            var output = new StringWriter();
            Console.SetIn(input);
            Console.SetOut(output);
            var calculator = new ConsoleCalculator();
            Assert.That(calculator.Run, Throws.Nothing);
            Assert.AreEqual(outputStrings.ToString(), output.ToString());
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
            var winStyleEquals = output.ToString().Equals(ConsoleMessages.DivisionByZero + "\r\n");
            var linStyleEquals = output.ToString().Equals(ConsoleMessages.DivisionByZero + "\n");
            Assert.That(winStyleEquals || linStyleEquals);
        }

        [Test]
        public void PrintInfoMessage_Test()
        {
            var output = new StringWriter();
            Console.SetOut(output);
            ConsoleCalculator.PrintInfoMessage(ConsoleMessages.DivisionByZero);
            var winStyleEquals = output.ToString().Equals(ConsoleMessages.DivisionByZero + "\r\n");
            var linStyleEquals = output.ToString().Equals(ConsoleMessages.DivisionByZero + "\n");
            Assert.That(winStyleEquals || linStyleEquals);
        }
    }

    [TestFixture]
    public class ParseString_Tests
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
        [TestCase("MR", ResultStatus.OK)]
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

        [Test]
        public void SplitStringErrorMessage_Test()
        {
            var inputStrings = new StringBuilder();
            inputStrings.AppendLine("0 + 0 + 2");
            inputStrings.AppendLine("exit");
            var outputStrings = new StringBuilder();
            outputStrings.AppendLine(ConsoleMessages.Greeting.Description);
            outputStrings.AppendLine(ConsoleMessages.ToBigInput.Description);
            outputStrings.AppendLine("0 + 0");
            outputStrings.AppendLine("Результат: 0");
            var input = new StringReader(inputStrings.ToString());
            var output = new StringWriter();
            Console.SetIn(input);
            Console.SetOut(output);
            var calculator = new ConsoleCalculator();
            Assert.That(calculator.Run, Throws.Nothing);
            Assert.AreEqual(outputStrings.ToString(), output.ToString());
        }
    }

    [TestFixture]
    public class DoubleOrCommandDecider_Tests
    {
        [Test]
        public void DoubleOrCommandEmptyArrs_Test()
        {
            var result = new bool[0];
            var parcedStr = new string[0];
            var actualResult = ConsoleCalculator.DoubleOrCommandDecider(result, parcedStr);
            Assert.AreEqual(ResultStatus.OK, actualResult);
        }

        [Test]
        public void DoubleOrCommandAsDouble_Test()
        {
            var result = new bool[] { false };
            var parcedStr = new string[] { "0.18535485" };
            var actualResult = ConsoleCalculator.DoubleOrCommandDecider(result, parcedStr);
            Assert.AreEqual(ResultStatus.OK, actualResult);
            Assert.AreEqual(new bool[] { true }, result);
        }

        [Test]
        public void DoubleOrCommandAsCommand_Test()
        {
            var result = new bool[] { false };
            var parcedStr = new string[] { "exit" };
            var actualResult = ConsoleCalculator.DoubleOrCommandDecider(result, parcedStr);
            Assert.AreEqual(ResultStatus.OK, actualResult);
            Assert.AreEqual(new bool[] { false }, result);
        }

        [Test]
        public void DoubleOrCommandMixedResult_Test()
        {
            var result = new bool[] { false, false, false, false };
            var parcedStr = new string[] { "exit", "help", "0", "68465.41354138" };
            var actualResult = ConsoleCalculator.DoubleOrCommandDecider(result, parcedStr);
            Assert.AreEqual(ResultStatus.OK, actualResult);
            Assert.AreEqual(new bool[] { false, false, true, true }, result);
        }
    }

    [TestFixture]
    public class StrToDouble_Tests
    {
        [TestCase("0", 0)]
        [TestCase("1.1865", 1.1865)]
        [TestCase("1,1865", 11865)]
        [TestCase("-0", 0)]
        [TestCase("-0.8453", -0.8453)]
        public void StrToDouble_Test(string str, double expectedResult)
        {
            var actualResult = ConsoleCalculator.StrToDouble(str);
            Assert.AreEqual(expectedResult, actualResult);
        }
    }

    [TestFixture]
    public class StrToOperator_Tests
    {
        [TestCase("+", "Add")]
        [TestCase("-", "Substract")]
        [TestCase("*", "Multiply")]
        [TestCase("/", "Divide")]
        [TestCase("^", "POW")]
        [TestCase("M+", "MPlus")]
        [TestCase("M-", "MMinus")]
        [TestCase("MR", "MR")]
        [TestCase("MC", "MC")]
        [TestCase("Help", "Help")]
        [TestCase("Exit", "Exit")]
        [TestCase("mR", "MR")]
        public void StrToDouble_Test(string str, string operatorName)
        {
            var actualResult = ConsoleCalculator.StrToOperator(str).Name;
            Assert.AreEqual(operatorName, actualResult);
        }
    }

    [TestFixture]
    public class DoOperation_Tests
    {
        [Test]
        public void DoOperationExisting_Test()
        {
            var operand1 = 3;
            var operand2 = 4;
            var calculator = new ConsoleCalculator(1, 2, operand1, operand2, CalculatorOperators.Add);
            var actualResult = calculator.DoOperation();
            Assert.AreEqual(operand1 + operand2, calculator.Result);
            Assert.AreEqual(ResultStatus.OK, actualResult);
        }
    }
}