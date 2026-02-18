using AbstractExceptions;

namespace DAL;

public class FileUtils
{
    public static string readFile(string source) 
    {
        if (File.Exists(source))
        {
            return File.ReadAllText(source);
        }
        else
        {
            throw new MissingSourceFileException(source);
        }
    }
}