using System.Diagnostics.CodeAnalysis;
using System.Text;

public class FileLoader{
    private StringBuilder builder;
    private string ctns;
    public FileLoader(){
        this.builder = new StringBuilder();
        this.ctns = "";
    }
    public string load(string path){
        discard();
        if(!File.Exists(path)){
                Console.WriteLine("File provided doesn't exists.");
                System.Environment.Exit(1);
        }
        //READ FILE AND PUT CONTENTS IN file_contents
        using (FileStream stream = File.OpenRead(path))
        {
            byte[] b = new byte[1024];
            UTF8Encoding temp = new UTF8Encoding(true);
            int readLen;
            while ((readLen = stream.Read(b,0,b.Length)) > 0)
            {
                builder.Append(temp.GetString(b,0,readLen));
            }
            ctns = builder.ToString();
        }
        return ctns;
    }
    private void discard(){
        ctns = "";
        builder.Clear();
    }
}