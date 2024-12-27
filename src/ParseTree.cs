using System.Linq;
using System.Reflection.Metadata;
namespace ParseTree{
    public enum FunctionType{
        Algo,
        Proc,
        Func
    }
    public enum NodeType{
        N_litteral,
        N_operator,
        N_function,
        N_fctCall,
        N_expr,
        N_root,
        N_args,
        N_fctBody,
        N_fctIn,
        N_fct_Out,
    }
    public abstract class Node{
        protected NodeType[] allowed = new NodeType[]{};
        protected NodeType type;
        private List<Node> children;
        public Node(NodeType type){
            this.type = type;
            children = new List<Node>();
        }
        public NodeType getNodeType(){
            return type;
        }
        public void addChildren(Node node){
            if(Array.IndexOf<NodeType>(allowed,node.getNodeType())==-1){
                Console.WriteLine("This NodeType isn't allowed here.");
            }
            children.Add(node);
        }
        
        public int getChildrenCount(){
            return this.children.Count;
        }
   }
   public abstract class LitteralNode<T> : Node {
        private T value;
        public LitteralNode(T value) : base(NodeType.N_litteral){
            this.value = value;
            this.allowed = new NodeType[]{};
        }
        public T getValue(){
            return value;
        }
   }
   public class IntegerLitteral : LitteralNode<int> {
        public IntegerLitteral(int n) : base(n){}
   }
   public class ExpressionNode : Node {
        public ExpressionNode() : base(NodeType.N_expr){

        }
   }
   public class RootNode :Node {
        public RootNode() : base(NodeType.N_root){
            allowed = new NodeType[]{NodeType.N_function};
        }
   }
   public class ArgumentNode : Node {
        public ArgumentNode() : base(NodeType.N_args){
            allowed = new NodeType[]{NodeType.N_litteral,NodeType.N_expr};
        }
   }
   public class FunctionNode : Node {
        private string name;
        private FunctionType ftype;
        public FunctionNode(string name,FunctionType ftype) : base(NodeType.N_function){
            this.name = name;
            this.ftype = ftype;
            this.allowed = new NodeType[]{NodeType.N_fctBody,NodeType.N_fctIn,NodeType.N_fct_Out};
        }
        public string getName(){
            return name;
        }
        public FunctionType GetFunctionType(){
            return ftype;
        }
   }
}