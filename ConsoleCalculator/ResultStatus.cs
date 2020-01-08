using System;
namespace ConsoleCalculator
{
    public enum ResultStatus
    {
        OK = 1,
        MemoryOp = 2,
        Help = 3,
        InvalidInput = -1,
        DivisionByZero = 0,
        Exit = -2
    }
}
