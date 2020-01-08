using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleCalculator
{
    public class CalculatorOperators
    {
        //Перечень всех операторов (формируется в конструкторе)
        public static List<CalculatorOperators> AllOperators { get; } = new List<CalculatorOperators>();

        //Перечень операторов
        public static CalculatorOperators Add { get; } = new CalculatorOperators(0, "Add", "+", false);
        public static CalculatorOperators Substract { get; } = new CalculatorOperators(1, "Substract", "-", false);
        public static CalculatorOperators Multiply { get; } = new CalculatorOperators(2, "Multiply", "*", false);
        public static CalculatorOperators Divide { get; } = new CalculatorOperators(3, "Divide", "/", false);
        public static CalculatorOperators POW { get; } = new CalculatorOperators(4, "POW", "^", false);
        public static CalculatorOperators MPlus { get; } = new CalculatorOperators(5, "MPlus", "M+", true);
        public static CalculatorOperators MMinus { get; } = new CalculatorOperators(6, "MMinus", "M-", true);
        public static CalculatorOperators MR { get; } = new CalculatorOperators(7, "MR", "MR", true);
        public static CalculatorOperators MC { get; } = new CalculatorOperators(8, "MC", "MC", true);
        public static CalculatorOperators Help { get; } = new CalculatorOperators(9, "Help", "Help", true);
        public static CalculatorOperators Exit { get; } = new CalculatorOperators(10, "Exit", "Exit", true);

        //перечень свойств
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Symbols { get; private set; }
        public bool OneElementCommand { get; private set; }

        //конструктор класса
        public CalculatorOperators(int id, string name, string str, bool oneElement)
        {
            Id = id;
            Name = name;
            Symbols = str;
            OneElementCommand = oneElement;
            AllOperators.Add(this);
        }

        public override string ToString() => Name;

        public static IEnumerable<CalculatorOperators> List()
        {
            return AllOperators;
        }

        //возвращает соответствующую команду по строке из консоли, соответствующей её вызову
        public static CalculatorOperators FromString(string str)
        {
            try
            {
                return List().Single(r => string.Equals(r.Symbols, str, StringComparison.OrdinalIgnoreCase));
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine(e);
                return CalculatorOperators.Help;
            }
        }

        //возвращает true, если в классе содержится команда, записывающаяся строкой str в консолт
        public static bool Contains(string str)
        {
            foreach (var op in AllOperators)
            {
                if (op.Symbols.ToLowerInvariant() == str.ToLowerInvariant())
                    return true;
            }
            return false;
        }

        //возвращает true, если команда - MR
        public static bool IsMR(string str)
        {
            if(MR.Symbols.ToLowerInvariant() == str.ToLowerInvariant())
                return true;
            return false;
        }

        //возвращает true, если команда может выполняться самостоятельно
        public static bool IsOneElementCommand(string str)
        {
            return FromString(str).OneElementCommand;
        }

        public override bool Equals(object obj)
        {
            var otherValue = obj as CalculatorOperators;
            if (otherValue == null)
                return false;
            var typeMatches = GetType().Equals(obj.GetType());
            var valueMatches = Id.Equals(otherValue.Id);
            return typeMatches && valueMatches;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
