using System;
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
            Console.WriteLine(ConvertMessageToString(ConsoleMessages.Greeting));
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
                        case ResultStatus.InvalidInput:
                            Console.WriteLine(ConvertMessageToString(ConsoleMessages.InvalidInput));
                            break;
                        case ResultStatus.DivisionByZero:
                            Console.WriteLine(ConvertMessageToString(ConsoleMessages.DivisionByZero));
                            break;
                        case ResultStatus.Exit:
                            return;
                        default:
                            throw new NotSupportedException();
                    }
                }
                else
                {
                    Console.WriteLine(ConvertMessageToString(ConsoleMessages.InvalidInput));
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
            return ResultStatus.OK; //TODO пока стоит заглушка
        }

        /// <summary>
        /// Выполняет действие, записанное в свойстве "Оператор"
        /// над значениями из свойств "Операнд1" и "Операнд2".
        /// </summary>
        /// <returns></returns>
        public ResultStatus DoOperation()
        {
            switch (Operator)
            {
                case CalculatorOperators.Add:
                    Result = Operand1 + Operand2;
                    return ResultStatus.OK;
                case CalculatorOperators.Substract:
                    Result = Operand1 - Operand2;
                    return ResultStatus.OK;
                case CalculatorOperators.Multiply:
                    Result = Operand1 * Operand2;
                    return ResultStatus.OK;
                case CalculatorOperators.Divide:
                    if (Math.Abs(Operand2) < 1e-10)
                    {
                        Console.WriteLine(ConvertMessageToString(ConsoleMessages.DivisionByZero));
                        return ResultStatus.DivisionByZero;
                    }
                    Result = Operand1 / Operand2;
                    return ResultStatus.OK;
                case CalculatorOperators.POW:
                    Result = Math.Pow(Operand1, Operand2);
                    return ResultStatus.OK;
                case CalculatorOperators.MPlus:
                    Memory += Result;
                    return ResultStatus.OK;
                case CalculatorOperators.MMinus:
                    Memory -= Result;
                    return ResultStatus.OK;
                case CalculatorOperators.MR:
                    Result = Memory;
                    return ResultStatus.OK;
                case CalculatorOperators.MC:
                    Memory = 0;
                    return ResultStatus.OK;
                case CalculatorOperators.Help:
                    Console.WriteLine(ConvertMessageToString(ConsoleMessages.Help));
                    return ResultStatus.OK;
                case CalculatorOperators.Exit:
                    return ResultStatus.Exit;
                default:
                    throw new NotSupportedException();
            }
        }

        public string ConvertMessageToString(ConsoleMessages message)
        {
            switch (message)
            {
                case ConsoleMessages.Greeting:
                    return @"Добрый день!
Вас приветствует консольный калькулятор!
Введите help для получения справочной информации по командам.
Введите exit для выхода из программы.";
                case ConsoleMessages.Help:
                    return @"Справочная информация по командам системы.
На данный момент в программе поддерживаются команды следующих видов:
1. Трёхэлементные.
Команды вида ""Операнд1 Оператор Операнд2""
В качестве операторов могут выступать символы: '+', '-', '*', '/', 'ˆ'.
В качестве операндов могут выступать вещественные числа или команда MR
(команда MR подставляет на место операнда значение из регистра памяти).

2. Двухэлементные.
Команды вида ""Оператор Операнд2""
Эта команда соответствует трёхэлементной команде,
в которой вместо ""Операнд1"" подставлен результат прошлой команды.

3. Одноэлементные.
Команды вида ""Оператор""
В качестве оператора могут выступать следующие элементы:
• M+ - увеличение значения в регистре памяти на последний результат.
• M- - уменьшение значения в регистре памяти на последний результат.
• MR - запись в качестве результата значения из регистра памяти.
• MC - обнуление значения в регистре памяти.
• help - вывод справочной информации по командам системы.
• exit - выход из приложения.";
                case ConsoleMessages.DivisionByZero:
                    return @"К сожалению, второй операнд равен нулю.
На ноль делить нельзя!
Попробуйте другой делитель.";
                case ConsoleMessages.InvalidInput:
                    return @"Нам не удалось понять вашу команду.
Пожалуйста, вводите команды в соответствии с инструкцией к программе.
Чтобы увидеть инструкцию, введите команду help.";
                default:
                    throw new NotSupportedException();
            }
        }
    }
}
