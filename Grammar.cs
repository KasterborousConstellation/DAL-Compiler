namespace DAL;
using System.Globalization;
using System.Text;

enum Separators
{
    Comma = ',',
    Colon = ':',
    Semicolon = ';',
    OpenParen = '(',
    CloseParen = ')',
    OpenBrace = '{',
    CloseBrace = '}',
    OpenBracket = '[',
    CloseBracket = ']'
}
public class Grammar
{   
    static string format(string s)
    {
        return RemoveDiacritics(s).ToLower().Trim();
    }
    static List<string> keywords = new List<string>
{
    "Algorithme",
};
    static List<char> operators = new List<char>
    { '+','-','*','/'};
    public static bool isKeyword(string word)
    {
        return keywords.Contains(format(word));
    }
    public static bool isSeparator(char c)
    {
        return Enum.IsDefined(typeof(Separators), (int)c);
    }

    static string RemoveDiacritics(string text)
    {
        var normalizedString = text.Normalize(NormalizationForm.FormD);
        var stringBuilder = new StringBuilder(capacity: normalizedString.Length);
        for (int i = 0; i < normalizedString.Length; i++)
        {
            char c = normalizedString[i];
            var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
            if (unicodeCategory != UnicodeCategory.NonSpacingMark)
            {
                stringBuilder.Append(c);
            }
        }
        return stringBuilder
            .ToString()
            .Normalize(NormalizationForm.FormC);
    }
    static List<string> types = new List<string>
    {
        "entier",
        "reel",
        "chaine",
        "booleen"
    };
    public static bool isType(string word)
    {
        return types.Contains(format(word));
    }
}