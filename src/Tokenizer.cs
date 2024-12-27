using System.Diagnostics.CodeAnalysis;
using System.Text;
using Token;
public class Tokenizer{
    
    const char VOID_CHAR = '\0';
    private string content;
    private int m_p;
    private int size;
    StringBuilder buffer = new StringBuilder();
    public Tokenizer(string content){
        this.content = content;
        m_p =0;
        this.size = content.Length;
        buffer.Clear();
    }
    char peek(int n){
        if(m_p + n >=size){
            return '\0';
        }else{
            //unsafe access
            return content[m_p+n];
        }
    }
    char peek(){
        return peek(0);
    }
    void consume(){
        if(m_p>=size){
            return;
        }else{
            buffer.Append(content[m_p++]);
        }
    }
    private Boolean isVoid(char c){
        return c == VOID_CHAR;
    }
    private string collapse(){
        string s = buffer.ToString();
        buffer.Clear();
        return s;
    }
    private void reset(){
        m_p = 0;
    }
    private void discard(){
        m_p++;
    }
    public List<Token.Token> tokenize(){
        List<Token.Token> tokens = new List<Token.Token>();
        while(!isVoid(peek())){
            //Default should not happen 
            char current = peek();
            //We are here parsing a function name, declaration, b_infunct ..
            if(Char.IsLetter(current)){
                consume();
                while(!isVoid(peek()) && Char.IsLetterOrDigit(peek())){
                    consume();
                }
                //TODO support function name. Should be done before searching for keyword but after builtin
                //As builtin functions should be prioritized
                string name = collapse();
                Token.TokenType type ;
                if(LexicalAnalyser.isBuiltIn(name)){
                    type = Token.TokenType.bin_func;
                }else if(LexicalAnalyser.isKeyword(name)){
                    type = Token.TokenType.keyword;
                }else if(LexicalAnalyser.isTypeRef(name)){
                    type = Token.TokenType.typeref;
                }else{
                    type = Token.TokenType.identifier;
                }
                Token.Token token = new Token.Token(type,name);
                tokens.Add(token);
            }else if (current == '\r'){
                consume();
                if(isVoid(peek())||peek()!='\n'){
                    Console.WriteLine("Error, malformed file: Carriage error.");
                    System.Environment.Exit(1);
                }
                consume();
                collapse();
                tokens.Add(new Token.Token(TokenType.endl,""));
            }else if(Char.IsWhiteSpace(current)){
                //Not taking into account for now
                discard();
            }else if(current == '\n'){
                consume();
                collapse();
                tokens.Add(new Token.Token(TokenType.endl,""));
            }else if(LexicalAnalyser.isSeparator(current)){    
                consume();
                tokens.Add(new Token.Token(TokenType.separator,collapse()));
            }else if(Char.IsDigit(current)){
                consume();
                while(!isVoid(peek()) && Char.IsDigit(peek())){
                    consume();
                }
                string name = collapse();
                tokens.Add(new Token.Token(TokenType.int_lit,name));
            }else{
                discard();
            }
        }
        //Automatically adds a endl if it's the end of the file
        if(tokens[tokens.Count-1].getType()!=TokenType.endl){
            tokens.Add(new Token.Token(TokenType.endl,""));
        }
        return tokens;
    }
}