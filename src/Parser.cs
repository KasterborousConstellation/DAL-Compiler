using System.Diagnostics.CodeAnalysis;
using System.Windows.Markup;
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
            error("L'entete de la fonction est malformée. Le séparateur ':' est manquant ou mal placé.");
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
    private void parseDeclarationLine(ref ParseTree.RootNode root, List<Token.Token> line_tokens){
        if(root.getChildrenCount()==0){
            error("L'entete de la fonction est malformée. Aucun algorithme n'a été défini.");
        }
        //We are defining the input of the last function added to the root node.
        Node? node = root.getNode(root.getChildrenCount()-1);
        if(node == null){
            //Should never happen
            Console.WriteLine("Erreur du compilateur. Code 1");
            System.Environment.Exit(1);
            return;
        }
        if(node is FunctionNode){
            ParseTree.FunctionNode functionNode = (ParseTree.FunctionNode)node;
            //Check the input separator
            Token.Token s_token = line_tokens[1];
            if(s_token.getType() != Token.TokenType.separator){
                 error("L'entete de la fonction est malformée. Le séparateur ':' est manquant ou mal placé.");
            }
            if(s_token.getValue()!=":"){
                error("L'entete de la fonction est malformée. Séparateur attendu: ':'.");
            }
            //Separator is placed properly and is correct
            //Delete unwanted
            line_tokens.RemoveRange(0,2);
            //We now need to parse properly arguments
            ParseTree.InputNode inputNode = new InputNode();
            //The form must be <identifier : type Optional','>
            //We therefore split the list if we find the proper separator
            List<List<Token.Token>> args = line_tokens.Aggregate(new List<List<Token.Token>>{new List<Token.Token>()},
            (list,value) => {
                if(value.getType()==Token.TokenType.separator
                &&value.getValue()==","
                ){
                    list.Add(new List<Token.Token>());
                }else{
                    list.Last().Add(value);
                }
                return list;
            }
            );
            //We can now parse each argument accordingly
            int n = args.Count;
            for(int i =0; i <n;i++){
                List<Token.Token> input_i = args[i];
                if(input_i.Count!=3){
                    error($"Le paramètre n°{i} est malformé. Forme attendue: <idenfieur:type>");
                    return;
                }
                //WE check all the types
                Token.Token id_t = input_i[0];
                Token.Token separator_t = input_i[1];
                Token.Token type_t = input_i[2];
                //Check id_t 
                if(id_t.getType()!=Token.TokenType.identifier){
                    error($"Le paramètre n°{i} est malformé. Forme attendue: <idenfieur:type>");
                }
                //Check separator_t
                if(separator_t.getType()!=Token.TokenType.separator
                ||separator_t.getValue()!=":"
                ){
                    error($"Le paramètre n°{i} est malformé. Forme attendue: <idenfieur:type>");
                }
                //Check type_t
                if(type_t.getType()!=Token.TokenType.typeref){
                    error($"Le paramètre n°{i} est malformé. Le type: {type_t.getValue()} n'existe pas.");
                }
                //Argument is form properly
                Types type = LexicalAnalyser.convert(type_t.getValue());
                //We add type to input
                inputNode.addTypeInput(id_t.getValue(),type);
            }
            //We modify the input node acordingly
            functionNode.setDeclarationNode(inputNode);
        }
    }
    private void parseInputLine(ref ParseTree.RootNode root, List<Token.Token> line_tokens){
        if(root.getChildrenCount()==0){
            error("L'entete de la fonction est malformée. Aucun algorithme n'a été défini.");
        }
        //We are defining the input of the last function added to the root node.
        Node? node = root.getNode(root.getChildrenCount()-1);
        if(node == null){
            //Should never happen
            Console.WriteLine("Erreur du compilateur. Code 1");
            System.Environment.Exit(1);
            return;
        }
        if(node is FunctionNode){
            ParseTree.FunctionNode functionNode = (ParseTree.FunctionNode)node;
            //Check the input separator
            Token.Token s_token = line_tokens[1];
            if(s_token.getType() != Token.TokenType.separator){
                 error("L'entete de la fonction est malformée. Le séparateur ':' est manquant ou mal placé.");
            }
            if(s_token.getValue()!=":"){
                error("L'entete de la fonction est malformée. Séparateur attendu: ':'.");
            }
            //Separator is placed properly and is correct
            //Delete unwanted
            line_tokens.RemoveRange(0,2);
            //We now need to parse properly arguments
            ParseTree.InputNode inputNode = new InputNode();
            //The form must be <identifier : type Optional','>
            //We therefore split the list if we find the proper separator
            List<List<Token.Token>> args = line_tokens.Aggregate(new List<List<Token.Token>>{new List<Token.Token>()},
            (list,value) => {
                if(value.getType()==Token.TokenType.separator
                &&value.getValue()==","
                ){
                    list.Add(new List<Token.Token>());
                }else{
                    list.Last().Add(value);
                }
                return list;
            }
            );
            //We can now parse each argument accordingly
            int n = args.Count;
            for(int i =0; i <n;i++){
                List<Token.Token> input_i = args[i];
                if(input_i.Count!=3){
                    error($"Le paramètre n°{i} est malformé. Forme attendue: <idenfieur:type>");
                    return;
                }
                //WE check all the types
                Token.Token id_t = input_i[0];
                Token.Token separator_t = input_i[1];
                Token.Token type_t = input_i[2];
                //Check id_t 
                if(id_t.getType()!=Token.TokenType.identifier){
                    error($"Le paramètre n°{i} est malformé. Forme attendue: <idenfieur:type>");
                }
                //Check separator_t
                if(separator_t.getType()!=Token.TokenType.separator
                ||separator_t.getValue()!=":"
                ){
                    error($"Le paramètre n°{i} est malformé. Forme attendue: <idenfieur:type>");
                }
                //Check type_t
                if(type_t.getType()!=Token.TokenType.typeref){
                    error($"Le paramètre n°{i} est malformé. Le type: {type_t.getValue()} n'existe pas.");
                }
                //Argument is form properly
                Types type = LexicalAnalyser.convert(type_t.getValue());
                //We add type to input
                inputNode.addTypeInput(id_t.getValue(),type);
            }
            //We modify the input node acordingly
            functionNode.setInputNode(inputNode);
        }
    }
    public void parseOutput(ref ParseTree.RootNode root,List<Token.Token> line_tokens){
        if(root.getChildrenCount()==0){
            error("L'entete de la fonction est malformée. Aucun algorithme n'a été défini.");
        }
        //We are defining the input of the last function added to the root node.
        Node? node = root.getNode(root.getChildrenCount()-1);
        if(node == null){
            //Should never happen
            Console.WriteLine("Erreur du compilateur. Code 1");
            System.Environment.Exit(1);
            return;
        }
        if(node is FunctionNode){
            ParseTree.FunctionNode functionNode = (ParseTree.FunctionNode)node;
            //Check the input separator
            Token.Token s_token = line_tokens[1];
            if(s_token.getType() != Token.TokenType.separator){
                 error("L'entete de la fonction est malformée. Le séparateur ':' est manquant ou mal placé.");
            }
            if(s_token.getValue()!=":"){
                error("L'entete de la fonction est malformée. Séparateur attendu: ':'.");
            }
            //Separator is placed properly and is correct
            //Delete unwanted
            line_tokens.RemoveRange(0,2);
            if(line_tokens.Count==0){
                error("L'entete de la fonction est malformée. Le mot-clef sortie prend un argument.");
            }
            if(line_tokens[0].getType()!=Token.TokenType.typeref){
                error($"Le type {line_tokens[0].getValue()} n'existe pas.");
            }
            Types type = LexicalAnalyser.convert(line_tokens[0].getValue());
            //We modify the output type acordingly
            functionNode.setOutput(type);
        }
    }
    public ParseTree.RootNode buildTree(){
        line = -1;
        ParseTree.RootNode root = new ParseTree.RootNode();
        List<Token.Token> line_tokens = new List<Token.Token>();
        Boolean fct_template = true;
        while(peek()is not null){
            while(peek()is not null && peek()?.getType()!=Token.TokenType.endl){
                line_tokens.Add(consume()!);
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
                fct_template = true;
            }else if(first_token.getType() == Token.TokenType.keyword
                && first_token.getValue() == "Entrées"
            )
            {
                if(!fct_template){
                    error("Le mot-clef Entrée devrait se trouver uniquement dans l'entete d'une fonction/procédure.");
                }
                
                parseInputLine(ref root,line_tokens);
                
            }else if(first_token.getType() == Token.TokenType.keyword
            && first_token.getValue() == "Déclaration"
            )
            {
                if(!fct_template){
                    error("Le mot-clef Déclaration devrait se trouver uniquement dans l'entete d'une fonction/procédure.");
                }
                parseDeclarationLine(ref root,line_tokens);
            }else if(
                first_token.getType() == Token.TokenType.keyword
                &&first_token.getValue() == "Sortie"
            ){
                if(!fct_template){
                    error("Le mot-clef Sortie devrait se trouver uniquement dans l'entete d'une fonction/procédure.");
                }
                parseOutput(ref root,line_tokens);
            }
            //Clear the line, as we are parsing the next one.
            line_tokens.Clear();
        }
        return root;
    }
}
