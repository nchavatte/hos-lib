using System;

namespace NChavatte.HumanOrientedSerialization.Common
{
    public class DeserializationResult
    {
        public DeserializationResult(DeserializationError error)
        {
            Error = error ?? throw new ArgumentNullException(nameof(error));
            Content = null;
        }

        public DeserializationResult(byte[] content)
        {
            Content = content ?? throw new ArgumentNullException(nameof(content));
            Error = null;
        }

        public bool IsError => Error != null;

        public DeserializationError? Error { get; }

        public byte[]? Content { get; }

        public static DeserializationResult WordMalformed(int lineNumber, int wordNumber)
        {
            DeserializationErrorType errorType = DeserializationErrorType.WordMalformed;
            DeserializationError error = new DeserializationError(errorType, lineNumber, wordNumber);
            return new DeserializationResult(error);
        }

        public static DeserializationResult ParityBitDoesNotMatch(int lineNumber, int wordNumber)
        {
            DeserializationErrorType errorType = DeserializationErrorType.ParityBitDoesNotMatch;
            DeserializationError error = new DeserializationError(errorType, lineNumber, wordNumber);
            return new DeserializationResult(error);
        }

        public static DeserializationResult LineCheckSumDoesNotMatch(int lineNumber)
        {
            DeserializationErrorType errorType = DeserializationErrorType.LineCheckSumDoesNotMatch;
            DeserializationError error = new DeserializationError(errorType, lineNumber, 0);
            return new DeserializationResult(error);
        }

        public static DeserializationResult LineCheckSumMissing(int lineNumber)
        {
            DeserializationErrorType errorType = DeserializationErrorType.LineCheckSumMissing;
            DeserializationError error = new DeserializationError(errorType, lineNumber, 0);
            return new DeserializationResult(error);
        }

        public static DeserializationResult ContentLengthDoesNotMatch()
        {
            DeserializationErrorType errorType = DeserializationErrorType.ContentLengthDoesNotMatch;
            DeserializationError error = new DeserializationError(errorType, 0, 0);
            return new DeserializationResult(error);
        }

        public static DeserializationResult ContentLengthMissing()
        {
            DeserializationErrorType errorType = DeserializationErrorType.ContentLengthMissing;
            DeserializationError error = new DeserializationError(errorType, 0, 0);
            return new DeserializationResult(error);
        }

        public static DeserializationResult OpeningLineMissing()
        {
            DeserializationErrorType errorType = DeserializationErrorType.OpeningLineMissing;
            DeserializationError error = new DeserializationError(errorType, 0, 0);
            return new DeserializationResult(error);
        }

        public static DeserializationResult ClosingLineMissing()
        {
            DeserializationErrorType errorType = DeserializationErrorType.ClosingLineMissing;
            DeserializationError error = new DeserializationError(errorType, 0, 0);
            return new DeserializationResult(error);
        }
    }
}
