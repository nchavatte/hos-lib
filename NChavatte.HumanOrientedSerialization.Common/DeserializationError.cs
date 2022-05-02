namespace NChavatte.HumanOrientedSerialization.Common
{
    public class DeserializationError
    {
        public DeserializationError(DeserializationErrorType errorType, int lineNumber, int wordNumber)
        {
            ErrorType = errorType;
            LineNumber = lineNumber;
            WordNumber = wordNumber;
        }

        public DeserializationErrorType ErrorType { get; }

        public int LineNumber { get; }

        public int WordNumber { get; }
    }
}
