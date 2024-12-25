namespace Token{
    public enum TokenType {
        int_lit,
        bin_func,
        endl,
        identifier,
        keyword
    }
    public class Token{
    
    private TokenType type;
    private int idlvl;
    private string value;
    public Token(TokenType type,string value,int idlvl){
         this.type = type;
         this.value = value;
         this.idlvl = idlvl;         
    }
    public TokenType getType(){
        return type;
    }
    public int getIdentationLvl(){
        return idlvl;
    }
    public string getValue(){
        return value;
    }
    }
}
