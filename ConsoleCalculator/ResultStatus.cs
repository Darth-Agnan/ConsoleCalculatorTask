using System;
namespace ConsoleCalculator
{
    public enum ResultStatus
    {
        OK = 1,
        InvalidInput = -1,
        DivisionByZero = 0,
        Exit = -2
    }
}
