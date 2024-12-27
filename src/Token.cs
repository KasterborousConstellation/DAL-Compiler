namespace Token{
    public enum TokenType {
        int_lit,
        bin_func,
        endl,
        identifier,
        keyword,
        unknown,
        separator,
        typeref
    }
    public class Token{
    
    private TokenType type;
    private string value;
    public Token(TokenType type,string value){
         this.type = type;
         this.value = value;     
    }
    public TokenType getType(){
        return type;
    }
    public string getValue(){
        return value;
    }
    }
}
