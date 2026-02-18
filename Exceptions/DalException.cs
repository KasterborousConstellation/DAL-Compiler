namespace AbstractExceptions
{
    public abstract class DalException : Exception
    {
        public virtual string Name { get; set; } = "DalException";

        private readonly string _source;

        protected DalException(string source)
        {
            _source = source;
        }

        public string GetSource() => _source;

        public override string ToString() => _source;

        public string GetMessage()
        {
            return Name + ": An error occured in " + _source;
        }
    }

    public abstract class PreTokenizationException : DalException
    {
        public override string Name { get; set; } = "PreTokenizationException";

        protected PreTokenizationException(string source) : base(source) { }
    }

    public class MissingSourceFileException : PreTokenizationException
    {
        public override string Name { get; set; } = "MissingSourceFileException";

        public MissingSourceFileException(string source) : base(source) { }
    }
    public class NotEnoughParametersException : PreTokenizationException{
        public override string Name { get; set; } = "NotEnoughParametersException";
        public NotEnoughParametersException(string source) : base(source) { }
        
    }
    public class TokenizationException : DalException
    {
        public override string Name { get; set; } = "TokenizationException";

        public TokenizationException(string source) : base(source) { }
    }
    public class NonTerminatedStringLiteralException : TokenizationException
    {
        public override string Name { get; set; } = "NonTerminatedStringLiteralException";

        public NonTerminatedStringLiteralException(string source) : base(source) { }
    }
}