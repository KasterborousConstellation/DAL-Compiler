public class LexicalAnalyser {
    public static string[] binfuncts = new string[]{"sortie"};
    public static string[] keywords = new string[]{"retourner"};
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
}