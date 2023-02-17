﻿
using System.Collections;

var input = Console.ReadLine();
var operators = new char[] { '+', '-', '/', '*', '^', '(', ')' };
var buffer = "";
var tokens = new ArrayList();

foreach (var character in input)
{
    if ((character == '-' && (tokens.Count() == 0 || buffer is "" && tokens.GetAt(tokens.Count() - 1) == "(")) || Char.IsDigit(character))
    {
        if (character == '-' && buffer.Length != 0)
        {
            tokens.Add(buffer);
            tokens.Add(character.ToString());
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
            tokens.Add(buffer);
            buffer = "";
        }
        tokens.Add(character.ToString());
    }
}

if (buffer != "")
{
    tokens.Add(buffer);
}

foreach (var token in tokens.GetElements())
{
    Console.WriteLine(token);
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

}