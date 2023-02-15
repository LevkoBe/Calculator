
var input = Console.ReadLine();
var operators = new char[] { '+', '-', '/', '*', '^', '(', ')' };
var buffer = "";
List<string> tokens = new List<string>();

foreach (var character in input)
{
    if ((character == '-' && (tokens.Count == 0 || buffer is "" && tokens[^1] == "(")) || Char.IsDigit(character))
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

foreach (var token in tokens)
{
    Console.WriteLine(token);
}