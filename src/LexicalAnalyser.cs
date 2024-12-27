public class LexicalAnalyser {
    public static string[] binfuncts = new string[]{"sortir"};
    public static string[] keywords = new string[]{"Algorithme","Entrées","Sortie","Déclaration","retourner"};
    public static char[] separators = new char[]{'(',',',')',':'};
    public static string[] types = new string[]{"entier"};
    public static Boolean isBuiltIn(string s){
        return isInside(binfuncts,s);
    }
    public static Boolean isKeyword(string s){
        return isInside(keywords,s);
    }
    private static Boolean isInside(string[] container,string elm){
        Boolean b = false;
        int i =0;
        int size = container.Length;
        while( i < size && !b){
            if(container.ElementAtOrDefault<string>(i) == elm){
                b = true;
            }
            i++;
        }
        return b;
    }
    public static Boolean isTypeRef(string s){
        return isInside(types,s);
    }
    public static Boolean isSeparator(char s){
        Boolean b = false;
        int i =0;
        int size = separators.Length;
        while( i < size && !b){
            if(separators.ElementAtOrDefault<char>(i) == s){
                b = true;
            }
            i++;
        }
        return b;
    }
}