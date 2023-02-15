
var input = Console.ReadLine();
var operators = new char[] { '+', '-', '/', '*', '^', '(', ')' };
var buffer = "";
List<string> tokens = new List<string>();

foreach (var character in input)
{
    if ((character == '-' && buffer is "" && tokens[^1] == "(") || Char.IsDigit(character))
    {
        buffer += character;
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

foreach (var token in tokens)
{
    Console.WriteLine(token);
}