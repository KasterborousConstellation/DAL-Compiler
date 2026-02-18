using AbstractExceptions;
using DAL;

try
{
    if (args.Length < 1)
    {
        throw new NotEnoughParametersException("DAL-Compiler");
    }
    string param1 = args[0];
    string source_content = FileUtils.readFile(param1);
    Tokenizer tokenizer = new Tokenizer(source_content);
    List<Token> tokens = tokenizer.Parse();
    foreach (Token token in tokens)
    {
        Console.WriteLine($"Type: {token.Type}, Value: '{token.Value}', Line: {token.Line}");
    }
    
}
catch (DalException dal)
{
    Console.WriteLine("A fatal error as occured : ");
    Console.WriteLine(dal.Message);
}