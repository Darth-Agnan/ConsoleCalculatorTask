using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ConsoleCalculator
{
    public abstract class Enumeration : IComparable
    {
        public string Description { get; private set; }

        public int Id { get; private set; }

        protected Enumeration(int id, string description)
        {
            Id = id;
            Description = description;
        }

        public override string ToString() => Description;

        public static IEnumerable<T> GetAll<T>() where T : Enumeration
        {
            var fields = typeof(T).GetFields(BindingFlags.Public |
                                             BindingFlags.Static |
                                             BindingFlags.DeclaredOnly);
            return fields.Select(f => f.GetValue(null)).Cast<T>();
        }

        public override bool Equals(object obj)
        {
            var otherValue = obj as Enumeration;
            if (otherValue == null)
                return false;
            var typeMatches = GetType().Equals(obj.GetType());
            var valueMatches = Id.Equals(otherValue.Id);
            return typeMatches && valueMatches;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public int CompareTo(object other) => Id.CompareTo(((Enumeration)other).Id);
    }

    public class ConsoleMessages : Enumeration
    {
        public static ConsoleMessages Greeting = new ConsoleMessages(1, @"Добрый день!
Вас приветствует консольный калькулятор!
Введите help для получения справочной информации по командам.
Введите exit для выхода из программы.");
        public static ConsoleMessages Help = new ConsoleMessages(2, @"Справочная информация по командам системы.
На данный момент в программе поддерживаются команды следующих видов:
1. Трёхэлементные.
Команды вида ""Операнд1 Оператор Операнд2""
В качестве операторов могут выступать символы: '+', '-', '*', '/', '^'.
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
• exit - выход из приложения.");
        public static ConsoleMessages DivisionByZero = new ConsoleMessages(3, @"К сожалению, второй операнд равен нулю.
На ноль делить нельзя!
Попробуйте другой делитель.");
        public static ConsoleMessages InvalidInput = new ConsoleMessages(4, @"Нам не удалось понять вашу команду.
Пожалуйста, вводите команды в соответствии с инструкцией к программе.
Чтобы увидеть инструкцию, введите команду help.");
        
        public ConsoleMessages(int id, string description)
            : base(id, description)
        {
        }
    }
}
