using System;
namespace ConsoleCalculator
{
    public interface IOperation
    {
        ResultStatus Run(ref double memory, ref double result, double operand1, double operand2);
    }
}
