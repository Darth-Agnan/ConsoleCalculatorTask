using System;
namespace ConsoleCalculator
{
    public class Add : IOperation
    {
        ResultStatus IOperation.Run(ref double memory, ref double result, double operand1, double operand2)
        {
            result = operand1 + operand2;
            return ResultStatus.OK;
        }
    }

    public class Substract : IOperation
    {
        ResultStatus IOperation.Run(ref double memory, ref double result, double operand1, double operand2)
        {
            result = operand1 - operand2;
            return ResultStatus.OK;
        }
    }

    public class Multiply : IOperation
    {
        ResultStatus IOperation.Run(ref double memory, ref double result, double operand1, double operand2)
        {
            result = operand1 * operand2;
            return ResultStatus.OK;
        }
    }

    public class Divide : IOperation
    {
        ResultStatus IOperation.Run(ref double memory, ref double result, double operand1, double operand2)
        {
            if (Math.Abs(operand2) < 1E-10)
                return ResultStatus.DivisionByZero;
            result = operand1 / operand2;
            return ResultStatus.OK;
        }
    }

    public class POW : IOperation
    {
        ResultStatus IOperation.Run(ref double memory, ref double result, double operand1, double operand2)
        {
            result = Math.Pow(operand1, operand2);
            return ResultStatus.OK;
        }
    }

    public class MPlus : IOperation
    {
        ResultStatus IOperation.Run(ref double memory, ref double result, double operand1, double operand2)
        {
            memory += result;
            return ResultStatus.OK;
        }
    }

    public class MMinus : IOperation
    {
        ResultStatus IOperation.Run(ref double memory, ref double result, double operand1, double operand2)
        {
            memory -= result;
            return ResultStatus.OK;
        }
    }

    public class MR : IOperation
    {
        ResultStatus IOperation.Run(ref double memory, ref double result, double operand1, double operand2)
        {
            result = memory;
            return ResultStatus.OK;
        }
    }

    public class MC : IOperation
    {
        ResultStatus IOperation.Run(ref double memory, ref double result, double operand1, double operand2)
        {
            memory = 0;
            return ResultStatus.OK;
        }
    }

    public class Help : IOperation
    {
        ResultStatus IOperation.Run(ref double memory, ref double result, double operand1, double operand2)
        {
            Console.WriteLine(ConsoleMessages.Help);
            return ResultStatus.OK;
        }
    }

    public class Exit : IOperation
    {
        ResultStatus IOperation.Run(ref double memory, ref double result, double operand1, double operand2)
        {
            return ResultStatus.Exit;
        }
    }
}
