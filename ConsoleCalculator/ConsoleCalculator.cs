using System;
using System.Globalization;
using System.Reflection;

namespace ConsoleCalculator
{
    public class ConsoleCalculator
    {
        public double Memory { get; private set; }
        public double Result { get; private set; }
        public double Operand1 { get; private set; }
        public double Operand2 { get; private set; }
        public CalculatorOperators Operator { get; private set; }

        /// <summary>
        /// Стандартный конструктор консольного калькулятора.
        /// Инициализирует нулями свойства "Результат" и регистр памяти.
        /// </summary>
        public ConsoleCalculator()
        {
            Memory = 0;
            Result = 0;
        }

        /// <summary>
        /// Перегрузка конструктора консольного калькулятора.
        /// Инициализирует нулями свойства "Результат" и регистр памяти.
        /// В поля операндов и оператора записывает переданные значения.
        /// </summary>
        /// <param name="Значение первого операнда"></param>
        /// <param name="Значение второго операнда"></param>
        /// <param name="Оператор (действие)"></param>
        public ConsoleCalculator(double operand1, double operand2, CalculatorOperators operator1)
        {
            Memory = 0;
            Result = 0;
            Operand1 = operand1;
            Operand2 = operand2;
            Operator = operator1;
        }

        /// <summary>
        /// Перегрузка конструктора консольного калькулятора.
        /// Инициализирует все имеющиеся свойства переданными значениями.
        /// Специально для тестов.
        /// </summary>
        /// <param name="Хранимое значение в памяти"></param>
        /// <param name="Хранимый результат"></param>
        /// <param name="Значение первого операнда"></param>
        /// <param name="Значение второго операнда"></param>
        /// <param name="Оператор (действие)"></param>
        public ConsoleCalculator(double memory, double result, double operand1, double operand2, CalculatorOperators operator1)
        {
            Memory = memory;
            Result = result;
            Operand1 = operand1;
            Operand2 = operand2;
            Operator = operator1;
        }

        /// <summary>
        /// Метод, запускающий цикл работы консольного калькулятора
        /// </summary>
        public void Run()
        {
            Console.WriteLine(ConsoleMessages.Greeting);
            while (true)
            {
                var inputStr = Console.ReadLine();
                if (ParceString(inputStr).Equals(ResultStatus.OK))
                {
                    var status = DoOperation();
                    switch (status)
                    {
                        case ResultStatus.OK:
                            Console.WriteLine("Result: " + Result);
                            break;
                        case ResultStatus.MemoryOp:
                            Console.WriteLine("Memory: " + Memory);
                            break;
                        case ResultStatus.Help:
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine(ConsoleMessages.Help);
                            Console.ResetColor();
                            break;
                        case ResultStatus.InvalidInput:
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine(ConsoleMessages.InvalidInput);
                            Console.ResetColor();
                            break;
                        case ResultStatus.DivisionByZero:
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine(ConsoleMessages.DivisionByZero);
                            Console.ResetColor();
                            break;
                        case ResultStatus.Exit:
                            return;
                        default:
                            throw new NotSupportedException();
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(ConsoleMessages.InvalidInput);
                    Console.ResetColor();
                }
            }
        }

        /// <summary>
        /// Парсит строчку в свойства операндов и оператора
        /// и возвращает ResultStatus.OK, если парсинг удался,
        /// или ResultStatus.InvalidInput, если парсинг неудачен.
        /// </summary>
        /// <param name="inputStr"></param>
        /// <returns></returns>
        public ResultStatus ParceString(string inputStr)
        {
            var separator = new char[] { ' ' };
            var splittedStr = inputStr.ToUpperInvariant().Split(separator, 3, StringSplitOptions.RemoveEmptyEntries);
            if (splittedStr.Length == 0)
            {
                return ResultStatus.InvalidInput;
            }
            if (splittedStr.Length == 1)
            {
                if (splittedStr[0].Length == 2)
                {
                    switch (splittedStr[0])
                    {
                        case "M+":
                            Operator = CalculatorOperators.MPlus;
                            return ResultStatus.OK;
                        case "M-":
                            Operator = CalculatorOperators.MMinus;
                            return ResultStatus.OK;
                        case "MR":
                            Operator = CalculatorOperators.MR;
                            return ResultStatus.OK;
                        case "MC":
                            Operator = CalculatorOperators.MC;
                            return ResultStatus.OK;
                        default:
                            return ResultStatus.InvalidInput;
                    }
                }
                if (splittedStr[0].Length == 4)
                {
                    switch (splittedStr[0])
                    {
                        case "HELP":
                            Operator = CalculatorOperators.Help;
                            return ResultStatus.OK;
                        case "EXIT":
                            Operator = CalculatorOperators.Exit;
                            return ResultStatus.OK;
                        default:
                            return ResultStatus.InvalidInput;
                    }
                }
                return ResultStatus.InvalidInput;
            }
            if (splittedStr.Length == 2)
            {
                Operand1 = Result;
                double operand2;
                if (splittedStr[1] == "MR")
                    operand2 = Memory;
                else if (!double.TryParse(splittedStr[1], System.Globalization.NumberStyles.Any, CultureInfo.InvariantCulture, out operand2))
                    return ResultStatus.InvalidInput;
                Operand2 = operand2;
                switch (splittedStr[0])
                {
                    case "+":
                        Operator = CalculatorOperators.Add;
                        return ResultStatus.OK;
                    case "-":
                        Operator = CalculatorOperators.Substract;
                        return ResultStatus.OK;
                    case "*":
                        Operator = CalculatorOperators.Multiply;
                        return ResultStatus.OK;
                    case "/":
                        Operator = CalculatorOperators.Divide;
                        return ResultStatus.OK;
                    case "^":
                        Operator = CalculatorOperators.POW;
                        return ResultStatus.OK;
                    default:
                        return ResultStatus.InvalidInput;
                }
            }
            if (splittedStr.Length == 3)
            {
                double operand1;
                double operand2;
                if (splittedStr[0] == "MR")
                    operand1 = Memory;
                else if (!double.TryParse(splittedStr[0], System.Globalization.NumberStyles.Any, CultureInfo.InvariantCulture, out operand1))
                    return ResultStatus.InvalidInput;
                if (splittedStr[2] == "MR")
                    operand2 = Memory;
                else if (!double.TryParse(splittedStr[2], System.Globalization.NumberStyles.Any, CultureInfo.InvariantCulture, out operand2))
                    return ResultStatus.InvalidInput;
                Operand1 = operand1;
                Operand2 = operand2;
                switch (splittedStr[1])
                {
                    case "+":
                        Operator = CalculatorOperators.Add;
                        return ResultStatus.OK;
                    case "-":
                        Operator = CalculatorOperators.Substract;
                        return ResultStatus.OK;
                    case "*":
                        Operator = CalculatorOperators.Multiply;
                        return ResultStatus.OK;
                    case "/":
                        Operator = CalculatorOperators.Divide;
                        return ResultStatus.OK;
                    case "^":
                        Operator = CalculatorOperators.POW;
                        return ResultStatus.OK;
                    default:
                        return ResultStatus.InvalidInput;
                }
            }
            return ResultStatus.OK; //TODO пока стоит заглушка
        }

        /// <summary>
        /// Выполняет действие, записанное в свойстве "Оператор"
        /// над значениями из свойств "Операнд1" и "Операнд2".
        /// </summary>
        /// <returns></returns>
        public ResultStatus DoOperation()
        {
            var instanceName = "ConsoleCalculator." + Operator;
            Assembly currentAssembly = Assembly.GetExecutingAssembly();
            var instance = currentAssembly.CreateInstance(instanceName);
            var tempMemory = Memory;
            var tempResult = Result;
            var status = ((IOperation)instance).Run(ref tempMemory, ref tempResult, Operand1, Operand2);
            Memory = tempMemory;
            Result = tempResult;
            return status;
        }
    }
}
