using ParseTree;

public class Parser {
    private List<Token.Token> tokens;
    private int size;
    private int m_p;
    public Parser(List<Token.Token> tokens){
        this.tokens = tokens;
        size = tokens.Count;
    }
    Token.Token? peek(int n){
        if(m_p + n >=size){
            return null;
        }else{
            //unsafe access
            return tokens[m_p+n];
        }
    }
    private void discard(){
        m_p++;
    }
    Token.Token? peek(){
        return peek(0);
    }
    Token.Token? consume(){
        if(m_p>=size){
            return null;
        }else{
            return tokens[m_p++];
        }
    }
    private int line;
    private void error(string reported){
        Console.Write("Erreur à la ligne ");
        Console.Write(line);
        Console.Write(":");
        Console.WriteLine(reported);
        System.Environment.Exit(1);
    }
    private FunctionNode createFunction(List<Token.Token> lineToken){
        if(lineToken.Count !=3){
            error("L'entete de la fonction est malformée. Forme attendue: <Algorithme/Fonction/Procédure> : <nom>");
        }
        Token.Token separator = lineToken[1];
        if(separator.getType()!=Token.TokenType.separator){
            error("L'entete de la fonction est malformée. Le séparateur ',' est manquant.");
        }
        if(separator.getValue()!=":"){
            error("L'entete de la fonction est malformée. Séparateur attendu: ':'.");
        }
        Token.Token identifier = lineToken[2];
        if(identifier.getType()!=Token.TokenType.identifier){
            error("L'entete de la fonction est malformée. Le nom de la fonction n'est pas un identifieur valide.");
        }
        FunctionType type= FunctionType.Algo;
        switch(lineToken[0].getValue()){
            case "Algorithme":
                type = FunctionType.Algo;
            break;
            case "Fonction":
                type = FunctionType.Func;
            break;
            case "Procédure":
                type = FunctionType.Func;
            break;
        }
        FunctionNode node = new FunctionNode(identifier.getValue(),type);
        return node;
    }
    public ParseTree.RootNode buildTree(){
        line = -1;
        ParseTree.RootNode root = new ParseTree.RootNode();
        List<Token.Token> line_tokens = new List<Token.Token>();
        Boolean fct_template = true;
        while(peek()!=null){
            while(peek()!=null && peek().getType()!=Token.TokenType.endl){
                line_tokens.Add(consume());
            }
            discard();
            line++;
            //Now we have a line, we must parse it properly
            if(line_tokens.Count == 0){
                //Line is empty we do nothing
                continue;
            }
            Token.Token first_token = line_tokens[0];
            if(first_token.getType() == Token.TokenType.keyword
            && LexicalAnalyser.isFunctionType(first_token.getValue())
            ){
                //We now we are creating a function node
                FunctionNode fnode = createFunction(line_tokens);
                root.addChildren(fnode);
            }
            //Clear the line, as we are parsing the next one.
            line_tokens.Clear();
        }
        return root;
    }
}
