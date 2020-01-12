using NUnit.Framework;

namespace ConsoleCalculator
{
    [TestFixture]
    public class Operations_Tests
    {
        [TestCase(0, 1, 2, 3, 5)]
        [TestCase(0, 1, 2, -3, -1)]
        [TestCase(0, 1, 0, -3, -3)]
        public void Add_Test(double memory, double result, double operand1, double operand2, double expectedResult)
        {
            var operator1 = new Add();
            var actualResult = ((IOperation)operator1).Run(ref memory, ref result, operand1, operand2);
            Assert.AreEqual(ResultStatus.OK, actualResult);
            Assert.AreEqual(expectedResult, result);
        }

        [TestCase(0, 1, 2, 3, -1)]
        [TestCase(0, 1, 2, -3, 5)]
        [TestCase(0, 1, 0, -3, 3)]
        public void Substract_Test(double memory, double result, double operand1, double operand2, double expectedResult)
        {
            var operator1 = new Substract();
            var actualResult = ((IOperation)operator1).Run(ref memory, ref result, operand1, operand2);
            Assert.AreEqual(ResultStatus.OK, actualResult);
            Assert.AreEqual(expectedResult, result);
        }

        [TestCase(0, 1, 2, 3, 6)]
        [TestCase(0, 1, 2, -3, -6)]
        [TestCase(0, 1, 0, -3, 0)]
        [TestCase(0, 1, -3, 0, 0)]
        public void Multiply_Test(double memory, double result, double operand1, double operand2, double expectedResult)
        {
            var operator1 = new Multiply();
            var actualResult = ((IOperation)operator1).Run(ref memory, ref result, operand1, operand2);
            Assert.AreEqual(ResultStatus.OK, actualResult);
            Assert.AreEqual(expectedResult, result);
        }

        [TestCase(0, 1, 3, 2, 1.5)]
        [TestCase(0, 1, 3, -2, -1.5)]
        [TestCase(0, 1, 0, -3, 0)]
        public void Divide_Test(double memory, double result, double operand1, double operand2, double expectedResult)
        {
            var operator1 = new Divide();
            var actualResult = ((IOperation)operator1).Run(ref memory, ref result, operand1, operand2);
            Assert.AreEqual(ResultStatus.OK, actualResult);
            Assert.AreEqual(expectedResult, result);
        }

        [TestCase(0, 1, 3, 0)]
        [TestCase(0, 1, 0, 0)]
        public void DivideByZero_Test(double memory, double result, double operand1, double operand2)
        {
            var operator1 = new Divide();
            var actualResult = ((IOperation)operator1).Run(ref memory, ref result, operand1, operand2);
            Assert.AreEqual(ResultStatus.DivisionByZero, actualResult);
        }

        [TestCase(0, 1, 2, 3, 8)]
        [TestCase(0, 1, 4, 0.5, 2)]
        [TestCase(0, 1, 2, 0, 1)]
        [TestCase(0, 1, 0, 0, 1)]
        [TestCase(0, 1, 2, -2, 0.25)]
        [TestCase(0, 1, -10, -3, -0.001)]
        public void POW_Test(double memory, double result, double operand1, double operand2, double expectedResult)
        {
            var operator1 = new POW();
            var actualResult = ((IOperation)operator1).Run(ref memory, ref result, operand1, operand2);
            Assert.AreEqual(ResultStatus.OK, actualResult);
            Assert.AreEqual(expectedResult, result);
        }

        [TestCase(1, 2, 3, 4, 3)]
        [TestCase(1, 0, 3, 4, 1)]
        [TestCase(1, -2, 3, 4, -1)]
        public void MPlus_Test(double memory, double result, double operand1, double operand2, double expectedResult)
        {
            var operator1 = new MPlus();
            var actualResult = ((IOperation)operator1).Run(ref memory, ref result, operand1, operand2);
            Assert.AreEqual(ResultStatus.MemoryOp, actualResult);
            Assert.AreEqual(expectedResult, memory);
        }

        [TestCase(1, 2, 3, 4, -1)]
        [TestCase(1, 0, 3, 4, 1)]
        [TestCase(1, -2, 3, 4, 3)]
        public void MMinus_Test(double memory, double result, double operand1, double operand2, double expectedResult)
        {
            var operator1 = new MMinus();
            var actualResult = ((IOperation)operator1).Run(ref memory, ref result, operand1, operand2);
            Assert.AreEqual(ResultStatus.MemoryOp, actualResult);
            Assert.AreEqual(expectedResult, memory);
        }

        [TestCase(1, 2, 3, 4, 1)]
        [TestCase(1, 0, 3, 4, 1)]
        [TestCase(1, -2, 3, 4, 1)]
        public void MR_Test(double memory, double result, double operand1, double operand2, double expectedResult)
        {
            var operator1 = new MR();
            var actualResult = ((IOperation)operator1).Run(ref memory, ref result, operand1, operand2);
            Assert.AreEqual(ResultStatus.OK, actualResult);
            Assert.AreEqual(expectedResult, result);
        }

        [TestCase(1, 2, 3, 4, 0)]
        [TestCase(0, 0, 3, 4, 0)]
        [TestCase(-5, -2, 3, 4, 0)]
        public void MC_Test(double memory, double result, double operand1, double operand2, double expectedResult)
        {
            var operator1 = new MC();
            var actualResult = ((IOperation)operator1).Run(ref memory, ref result, operand1, operand2);
            Assert.AreEqual(ResultStatus.MemoryOp, actualResult);
            Assert.AreEqual(expectedResult, memory);
        }

        [Test]
        public void Help_Test()
        {
            double memory = 0;
            double result = 0;
            double operand1 = 0;
            double operand2 = 0;
            var operator1 = new Help();
            var actualResult = ((IOperation)operator1).Run(ref memory, ref result, operand1, operand2);
            Assert.AreEqual(ResultStatus.Help, actualResult);
        }

        [Test]
        public void Exit_Test()
        {
            double memory = 0;
            double result = 0;
            double operand1 = 0;
            double operand2 = 0;
            var operator1 = new Exit();
            var actualResult = ((IOperation)operator1).Run(ref memory, ref result, operand1, operand2);
            Assert.AreEqual(ResultStatus.Exit, actualResult);
        }
    }
}
