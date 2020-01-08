using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ConsoleCalculator
{
    public abstract class OperatorEnumeration : IComparable
    {
        public string Name { get; private set; }

        public int Id { get; private set; }

        public string String { get; private set; }

        protected OperatorEnumeration(int id, string name, string str)
        {
            Id = id;
            Name = name;
            String = str;
        }

        public override string ToString() => Name;

        public static IEnumerable<T> GetAll<T>() where T : OperatorEnumeration
        {
            var fields = typeof(T).GetFields(BindingFlags.Public |
                                             BindingFlags.Static |
                                             BindingFlags.DeclaredOnly);
            return fields.Select(f => f.GetValue(null)).Cast<T>();
        }

        public override bool Equals(object obj)
        {
            var otherValue = obj as OperatorEnumeration;
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

        public int CompareTo(object other) => Id.CompareTo(((OperatorEnumeration)other).Id);
    }

    public class CalculatorOperators : OperatorEnumeration
    {
        public static CalculatorOperators Add = new CalculatorOperators(1, "Add", "+");
        public static CalculatorOperators Substract = new CalculatorOperators(2, "Substract", "-");
        public static CalculatorOperators Multiply = new CalculatorOperators(3, "Multiply", "*");
        public static CalculatorOperators Divide = new CalculatorOperators(4, "Divide", "/");
        public static CalculatorOperators POW = new CalculatorOperators(5, "POW", "^");
        public static CalculatorOperators MPlus = new CalculatorOperators(6, "MPlus", "M+");
        public static CalculatorOperators MMinus = new CalculatorOperators(7, "MMinus", "M-");
        public static CalculatorOperators MR = new CalculatorOperators(8, "MR", "MR");
        public static CalculatorOperators MC = new CalculatorOperators(9, "MC", "MC");
        public static CalculatorOperators Help = new CalculatorOperators(10, "Help", "Help");
        public static CalculatorOperators Exit = new CalculatorOperators(11, "Exit", "Exit");

        public CalculatorOperators(int id, string name, string str)
            : base(id, name, str)
        {
        }
    }
}
