
using System.Collections;

var input = Console.ReadLine();
var operators = new char[] { '+', '-', '/', '*', '^', '(', ')' };
var buffer = "";
var tokens = new Queue();
var west = new Queue();
var south = new Stack();
var output = new Stack();

foreach (var character in input)
{
    if ((character == '-' && (tokens.Length() == 0 || buffer is "" && tokens.GetAt(tokens.Length() - 1) == "(")) || Char.IsDigit(character))
    {
        if (character == '-' && buffer.Length != 0)
        {
            tokens.Enqueue(buffer);
            tokens.Enqueue(character.ToString());
            buffer = "";
        }
        else
        {
            buffer += character;
        }
    }
    else if (operators.Contains(character))
    {
        if (buffer != "")
        {
            tokens.Enqueue(buffer);
            buffer = "";
        }
        tokens.Enqueue(character.ToString());
    }
}

if (buffer != "") tokens.Enqueue(buffer);

foreach (var token in tokens.GetElements())
{
    Console.WriteLine(token);
}
Console.WriteLine("\n\n");
while (tokens.Length() != 0)
{
    var something = tokens.Dequeue();
    if (something.Length != 1 || Char.IsDigit(something.ToCharArray()[0]))
    {
        west.Enqueue(something);
    }
    else if (south.Length() == 0)
    {
        south.Push(something);
    }
    else
    {
        switch (something)
        {
            case "+" or "-":
            {
                while (south.Length() != 0 && south.GetLast() != "(")
                {
                    west.Enqueue(south.Pop());
                }
                south.Push(something);
                break;
            }
            case "*" or "/":
            {
                while (south.GetLast() is "*" or "/" or "^")
                {
                    west.Enqueue(south.Pop());
                }
                south.Push(something);
                break;
            }
            case "^" or "(":
                south.Push(something);
                break;
            default:
            {
                while (south.GetLast() != "(")
                {
                    west.Enqueue(south.Pop());
                }

                south.Pop();
                break;
            }
        }
    }
}

while (south.Length() != 0)
{
    west.Enqueue(south.Pop());
}

foreach (var token in west.GetElements())
{
    Console.WriteLine(token);
}


while (west.Length() != 0)
{
    var element = west.Dequeue();
    if (element.Length == 1 && operators.Contains(element.ToCharArray()[0]))
    {
        var number2 = int.Parse(output.Pop());
        var number1 = int.Parse(output.Pop());
        switch (element)
        {
            case "+":
            {
                output.Push((number1 + number2).ToString());
                break;
            }
            case "-":
            {
                output.Push((number1 - number2).ToString());
                break;
            }
            case "*":
            {
                output.Push((number1 * number2).ToString());
                break;
            }
            case "/":
            {
                output.Push((number1 / number2).ToString());
                break;
            }
            default:
            {
                output.Push(Math.Pow(number1, number2).ToString());
                break;
            }
        }
    }
    else
    {
        output.Push(element);
    }
}
Console.WriteLine(output.Pop());


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
//- 3 -( - 3) * 7 ^ 2 * 3/  4 - 2  5