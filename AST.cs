namespace DAL.AST
{
    public enum NodeType
    {
        Function,
        FunctionHeader,
        FunctionBody,
        FunctionInput
    }
    public abstract class Node
    {
        private List<Node> children;
        public Node()
        {
            this.children  = new List<Node>();
        }

        public void append(Node node)
        {
            this.children.Add(node);
        }
        public abstract NodeType getType();
        public abstract bool isSyntacticallyValid();
        public abstract List<String> generateCode();
    }

    class FunctionInput : Node
    {
        private List<Node> children;

        public FunctionInput()
        {
            this.children  = new List<Node>();
        }

        public override NodeType getType()
        {
            return NodeType.FunctionInput;
        }
        public override bool isSyntacticallyValid()
        {
            return false;
        }

        public override List<string> generateCode()
        {
            return new List<string>();
        }
    }

    class FunctionHeader : Node
    {
        public List<Node> children;

        public FunctionHeader(List<FunctionInput> inputs)
        {
            this.children  = new List<Node>();
            children.AddRange(inputs);
        }

        public override NodeType getType()
        {
            return NodeType.FunctionHeader;
        }

        public override bool isSyntacticallyValid()
        {
            return false;
        }

        public override List<string> generateCode()
        {
            return new List<string>();
        }
    }
    class FunctionNode : Node
    {
        private string name;
        public FunctionNode(string name) : base()
        {
            this.name = name;
        }
        public override NodeType getType()
        {
            return NodeType.Function;
        }

        public override bool isSyntacticallyValid()
        {
            return false;
        }

        public override List<String> generateCode()
        {
            return new List<string>();
        }
    }
}