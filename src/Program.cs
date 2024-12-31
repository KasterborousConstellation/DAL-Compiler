using System;
using System.Text;
using ParseTree;
namespace DAL_Compiler{
    class Program {
        private static int ARGS_COUNT =1;
        private static FileLoader loader = new FileLoader();
        
        static void Main(string[] args ){   
            if(args.Length!=ARGS_COUNT){
                Console.Write("Wrong number of arguments passed to the compiler.\n Correct usage ./DAL-Compiler.exe <file_path>");
                System.Environment.Exit(1);
            }
            string path = args[0];
            string content = loader.load(path);
            Tokenizer tokenizer = new Tokenizer(content);
            List<Token.Token> tokens = tokenizer.tokenize();
            Parser parser = new Parser(tokens);
            RootNode root = parser.buildTree();
            int a = root.getChildrenCount();
            Console.Write(a);
        }
    }
}