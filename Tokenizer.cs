using System.Text;
using AbstractExceptions;
namespace DAL;

class Tokenizer
{
    private int line = 0;
    private int position;
    private string content;
    public Tokenizer(string content) : base()
    {
        this.content = content;
        this.position = 0;
        this.builder = new StringBuilder();
    }

    public StringBuilder builder;
    public void consume()
    {
        builder.Append(content[position++]);

    }
    public void advance()
    {
        position++;
    }
    public char peekAhead()
    {
        return peekAhead(1);
    }
    public char peekAhead(int offset)
    {
        if (position + offset >= content.Length)
            return '\0';
        return content[position + offset];
    }
    public char peek()
    {
        if (position >= content.Length)
            return '\0';
        return content[position];
    }
    public bool isNumber(char c)
    {
        return char.IsDigit(c);
    }

    public string readNumber()
    {
        builder.Clear();
        while (isNumber(peek()))
        {
            consume();
        }
        return builder.ToString();
    }
    public void discard()
    {
        position++;
    }
    public string clear()
    {
        string result = builder.ToString();
        builder.Clear();
        return result;
    }
    public string consumeAndClear()
    {
        consume();
        string result = builder.ToString();
        builder.Clear();
        return result;
    }
    public bool isAlphaNumeric(char c)
    {
        return char.IsLetterOrDigit(c) || c == '_';
    }
    public List<Token> Parse()
    {
        List<Token> tokens = new List<Token>();
        while (position < content.Length)
        {
            char current = peek();
            //Console.WriteLine($"Current char: '{current}' at position {position} (line {line})");
            if (current == '\n')
            {
                line++;
                discard();
                continue;
            }
            else if (current == '\r')
            {
                discard();
                continue;
            }
            else if (char.IsWhiteSpace(current))
            {
                discard();
                continue;
            }
            else if (isNumber(current))
            {
                string number = readNumber();
                clear();
                tokens.Add(new Token(TokenType.Literal, number, line));
            }
            else if (Grammar.isSeparator(current))
            {
                tokens.Add(new Token(TokenType.Separator, consumeAndClear(), line));
            }
            else if (isAlphaNumeric(current))
            {
                builder.Clear();
                while (isAlphaNumeric(peek()))
                {
                    consume();
                }
                string word = clear();
                if (Grammar.isKeyword(word))
                {
                    tokens.Add(new Token(TokenType.Keyword, word, line));
                }
                else
                {
                    tokens.Add(new Token(TokenType.Identifier, word, line));
                }
            }
            else if (current == '<')
            {
                if (peekAhead() == '-')
                {
                    //assignment operator
                    consume();
                    consume();
                    tokens.Add(new Token(TokenType.Operator, "<-", line));
                }
                else
                {
                    //less than operator
                    consume();
                    tokens.Add(new Token(TokenType.Operator, "<", line));
                }
                clear();
            }
            else if(current =='"')
            {
                //string literal 
                discard();
                while (peek() != '"')
                {
                    if(peek() == '\0')
                    {
                        
                        throw new NonTerminatedStringLiteralException($"Unterminated string literal at line {line}");
                    }
                    consume();
                }
                string str = clear();
                discard();
                tokens.Add(new Token(TokenType.Literal, str, line));
            }
            else 
            {
                discard();
            }

        }
        return tokens;
    }
}