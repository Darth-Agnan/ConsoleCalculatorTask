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
            PrintInfoMessage(ConsoleMessages.Greeting);
            while (true)
            {
                var inputStr = Console.ReadLine();
                if (ParseString(inputStr).Equals(ResultStatus.OK))
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
                            PrintInfoMessage(ConsoleMessages.Help);
                            break;
                        case ResultStatus.InvalidInput:
                            PrintErrorMessage(ConsoleMessages.InvalidInput);
                            break;
                        case ResultStatus.DivisionByZero:
                            PrintErrorMessage(ConsoleMessages.DivisionByZero);
                            break;
                        case ResultStatus.Exit:
                            return;
                        default:
                            throw new NotSupportedException();
                    }
                }
                else
                {
                    PrintErrorMessage(ConsoleMessages.InvalidInput);
                }
            }
        }

        public static void PrintErrorMessage(ConsoleMessages message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        public static void PrintInfoMessage(ConsoleMessages message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        /// <summary>
        /// Парсит строчку в свойства операндов и оператора
        /// и возвращает ResultStatus.OK, если парсинг удался,
        /// или ResultStatus.InvalidInput, если парсинг неудачен.
        /// </summary>
        /// <param name="inputStr"></param>
        /// <returns></returns>
        public ResultStatus ParseString(string str)
        {
            var parsedStr = SplitString(str);
            var result = new bool[parsedStr.Length];
            var status = DoubleOrCommandDecider(result, parsedStr);
            if (status == ResultStatus.InvalidInput)
            {
                return ResultStatus.InvalidInput;
            }
            var elementsCount = parsedStr.Length;
            if (elementsCount == 1 && !result[0] && CalculatorOperators.IsOneElementCommand(parsedStr[0]))
            {
                Operator = StrToOperator(parsedStr[0]);
                return ResultStatus.OK;
            }
            for (int i = 0; i < elementsCount; i++)
            {
                if (CalculatorOperators.IsMR(parsedStr[i]))
                {
                    parsedStr[i] = Memory.ToString();
                    result[i] = true;
                }
            }
            if (elementsCount == 2 && !result[0] & result[1] && !CalculatorOperators.IsOneElementCommand(parsedStr[0]))
            {
                Operand1 = Result;
                Operand2 = StrToDouble(parsedStr[1]);
                Operator = StrToOperator(parsedStr[0]);
                return ResultStatus.OK;
            }
            if (elementsCount == 3 && result[0] && !result[1] && result[2])
                if (!CalculatorOperators.IsOneElementCommand(parsedStr[1]))
                {
                    Operand1 = StrToDouble(parsedStr[0]);
                    Operand2 = StrToDouble(parsedStr[2]);
                    Operator = StrToOperator(parsedStr[1]);
                    return ResultStatus.OK;
                }
            return ResultStatus.InvalidInput;
        }

        /// <summary>
        /// Метод предназначен для разделения входящей строки на подстроки.
        /// При этом обрезаем строку до 80 символов и 3х аргументов.
        /// </summary>
        /// <param name="inputStr"></param>
        /// <returns></returns>
        public static string[] SplitString(string str)
        {
            bool wasModified = false;
            if (str.Length > 80)
            {
                str = str.Substring(0, 80);
                wasModified = true;
            }
            var separator = new char[] { ' ' };
            var parsedStr = str.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            if (parsedStr.Length > 3)
            {
                var tempArray = new string[3];
                for (int i = 0; i < 3; i++)
                    tempArray[i] = parsedStr[i];
                parsedStr = tempArray;
                wasModified = true;
            }
            if (wasModified)
            {
                PrintErrorMessage(ConsoleMessages.ToBigInput);
                Console.WriteLine(string.Join(" ", parsedStr));
            }
            return parsedStr;
        }

        /// <summary>
        /// Метод-решатель, определяет, находится ли на i-ой позиции в массиве распарсеной строки
        /// вещественное число (тогда в массиве result записываем true), команда из соответствующего класса
        /// (тогда false) или какой-то мусор (возвращается InvalidInput)
        /// </summary>
        /// <param name="result"></param>
        /// <param name="parsedStr"></param>
        /// <returns></returns>
        public static ResultStatus DoubleOrCommandDecider(bool[] result, string[] parsedStr)
        {
            double parsedDouble;
            for (int i = 0; i < parsedStr.Length; i++)
            {
                if (double.TryParse(parsedStr[i], System.Globalization.NumberStyles.Any, CultureInfo.InvariantCulture, out parsedDouble))
                {
                    result[i] = true;
                }
                else
                {
                    if (!CalculatorOperators.Contains(parsedStr[i]))
                        return ResultStatus.InvalidInput;
                }
            }
            return ResultStatus.OK;
        }

        /// <summary>
        /// Обертка для превращения строки с вещественным числом в вещественное число
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static double StrToDouble(string str)
        {
            double parsedDouble;
            double.TryParse(str, System.Globalization.NumberStyles.Any, CultureInfo.InvariantCulture, out parsedDouble);
            return parsedDouble;
        }

        /// <summary>
        /// Обертка для превращения строки с оператором (командой) в оператор
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static CalculatorOperators StrToOperator(string str)
        {
            return CalculatorOperators.FromString(str);
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
            object instance;
            ResultStatus status;
            var tempMemory = Memory;
            var tempResult = Result;
            try
            {
                instance = currentAssembly.CreateInstance(instanceName);
                status = ((IOperation)instance).Run(ref tempMemory, ref tempResult, Operand1, Operand2);
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine(e);
                return ResultStatus.InvalidInput;
            }                     
            Memory = tempMemory;
            Result = tempResult;
            return status;
        }
    }
}
