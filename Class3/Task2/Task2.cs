using System.Text;
using NUnit.Framework;
using OneVariableFunction = System.Func<double, double>;
using FunctionName = System.String;

namespace Task2
{
    public class Task2
    {
/*
 * В этом задании необходимо написать программу, способную табулировать сразу несколько
 * функций одной вещественной переменной на одном заданном отрезке.
 */


// Сформируйте набор как минимум из десяти вещественных функций одной переменной
        internal static Dictionary<FunctionName, OneVariableFunction> AvailableFunctions =
            new Dictionary<FunctionName, OneVariableFunction>
            {
                { "square", x => x * x },
                { "sin", Math.Sin },
                { "sqrt", Math.Sqrt },
                { "abs", Math.Abs },
                { "sign", x => (x == 0) ? 0 : x / Math.Abs(x) },
                { "floor", Math.Floor },
                { "mantissa", x => x - Math.Floor(x) },
                { "cos", Math.Cos },
                { "cube", x => Math.Pow(x, 3) },
                { "strange", x => x * x - 2 * x + 17 }
            };

// Тип данных для представления входных данных
        internal record InputData(double FromX, double ToX, int NumberOfPoints, List<string> FunctionNames);

// Чтение входных данных из параметров командной строки
        private static InputData? prepareData(string[] args)
        {
            if (args.Length < 4)
            {
                return null;
            }

            var functionList = new List<string>();
            for (int i = 3; i < args.Length; ++i)
            {
                functionList.Add(args[i]);
            }

            return new InputData(Convert.ToDouble(args[0]), Convert.ToDouble(args[1]),
                Convert.ToInt32(args[2]), functionList);
        }

// Тип данных для представления таблицы значений функций
// с заголовками столбцов и строками (первый столбец --- значение x,
// остальные столбцы --- значения функций). Одно из полей --- количество знаков
// после десятичной точки.
        internal record FunctionTable
        {
            private List<double> coords = new List<double> ();
            private List<string> functions = new List<string> ();
            // Код, возвращающий строковое представление таблицы (с использованием StringBuilder)
            // Столбец x выравнивается по левому краю, все остальные столбцы по правому.
            // Для форматирования можно использовать функцию String.Format.
            public override string ToString()
            {
                var table = new StringBuilder();
                var maxLength = new List<int>();
                maxLength.Add(1);
                for (int i = 0; i < functions.Count; ++i)
                {
                    maxLength.Add(functions[i].Length);
                }

                for (int i = 0; i < coords.Count; ++i)
                {
                    maxLength[0] = Math.Max(maxLength[0], coords[i].ToString().Length);
                    for (int j = 0; j < functions.Count; ++j)
                    {
                        maxLength[j + 1] = Math.Max(maxLength[j + 1],
                            AvailableFunctions[functions[j]](coords[i]).ToString().Length);
                    }
                }

                table.Append("x".PadRight(maxLength[0]) + ' ');
                for (int i = 0; i < functions.Count; ++i)
                {
                    table.Append(functions[i].PadLeft(maxLength[i + 1]));
                    if (i != functions.Count - 1)
                    {
                        table.Append(' ');
                    }
                }

                table.Append(Environment.NewLine);

                for (int i = 0; i < coords.Count; ++i)
                {
                    table.Append(coords[i].ToString().PadRight(maxLength[0]) + ' ');
                    for (int j = 0; j < functions.Count; ++j)
                    {
                        table.Append(AvailableFunctions[functions[j]](coords[i]).ToString().PadLeft(maxLength[j + 1]));
                        if (j != functions.Count - 1)
                        {
                            table.Append(' ');
                        }
                    }

                    if (i != coords.Count - 1)
                    {
                        table.Append(Environment.NewLine);
                    }
                }

                return table.ToString();
            }

            public void AddCoord(double x)
            {
                coords.Add(x);
            }

            public void AddFunction(string name)
            {
                functions.Add(name);
            }
        }

/*
 * Возвращает таблицу значений заданных функций на заданном отрезке [fromX, toX]
 * с заданным количеством точек.
 */
        internal static FunctionTable tabulate(InputData input)
        {
            var functionTable = new FunctionTable();
            double delta = (input.ToX - input.FromX) / (input.NumberOfPoints - 1);
            double currentCoord = input.FromX;
            for (int i = 0; i < input.NumberOfPoints; ++i)
            {
                functionTable.AddCoord(currentCoord);
                currentCoord += delta;
            }

            foreach (var functionName in input.FunctionNames)
            {
                functionTable.AddFunction(functionName);
            }

            return functionTable;
        }

        public static void Main(string[] args)
        {
            // Входные данные принимаются в аргументах командной строки
            // fromX fromY numberOfPoints function1 function2 function3 ...

            var input = prepareData(args);

            // Собственно табулирование и печать результата (что надо поменять в этой строке?):
            Console.WriteLine(tabulate(input).ToString());
        }
    }
}