using System.Linq;
using System.Reflection.Metadata;
namespace ParseTree{
    public enum Types{
        T_int,
        T_real,
        T_boolean,
    }
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
        N_identifier
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
        public void setNode(Node node,int i){
            children[i]=node;
        }
        public void addChildren(Node node){
            if(Array.IndexOf<NodeType>(allowed,node.getNodeType())==-1){
                Console.WriteLine("This NodeType isn't allowed here.");
            }
            children.Add(node);
        }
        public Node? getNode(int i ){
            return children.ElementAtOrDefault<Node>(i);
        }
        public int getChildrenCount(){
            return this.children.Count;
        }
   }
   public class LitteralNode<T> : Node {
        private T value;
        public LitteralNode(T value) : base(NodeType.N_litteral){
            this.value = value;
            this.allowed = new NodeType[]{};
        }
        public T getValue(){
            return value;
        }
   }
   public class IdentifierNode : Node{
        public IdentifierNode(Types type) : base(NodeType.N_identifier){
            
        }
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
   
   public class InputNode :Node{
        private List<Types> types;
        private List<string> identifiers;
        public InputNode(): base(NodeType.N_fctIn){
            this.types = new List<Types>();
            this.identifiers = new List<string>();
        }
        public void addTypeInput(string idenfier,Types type){
            types.Add(type);
            identifiers.Add(idenfier);
        }
        public int getNumberOfInput(){
            return types.Count;
        }
        public Types getTypes(int i){
            return types[i];
        }
   }
   public class FunctionNode : Node {
        private string name;
        private FunctionType ftype;
        private Types r_type;
        public FunctionNode(string name,FunctionType ftype) : base(NodeType.N_function){
            this.name = name;
            this.ftype = ftype;
            this.allowed = new NodeType[]{NodeType.N_fctBody,NodeType.N_fctIn,NodeType.N_fct_Out};
            addChildren(new InputNode());
            addChildren(new InputNode());
        }
        public void setOutput(Types type){
            r_type = type;
        }
        public void setDeclarationNode(InputNode node){
            setNode(node,1);
        }
        public InputNode getDeclarationNode(){
            return (InputNode) getNode(1)!;
        }
        public void setInputNode(InputNode node){
            setNode(node,0);
        }
        public InputNode getInput(){
            return (InputNode)getNode(0)!;
        }
        public string getName(){
            return name;
        }
        public FunctionType GetFunctionType(){
            return ftype;
        }
   }
}