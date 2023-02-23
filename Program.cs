using System;
using System.Collections;
using System.Numerics;

class Program
{
    static void Main()
    {
        while (true)
        {
            Console.WriteLine("Enter an arithmetic expression to evaluate:");
            var input = Console.ReadLine();
            var postfixTokens = ConvertToPostfix(Tokenize(input));
            ArrayList postfixList = new ArrayList();
            postfixList.AddRange(postfixTokens);
            var result = Evaluate(postfixList);
            Console.WriteLine($"Result: {result}");
        }
    }

    static ArrayList Tokenize(string input)
    {
        var operators = new char[] { '+', '-', '/', '*', '^', '(', ')', '!' };
        var trigonometry = new string[] { "sin", "cos", "tan", "cot" };
        var currentToken = ""; //an empty string
        var tokens = new ArrayList();
        var isStartOfExpression = true;
        var lastWasOperator = true;

        foreach (var character in input)
        {
            if (Char.IsDigit(character))
            {
                if (!int.TryParse(currentToken, out _)) currentToken = "";
                currentToken += character;
                lastWasOperator = false;
            }
            else if (operators.Contains(character))
            {
                if (currentToken != "")
                {
                    tokens.Add(currentToken);
                    currentToken = "";
                }

                if (lastWasOperator && character == '-')
                {
                    tokens.Add("0");
                }
            
                isStartOfExpression = false || character == '(';
                lastWasOperator = true;

                tokens.Add(character.ToString());
            }
            else if (("sincota").Contains(character))
            {
                if (!int.TryParse(currentToken, out _))
                {
                    currentToken += character;
                }
            }

            if (trigonometry.Contains(currentToken))
            {
                tokens.Add(currentToken);
                currentToken = "";
            }
        }

        if (currentToken != "") tokens.Add(currentToken);

        return tokens;
    }



    static string[] ConvertToPostfix(ArrayList tokens)
    {
        Queue outputQueue = new Queue();
        Stack operatorStack = new Stack();
        var trigonometry = new string[] { "sin", "cos", "tan", "cot" };
        Dictionary<char, int> precedence = new Dictionary<char, int>
        {
            { '+', 1 },
            { '-', 1 },
            { '*', 2 },
            { '/', 2 },
            { '^', 3 }
        };

        foreach (string token in tokens.GetElements())
        {
            if (Char.IsDigit(token.ToCharArray()[0]))
            {
                outputQueue.Enqueue(token);
            }
            else if (token == "!") outputQueue.Enqueue(token);
            else if (operatorStack.Count() == 0)
            {
                operatorStack.Push(token);
            }
            else
            {
                switch (token)
                {
                    case "+" or "-":
                    {
                        while (operatorStack.Count() != 0 && operatorStack.GetLast() != "(")
                        {
                            outputQueue.Enqueue(operatorStack.Pop());
                        }

                        operatorStack.Push(token);
                        break;
                    }
                    case "*" or "/":
                    {
                        while (operatorStack.Count() != 0 && (operatorStack.GetLast() == "*" || operatorStack.GetLast() == "/" || operatorStack.GetLast() == "^"))
                        {
                            char lastOperator = operatorStack.GetLast()[0];
                            if (precedence[lastOperator] >= precedence[token[0]])
                            {
                                outputQueue.Enqueue(operatorStack.Pop());
                            }
                            else
                            {
                                break;
                            }
                        }

                        operatorStack.Push(token);
                        break;
                    }
                    case "^" or "(":
                        operatorStack.Push(token);
                        break;
                    case ")":
                    {
                        while (operatorStack.GetLast() != "(")
                        {
                            outputQueue.Enqueue(operatorStack.Pop());
                        }

                        if (operatorStack.Length() != 0 && trigonometry.Contains(operatorStack.GetLast()))
                        {
                            outputQueue.Enqueue(operatorStack.Pop());
                        }

                        operatorStack.Pop();
                        break;
                    }
                }
            }
        }

        while (operatorStack.Count() != 0)
        {
            outputQueue.Enqueue(operatorStack.Pop());
        }

        return outputQueue.GetElements().ToArray();
    }

    static int Factorial(int number)
    {
        if (number == 1) return 1;
        return number * Factorial(number - 1);
    }


    static double Evaluate(ArrayList postfixTokens)
    {
        var operators = new Dictionary<char, Func<double, double, double>>()
        {
            ['+'] = (a, b) => a + b,
            ['-'] = (a, b) => a - b,
            ['*'] = (a, b) => a * b,
            ['/'] = (a, b) => a / b,
            ['^'] = Math.Pow
        };

        var output = new Stack<double>();
        foreach (var token in postfixTokens.GetElements())
        {
            if (token == "!" && output.Count > 0)
            {
                output.Push(Convert.ToDouble(Factorial(Convert.ToInt32(output.Pop()))));
            }
            else if (token == "sin" && output.Count > 0)
            {
                output.Push(Math.Sin(output.Pop()));
            }
            else if (token == "cos" && output.Count > 0)
            {
                output.Push(Math.Cos(output.Pop()));
            }
            else if (token == "tan" && output.Count > 0)
            {
                output.Push(Math.Sin(output.Pop()));
            }
            else if (token == "cot" && output.Count > 0)
            {
                var number = output.Pop();
                output.Push(Math.Cos(number) / Math.Sin(number));
            }
            else if (operators.TryGetValue(token[0], out var op))
            {
                var number2 = output.Pop();
                var number1 = output.Pop();
                output.Push(op(number1, number2));
            }
            else
            {
                output.Push(double.Parse(token));
            }
        }

        return output.Pop();
    }
}

public class ArrayList
{
    private string[] _array = new string[10];
    private int _pointer = 0;

    public void Add(string element)
    {
        _array[_pointer] = element;
        _pointer++;
        if (_pointer == _array.Length)
        {
            var extendedArray = new string[_array.Length * 2];
            for (var i = 0; i < _array.Length; i++)
            {
                extendedArray[i] = _array[i];
            }

            _array = extendedArray;
        }
    }

    public void Remove(string element)
    {
        for (var i = 0; i < _pointer; i++)
        {
            if (_array[i] == element)
            {
                for (var j = i; j < _pointer - 1; j++)
                {
                    _array[j] = _array[j + 1];
                }

                _pointer--;
                return;
            }
        }
    }

    public string GetAt(int index)
    {
        return _array[index];
    }

    public int IndexOf(string value)
    {
        for (var i = 0; i < _array.Length; i++)
        {
            if (_array[i] == value)
            {
                return i;
            }
        }

        return -1;
    }

    public bool Contains(string element)
    {
        return IndexOf(element) != -1;
    }

    public int Count()
    {
        return _pointer;
    }

    public string[] GetElements()
    {
        return _array[.._pointer];
    }
    
    public void AddRange(string[] items)
    {
        foreach (var item in items)
        {
            Add(item);
        }
    }
    
}

public class Stack
{
    private string[] _array = new string[10];
    private int _pointer = 0;

    public void Push(string value)
    {
        _array[_pointer] = value;
        _pointer++;
        if (_pointer != _array.Length) return;
        var expandedArray = new string[_array.Length * 2];
        for (var i = 0; i < _pointer; i++)
        {
            expandedArray[i] = _array[i];
        }

        _array = expandedArray;
    }

    public string Pop()
    {
        if (_pointer == 0) return "-0";
        _pointer--;
        return _array[_pointer];
    }

    public int Length() => _pointer;

    public string GetLast() => _array[_pointer-1];
    
    public int Count()
    {
        int count = 0;
        for (int i = 0; i < _pointer; i++)
        {
            count++;
        }
        return count;
    }
}

public class Queue
{
    private string[] _array = new string[10];
    private int _pointer = 0;

    public void Enqueue(string value)
    {
        _array[_pointer] = value;
        _pointer++;
        if (_pointer != _array.Length) return;
        var expandedArray = new string[_array.Length * 2];
        for (var i = 0; i < _pointer; i++)
        {
            expandedArray[i] = _array[i];
        }

        _array = expandedArray;
    }

    public string? Dequeue()
    {
        if (_pointer == 0) return null;

        var value = _array[0];
        _pointer--;
        for (var i = 0; i < _pointer; i++)
        {
            _array[i] = _array[i + 1];
        }

        return value;
    }

    public int Length() => _pointer;

    public string GetAt(int index) => _array[index];

    public string[] GetElements() => _array[.._pointer];
}
//-3 ^ 2 -( - 3) ^ 2 * 7 ^ 2 * 3/  4 - 2  5 / 4 + (2^2^ 2/ 8 * 3)!